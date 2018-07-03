using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;


public class HostileResourceManager : MonoBehaviour
{
    public Slider[] healthBars;
    public float maxHealth;

    private Canvas uiCanvas;
    private float currentHealth;
    private IEnumerator healthBarVisibilityCoroutine;

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
        uiCanvas = GetComponentInChildren<Canvas>();
        for (int i = 0; i < healthBars.Length; i++)
        {
            healthBars[i].maxValue = maxHealth;
            healthBars[i].value = maxHealth;
        }
        healthBarVisibilityCoroutine = HideHealthBars(0);
        StartCoroutine(healthBarVisibilityCoroutine);
    }

    public virtual void GotHit(float baseDamage) {
        CurrentHealth -= baseDamage;
        StartCoroutine(OnHitDrops(1,2));
        StartCoroutine(OnHitPhysicsFeedBack());
    }

    public virtual void GotHitOnCritSpot(float baseDamage)
    {
        CurrentHealth -= baseDamage * 2;
        StartCoroutine(OnHitDrops(2,4));
        StartCoroutine(OnHitPhysicsFeedBack());
    }

    protected virtual IEnumerator OnHitPhysicsFeedBack() {

        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.GetComponent<Agent>().isTimeStoped = true;
        gameObject.GetComponent<NavMeshAgent>().updatePosition = false;
        gameObject.GetComponent<NavMeshAgent>().updateRotation = false;
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<NavMeshAgent>().updatePosition = true;
        gameObject.GetComponent<NavMeshAgent>().updateRotation = true;
        gameObject.GetComponent<Agent>().isTimeStoped = false;        
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
            yield return new WaitForSeconds(0.2f);
        }
        yield break;
    }


    public virtual IEnumerator OnDeathDrops()
    {
        OnHitDrops(3, 5);
        SpawnSourcePoint();
        yield break;
    }

    public float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            currentHealth = value;
            if (currentHealth < 0) {
                OnDeathDrops();
                //death animation
                //StartCoroutine(OnDeathPhysicsFeedBack());
                enabled = false;
                gameObject.GetComponent<Agent>().enabled = false;
            }
            if (currentHealth > maxHealth) { currentHealth = maxHealth; }
            ShowHealthBars();
            StartCoroutine(LerpHealthBarValue(currentHealth));
            StopCoroutine(healthBarVisibilityCoroutine);
            healthBarVisibilityCoroutine = HideHealthBars(6);
            StartCoroutine(healthBarVisibilityCoroutine);
        }
    }

    private void ShowHealthBars()
    {
        healthBars[0].gameObject.SetActive(true);
        healthBars[1].gameObject.SetActive(true);
    }

    private IEnumerator HideHealthBars(int hideInSecond) {
        yield return new WaitForSeconds(hideInSecond);
        healthBars[0].gameObject.SetActive(false);
        healthBars[1].gameObject.SetActive(false);
        yield break;
    }

    private IEnumerator LerpHealthBarValue(float targetValue) {

        while (!(healthBars[1].value - 0.5f <= targetValue))
        {
            if (healthBars[0].value != targetValue)
            {
                healthBars[0].value = Mathf.Lerp(healthBars[0].value, targetValue, 0.3f);
            }
            healthBars[1].value = Mathf.Lerp(healthBars[1].value, targetValue, 0.1f);
            yield return new WaitForSeconds(0.02f);
        }
        yield break;
    }
}
