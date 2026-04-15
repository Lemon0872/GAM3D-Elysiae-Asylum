using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class AgentNavMeshHandler : MonoBehaviour
{
    private NavMeshAgent agent;
    private Rigidbody rb;
    private bool snapped = false;

    [Header("Snap Settings")]
    public float snapRadius = 2f; // bán kính tìm NavMesh gần nhất
    public float groundCheckVelocity = 0.1f; // ngưỡng tốc độ để coi như đã chạm đất

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        // Ban đầu disable agent để không xung đột với Rigidbody khi rơi
        agent.enabled = false;
    }

    void Update()
    {
        // Khi Rigidbody gần như đứng yên (đã chạm đất)
        if (!snapped && rb.linearVelocity.magnitude < groundCheckVelocity)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, snapRadius, NavMesh.AllAreas))
            {
                // Snap prefab lên NavMesh
                transform.position = hit.position;

                // Bật lại NavMeshAgent để điều khiển di chuyển
                agent.enabled = true;
                agent.Warp(hit.position); // đảm bảo agent đồng bộ vị trí

                // Nếu muốn, có thể đặt Rigidbody thành kinematic để tránh xung đột
                rb.isKinematic = true;

                snapped = true;
            }
        }
    }
}
