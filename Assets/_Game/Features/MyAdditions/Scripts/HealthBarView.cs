using UnityEngine;
using UnityEngine.UI;

public sealed class HealthBarView : MonoBehaviour
{
    [SerializeField] private Image fillImage;


    public void SetValues(int current, int max)
    {
        if (max <= 0F)
        {
            SetNormalized(0F);
            return;
        }

        SetNormalized((float)current / max);
    }

    private void SetNormalized(float normalized)
    {
        fillImage.fillAmount = Mathf.Clamp01(normalized);
    }
}