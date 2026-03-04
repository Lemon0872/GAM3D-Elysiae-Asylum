using UnityEngine;
using UnityEngine.AI;

public class RandomDirectionMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    public float moveDistance = 5f;
    public float idleTime = 2f;

    private float timer;
    private bool grounded = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false; // tắt khi spawn trên không
    }

    void Update()
    {
        // Kiểm tra nếu đã chạm đất
        if (!grounded && Physics.Raycast(transform.position, Vector3.down, 1f))
        {
            grounded = true;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 2f, NavMesh.AllAreas))
            {
                transform.position = hit.position;
                agent.enabled = true; // bật lại NavMeshAgent
                SetRandomDirection();
            }
        }

        if (agent.enabled && agent.isOnNavMesh)
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                timer += Time.deltaTime;
                if (timer >= idleTime)
                {
                    SetRandomDirection();
                    timer = 0f;
                }
            }

            if (agent.velocity.sqrMagnitude == 0f && agent.remainingDistance > agent.stoppingDistance)
            {
                SetRandomDirection();
            }
        }
    }

    void SetRandomDirection()
    {
        if (!agent.enabled || !agent.isOnNavMesh) return;

        Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        Vector3 targetPos = transform.position + randomDir * moveDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPos, out hit, moveDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}