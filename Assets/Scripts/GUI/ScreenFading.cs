using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFading : MonoBehaviour
{
    [SerializeField] private Image img;

    private static ScreenFading self;

    private Coroutine changing;

    [SerializeField] private float fadeSpeed;

    void Awake()
    {
        self = this;
        changing = null;
    }

    void StartFade(bool fadeIn)
    {
        if (changing != null)
        {
            StopCoroutine(changing);
        }
        changing = StartCoroutine(Fading(fadeIn));
    }

    IEnumerator Fading(bool fadeIn)
    {
        Color col = img.color;
        int side = fadeIn ? 1 : -1;
        bool continueOperation = true;

        while (continueOperation)
        {
            col.a += Time.deltaTime * fadeSpeed * side;
            img.color = col;
            if (col.a <= 0 || col.a >= 1)
            {
                continueOperation = false;
            }
            yield return new WaitForEndOfFrame();
        }

        changing = null;
    }

    void Start()
    {
        img.color = Color.black;
        ScreenFading.FadeOut();
    }

    public static bool fading { get { return self.changing != null; } }

    public static void FadeIn()
    {
        self.StartFade(true);
    }

    public static void FadeOut()
    {
        self.StartFade(false);
    }
}
