using UnityEngine;

public class BubbleBlower : MonoBehaviour
{
    public Vector3 dir;
    public float moveSpeed;
    public Vector3 boxCenterOffset = Vector3.zero; // 方形区域相对脚本携带者的位置偏移
    public Vector3 boxSize = new Vector3(5f, 2f, 5f); // 方形区域的尺寸（宽、高、深）
    public Quaternion boxRotation = Quaternion.identity; // 方形区域的旋转

    private void Update()
    {
        BlowBubble();
    }

    private void BlowBubble()
    {
        // 计算方形区域中心
        Vector3 boxCenter = transform.position + boxCenterOffset;

        // 检测方形区域内的所有碰撞体
        Collider[] colliders = Physics.OverlapBox(boxCenter, boxSize / 2, boxRotation);


        foreach (var collider in colliders)
        {
            // 检查物体是否挂载了Bubble脚本
            var bubble = collider.GetComponent<IInit>();
            if (bubble != null)
            {
                // 让泡泡向目标方向移动
                collider.transform.position += dir * (moveSpeed * Time.deltaTime);
            }
        }
    }
}