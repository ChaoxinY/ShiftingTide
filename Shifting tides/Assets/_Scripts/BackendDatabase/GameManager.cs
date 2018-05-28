using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public bool isTimeStoped;
    private PlayerCamera plyCamera;
    
    public void Start()
    {      
        plyCamera = Camera.main.GetComponent<PlayerCamera>();
        isTimeStoped = false;      
    }
    public void EnableTimeStop()
    {   
        isTimeStoped = !isTimeStoped;
    }
    public void Update()
    {
        transform.rotation = Quaternion.Euler(0, plyCamera.angleH, 0);
        if (isTimeStoped) {
            if (PlayerResourcesManager.IsThereEnoughResource(1,0))
            {
                PlayerResourcesManager.SourceReserve -= Time.deltaTime*5;
            }
            else if(!PlayerResourcesManager.IsThereEnoughResource(1, 0)){
                EnableTimeStop();
            }

        }

    } 

}
