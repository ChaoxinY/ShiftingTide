using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerTideComboManager : MonoBehaviour
{  
    public Text comboDisplay;
    public Slider comboSlider;
    public PlayerBlinkersManager playerBlinkersManager;
    public float defaultComboTime;
    public float minimunComboTime;
    public int currentCombo;
    public bool comboActive;

    private float comboDuration;
    private float comboTimeLeft;
    private float bowChargeSpeedReduction;

    private void Update()
    {
        if (comboActive)
        {
            ComboTimeLeft -= Time.deltaTime;        
            comboSlider.value = ComboTimeLeft;
        }
    }

    public void StartCombo()
    {
        if (currentCombo == 0)
        {
            comboSlider.gameObject.SetActive(true);
            ComboDuration = defaultComboTime;
            ComboTimeLeft = ComboDuration;
            comboActive = true;
          
        }
        if (currentCombo > 0)
        {
            RefreshTimer();
        }
        comboSlider.maxValue = ComboDuration;
        comboSlider.value = ComboDuration;
    }

    public void RefreshTimer() {
        ComboDuration = defaultComboTime - currentCombo * 0.2f;
        ComboTimeLeft = ComboDuration;
    }

    public void AddCombo()
    {
        if (comboActive)
        {
            currentCombo += 1;
            comboDisplay.gameObject.SetActive(true);
            comboDisplay.text = ("HIT : " + currentCombo);
            StartCoroutine(PlayerResourcesManager.ChargeUpDash(0));
            bowChargeSpeedReduction = currentCombo * 0.5f;
            playerBlinkersManager.blinkerCooldownReduction = bowChargeSpeedReduction;
        }
    }

    public void ResetCombo()
    {
        if (comboActive)
        {
            currentCombo = 0;
            bowChargeSpeedReduction = 0;
            playerBlinkersManager.blinkerCooldownReduction = bowChargeSpeedReduction;
            comboSlider.gameObject.SetActive(false);
            comboDisplay.gameObject.SetActive(false);
            comboActive = false;
            comboDisplay.text = ("HIT : " + currentCombo);
        }
    }

    public float ComboTimeLeft
    {

        get
        {
            return comboTimeLeft;
        }
        set
        {
            comboTimeLeft = value;
            if (comboTimeLeft < 0)
            {
                ResetCombo();
            }

        }
    }

    public float BowChargeSpeedReduction
    {
        get
        {
            return bowChargeSpeedReduction;
        }

        set
        {
            bowChargeSpeedReduction = value;
            if (bowChargeSpeedReduction > 4)
            {
                bowChargeSpeedReduction = 4;
            }

        }
    }

    public float ComboDuration
    {
        get
        {
            return comboDuration;
        }

        set
        {
            comboDuration = value;
            if (comboDuration < minimunComboTime)
            {
                comboDuration = minimunComboTime;
            }
        }
    }
}
