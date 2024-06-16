using Core.Sfx;
using Infrastructure.Core;
using UnityEngine;

namespace Infrastructure.SortingGame
{
    [CreateAssetMenu(fileName = "SortingSfxSettings")]
    public class SortingSfxSettings : SfxSettingBase
    {
        public SfxType ElementTaken;
        public SfxType ElementPlaced;
    }
}