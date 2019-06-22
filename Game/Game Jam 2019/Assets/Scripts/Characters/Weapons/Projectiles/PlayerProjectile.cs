using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviorExtended
{
    public GameObject ProjectileTrail;
    public Sprite ProjectileCollisionEffect;

    private float ShotSpeed { get; set; }
    private Vector2 ShotDirection { get; set; }

    private float timeCounter = 0;
    private float trailTime = 10;

    // Call on instantiate 
    public void Initialize(Vector2 direction, float speed)
    {
        ShotDirection = direction;
        ShotSpeed = speed;
    }

    private void Update()
    {
        timeCounter += Time.deltaTime;
        if (timeCounter >= trailTime)
        {
            timeCounter = 0;
            GameObject f = Instantiate(ProjectileTrail, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1), Quaternion.identity) as GameObject;
            SpriteRenderer faderSprite = f.GetComponent<SpriteRenderer>();
            //faderSprite.sprite = mySprite.sprite;
        }
    }
}
