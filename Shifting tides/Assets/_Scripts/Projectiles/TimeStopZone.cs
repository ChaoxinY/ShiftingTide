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
 
        influencedGameObjects = UpdateInfluenceList();
        foreach (GameObject influencedGameObject in influencedGameObjects)
        {   
            if (influencedGameObject.GetComponent<HostileResourceManager>()&&
                influencedGameObject.GetComponent<HostileResourceManager>().enabled == true)
            {
                HostileResourceManager hostileResource = influencedGameObject.GetComponent<HostileResourceManager>();
                hostileResource.CurrentHealth -= 2.5f;
            }
        }
        influencedGameObjects = UpdateInfluenceList();
    }

    private IEnumerator StopInfluencing()
    {
        yield return new WaitForSeconds(6.7f);
        gameObject.GetComponent<SphereCollider>().enabled = false;
        influencedGameObjects = UpdateInfluenceList();
        yield return StartCoroutine(ReturnGameObjectToNormalState());
        Destroy(transform.parent.gameObject);
        yield break;
    }

    private List<GameObject> UpdateInfluenceList() {
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(transform.position, 3.2f, Vector3.up, 0.1f);
        List<GameObject> gameObjectHit = new List<GameObject>();
        foreach (RaycastHit hit in hits)
        {
            if (IsGameObjectinfluenceable(hit.transform.gameObject))
            {
                gameObjectHit.Add(hit.transform.gameObject);               
            }
        }         
         return gameObjectHit;
    }

    private IEnumerator ReturnGameObjectToNormalState()
    {
        foreach (GameObject influencedGameObject in influencedGameObjects)
        {
            if (GameObject.Find(influencedGameObject.name))
            {
                InfluenceGameObject(influencedGameObject);
            }
        }
        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (IsGameObjectinfluenceable(other.gameObject))
        {
            InfluenceGameObject(other.gameObject, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsGameObjectinfluenceable(other.gameObject))
        {
            InfluenceGameObject(other.gameObject);
        }
    }

    private bool IsGameObjectinfluenceable(GameObject gameObjectCaught) {

        bool validGameObject = false;

        if ((gameObjectCaught.GetComponent<TimeBoundGameObject>() || gameObjectCaught.gameObject.name == "Player") )  {
            validGameObject = true;
        }       
            return validGameObject;
    }

    private void InfluenceGameObject(GameObject gameObjectCaught,bool turnOn = false)
    {                
        if (gameObjectCaught.gameObject.name == "Player")
        {
            PlayerSkillModule playerSkillModule = gameObjectCaught.GetComponentInChildren<PlayerSkillModule>();
            playerSkillModule.isTimeStopped = turnOn;
            return;
        }
        TimeBoundGameObject tbGameObject = gameObjectCaught.GetComponent<TimeBoundGameObject>();
        tbGameObject.isTimeStopped = turnOn;
    }

  
}


