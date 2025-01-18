using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (Utils.CheckIfPlayer(other))
        {
            other.GetComponent<Snake>().Die();
        }
    }
}