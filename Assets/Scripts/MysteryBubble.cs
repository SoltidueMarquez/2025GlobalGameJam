using UnityEngine;

public class MysteryBubble : MonoBehaviour
{
    private float duration = 8f;
    public void Init()
    {
        this.tag = "Tool";
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (Utils.CheckIfPlayer(other))
        {
            
            Destroy(gameObject);
        }
    }

    private void MakeEffect()
    {
        
    }
    
    private void SpeedUp()
    {
        
    }

    private void Absorb()
    {
        
    }
}