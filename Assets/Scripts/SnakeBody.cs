using System.Collections;
using UnityEngine;

public class SnakeBody : MonoBehaviour
{
    public bool ifActive { get; private set; }

    public void Init(float lateActiveTime, string tag)
    {
        this.ifActive = false;
        StartCoroutine(ActivateBody(this, lateActiveTime)); // 延时激活
        this.tag = tag;
    }
    
    private IEnumerator ActivateBody(SnakeBody body, float lateActiveTime)
    {
        yield return new WaitForSeconds(lateActiveTime);
        body.ifActive = true;
    }
}