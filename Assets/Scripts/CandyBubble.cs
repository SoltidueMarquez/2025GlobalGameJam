using UnityEngine;

public class CandyBubble : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (Utils.CheckIfPlayer(other))
        {
            other.GetComponent<Snake>().AddBodyPart();
            Destroy(gameObject);
        }
    }
}