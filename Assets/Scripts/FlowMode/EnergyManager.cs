using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnergyManager : MonoBehaviour
{
    [Header("Energia")]
    public float maxEnergy = 25f;
    public float currentEnergy;

    [Header("UI")]
    public Image energyFillImage;
    public TextMeshProUGUI energyText;

    bool locked = false;

    void Start()
    {
        currentEnergy = maxEnergy;
        UpdateUI();
    }

    public void UseEnergy(float amount)
    {
        if (locked) return;

        currentEnergy -= amount;
        currentEnergy  = Mathf.Clamp(currentEnergy, 0f, maxEnergy);
        UpdateUI();

        if (currentEnergy <= 0f)
            FindObjectOfType<GameManager>()?.OnEnergyOut();
    }

    public void SetLocked(bool val) => locked = val;

    void UpdateUI()
    {
        if (energyFillImage != null)
            energyFillImage.fillAmount = currentEnergy / maxEnergy;

        if (energyText != null)
            energyText.text = Mathf.CeilToInt(currentEnergy).ToString();
    }
}