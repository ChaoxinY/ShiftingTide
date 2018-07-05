using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : TimeBoundGameObject
{
    protected Rigidbody rbObject;
    protected Vector3 startVelocity;
    protected float gravity;

    protected override IEnumerator PauseOnTimeStop()
    {
        if (isTimeStopped && rbObject.velocity != Vector3.zero)
        {
            RestrictVelocity();
            yield return new WaitUntil(() => !isTimeStopped);
        }
        else if (!isTimeStopped)
        {
            if (startVelocity != Vector3.zero)
            {
                ResumeVelocity();
            }
            rbObject.AddForce(new Vector3(0, gravity, 0));
        }
    }

    protected void RestrictVelocity()
    {
        startVelocity = this.rbObject.velocity;
        rbObject.drag = 15f;
    }

    protected void ResumeVelocity()
    {
        rbObject.velocity = startVelocity;
        startVelocity = Vector3.zero;
        rbObject.drag = 0.1f;
    }
}