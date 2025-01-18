using UnityEngine;

public class ToolManager : MonoBehaviour
{
    public static ToolManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    
    public GameObject candyPrefab;
    [Tooltip("x,z,检测半径")]public Vector3 range;
    public void CreateCandy()
    {
        Instantiate(candyPrefab, this.transform);
        candyPrefab.transform.position = Utils.GetRandomPosition(range.x, range.y, range.z);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateCandy();
        }
    }
}