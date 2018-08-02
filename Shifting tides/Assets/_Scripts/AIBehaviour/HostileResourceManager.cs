using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;


public class HostileResourceManager : MonoBehaviour
{
    public Slider[] healthBars,armorBars;
    public float maxHealth, maxArmor;

    private Canvas uiCanvas;
    private float currentHealth,currentArmor;
   
    private IEnumerator healthBarVisibilityCoroutine, armorhBarVisibilityCoroutine;

    private void Start()
    {
        Initialize();
    }

    private void LateUpdate()
    {
        uiCanvas.transform.LookAt(GameObject.Find("Player").transform);
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

    public virtual void GotHit(float baseDamage) {
        if (CurrentArmor != 0) {
            CurrentArmor -= baseDamage * 0.5f;
            return;
        }
        CurrentHealth -= baseDamage;
        StartCoroutine(OnHitDrops(1,2));
       // StartCoroutine(OnHitPhysicsFeedBack());
    }

    public virtual void GotHitOnCritSpot(float baseDamage)
    {
        if (CurrentArmor != 0)
        {
            CurrentArmor -= baseDamage * 0.75f;
            return;
        }
        CurrentHealth -= baseDamage * 2;
        StartCoroutine(OnHitDrops(2,4));
      //  StartCoroutine(OnHitPhysicsFeedBack());
    }

    protected virtual IEnumerator OnHitPhysicsFeedBack() {

        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.GetComponent<Agent>().isTimeStopped = true;
        gameObject.GetComponent<NavMeshAgent>().updatePosition = false;
        gameObject.GetComponent<NavMeshAgent>().updateRotation = false;
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<NavMeshAgent>().updatePosition = true;
        gameObject.GetComponent<NavMeshAgent>().updateRotation = true;
        gameObject.GetComponent<Agent>().isTimeStopped = false;        
        yield break; 
    }
    protected virtual IEnumerator OnDeathFeedBack()
    {
        //gameObject.GetComponent<Agent>().isTimeStoped = true;
        //gameObject.GetComponent<NavMeshAgent>().enabled = false;
        //gameObject.GetComponent<Rigidbody>().mass = 8;
        //gameObject.GetComponent<Rigidbody>().isKinematic = false;   
        yield break;
    }
    public void SpawnSourcePoint() {
        Debug.Log("Spawned");
        GameObject sourcePoint = Instantiate(Resources.Load("Prefabs/Source") as GameObject, transform.position, Quaternion.identity);
        sourcePoint.GetComponent<SourcePoint>().OnSpawnInit(4, GameObject.Find("Player").transform.position, 15);
        sourcePoint.GetComponent<SourcePoint>().objectToChase = GameObject.Find("Player");
    }

    public virtual IEnumerator OnHitDrops(int minDrop, int maxDrop) {
        for (int i = 0; i < Random.Range(minDrop, maxDrop); i++)
        {
            GameObject scrap = Instantiate(Resources.Load("Prefabs/Source") as GameObject, transform.position, Quaternion.identity);
            Vector3 randomPoistion = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
            scrap.GetComponent<SourcePoint>().OnSpawnInit(4, GameObject.Find("Player").transform.position + randomPoistion,20, 4);
            scrap.GetComponent<SourcePoint>().objectToChase = GameObject.Find("Player");
            yield return new WaitForSeconds(0.1f);
        }
        yield break;
    }


    public virtual void OnDeathDrops()
    {
        OnHitDrops(3, 5);
        SpawnSourcePoint();
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
            if (currentHealth < 0)
            {
                OnDeathDrops();
                //death animation
                //StartCoroutine(OnDeathPhysicsFeedBack());
                //Destroy(gameObject,0.3f);
            }
            if (currentHealth > maxHealth) { currentHealth = maxHealth; }
            ShowSliderBars(healthBars);
            StartCoroutine(LerpSliderBarValue(healthBars,currentHealth,0.3f,0.2f,0.05f));
            StopCoroutine(healthBarVisibilityCoroutine);
            healthBarVisibilityCoroutine = HideSliderBars(healthBars,6);
            StartCoroutine(healthBarVisibilityCoroutine);
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
