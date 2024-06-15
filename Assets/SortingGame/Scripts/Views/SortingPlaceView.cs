using UnityEngine;

namespace Infrastructure.SortingGame
{
    public class SortingPlaceView : SortingGameView
    {
        [SerializeField] public RectTransform RectTransform;
        [SerializeField] public Transform ElementRoot;
        
        protected override Vector2 Size => _settings.ElementSize;

        public SortingElementView Element { get; private set; }
        
        public bool Empty() => !Element;
        
        public void SetElement(SortingElementView element)
        {
            Element = element;
        }
    }
}