using UnityEngine;
using UnityEngine.Events;

public class MysteryBubble : MonoBehaviour
{
    private float duration = 8f;
    private float deltaSpeed = 5f;
    private float deltaRadius = 5f;
    public void Init(float duration,float deltaSpeed,float deltaRadius)
    {
        this.tag = "Tool";
        this.duration = duration;
        this.deltaSpeed = deltaSpeed;
        this.deltaRadius = deltaRadius;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (Utils.CheckIfPlayer(other))
        {
            var snake = other.GetComponent<Snake>();
            MakeEffect(snake);
            Destroy(gameObject);
        }
    }

    private void MakeEffect(Snake snake)
    {
        if (Random.Range(0f, 1f) > 0.5f)
        {
            SpeedUp(snake);
        }
        else
        {
            Absorb(snake);
        }
    }
    
    private void SpeedUp(Snake snake)
    {
        snake.ChangeSpeed(deltaSpeed);
        var onEnd = new UnityEvent();
        onEnd.AddListener(() =>
        {
            snake.ChangeSpeed(-deltaSpeed);//恢复
        });
        UIManager.Instance.CreateCountTimer(snake.uiParent, "获得加速", duration, onEnd);
    }

    private void Absorb(Snake snake)
    {
        snake.ChangeRadius(deltaRadius);
        var onEnd = new UnityEvent();
        onEnd.AddListener(() =>
        {
            snake.ChangeRadius(-deltaRadius);//恢复
        });
        UIManager.Instance.CreateCountTimer(snake.uiParent, "增加吸收范围", duration, onEnd);
    }
}