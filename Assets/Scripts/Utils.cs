using UnityEngine;

public class Utils
{
    public static Vector3 GetRandomPosition(float xRange, float zRange, float radius = 1f)
    {
        Vector3 randomPosition = Vector3.zero;
        bool positionFound = false;

        // 循环直到找到一个没有物体的随机位置
        while (!positionFound)
        {
            // 生成随机坐标
            randomPosition = new Vector3(Random.Range(-xRange, xRange), 0, Random.Range(-zRange, zRange));

            // 检查该位置是否有物体（通过检查半径范围内的碰撞）
            if (!Physics.CheckSphere(randomPosition, radius))
            {
                positionFound = true; // 没有物体就接受这个位置
            }
        }

        return randomPosition;
    }

}