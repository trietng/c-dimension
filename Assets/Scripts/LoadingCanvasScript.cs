using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingCanvasScript : MonoBehaviour
{
    [SerializeField] float fadingSpeed = 2f;

    [SerializeField] string[] quotes = new string[3];

    [SerializeField] float quoteTime = 5f; // 5s
    
    private CanvasGroup objective, image;
    private bool fadeIn = false;
    private bool fadeOut = false;

    public bool visible = true;

    private Canvas canvas;
    private TextMeshProUGUI quotesText;
    private float quoteLastChecked = -1f;
    // Start is called before the first frame update
    void Start()
    {
        objective = transform.Find("LoadingCanvas").GetComponent<CanvasGroup>();
        image = transform.Find("image").GetComponent<CanvasGroup>();
        canvas = objective.gameObject.GetComponent<Canvas>();
        quotesText = objective.transform.Find("Quotes").GetComponent<TextMeshProUGUI>();
        visible = objective.alpha > 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (objective == null) return;

        // load quote text
        if (quoteLastChecked == -1 || Time.time - quoteLastChecked > quoteTime) loadQuote();

        float speed = Time.deltaTime * fadingSpeed;

        // handle fading effects
        if (fadeIn) {
            if (objective.alpha < 1) {
                objective.alpha += speed;
                image.alpha += speed;
            }
            else {
                fadeIn = false;
                visible = true;
            }
            return;
        }

        if (fadeOut) {
            if (objective.alpha > 0) {
                objective.alpha -= speed;
                image.alpha -= speed;
            }
            else {
                fadeOut = false;
                visible = false;
                transform.gameObject.SetActive(false);
            }
        }
    }

    private void loadQuote () {
        if (quotesText == null) return;
        quotesText.text = quotes[UnityEngine.Random.Range(0, quotes.Length)];
        quoteLastChecked = Time.time;
    }

    public void OnEnable () {
        loadQuote();
    }

    public void show () {
        fadeIn = true;
        transform.gameObject.SetActive(true);
    }

    public void hide () {
        fadeOut = true;
    }
}
