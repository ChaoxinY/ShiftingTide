using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBodyIsFuelMyBloodIsGasoline : MonoBehaviour {

    private void Start()
    {
       
    }
    void Update () {

        if (Input.GetKeyDown(KeyCode.Space)) {

            GetComponent<Rigidbody>().AddForce(new Vector3(0, 100, 0), ForceMode.Impulse);
        }
	}
}
