using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerDeath : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private MonoBehaviour movementScript;
    [SerializeField] private CharacterController characterController;

    private Rigidbody[] ragdollBodies;
    private bool isDead;
    public AudioClip[] DeathClip;
    private AudioSource audioSource;

    [Header("Death Settings")]
    public float monsterKillRange = 3f;
    public string monsterTag = "Monster";

    void Awake()
    {
        // Cache ragdoll rigidbodies (children only)
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        audioSource = GetComponentInChildren<AudioSource>();
    }

    [System.Obsolete]
    void Update()
    /*{
        if (!isDead && Keyboard.current.oKey.wasPressedThisFrame)
        {
            Die();
            
        }
    }*/
    {
        if (isDead) return;

        // TEST KEY
        if (Keyboard.current.oKey.wasPressedThisFrame)
        {
            Die();
            return;
        }

        CheckMonsterDistance();
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        // 1️⃣ Disable gameplay systems FIRST
        if (movementScript) movementScript.enabled = false;
        if (characterController) characterController.enabled = false;
        if (animator) animator.enabled = false;

        // 2️⃣ Stabilize ragdoll
        //StartCoroutine(StabilizeRagdoll());
        EnableRagdoll();
        PlayDeathSound();
        FindFirstObjectByType<MonsterAI>()?.OnPlayerDied();
    }

    void CheckMonsterDistance()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag(monsterTag);

        foreach (GameObject monster in monsters)
        {
            float sqrDist = (monster.transform.position - transform.position).sqrMagnitude;

            if (sqrDist <= monsterKillRange * monsterKillRange)
            {
                Die();
                return;
            }
        }
    }

    /*IEnumerator StabilizeRagdoll()
    {
        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.detectCollisions = false; // 👈 critical
        }

        // Wait one physics frame
        yield return new WaitForFixedUpdate();

        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.detectCollisions = true;
        }
    }*/

    void EnableRagdoll()
    {
        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
            rb.useGravity = true;
        }
    }



    public void PlayDeathSound()
    {
        if (audioSource == null || DeathClip.Length == 0)
            return;

        AudioClip clip = DeathClip[Random.Range(0, DeathClip.Length)];
        audioSource.PlayOneShot(clip);
    }
}
