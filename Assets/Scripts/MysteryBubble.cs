using UnityEngine;
using UnityEngine.Events;

public class MysteryBubble : MonoBehaviour, IInit
{
    public float duration = 5f;
    public float deltaSpeed = 3f;
    public float deltaRadius = 2f;
    public void Init()
    {
        this.tag = "Tool";
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (Utils.CheckIfPlayer(other))
        {
            var snake = other.GetComponent<Snake>();
            MakeEffect(snake);
            if(AudioManager.Instance!=null) AudioManager.Instance.PlayRandomSound("Mystery");
            Destroy(gameObject.transform.parent.gameObject);
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
        GameManager.Instance.CreateCountTimer(snake.uiParent, "获得加速", duration, onEnd);
    }

    private void Absorb(Snake snake)
    {
        snake.ChangeRadius(deltaRadius);
        var onEnd = new UnityEvent();
        onEnd.AddListener(() =>
        {
            snake.ChangeRadius(-deltaRadius);//恢复
        });
        GameManager.Instance.CreateCountTimer(snake.uiParent, "增加吸收范围", duration, onEnd);
    }
}