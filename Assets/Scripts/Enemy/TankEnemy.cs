using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEnemy : BaseEnemy
{
    public override void Initialize(float health, float speed, float damage, float coin)
    {
        base.Initialize(100, 2, 1, 10);
    }
}
