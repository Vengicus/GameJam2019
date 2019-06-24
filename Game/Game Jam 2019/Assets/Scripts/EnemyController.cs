using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Vehicle
{
    

    /// <summary>
    /// Default implementation of Enemy Behavior, by default enemies will seek player, unless overridden in children
    /// </summary>
    protected virtual List<EnemyBehaviorTypes> EnemyBehaviors
    {
        get
        {
            return new List<EnemyBehaviorTypes>() { EnemyBehaviorTypes.Seeking };
        }
    }
    
    protected override void Start()
    {
        base.Start();
    }
    
    protected override void Update()
    {
        base.Update();
    }
    
    protected override void CalcSteeringForces()
    {
        Vector2 force = Vector2.zero;

        //force += PathFollow(GameObject.Find("PathNodes"));
        //force += StayNearPatrolCenter(GameObject.Find("PathNodes"), 2);
        if (EnemyBehaviors.Contains(EnemyBehaviorTypes.Seeking))
        {
            
            force += Seek(GlobalGameManager.PlayerObject.RigBody.position);
            //force += Arrive(GlobalGameManager.PlayerObject.RigBody.position);
        }
        else if(EnemyBehaviors.Contains(EnemyBehaviorTypes.Fleeing))
        {
            force += Flee(GlobalGameManager.PlayerObject.RigBody.position);
        }

        force += AvoidObstacle(1);

        ApplyForce(force);
    }
}
