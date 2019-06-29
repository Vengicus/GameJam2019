using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviorExtended
{
    public Vector2 Velocity
    {
        get; set;
    }

    private BulletState bulletState;

    private float maxBulletDistance = 20;

    private GameObject beamStart, beam, beamEnd;

    private PlayerBullet bulletCollisionData;

    // Start is called before the first frame update
    void Start()
    {
        bulletState = BulletState.Starting;
        Quaternion lookRot = Quaternion.LookRotation(Vector3.forward, new Vector2(Velocity.y, -Velocity.x));
        beamStart = transform.GetChild(0).gameObject;
        beam = transform.GetChild(1).gameObject;
        beamEnd = transform.GetChild(2).gameObject;

        bulletCollisionData = beam.GetComponent<PlayerBullet>();
        transform.rotation = lookRot;
        beamEnd.SetActive(false);

        Vector3 dir = new Vector3(Velocity.x, Velocity.y).normalized;
        beamStart.transform.position += dir / 2;
        beam.transform.position += dir / 2;
        StartCoroutine(YieldForShooting());
    }

    // Update is called once per frame
    void Update()
    {
        float speedOfBullet = 2f;
        Vector3 scalingFactor = new Vector3(0.5f, 0) * speedOfBullet;
        Vector3 translationVelocity = new Vector3(-3.42f * Time.deltaTime, 0, 0) * speedOfBullet;
        
        if (bulletState == BulletState.Starting)
        {
            
        }
        else if(bulletState == BulletState.Shooting)
        {
            beam.transform.localScale += scalingFactor;
            
            beam.transform.Translate(translationVelocity);
            beamEnd.transform.Translate(translationVelocity * 2);
            if (bulletCollisionData.Collided || beam.transform.localScale.x >= maxBulletDistance)
            {
                PlayerState.UpdatePlayerState(PlayerInteractionState.None);
                transform.parent = null;
                bulletState = BulletState.Collided;
                beamEnd.SetActive(true);
                GlobalGameManager.DestroyObject(beamStart);
            }
        }
        else if(bulletState == BulletState.Collided)
        {
            if(beam.transform.localScale.x <= 0.1f)
            {
                StartCoroutine(DestroyBullet());
            }
            else
            {
                beam.transform.localScale -= scalingFactor;
                beam.transform.Translate(translationVelocity);
            }
        }

        //transform.Translate(Velocity * speed * Time.deltaTime);
    }
    
    
    IEnumerator YieldForShooting()
    {
        yield return new WaitForSeconds(0.25f);
        bulletState = BulletState.Shooting;
        
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(0.05f);
        GlobalGameManager.DestroyObject(gameObject);
    }
}
