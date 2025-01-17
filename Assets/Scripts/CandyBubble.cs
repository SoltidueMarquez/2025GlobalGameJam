using UnityEngine;

public class CandyBubble : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Snake>().AddBodyPart();
            Destroy(gameObject);
        }
    }
}