using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Extensions;
using Infrastructure.Services;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Infrastructure.SortingGame
{
    public class SortingGroupView : SortingGameView, IDropHandler
    {
        [SerializeField] private Transform PlacesRoot;

        private HashSet<SortingPlaceView> _places = new HashSet<SortingPlaceView>();

        protected override Vector2 Size => _settings.GroupSize;
        
        public event Action<SortingGroupView, SortingElementView> OnElementDropped;

        public bool HasEmptyPlace() => _places.Any(place => place.Empty());

        public bool ContainElement(SortingElementView element) =>
            _places.Any(place => place.Element == element);

        public bool IsEmpty() => _places.All(place => place.Empty());

        public bool AllElementsAreSame() =>
            _places.Count(place => !place.Empty()) == _settings.ElementsInGroup
            &&
            _places
                .Where(place => !place.Empty())
                .GroupBy(place => place.Element.Type)
                .Count() == 1;

        public SortingPlaceView GetRandomEmptyPlace() =>
            _places
                .Where(place => place.Empty())
                .RandomElement();
        
        public SortingPlaceView GetNearEmptyPlace(Vector3 position) =>
            _places
                .Where(place => place.Empty())
                .OrderBy(place => Vector3.Distance(place.transform.position, position))
                .First();

        public void Clear()
        {
            foreach (SortingPlaceView place in _places)
                if (!place.Empty())
                    place.Element.Clear();

            _places.Clear();
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag.TryGetComponent<SortingElementView>(out var element)) 
                OnElementDropped?.Invoke(this, element);
        }

        protected override void Construct()
        {
            CreatePlaces();
        }
        
        private void CreatePlaces()
        {
            SortingGameFactory factory = ServiceLocator.GetService<SortingGameFactory>();

            for (int i = 0; i < _settings.ElementsInGroup; i++)
            {
                SortingPlaceView placeView = factory.CreateView<SortingPlaceView>(PlacesRoot);
                
                placeView.RectTransform.anchoredPosition = _settings.ElementsOffsets[i];
                
                _places.Add(placeView);
            }
        }
    }
}