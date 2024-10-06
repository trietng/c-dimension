using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class ButtonAnchorScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public bool unclickable = false;
    [SerializeField] string style = "> %s <";

    [SerializeField] bool noHoverEffect = false;

    private AudioManager audioManager;

    private string intialText;
    private TextMeshProUGUI textElement;

    private Canvas canvasData;

    private bool hiddenHover = false;
    // Start is called before the first frame update
    void Start()
    {
        textElement = GetComponentInChildren<TextMeshProUGUI>();
        canvasData = transform.parent.GetComponent<Canvas>();
        intialText = textElement.text;
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        transform.gameObject.GetComponent<Button>().onClick.AddListener(() => {
            if (isClickable(true)) audioManager.playClickButton();
        });
    }

    private bool isClickable (bool doNotAddInactive = false) {
        return !unclickable && (canvasData != null ? canvasData.sortingOrder >= 0 : (doNotAddInactive || transform.gameObject.activeInHierarchy));
    }

    // Update is called once per frame
    void Update()
    {
        if (hiddenHover && !noHoverEffect && isClickable()) {
            setHoverEffect();
            hiddenHover = false;
        }
    }

    private void setHoverEffect () {
        audioManager.playHoverButton();
        textElement.text = style.Replace("%s", intialText);
        textElement.fontStyle = TMPro.FontStyles.Bold;
    }

    private void resetHoverEffect () {
        if (noHoverEffect || textElement == null) return;
        textElement.text = intialText;
        textElement.fontStyle = TMPro.FontStyles.Normal;
        hiddenHover = false;
    }

    public void setInitialText (string text) {
        intialText = text;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (noHoverEffect || !isClickable()) hiddenHover = true;
        else setHoverEffect();
    }

    public void OnPointerExit(PointerEventData eventData) {
        resetHoverEffect();
    }

    void OnEnable () {
        resetHoverEffect();
    }
}
