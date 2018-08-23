using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftingTide : MonoBehaviour
{
    public float growSpeed, maxSize;
    public Transform meshTransform;

    private int rightBound;
   
    void Update()
    {
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = Mathf.Lerp(sphereCollider.radius,maxSize/2, Time.deltaTime * growSpeed);
        meshTransform.localScale = Vector3.Lerp(meshTransform.localScale, Vector3.one * maxSize, Time.deltaTime * growSpeed);

        if (sphereCollider.radius >= Mathf.Abs(maxSize/2-100f))
        {
            Destroy(gameObject);
        }
    }

    public void Init(float growSpeed, float maxSize, int changeRangeRightBound)
    {
        rightBound = changeRangeRightBound;
        this.maxSize = maxSize;
        this.growSpeed = growSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "SourcePoint") {
            collision.gameObject.GetComponent<SourcePoint>().OnSpawnInit(rightBound, collision.gameObject.transform.position);
            collision.gameObject.layer = 8;
        }
    }
}
