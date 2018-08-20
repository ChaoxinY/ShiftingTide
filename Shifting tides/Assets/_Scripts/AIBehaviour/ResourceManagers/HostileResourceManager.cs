using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;


public class HostileResourceManager : MonoBehaviour
{
    public Component[] componentstoRemoveOnDeath;
    public Slider[] healthBars,armorBars;
    public float maxHealth, maxArmor;
    public int minOnHitDrop, maxOnHitDrop, minOnDeathDrop, maxOnDeathDrop;

    private Canvas uiCanvas;
    private Vector3 lastCollisionPoint, lastCollisionImpactforce;
    private float currentHealth,currentArmor;
   
    private IEnumerator healthBarVisibilityCoroutine, armorhBarVisibilityCoroutine;

    private void Start()
    {
        Initialize();
    }

    private void LateUpdate()
    {
        if (CurrentHealth > 0)
        {
            uiCanvas.transform.LookAt(GameObject.Find("Player").transform);
        }
    }

    protected virtual void Initialize()
    {
        currentHealth = maxHealth;
        currentArmor = maxArmor;
        uiCanvas = GetComponentInChildren<Canvas>();

        for (int i = 0; i < healthBars.Length; i++)
        {
            healthBars[i].maxValue = maxHealth;
            healthBars[i].value = maxHealth;
            armorBars[i].maxValue = maxArmor;
            armorBars[i].value = maxArmor;
        }
        armorhBarVisibilityCoroutine = HideSliderBars(armorBars,0);
        healthBarVisibilityCoroutine = HideSliderBars(healthBars, 0);
        StartCoroutine(armorhBarVisibilityCoroutine);
        StartCoroutine(healthBarVisibilityCoroutine);
    }

    public void SpawnSourcePoint(int SourcePointType)
    {
        Debug.Log("Spawned");
        GameObject sourcePoint = Instantiate(Resources.Load("Prefabs/Source") as GameObject, transform.position, Quaternion.identity);
        sourcePoint.GetComponent<SourcePoint>().OnSpawnInit(SourcePointType, GameObject.Find("Player").transform.position, 15);
        sourcePoint.GetComponent<SourcePoint>().objectToChase = GameObject.Find("Player");
    }

    public virtual void GotHit(float baseDamage, Vector3 impactPoint,Vector3 impactForce) {
        if (CurrentArmor != 0) {
            CurrentArmor -= baseDamage * 0.5f;
            return;
        }     
        StartCoroutine(OnHitDrops(minOnHitDrop,maxOnHitDrop));
        RegisterCollision(impactPoint, impactForce);
        CurrentHealth -= baseDamage;
        // StartCoroutine(OnHitPhysicsFeedBack());
    }

    public virtual void GotHitOnCritSpot(float baseDamage, Vector3 impactPoint, Vector3 impactForce)
    {
        if (CurrentArmor != 0)
        {
            CurrentArmor -= baseDamage * 0.75f;
            return;
        }
        Debug.Log(baseDamage);
      
        RegisterCollision(impactPoint, impactForce);
        StartCoroutine(OnHitDrops(minOnHitDrop*2, maxOnHitDrop*2));
        CurrentHealth -= baseDamage * 2;
    }

 
    public virtual IEnumerator OnHitDrops(int minDrop, int maxDrop) {
        for (int i = 0; i < Random.Range(minDrop, maxDrop); i++)
        {
            GameObject scrap = Instantiate(Resources.Load("Prefabs/Source") as GameObject, transform.position, Quaternion.identity);
            Vector3 randomPoistion = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
            scrap.GetComponent<SourcePoint>().OnSpawnInit(4, GameObject.Find("Player").transform.position + randomPoistion,10, 4);
            scrap.GetComponent<SourcePoint>().objectToChase = GameObject.Find("Player");
            yield return new WaitForSeconds(0.1f);
        }
        yield break;
    }

    protected virtual IEnumerator OnDeathFeedBack()
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
        this.enabled = false;
    }

    private void RegisterCollision(Vector3 impactPoint, Vector3 impactForce) {

        lastCollisionImpactforce = impactForce;
        lastCollisionPoint = impactPoint;
        //lastCollisionImpactforce = other.relativeVelocity;
        //lastCollisionPoint = other.contacts[0].point;
    }

    private void ShowSliderBars(Slider[] sliderBars)
    {
        sliderBars[0].gameObject.SetActive(true);
        sliderBars[1].gameObject.SetActive(true);
    }

    private IEnumerator HideSliderBars(Slider[] sliderBars,int hideInSecond) {
        yield return new WaitForSeconds(hideInSecond);
        sliderBars[0].gameObject.SetActive(false);
        sliderBars[1].gameObject.SetActive(false);
        yield break;
    }

    private IEnumerator LerpSliderBarValue(Slider[] sliderBars,float targetValue,float foreGroundDecreaseSpeed, float backGroundDecreaseSpeed,float updateSpeed) {

        while (!(sliderBars[0].value - 0.2f <= targetValue))
        {
            if (sliderBars[0].value != targetValue)
            {
                sliderBars[0].value = Mathf.Lerp(sliderBars[0].value, targetValue, foreGroundDecreaseSpeed);
            }
            sliderBars[1].value = Mathf.Lerp(sliderBars[1].value, sliderBars[0].value, backGroundDecreaseSpeed);
            yield return new WaitForSeconds(updateSpeed);
        }
        yield break;
    }

    public float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            currentHealth = value;
           
            if (currentHealth > maxHealth) { currentHealth = maxHealth; }
            ShowSliderBars(healthBars);
            StartCoroutine(LerpSliderBarValue(healthBars,currentHealth,0.3f,0.2f,0.05f));
            StopCoroutine(healthBarVisibilityCoroutine);
            healthBarVisibilityCoroutine = HideSliderBars(healthBars,6);
            StartCoroutine(healthBarVisibilityCoroutine);
            if (currentHealth <= 0)
            {
                StartCoroutine(OnDeathFeedBack());
            }
        }
    }
    public float CurrentArmor
    {
        get { return currentArmor; }
        set
        {
            currentArmor = value;
            if (currentArmor < 0)
            {
                currentArmor = 0;
            }
            ShowSliderBars(armorBars);
            StartCoroutine(LerpSliderBarValue(armorBars, currentArmor, 0.03f, 0.2f,0.01f));
            StopCoroutine(armorhBarVisibilityCoroutine);
            armorhBarVisibilityCoroutine = HideSliderBars(armorBars, 6);
            StartCoroutine(armorhBarVisibilityCoroutine);
        }
    }
}
