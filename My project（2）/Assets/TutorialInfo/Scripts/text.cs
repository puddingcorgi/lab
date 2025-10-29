using UnityEngine;
using UnityEngine.UI;

public class SimpleTextCharge : MonoBehaviour
{
    public Text chargeText;
    private float chargeTimer = 0f;

    void Start()
    {
        if (chargeText != null)
        {
            chargeText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // 鼠标左键蓄力
        if (Input.GetMouseButton(0))
        {
            chargeTimer += Time.deltaTime;
            float progress = Mathf.Clamp01(chargeTimer / 3f);
            int percentage = Mathf.RoundToInt(progress * 100);

            if (chargeText != null)
            {
                chargeText.gameObject.SetActive(true);
                chargeText.text = $"{percentage}%";
            }
        }
        else
        {
            chargeTimer = 0f;
            if (chargeText != null)
            {
                chargeText.gameObject.SetActive(false);
            }
        }
    }
}