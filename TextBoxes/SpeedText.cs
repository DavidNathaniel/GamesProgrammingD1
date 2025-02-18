using TMPro;
using UnityEngine;

public class SpeedText : MonoBehaviour
{
    public string textToDisplay;
    public TMP_Text textElement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textElement.text = textToDisplay;
    }

    // Update is called once per frame
    void Update()
    {
        textElement.text = textToDisplay;
    }
}
