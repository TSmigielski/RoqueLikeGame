using UnityEngine;

[CreateAssetMenu(fileName = "Level X", menuName ="Level/New Level Data")]
public class LevelGenerationData : ScriptableObject
{
    [Header("Level Size")]
    [Min(1)] public int numberOfCrawlers;
    [Min(1)] public int minimumCrawls; // per crawler
    [Min(1)] public int maximumCrawls; // per crawler

    [Header("Room Types")] // the following values don't do anything yet
    public bool hasStartingRoom = true;
    [Range(0, 100)] public float staircaseRoomChance = 50;
    [Range(0, 100)] public float elevatorRoomChance = 50;
    [Range(0, 100)] public float exitRoomChance = 80;

    [Header("Big Rooms")]
    [Tooltip("0 = no big rooms ; 100 = every room is big")]
    [Range(0, 100)] public float individualBigRoomChance = 5;  // the chance of a room becoming big
    [Range(0, 100)] public float doubleOverQuadrupleBias = 50; // if set to '0' then every big room will be a 2x2, if set to '100' every room will be either 1x2 or 2x1
}
