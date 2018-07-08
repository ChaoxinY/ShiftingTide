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
        gameObject.GetComponent<SphereCollider>().enabled = true;
        foreach (GameObject influencedGameObject in influencedGameObjects)
        {
            if (influencedGameObject.GetComponentInParent<HostileResourceManager>())
            {
                HostileResourceManager hostileResource = influencedGameObject.GetComponentInParent<HostileResourceManager>();
                hostileResource.CurrentHealth -= 2.5f;
            }
        }
    }

    private IEnumerator StopInfluencing()
    {
        yield return new WaitForSeconds(6.7f);
        yield return StartCoroutine(ReturnGameObjectToNormalState());
        Destroy(transform.parent.gameObject);
        yield break;
    }

    private IEnumerator ReturnGameObjectToNormalState()
    {
        foreach (GameObject influencedGameObject in influencedGameObjects)
        {
            InfluenceGameObject(influencedGameObject, false);
        }
        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsGameObjectinfluenceable(other.gameObject))
        {
            InfluenceGameObject(other.gameObject, true);
            influencedGameObjects.Add(other.gameObject);
        }
    }

    private bool IsGameObjectinfluenceable(GameObject gameObjectCaught) {

        bool validGameObject = false;

        if (gameObjectCaught.GetComponent<TimeBoundGameObject>() || gameObjectCaught.gameObject.tag == "Enemy"
            || gameObjectCaught.gameObject.name == "Player") {
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


