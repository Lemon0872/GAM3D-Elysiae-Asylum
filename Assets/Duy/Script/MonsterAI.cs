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
        Camera cam = Camera.main;
        if (cam == null) return false;

        Vector3 viewportPoint = cam.WorldToViewportPoint(transform.position);

        // In front of camera
        if (viewportPoint.z <= 0f) return false;

        // Inside screen
        if (viewportPoint.x < 0f || viewportPoint.x > 1f ||
            viewportPoint.y < 0f || viewportPoint.y > 1f)
            return false;

        // Line of sight
        Vector3 dir = (transform.position - cam.transform.position).normalized;

        if (Physics.Raycast(cam.transform.position, dir, out RaycastHit hit, 100f))
        {
            if (hit.transform.root == transform)
                return true;
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
