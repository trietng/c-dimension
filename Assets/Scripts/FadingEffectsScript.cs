using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingEffectsScript : MonoBehaviour
{
    [SerializeField] float fadingSpeed = 5f;
    
    private CanvasGroup objective;
    private bool fadeIn = false;
    private bool fadeOut = false;

    public bool visible = true;
    // Start is called before the first frame update
    void Start()
    {
        objective = GetComponent<CanvasGroup>();
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
                objective.GetComponent<Canvas>().sortingOrder = 1;
            }
            return;
        }

        if (fadeOut) {
            if (objective.alpha > 0) {
                objective.alpha -= Time.deltaTime * fadingSpeed;
            }
            else {
                fadeOut = false;
                visible = false;
                objective.GetComponent<Canvas>().sortingOrder = -1;
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
