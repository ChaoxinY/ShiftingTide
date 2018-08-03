using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private PlayerCamera plyCamera;
   
    public void Start()
    {      
        plyCamera = Camera.main.GetComponent<PlayerCamera>();
            
    }
    public void Update()
    {
        transform.rotation = Quaternion.Euler(0, plyCamera.angleH, 0);
    } 
}
