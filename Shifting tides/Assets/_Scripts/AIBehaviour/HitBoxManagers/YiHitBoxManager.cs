using UnityEngine;
using System.Collections;

public class YiHitBoxManager : HostilHitboxManager
{
    public IEnumerator HeadSlamHitBoxAnimation() {
        HostileHitbox mouthGuard = FindHostileHitBox("MouthGuard");
        HostileHitbox neckHitBox = FindHostileHitBox("NeckHitBox");
        mouthGuard.hitBoxCollider.enabled = true;
        mouthGuard.hitBoxDamage = 50f;
        neckHitBox.hitBoxCollider.enabled = true;
        neckHitBox.hitBoxDamage = 50f;
        yield return new WaitForSeconds(1.6f);
        mouthGuard.hitBoxCollider.enabled = false;
        mouthGuard.hitBoxDamage = 0;
        neckHitBox.hitBoxCollider.enabled = false;
        neckHitBox.hitBoxDamage = 0;
        yield break;
    }

}
