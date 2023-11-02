using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
    public Color firstColor;
    public Color lastColor;
    public float timeEffect;
    public bool firstToLast;
    private float speed;
    private Image blackImage;
    private float currentValue;
    private bool performEffect = false;
    private bool finished = false;

    void Start()
    {
        speed = 1 / timeEffect;
        blackImage = GetComponent<Image>();

        if (firstToLast)
        {
            currentValue = 0f;
            blackImage.color = firstColor;
        }
        else
        {
            currentValue = 1f;
            blackImage.color = lastColor;
        }
    }

    void FixedUpdate()
    {
        if (performEffect)
        {
            if (firstToLast)
            {
                if (PerformFadeIn())
                {
                    finished = true;
                }
            }
            else
            {
                if (PerformFadeOut())
                {
                    finished = true;
                }
            }

            blackImage.color = Color.Lerp(firstColor, lastColor, currentValue);

            if (finished)
            {
                performEffect = false;
                firstToLast = !firstToLast;
            }
        }
    }

    // Last to first color
    private bool PerformFadeIn()
    {
        if (currentValue != 1f)
        {
            currentValue += speed * Time.deltaTime;

            if (currentValue > 1f)
            {
                currentValue = 1f;
                return true;
            }
        }
        return false;
    }

    // First to last color
    private bool PerformFadeOut()
    {
        if (currentValue != 0f)
        {
            currentValue -= speed * Time.deltaTime;

            if (currentValue < 0f)
            {
                currentValue = 0f;
                return true;
            }
        }
        return false;
    }

    public void StartEffect()
    {
        performEffect = true;
        finished = false;
    }
}
