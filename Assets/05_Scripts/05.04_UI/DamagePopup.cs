using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    private const float DISAPPEAR_TIMER_MAX = 0.5f;
    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    private Vector3 moveVector;
    private static int sortingOrder;

    public static DamagePopup Create(Transform pfDamagePopup, Vector3 position, int damageAmount, DamageType damageType)
    {
        Transform damagePopupTransform = Instantiate(pfDamagePopup, position , Quaternion.Euler(new Vector3(30f, 45f, 0f)));
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount, damageType);
        damagePopupTransform.Translate(new Vector3(0,1,0));
        return damagePopup;
    }

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(int damageAmount, DamageType damageType)
    {
        textMesh.SetText(damageAmount.ToString());
        if (damageType.damageSourceName == "Enemy")
        {
            textColor = new Color(0xAB / 255f, 0x07 / 255f, 0x07 / 255f);
        }
        else
        {
            textColor = textMesh.color;
        }
        textMesh.color = textColor;
        disappearTimer = DISAPPEAR_TIMER_MAX;
        float randomDirection = Random.Range(-0.2f, 0.2f);
        moveVector = new Vector3(randomDirection, 1) * 5f;
        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;
    }

    void Update()
    {
        transform.Translate(moveVector * Time.deltaTime);
        moveVector -= moveVector * 1f * Time.deltaTime;

        if (disappearTimer > DISAPPEAR_TIMER_MAX * 0.5f)
        {
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            float disappearSpeed = 5f;
            float darkenSpeed = 20f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textColor.r -= darkenSpeed * Time.deltaTime;
            textColor.g -= darkenSpeed * Time.deltaTime;
            textColor.b -= darkenSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if(textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
