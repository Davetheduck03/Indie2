using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageEnemy : BaseEnemy
{
    public override void Initialize(float health, float speed, float damage)
    {
        base.Initialize(100, 2, 1);
    }
}
