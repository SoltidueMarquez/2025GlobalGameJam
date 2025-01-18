using UnityEngine;
using UnityEngine.Events;

public class Mark : MonoBehaviour
{
    public GameObject canvas;
    public CountText countText;

    public void Init(float time, UnityEvent onEnd)
    {
        canvas.transform.LookAt(Camera.main.transform);
        canvas.transform.Rotate(0, 180, 0);
        
        onEnd.AddListener(SelfDestroy);
        countText.Init(time, onEnd);
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
}