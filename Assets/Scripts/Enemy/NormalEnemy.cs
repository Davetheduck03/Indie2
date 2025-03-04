using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NormalEnemy : BaseEnemy
{
    public Player player;
    NavMeshAgent agent;
    public override void Initialize(float health, float speed, float damage)
    {
        base.Initialize(100, 1f, 1);
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        agent.SetDestination(player.transform.localPosition);
    }
}
