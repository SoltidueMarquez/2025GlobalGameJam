using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        // 如果实例为空，初始化并保持物体不被销毁
        if (Instance == null)
        {
            Instance = this as T;
            DontDestroyOnLoad(gameObject);  // 保持物体在场景切换时不被销毁
        }
        else if (Instance != this)
        {
            Destroy(gameObject);  // 如果已经存在实例，销毁当前对象
        }
    }
}