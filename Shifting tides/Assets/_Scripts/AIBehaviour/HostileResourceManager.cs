using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HostileResourceManager : MonoBehaviour
{
    public Slider[] healthBars;
    public float maxHealth;
    private Canvas uiCanvas;
    private float currentHealth;

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
    }

    public virtual void GotHit(float baseDamage) {
        CurrentHealth -= baseDamage;
        StartCoroutine(OnHitDrops(1,2));
    }

    public virtual void GotHitOnCritSpot(float baseDamage)
    {
        CurrentHealth -= baseDamage * 2;
        StartCoroutine(OnHitDrops(2,4));
        SpawnSourcePoint();
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
                Destroy(gameObject,0.5f);
                this.enabled = false;
            }
            if (currentHealth > maxHealth) { currentHealth = maxHealth; }
            StartCoroutine(LerpHealthBarValue(currentHealth));
        }
    }

    private IEnumerator LerpHealthBarValue(float targetValue) {

        while (!(healthBars[1].value + 0.5f >= targetValue) || !(healthBars[1].value - 0.5f <= targetValue))
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
