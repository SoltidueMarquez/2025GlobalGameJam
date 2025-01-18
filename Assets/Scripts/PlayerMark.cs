using UnityEngine;
using UnityEngine.UI;

public class PlayerMark : MonoBehaviour
{
    public GameObject canvas;
    public Text nameText;

    public void Init(string title)
    {
        nameText.text = title;
    }
}