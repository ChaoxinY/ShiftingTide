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
    public virtual void GotHitOnCritSpot(float basedamage)
    {
        CurrentHealth -= basedamage * 2;
    }
    public float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            currentHealth = value;
            if (currentHealth < 0) {
                //Destroy(gameObject); 
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
