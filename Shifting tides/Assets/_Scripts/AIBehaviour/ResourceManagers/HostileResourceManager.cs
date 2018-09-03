using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;


public class HostileResourceManager : MonoBehaviour
{
    public Component[] componentstoRemoveOnDeath;
    public float maxHealth, maxArmor;
    public int minOnHitDrop, maxOnHitDrop, minOnDeathDrop, maxOnDeathDrop;

    protected Vector3 lastCollisionPoint, lastCollisionImpactforce;
    protected IEnumerator sliderbarLerpCoroutine;
    protected float currentHealth,currentArmor;
    
    private void Start()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        currentHealth = maxHealth;
        currentArmor = maxArmor;
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
            scrap.GetComponent<SourcePoint>().OnSpawnInit(4, GameObject.Find("Player").transform.position + randomPoistion,20, 4);
            scrap.GetComponent<SourcePoint>().objectToChase = GameObject.Find("Player");
            yield return new WaitForSeconds(0.1f);
        }
        yield break;
    }

    protected virtual IEnumerator OnDeathFeedBack()
    {
        yield break;
    }

    private void RegisterCollision(Vector3 impactPoint, Vector3 impactForce) {

        lastCollisionImpactforce = impactForce;
        lastCollisionPoint = impactPoint;
    }


    public virtual float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            currentHealth = value;              
        }
    }
    public virtual float CurrentArmor
    {
        get { return currentArmor; }
        set
        {
            currentArmor = value;
          
        }
    }
}
