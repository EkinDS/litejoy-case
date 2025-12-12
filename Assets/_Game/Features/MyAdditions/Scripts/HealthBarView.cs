using UnityEngine;
using UnityEngine.UI;

public sealed class HealthBarView : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    public void SetNormalized(float normalized)
    {
     
        fillImage.fillAmount = Mathf.Clamp01(normalized);
    }

    public void SetValues(float current, float max)
    {
        if (max <= 0f)
        {
            SetNormalized(0f);
            return;
        }

        SetNormalized(current / max);
    }
}