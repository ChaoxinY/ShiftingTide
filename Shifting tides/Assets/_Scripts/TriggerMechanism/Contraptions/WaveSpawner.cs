using UnityEngine;
using System.Collections;

public class WaveSpawner : TriggerBoundMechanism
{
    public GameObject[] objectsToSpawn;
    public Transform[] spawnLocations;
    public float[] waveIntervals;

    public override void OnTriggerFunction()
    {
        GetComponent<Collider>().enabled = false;
        StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWaves() {

        for (int i = 0; i < objectsToSpawn.Length; i++) {
            Instantiate(objectsToSpawn[i], spawnLocations[i]);
            Debug.Log("Spawned");            
            yield return new WaitForSeconds(waveIntervals[i]);
            this.enabled = false;
        }      
        yield break ;
    }
}