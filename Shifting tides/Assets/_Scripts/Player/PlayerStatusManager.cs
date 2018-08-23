using UnityEngine;
using System.Collections;

public class PlayerStatusManager : MonoBehaviour
{
    private PlayerModuleManager playerModuleManager;
    private bool Invulnerable, Staggered, cameraShake;

    private void Start()
    {
        playerModuleManager = GetComponent<PlayerModuleManager>();  
    }

    private void LateUpdate()
    {
        if (cameraShake)
        {
            cameraShake = false;
            StartCoroutine(DoShake(1, 0.2f, 0.2f));
        }
    }

    public void ApplyDamage(float damage)
    {
        if (Invulnerable)
        {
            return;
        }
        else
        {
            PlayerResourcesManager.Health -= damage;
            cameraShake = true;
            StartCoroutine(ApplyInvulnerbility(0.5f));
        }
    }

    private IEnumerator DoShake(int timesToShake, float shakeInterval, float shakeAmout)
    {
      
      
        Vector3 camPos = Camera.main.transform.position;
        for (int i = 0; i < timesToShake; i++)
        {
            float shakeAmtX = Random.Range(-shakeAmout, shakeAmout);
            float shakeAmtY = Random.Range(-shakeAmout, shakeAmout);
            camPos.x += shakeAmtX;
            camPos.y += shakeAmtY;
            Camera.main.transform.position = camPos;
            yield return new WaitForSeconds(shakeInterval);
        }
        yield break;
    }

    private IEnumerator ApplyInvulnerbility(float seconds)
    {
        Invulnerable = true;
        yield return new WaitForSeconds(seconds);
        Invulnerable = false;
    }
}
