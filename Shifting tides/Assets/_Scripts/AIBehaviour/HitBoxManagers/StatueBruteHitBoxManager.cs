using UnityEngine;
using System.Collections;

public class StatueBruteHitBoxManager : HostilHitboxManager
{
    public IEnumerator SpearChargeHitBoxAnimation() {
        yield return new WaitForSeconds(1.8f);
        HostileHitbox spearHitBox = hitBoxesToManage[0];
        spearHitBox.hitBoxCollider.enabled = true;
        spearHitBox.hitBoxDamage = 30f;
        yield return new WaitForSeconds(1.8f);
        spearHitBox.hitBoxCollider.enabled = false;
        spearHitBox.hitBoxDamage = 0f;
        yield break;
    }
}
