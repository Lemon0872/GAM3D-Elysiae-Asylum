using UnityEngine;
using UnityEngine.AI;

public class RandomDirectionMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    public float moveDistance = 5f; // khoảng cách đi theo hướng
    public float idleTime = 2f;

    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetRandomDirection();
    }

    void Update()
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

        // Nếu bị kẹt
        if (agent.velocity.sqrMagnitude == 0f && agent.remainingDistance > agent.stoppingDistance)
        {
            SetRandomDirection();
        }
    }

    void SetRandomDirection()
    {
        // Tạo hướng ngẫu nhiên trên mặt phẳng XZ
        Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        Vector3 targetPos = transform.position + randomDir * moveDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPos, out hit, moveDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}
