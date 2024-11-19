using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NormalEnemy : BaseEnemy
{

    public override void Initialize(float health, float speed, float damage, float coin)
    {
        base.Initialize(100, 2, 1, 10);
    }



}
