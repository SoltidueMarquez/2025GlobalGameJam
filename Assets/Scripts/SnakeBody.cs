using UnityEngine;

public class SnakeBody : MonoBehaviour
{
    public void Init(string tag)
    {
        this.tag = tag;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Utils.CheckIfPlayer(other, this.tag))
        {
            other.GetComponent<Snake>().Die();
        }
    }
}