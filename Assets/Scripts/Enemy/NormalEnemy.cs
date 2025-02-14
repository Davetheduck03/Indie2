using System.Collections;
using UnityEngine;

public class NormalEnemy : BaseEnemy
{
    public override void Initialize(float health, float speed, float damage)
    {
        base.Initialize(100, 1f, 1);
    }

    private void FixedUpdate()
    {
       
    }

    
}
