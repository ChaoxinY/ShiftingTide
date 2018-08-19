using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopZone : MonoBehaviour
{
    public List<GameObject> influencedGameObjects = new List<GameObject>();

    private void Start()
    {
        InvokeRepeating("AreaDamage", 1.4f, 0.65f);
        StartCoroutine(StopInfluencing());
    }

    private void AreaDamage()
    {
        if (!gameObject.GetComponent<SphereCollider>().enabled) {
            gameObject.GetComponent<SphereCollider>().enabled = true;
        }
        foreach (GameObject influencedGameObject in influencedGameObjects)
        {
            if (influencedGameObject.GetComponentInParent<HostileResourceManager>()&& GameObject.Find(influencedGameObject.name))
            {
                HostileResourceManager hostileResource = influencedGameObject.GetComponentInParent<HostileResourceManager>();
                hostileResource.CurrentHealth -= 2.5f;
            }
        }
    }

    private IEnumerator StopInfluencing()
    {
        yield return new WaitForSeconds(6.7f);
        gameObject.GetComponent<SphereCollider>().enabled = false;
        yield return StartCoroutine(ReturnGameObjectToNormalState());
        Destroy(transform.parent.gameObject);
        yield break;
    }

    private IEnumerator ReturnGameObjectToNormalState()
    {
        foreach (GameObject influencedGameObject in influencedGameObjects)
        {
            Debug.Log(influencedGameObject.name);
            if (GameObject.Find(influencedGameObject.name))
            {
                InfluenceGameObject(influencedGameObject);
            }
        }
        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsGameObjectinfluenceable(other.gameObject))
        {
            Debug.Log(Time.time);
            InfluenceGameObject(other.gameObject, true);
            influencedGameObjects.Add(other.gameObject);
        }
    }

    private bool IsGameObjectinfluenceable(GameObject gameObjectCaught) {

        bool validGameObject = false;

        if ((gameObjectCaught.GetComponent<TimeBoundGameObject>() || gameObjectCaught.gameObject.tag == "Enemy"
            || gameObjectCaught.gameObject.name == "Player") && gameObjectCaught.name != "CritSpot")  {
            validGameObject = true;
        }       
            return validGameObject;
    }

    private void InfluenceGameObject(GameObject gameObjectCaught,bool turnOn = false)
    {   
        if (gameObjectCaught.GetComponent<TimeBoundGameObject>())
        {
            TimeBoundGameObject tbGameObject = gameObjectCaught.GetComponent<TimeBoundGameObject>();
            tbGameObject.isTimeStopped = turnOn;       
        }
        else if (gameObjectCaught.gameObject.tag == "Enemy")
        {
            TimeBoundGameObject tbGameObject = gameObjectCaught.GetComponentInParent<TimeBoundGameObject>();
            tbGameObject.isTimeStopped = turnOn;
        }
        else if (gameObjectCaught.gameObject.name == "Player")
        {
            PlayerSkillModule playerSkillModule = gameObjectCaught.GetComponentInChildren<PlayerSkillModule>();
            playerSkillModule.isTimeStopped = turnOn;
        }    
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (GameObject influencedGameObject in influencedGameObjects)
        {
            if (influencedGameObject == other.gameObject)
            {
                if (other.gameObject.gameObject.name == "Player")
                {
                    PlayerSkillModule playerSkillModule = other.gameObject.GetComponentInChildren<PlayerSkillModule>();
                    playerSkillModule.isTimeStopped = false;
                }
            }
        }

    }
}


