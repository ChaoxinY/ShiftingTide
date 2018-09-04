using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MobResourceManager : HostileResourceManager
{
    protected Canvas uiCanvas;
    protected Ui playerUi;
    protected Slider[] healthBars = new Slider[2], armorBars = new Slider[2];
    private IEnumerator healthBarVisibilityCoroutine, armorhBarVisibilityCoroutine;

    private void LateUpdate()
    {
        if (CurrentHealth > 0)
        {
            uiCanvas.transform.LookAt(GameObject.Find("Player").transform);
        }
    }

    protected override void Initialize()
    {
        base.Initialize();
        uiCanvas = GetComponentInChildren<Canvas>();
        playerUi = GameObject.Find("UI").GetComponent<Ui>();
        healthBars[0] = uiCanvas.transform.Find("HealthBar").gameObject.GetComponent<Slider>();
        healthBars[1] = uiCanvas.transform.Find("HealthBarBackGround").gameObject.GetComponent<Slider>();
        armorBars[0] = uiCanvas.transform.Find("ArmorBar").gameObject.GetComponent<Slider>();
        armorBars[1] = uiCanvas.transform.Find("ArmorBarBackGround").gameObject.GetComponent<Slider>();

        for (int i = 0; i < healthBars.Length; i++)
        {
            healthBars[i].maxValue = maxHealth;
            healthBars[i].value = maxHealth;
            armorBars[i].maxValue = maxArmor;
            armorBars[i].value = maxArmor;
        }
        armorhBarVisibilityCoroutine = StaticToolMethods.DisplaySliderBars(armorBars, 0,false);
        healthBarVisibilityCoroutine = StaticToolMethods.DisplaySliderBars(healthBars, 0, false);
        StartCoroutine(armorhBarVisibilityCoroutine);
        StartCoroutine(healthBarVisibilityCoroutine);
    }

    public override void GotHit(float baseDamage, Vector3 impactPoint, Vector3 impactForce)
    {
        base.GotHit(baseDamage, impactPoint, impactForce);
        playerUi.TrackTargetEnemyStatus(gameObject.name, healthBars, armorBars);
    }

    public override void GotHitOnCritSpot(float baseDamage, Vector3 impactPoint, Vector3 impactForce)
    {
        base.GotHitOnCritSpot(baseDamage, impactPoint, impactForce);
        playerUi.TrackTargetEnemyStatus(gameObject.name, healthBars, armorBars);
    }

    protected override IEnumerator OnDeathFeedBack()
    {
        foreach (Component componet in componentstoRemoveOnDeath)
        {
            Destroy(componet);
        }
        RagdollManager ragdollManager = GetComponent<RagdollManager>();
        ragdollManager.EnableRagdoll();
        ragdollManager.ApplyRagdollForce(lastCollisionPoint, lastCollisionImpactforce);
        yield return StartCoroutine(OnHitDrops(minOnDeathDrop, maxOnDeathDrop));
        uiCanvas.gameObject.SetActive(false);
        playerUi.StopTrackingStatus();
        this.enabled = false;
    }

    public override float CurrentHealth
    {
        get
        {
            return base.CurrentHealth;
        }

        set
        {
            currentHealth = value;
            if (currentHealth > maxHealth) { currentHealth = maxHealth; }
            StaticToolMethods.DisplaySliderBars(healthBars, 0, true);

            if (sliderbarLerpCoroutine != null)
            {
                StopCoroutine(sliderbarLerpCoroutine);
            }

            sliderbarLerpCoroutine = StaticToolMethods.LerpSliderBarValue(healthBars, currentHealth, 0.3f, 0.2f, 0.05f);
            StartCoroutine(sliderbarLerpCoroutine);

            StopCoroutine(healthBarVisibilityCoroutine);
            StaticToolMethods.DisplaySliderBars(healthBars, 6, false);
            StartCoroutine(healthBarVisibilityCoroutine);
            if (currentHealth <= 0)
            {
                StartCoroutine(OnDeathFeedBack());
            }
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
            StaticToolMethods.DisplaySliderBars(armorBars, 0, true);

            if (sliderbarLerpCoroutine != null)
            {
                StopCoroutine(sliderbarLerpCoroutine);
            }

            sliderbarLerpCoroutine = StaticToolMethods.LerpSliderBarValue(armorBars, currentArmor, 0.03f, 0.2f, 0.01f);
            StartCoroutine(sliderbarLerpCoroutine);

            StopCoroutine(armorhBarVisibilityCoroutine);
            StaticToolMethods.DisplaySliderBars(armorBars, 6, false);
            StartCoroutine(armorhBarVisibilityCoroutine);
        }
    }
}
