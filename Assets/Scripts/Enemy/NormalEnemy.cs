using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NormalEnemy : BaseEnemy
{
    public override void Initialize(float health, float speed, float damage)
    {
        base.Initialize(100, 1f, 1);
    }
}
