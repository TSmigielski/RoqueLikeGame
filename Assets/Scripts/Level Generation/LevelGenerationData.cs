using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level X", menuName ="Level/New Level Data")]
public class LevelGenerationData : ScriptableObject
{
    [Range(1, 9)] public int numberOfCrawlers;
    [Min(0)] public Vector2Int iterationMinMax;

    [Space]

    [Range(0f, 100f)] public float doubleRoomChance;
    [Range(0f, 100f)] public float quadrupleRoomChance;
}
