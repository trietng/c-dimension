using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonAnchorScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public bool unclickable = false;
    [SerializeField] string style = "> %s <";

    private AudioManager audioManager;

    private string intialText;
    private TextMeshProUGUI textElement;

    private bool hiddenHover = false;
    // Start is called before the first frame update
    void Start()
    {
        textElement = GetComponentInChildren<TextMeshProUGUI>();
        intialText = textElement.text;
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        transform.gameObject.GetComponent<Button>().onClick.AddListener(() => {
            if (isClickable()) audioManager.playClickButton();
        });
    }

    private bool isClickable () {
        return !unclickable && transform.parent.GetComponent<Canvas>().sortingOrder >= 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (hiddenHover && isClickable()) {
            setHoverEffect();
            hiddenHover = false;
        }
    }

    private void setHoverEffect () {
        audioManager.playHoverButton();
        textElement.text = style.Replace("%s", intialText);
        textElement.fontStyle = TMPro.FontStyles.Bold;
    }

    public void setInitialText (string text) {
        intialText = text;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (!isClickable()) hiddenHover = true;
        else setHoverEffect();
    }

    public void OnPointerExit(PointerEventData eventData) {
        textElement.text = intialText;
        textElement.fontStyle = TMPro.FontStyles.Normal;
        hiddenHover = false;
    }
}
