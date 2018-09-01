using UnityEngine;
using System.Collections;

public class HostileHitbox : MonoBehaviour
{
    public Collider hitBoxCollider;
    public AudioSource hitboxAudioSource;
    public bool disableUponHit = true;
    public float hitBoxDamage;

    private PlayerStatusManager playerStatusManager;

    private void Start()
    {
        hitBoxCollider = GetComponent<Collider>();
        hitboxAudioSource = GetComponent<AudioSource>();
        playerStatusManager = GameObject.Find("Player").GetComponent<PlayerStatusManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (disableUponHit)
            {
                hitBoxCollider.enabled = false;
            }
            playerStatusManager.ApplyDamage(hitBoxDamage);
            hitboxAudioSource.Play();
        }
    }
}
