using System.Collections.Generic;
using Infrastructure.Core;
using UnityEngine;

namespace Infrastructure.SortingGame
{
    [CreateAssetMenu(fileName = "SortingGameSettings")]
    public class SortingGameSettings : GameSettingsBase
    {
        [SerializeField] public List<Sprite> ElementTypes;
        
        [SerializeField] public int Groups;
        [SerializeField] public int GroupsRows;
        [SerializeField] public Vector2 GroupSize;
        [SerializeField] public Vector2 GroupsSpacing;

        [SerializeField] public int ElementsInGroup;
        [SerializeField] public Vector2 ElementSize;
        [SerializeField] public List<Vector2> ElementsOffsets;
    }
}