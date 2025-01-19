using UnityEngine;

[CreateAssetMenu]
public class SnakeSettings : ScriptableObject
{
    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;
    
    public GameObject bodyPrefab;
    public string playerTag;
    public Color color;
}