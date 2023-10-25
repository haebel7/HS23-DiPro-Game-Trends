using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
    public Color firstColor;
    public Color lastColor;
    public float timeEffect;
    public bool firstToLast = true;
    private float speed;
    private Image blackImage;
    private float currentValue;
    private bool performEffect = false;
    private bool finished = false;
    private bool goingToLast;

    void OnEnable()
    {
        speed = 1 / timeEffect;
        goingToLast = firstToLast;

        if (blackImage == null)
        {
            blackImage = GetComponent<Image>();
        }

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
            if (goingToLast)
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
            }
        }
    }

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
