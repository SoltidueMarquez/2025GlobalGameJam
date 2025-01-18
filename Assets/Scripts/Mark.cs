using UnityEngine;
using UnityEngine.Events;

public class Mark : MonoBehaviour
{
    public GameObject canvas;
    public CountText countText;

    public void Init(float time, UnityEvent onEnd)
    {
        this.tag = "Tool";
        // 获取物体指向相机的方向向量
        Vector3 directionToCamera = Camera.main.transform.position - canvas.transform.position;
        // 将方向向量的Y和Z分量归零，仅计算X轴的旋转分量
        directionToCamera.y = 0; 
        directionToCamera.z = 0;
        // 计算物体指向相机的X轴旋转分量
        float angleX = Vector3.SignedAngle(Vector3.forward, directionToCamera, Vector3.right);
        // 应用旋转分量到Canvas
        canvas.transform.Rotate(angleX, 0, 0);
        
        onEnd.AddListener(SelfDestroy);
        countText.Init(time, onEnd);
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
}