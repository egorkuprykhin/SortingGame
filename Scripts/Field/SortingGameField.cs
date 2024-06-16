using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using Infrastructure.Extensions;
using Infrastructure.Services;
using Services.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.SortingGame
{
    public class SortingGameField : MonoService, IInitializableService
    {
        [SerializeField] private Transform GameContainer;
        [SerializeField] private Transform DragLayer;
        [SerializeField] private GridLayoutGroup Grid;

        private CancellationTokenSource _cancellationTokenSource;

        private SfxService _sfxService;
        private SortingGameFactory _gameFactory;
        private GameLifecycleService _gameLifecycleService;
        private ConfigurationService _configurationService;

        private SortingGameSettings _gameSettings;
        private SortingSfxSettings _sfxSettings;
        private SortingAnimationSettings _animationSettings;

        public HashSet<SortingGroupView> Groups { get; } = new HashSet<SortingGroupView>();
        
        public void Initialize()
        {
            _sfxService = ServiceLocator.GetService<SfxService>();
            _gameFactory = ServiceLocator.GetService<SortingGameFactory>();
            _gameLifecycleService = ServiceLocator.GetService<GameLifecycleService>();
            _configurationService = ServiceLocator.GetService<ConfigurationService>();

            _gameSettings = _configurationService.GetSettings<SortingGameSettings>();
            _sfxSettings = _configurationService.GetSettings<SortingSfxSettings>();
            _animationSettings = _configurationService.GetSettings<SortingAnimationSettings>();
        }
        
        public async void Create()
        {
            ConfigureGrid();
            CreateGroups(GameContainer);

            _cancellationTokenSource = new CancellationTokenSource();
            
            _gameLifecycleService.LockField();
            
            try
            {
                await CreateElementsInGroups(_cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                _gameLifecycleService.UnlockField();
            }
        }

        public void Clear()
        {
            _cancellationTokenSource?.Cancel();

            foreach (SortingGroupView group in Groups)
            {
                group.Clear();
                Destroy(group.gameObject);
            }
            
            Groups.Clear();
        }

        private void ConfigureGrid()
        {
            Grid.spacing = _gameSettings.GroupsSpacing;
            Grid.cellSize = _gameSettings.GroupSize;
            Grid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            Grid.constraintCount = _gameSettings.GroupsRows;
        }

        private void CreateGroups(Transform parent)
        {
            for (int i = 0; i < _gameSettings.Groups; i++)
            {
                SortingGroupView group = _gameFactory.CreateView<SortingGroupView>(parent);
                group.OnElementDropped += OnElementDroppedInGroup;
                
                Groups.Add(group);
            }
        }

        private async Task CreateElementsInGroups(CancellationToken token)
        {
            List<Sprite> types = GetTypes();

            while (types.Count > 0)
            {
                SortingGroupView group = GetRandomGroupWithEmptyPlaces();
                SortingPlaceView place = group.GetRandomEmptyPlace();
                Sprite type = types.RandomElement();

                CreateElement(place, type, true);

                types.Remove(type);
            }

            await Task.Delay((int)(_animationSettings.CreatingTime * 1000), token);
        }

        private void OnElementDroppedInGroup(SortingGroupView group, SortingElementView element)
        {
            if (group.HasEmptyPlace() && !group.ContainElement(element))
                CreateElementInGroup(group, element);
            else
                ReturnElementBack(element);
            
            _sfxService.PlaySfx(_sfxSettings.ElementPlaced);
        }

        private void CreateElementInGroup(SortingGroupView group, SortingElementView element)
        {
            SortingPlaceView place = group.GetNearEmptyPlace(element.transform.position);
            
            SortingElementView created = CreateElement(place, element.Type, false);
            created.ScaleOutCreated();

            element.transform.DOKill();
            Destroy(element.gameObject);
        }

        private SortingElementView CreateElement(SortingPlaceView place, Sprite type, bool animate)
        {
            SortingElementView element = _gameFactory.CreateView<SortingElementView>(place.ElementRoot);
            
            element.Construct(type, DragLayer);
            if (animate)
                element.AnimateCreated();
            place.SetElement(element);

            return element;
        }

        private void ReturnElementBack(SortingElementView element)
        {
            element.RestoreInitialState();
        }

        private SortingGroupView GetRandomGroupWithEmptyPlaces() =>
            Groups
                .Where(group => group.HasEmptyPlace())
                .RandomElement();

        private List<Sprite> GetTypes() =>
            _gameSettings.ElementTypes
                .Select(type => Enumerable.Repeat(type, _gameSettings.ElementsInGroup))
                .SelectMany(types => types)
                .ToList();
    }
}