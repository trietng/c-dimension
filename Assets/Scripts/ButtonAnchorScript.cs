using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonAnchorScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public bool unclickable = false;
    [SerializeField] string style = "> %s <";
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
        if (unclickable) return;
        textElement.text = style.Replace("%s", intialText);
        textElement.fontStyle = TMPro.FontStyles.Bold;
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (unclickable) return;
        textElement.text = intialText;
        textElement.fontStyle = TMPro.FontStyles.Normal;
    }
}
