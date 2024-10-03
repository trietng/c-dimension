using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingCanvasScript : MonoBehaviour
{
    [SerializeField] float fadingSpeed = 2f;
    
    private CanvasGroup objective;
    private bool fadeIn = false;
    private bool fadeOut = false;

    public bool visible = true;

    private Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        objective = transform.Find("LoadingCanvas").GetComponent<CanvasGroup>();
        canvas = objective.gameObject.GetComponent<Canvas>();
        visible = objective.alpha > 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (objective == null) return;
        if (fadeIn) {
            if (objective.alpha < 1) {
                objective.alpha += Time.deltaTime * fadingSpeed;
            }
            else {
                fadeIn = false;
                visible = true;
                canvas.sortingOrder = 100;
                objective.interactable = true;
                objective.blocksRaycasts = true;
            }
        }

        if (fadeOut) {
            if (objective.alpha > 0) {
                objective.alpha -= Time.deltaTime * fadingSpeed;
            }
            else {
                fadeOut = false;
                visible = false;
                canvas.sortingOrder = -1;
                objective.interactable = false;
                objective.blocksRaycasts = false;
            }
        }
    }

    public void show () {
        fadeIn = true;
    }

    public void hide () {
        fadeOut = true;
    }
}
