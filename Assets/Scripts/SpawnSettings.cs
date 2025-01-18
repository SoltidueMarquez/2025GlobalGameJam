using UnityEngine;

[CreateAssetMenu]
public class SpawnSettings : ScriptableObject
{
    [Tooltip("x,z,检测半径")] public Vector3 range;
    public float intervalTime;
    public int spawnCount;
    public int maxNum;
}