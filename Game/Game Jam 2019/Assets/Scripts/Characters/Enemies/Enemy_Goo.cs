﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Goo : EnemyController
{
    protected override float MaxSpeed => base.MaxSpeed;

    public override EnemyType EnemyType
    {
        get
        {
            return EnemyType.Small;
        }
    }

    public override List<EnemyBehaviorTypes> EnemyBehavior
    {
        get
        {
            return new List<EnemyBehaviorTypes>() { EnemyBehaviorTypes.Seeking };
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
