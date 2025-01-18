using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonPointerEnter : MonoBehaviour,IPointerEnterHandler
{
    public int num = 0;
    public Text text;

    public void OnPointerEnter(PointerEventData eventData)
    {
        switch (num)
        {
            case(1) :
                text.text = "Player1: WSAD";
                break;
            case(2) :
                text.text = "Player1: WSAD\t\tPlayer2: \u2191\u2193\u2190\u2192";
                break;
            case(3) :
                text.text = "Player1: WSAD\t\tPlayer2: \u2191\u2193\u2190\u2192\nPlayer3: IKJL";
                break;
            case(4) :
                text.text = "Player1: WSAD\t\tPlayer2: \u2191\u2193\u2190\u2192\nPlayer3: IKJL\t\tPlayer4:5213";
                break;
        }
    }

}
