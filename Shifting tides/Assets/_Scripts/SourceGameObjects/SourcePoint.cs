using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourcePoint : StandardInteractiveGameObject
{
    public Vector3 destination;
    public GameObject objectToChase;
    public Material[] surfaceColors;
    public Material[] negativeSurfaceColors;
    public float movementSpeed;
    public int colorIndex;
    public bool preSpawned;

    private Vector3 rotationValue;
    /// <summary>
    /// addResource 0 : health , 1 : sourceReserve, 2 : jumps , 3 : dashes, 4: Scraps
    /// </summary>
    private int[] addResource = new int[5];
    private int[] addResourceValue = { 5, 10, 1, 1, 1 };
    private float immuneTime, rotationSpeed;
    //private Coroutine pickedUpCorutine;

    protected override void Initialize()
    {
        base.Initialize();
        rotationValue = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        rotationSpeed = Random.Range(1.1f, 3);
        transform.localScale = new Vector3(Random.Range(0.1f, 0.4f), Random.Range(0.1f, 0.4f), Random.Range(0.1f, 0.4f));
        meshRenderer = GetComponent<MeshRenderer>();
        if (preSpawned)
        {
            OnSpawnInit(colorIndex, transform.position, 0, colorIndex);
        }
    }

    //Using late update to avoid jittering 
    public void LateUpdate()
    {
        if (objectToChase != null)
        {
            destination = objectToChase.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * movementSpeed);
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.05f, 0.05f, 0.05f),Time.deltaTime*0.3f);
        }
    }

    /// <summary>
    /// Colorindex  0 : health , 1 : sourceReserve, 2 : jumps , 3 : dashes 4: Decay 
    /// </summary>
    /// <param name="rightBound">Max Index.</param> 
    /// <param name="destination">Something.</param>
    /// <param name="leftBound">Min Index.</param> 
    public void OnSpawnInit(int rightBound, Vector3 destination, float movementSpeed = 1, int leftBound = 0)
    {
        if (rightBound > surfaceColors.Length)
        {
            rightBound = surfaceColors.Length;
        }
        colorIndex = Random.Range(leftBound, rightBound);
        this.destination = destination;
        meshRenderer = GetComponent<MeshRenderer>();
        this.movementSpeed = movementSpeed;
        //Needs to be reworked
        if (colorIndex < 0)
        {
            addResource[colorIndex] = -addResourceValue[colorIndex];
            meshRenderer.GetComponent<MeshRenderer>().material = negativeSurfaceColors[Mathf.Abs(colorIndex)];
            return;
        }
        addResource[colorIndex] = addResourceValue[colorIndex];
        meshRenderer.GetComponent<MeshRenderer>().material = surfaceColors[colorIndex];
    }
    protected override IEnumerator LocalUpdate()
    {
        //IEnumerator way of running base code.
        yield return StartCoroutine(base.LocalUpdate());
        transform.Rotate(rotationValue * rotationSpeed);

        if (transform.position != destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * movementSpeed);
        }
        if (gameObject.layer == 8)
        {
            WearOffImmunity();
        }
    }

    private void WearOffImmunity()
    {
        immuneTime += Time.deltaTime * 20f;
        if (immuneTime >= 200f)
        {

            gameObject.layer = 10;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //if (PlayerResourcesManager.IsThisResourceAtMax(colorIndex) && PlayerResourcesManager.IsThisResourceAtMax(5))
            //{
            //    Debug.Log("Max");
            //    return;
            //}
            meshRenderer.enabled = false;
            PickedUp();
        }
    }

    private void PickedUp()
    {
        PlayerResourcesManager.Health += addResource[0];
        PlayerResourcesManager.SourceReserve += addResource[1];
        PlayerResourcesManager.JumpsLeft += addResource[2];
        PlayerResourcesManager.Dashes += addResource[3];
        PlayerResourcesManager.ScrapSource += addResource[4];
        Destroy(gameObject);
    }
}
