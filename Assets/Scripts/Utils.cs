using UnityEngine;

public static class Utils
{
    private const int maxTryCount = 20;

    public static Vector3 GetRandomPosition(float xRange, float zRange, float radius = 1f)
    {
        Vector3 randomPosition = Vector3.zero;
    
        // 循环直到找到一个没有物体的随机位置，最多尝试20次
        for (int attempts = 0; attempts < maxTryCount; attempts++)
        {
            // 生成随机坐标
            randomPosition = new Vector3(Random.Range(-xRange, xRange), 0, Random.Range(-zRange, zRange));

            // 使用OverlapSphere检查当前位置的碰撞器
            Collider[] colliders = Physics.OverlapSphere(randomPosition, radius);

            bool positionValid = true;

            // 检查是否有碰撞器，且是否有标记为"Tool"的物体
            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Tool") || collider.CompareTag("Player"))
                {
                    positionValid = false;
                    break;
                }
            }

            // 如果当前位置没有被物体占据，并且不包含标记为"Tool"的物体，返回位置
            if (positionValid)
            {
                return randomPosition;
            }
        }

        // 如果超出最大尝试次数，返回最后生成的位置
        return randomPosition;
    }

    public static bool CheckIfPlayer(Collider other, string excludeTag = "Player")
    {
        if (other.CompareTag(excludeTag))
        {
            return false;
        }
        else
        {
            return other.CompareTag("Player1")||other.CompareTag("Player2")||other.CompareTag("Player3");
        }
    }

    public static bool CheckIfPlayer(Collision other, string excludeTag = "Player")
    {
        if (other.gameObject.CompareTag(excludeTag))
        {
            return false;
        }
        else
        {
            return other.gameObject.CompareTag("Player1")||other.gameObject.CompareTag("Player2")||other.gameObject.CompareTag("Player3");
        }
    }
}