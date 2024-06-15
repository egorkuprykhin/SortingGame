using DG.Tweening;
using Infrastructure.Core;
using UnityEngine;

namespace Infrastructure.SortingGame
{
    [CreateAssetMenu(fileName = "SortingAnimationSettings")]
    public class SortingAnimationSettings : AnimationSettingsBase
    {
        public float ScaleValue;
        public float ScaleTime;
        public Ease ScaleEase;

        public float CreatingTime;
        public Ease CreatingEase;

        public float ReturnMoveTime;
        public Ease ReturnMoveEase;
    }
}