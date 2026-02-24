using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public Transform player;
    private Animator animator;

    private bool hasWon;
    private NavMeshAgent agent;

    [Header("Win Behavior")]
    public string roarAnimName = "Roar";
    public AudioClip roarClip;
    public float disappearDelay = 2.5f;
    [Header("Vision Settings")]
    public Camera playerCamera;
    [Range(0.5f, 0.99f)]
    public float viewDotThreshold = 0.85f;
    public float maxViewDistance = 40f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

    }

    void LateUpdate()
    {
        if (hasWon) return;
        if (IsPlayerLookingAtMe())
            Freeze();
        else
            ChasePlayer();
    }

    bool IsPlayerLookingAtMe()
    {
        if (playerCamera == null)
        {
            Debug.LogWarning("MonsterAI: No camera assigned!");
            return false;
        }

        Vector3 camPos = playerCamera.transform.position;
        Vector3 directionToMonster = (transform.position - camPos).normalized;

        float distance = Vector3.Distance(camPos, transform.position);
        if (distance > maxViewDistance)
            return false;

        float dot = Vector3.Dot(playerCamera.transform.forward, directionToMonster);

        Debug.Log("Dot value: " + dot);

        if (dot < viewDotThreshold)
            return false;

        if (Physics.Raycast(camPos, directionToMonster, out RaycastHit hit, maxViewDistance))
        {
            Debug.Log("Raycast hit: " + hit.transform.name);

            if (hit.transform.root == transform)
            {
                Debug.Log("PLAYER IS LOOKING AT MONSTER");
                return true;
            }
        }

        return false;
    }

    void Freeze()
    {
        agent.isStopped = true;
        agent.ResetPath();
        agent.velocity = Vector3.zero;

        animator.speed = 0f; // ❄️ FREEZE animation
    }

    void ChasePlayer()
    {
        agent.isStopped = false;
        animator.speed = 1f; // ▶ Resume animation
        agent.SetDestination(player.position);
    }


    
    public AudioSource audioSource;
    public AudioClip[] footstepClips;

    public void PlayFootstep()
    {
        if (audioSource == null || footstepClips.Length == 0)
            return;

        AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
        audioSource.PlayOneShot(clip);
    }

    public void OnPlayerDied()
    {
        if (hasWon) return;
        hasWon = true;

        // Stop movement & AI
        agent.isStopped = true;
        agent.ResetPath();
        agent.velocity = Vector3.zero;

        // Play roar animation
        animator.speed = 1f;
        animator.Play(roarAnimName);

        // Play roar sound
        if (audioSource && roarClip)
            audioSource.PlayOneShot(roarClip);

        // Disappear after delay
        StartCoroutine(DisappearAfterDelay());
    }

    IEnumerator DisappearAfterDelay()
    {
        yield return new WaitForSeconds(disappearDelay);
        Destroy(gameObject);
    }
}
