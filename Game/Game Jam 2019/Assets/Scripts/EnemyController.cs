using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : Vehicle
{
    protected int Health = 2;


    public abstract EnemyType EnemyType { get; }
    public abstract List<EnemyBehaviorTypes> EnemyBehavior { get; }


    /*/// <summary>
    /// Default implementation of Enemy Behavior, by default enemies will seek player, unless overridden in children
    /// </summary>
    protected virtual List<EnemyBehaviorTypes> EnemyBehaviors
    {
        get
        {
            return new List<EnemyBehaviorTypes>() { EnemyBehaviorTypes.Seeking };
        }
    }*/
    
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
        if (EnemyBehavior.Contains(EnemyBehaviorTypes.Seeking))
        {
            
            force += Seek(GlobalGameManager.PlayerObject.RigBody.position);
            //force += Arrive(GlobalGameManager.PlayerObject.RigBody.position);
        }
        else if(EnemyBehavior.Contains(EnemyBehaviorTypes.Fleeing))
        {
            force += Flee(GlobalGameManager.PlayerObject.RigBody.position);
        }

        force += AvoidObstacle(0.2f);

        ApplyForce(force);
    }

    public void DamageEnemy(int damage)
    {
        Health -= damage;
        if(Health <= 0)
        {
            KillEnemy();
        }
    }

    private void KillEnemy()
    {
        GlobalGameManager.DestroyObject(gameObject);
    }

    protected virtual void AttackPlayerClose()
    {
        PlayerController player = GlobalGameManager.PlayerObject.GetComponent<PlayerController>();
        player.TakeDamage(1);
    }

    protected virtual void AttackPlayerRanged()
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObj = collision.gameObject;
        if (collidedObj.tag == Tags.PLAYER)
        {
            AttackPlayerClose();
        }
    }
}
