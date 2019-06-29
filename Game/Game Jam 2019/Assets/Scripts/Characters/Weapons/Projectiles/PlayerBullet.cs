using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public bool Collided { get; set; }
    GameObject CollidedObject { get; set; }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObj = collision.gameObject;
        GameObject parent = transform.parent.gameObject;
        if (collidedObj.tag != Tags.PLAYER)
        {
            if (collidedObj.tag == Tags.WALL || collidedObj.tag == Tags.ENEMY)
            {
                CollidedObject = collidedObj;
                Collided = true;
            }
            if (collidedObj.tag == Tags.ENEMY)
            {
                collidedObj.GetComponent<EnemyController>().DamageEnemy(1);
            }
        }
        
    }
}
