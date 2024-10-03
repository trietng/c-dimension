using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonAnchorScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private string intialText;
    private TextMeshProUGUI textElement;
    // Start is called before the first frame update
    void Start()
    {
        textElement = GetComponentInChildren<TextMeshProUGUI>();
        intialText = textElement.text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData) {
        textElement.text = "> " + intialText + " <";
        textElement.fontStyle = TMPro.FontStyles.Bold;
    }

    public void OnPointerExit(PointerEventData eventData) {
        textElement.text = intialText;
        textElement.fontStyle = TMPro.FontStyles.Normal;
    }
}
