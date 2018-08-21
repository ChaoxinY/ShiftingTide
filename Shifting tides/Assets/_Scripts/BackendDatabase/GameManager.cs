using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private PlayerCamera plyCamera;
   
    public void Start()
    {      
        plyCamera = Camera.main.GetComponent<PlayerCamera>();
            
    }
    public void Update()
    {
        transform.rotation = Quaternion.Euler(0, plyCamera.angleH, 0);
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SceneManager.LoadScene("Prologue");
            Time.timeScale = 1;
        }
    } 
}
