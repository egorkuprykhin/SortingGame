using DG.Tweening;
using Infrastructure.Data;
using Infrastructure.Services;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Infrastructure.SortingGame
{
    public class SortingElementView : SortingGameView, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Graphic DragRaycastSource;
        [SerializeField] private Image Image;

        private Transform _initialParent;
        private Transform _dragLayer;
        private bool _created;
        
        private SortingGameFinisher _sortingGameFinisher;
        private SfxService _sfxService;
        private GameLifecycleService _gameLifecycleService;

        protected override Vector2 Size => _settings.ElementSize;

        public Sprite Type { get; private set; }

        public void Construct(Sprite type, Transform dragLayer)
        {
            _dragLayer = dragLayer;
            Type = type;
            
            UpdateView();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(!CanDrag())
                return;
            DragRaycastSource.raycastTarget = false;
            SaveInitialParent();
            transform.SetParent(_dragLayer);
            ScaleIn();
            _sfxService.PlaySfx(SfxType.SwapChip);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(!CanDrag() || !_initialParent)
                return;
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if(!CanDrag() || !_initialParent)
                return;
            if (transform.parent == _dragLayer) 
                RestoreInitialState();
            
            DragRaycastSource.raycastTarget = true;
        }

        public void RestoreInitialState()
        {
            MoveToInitialPosition();
            ScaleOut();
        }

        public void ScaleOutCreated()
        {
            transform
                .DOScale(_animationSettings.ScaleValue, _animationSettings.ScaleTime)
                .SetEase(_animationSettings.ScaleEase)
                .From()
                .OnComplete(() =>
                {
                    _sortingGameFinisher.TryFinishGame();
                });
        }

        public void AnimateCreated()
        {
            transform.localScale = Vector3.zero;
            transform
                .DOScale(1, _animationSettings.CreatingTime)
                .SetEase(_animationSettings.CreatingEase)
                .OnComplete(SetCreated);
        }

        private void SetCreated()
        {
            _created = true;
        }

        public void Clear()
        {
            transform.DOKill();
        }

        protected override void Construct()
        {
            _sortingGameFinisher = ServiceLocator.GetService<SortingGameFinisher>();
            _sfxService = ServiceLocator.GetService<SfxService>();
            _gameLifecycleService = ServiceLocator.GetService<GameLifecycleService>();
        }

        private bool CanDrag()
        {
            return _created && !_gameLifecycleService.FieldLocked;
        }

        private void ScaleIn()
        {
            transform.DOScale(_animationSettings.ScaleValue, _animationSettings.ScaleTime)
                .SetEase(_animationSettings.ScaleEase);
        }

        private void ScaleOut()
        {
            transform.DOScale(1, _animationSettings.ScaleTime)
                .SetEase(_animationSettings.ScaleEase);
        }

        private void MoveToInitialPosition()
        {
            transform.DOMove(_initialParent.position, _animationSettings.ReturnMoveTime)
                .SetEase(_animationSettings.ReturnMoveEase)
                .OnComplete(() => transform.SetParent(_initialParent));
        }

        private void SaveInitialParent()
        {
            _initialParent = transform.parent;
        }

        private void UpdateView()
        {
            Image.sprite = Type;
        }
    }
}