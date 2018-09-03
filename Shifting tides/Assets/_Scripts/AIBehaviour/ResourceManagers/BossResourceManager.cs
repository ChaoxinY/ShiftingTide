using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossResourceManager : HostileResourceManager
{
    private Slider[] healthBars = new Slider[2];
    private Text text;

    protected override void Initialize()
    {
        base.Initialize();
        Transform UiTransform = GameObject.Find("UI").transform;
        healthBars[0] = UiTransform.Find("BossHealthBar").GetComponent<Slider>();
        healthBars[1] = UiTransform.Find("BossHealthBarBackground").GetComponent<Slider>();
        text = UiTransform.Find("BossName").GetComponent<Text>();
        text.text = gameObject.name;
        text.gameObject.SetActive(true);

        for (int i = 0; i < healthBars.Length; i++)
        {
            healthBars[i].gameObject.SetActive(true);
            healthBars[i].maxValue = maxHealth;
            healthBars[i].value = 0;
        }
        StartCoroutine(StaticToolMethods.LerpSliderBarValue(healthBars, healthBars[0].maxValue, 0.15f, 0.2f, 0.01f));
    }


    public override float CurrentHealth
    {
        get
        {
            return base.CurrentHealth;
        }

        set
        {
            base.currentHealth = value;
            if (sliderbarLerpCoroutine != null)
            {
                StopCoroutine(sliderbarLerpCoroutine);
            }
            sliderbarLerpCoroutine = StaticToolMethods.LerpSliderBarValue(healthBars, currentHealth, 0.3f, 0.2f, 0.05f);
            StartCoroutine(sliderbarLerpCoroutine);

        }
    }

    public override float CurrentArmor
    {
        get
        {
            return base.CurrentArmor;
        }

        set
        {
            base.CurrentArmor = value;
        }
    }

}
