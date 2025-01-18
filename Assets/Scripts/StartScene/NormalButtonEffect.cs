using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NormalButtonEffect : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    public Vector3 normalScale = new Vector3(1, 1, 1);

    public Vector3 hoverScale = new Vector3(1.2f, 1.2f, 1.2f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.transform.localScale = new Vector3();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
