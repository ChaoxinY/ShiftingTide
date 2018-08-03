﻿using UnityEngine;
using System.Collections;


public class Item : MonoBehaviour
{
    public int itemInformationID;
    public ItemInformation itemInformation;

    private Rigidbody itemRigidBody;
    private Vector3 rotationValue, displayPosition;
    private ItemDataBase itemDataBase;
    private float rotationSpeed;
    private bool isOnGround;

    public void Start()
    {
        itemRigidBody = GetComponent<Rigidbody>();
        itemDataBase = GameObject.Find("GameManager").GetComponent<ItemDataBase>();
        itemInformation = itemDataBase.availableItems[itemInformationID];
        rotationValue = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        rotationSpeed = Random.Range(1.1f, 3);
    }

    public void Update()
    {
        if (isOnGround)
        {
            transform.Rotate(rotationValue * rotationSpeed);
            if (transform.position != displayPosition)
                transform.position = Vector3.Lerp(transform.position, displayPosition, Time.deltaTime);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Invoke("PlayOnGroundAnimation", 0.5f);
        }
        else if (collision.gameObject.tag == "Player")
        {   
            PlayerInventoryManager.AddItemToInventory(this);
            Destroy(gameObject);
        }
    }

    private void PlayOnGroundAnimation()
    {
        itemRigidBody.isKinematic = true;
        displayPosition = transform.position + Vector3.up * 2f;
    }
}
[System.Serializable]
public class ItemInformation {
    public string itemName;
}