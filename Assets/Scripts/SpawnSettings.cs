using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpawnSettings : ScriptableObject
{
    [Tooltip("x,z,检测半径")] public Vector3 range;
    public List<SpawnInfo> infos = new List<SpawnInfo>();
    public int maxNum;
}

[Serializable]
public struct SpawnInfo
{
    public float intervalTime;
    public int spawnCount;
}