using UnityEngine;
using UnityEngine.EventSystems;

namespace StartScene
{
    public class ButtonPointerEnter : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
    {
        public int num = 0;
        public GameObject buttonTextGroup;
    
        public void OnPointerEnter(PointerEventData eventData)
        {
            buttonTextGroup.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            buttonTextGroup.SetActive(false);
        }
    }
}
