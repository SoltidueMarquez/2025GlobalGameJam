using UnityEngine;

public class CandyBubble : MonoBehaviour, IInit
{
    public void Init()
    {
        this.tag = "Tool";
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (Utils.CheckIfPlayer(other))
        {
            other.GetComponent<Snake>().AddBodyPart();
            Destroy(gameObject);
        }
    }
}