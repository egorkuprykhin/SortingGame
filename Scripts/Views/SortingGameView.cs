using Infrastructure.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.SortingGame
{
    public abstract class SortingGameView : GameView<SortingGameSettings, SortingAnimationSettings>
    {
        [SerializeField] private LayoutElement LayoutElement;

        protected abstract Vector2 Size { get; }

        protected sealed override void PreConstruct()
        {
            if (LayoutElement)
            {
                LayoutElement.preferredWidth = Size.x;
                LayoutElement.preferredHeight = Size.y;
            }
        }
    }
}