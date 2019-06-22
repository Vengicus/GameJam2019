using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviorExtended
{
    protected float MovementSpeed = 3;
    public GameObject bullet;
    
    public PlayerWeapon MirrorShield
    {
        get
        {
            return transform.GetComponentInChildren<PlayerWeapon>();
        }
    }


    void Start()
    {
        PlayerState.InitializePlayerStates();
    }
    
    void FixedUpdate()
    {
        HandlePlayerInteraction();

        // No need to run this logic if player isn't even moving
        if(PlayerState.PlayerMovementState != PlayerMovementState.Idle)
            HandlePlayerMovement();
    }

    protected void HandlePlayerInteraction()
    {
        // Player hasn't pressed anything
        if (!Input.anyKey)
        {
            PlayerState.UpdatePlayerState(PlayerMovementState.Idle);
            //PlayerState.UpdatePlayerState(PlayerInteractionState.None);
        }
        // Player pressed something
        else
        {
            if (Inputs.HorizontalInput != 0 || Inputs.VerticalInput != 0)
            {
                PlayerState.UpdatePlayerState(PlayerMovementState.Walking);
            }
            // Button 1
            if (Inputs.ShootingHorizontalInput != 0 || Inputs.ShootingVerticalInput != 0)
            {
                // IDEA: for more than one bullet, set a timer
                // timer is 0 by default. on fire, set timer to a number
                // each frame, subtract time. when timer is 0 AND bullets < MAX COUNT you can fire again
                Vector2 bulletPos = transform.position;
                // generate bullets away from character sprite
                bulletPos.x += 1.25f * Inputs.ShootingHorizontalInput;
                bulletPos.y += 1.25f * -1f * Inputs.ShootingVerticalInput;
                // reduces distance for diagonals
                if (Inputs.ShootingHorizontalInput != 0 && Inputs.ShootingVerticalInput != 0)
                {
                    bulletPos.x -= .5f * Inputs.ShootingHorizontalInput;
                    bulletPos.y -= .5f * -1f * Inputs.ShootingVerticalInput;
                }
                PlayerState.UpdatePlayerState(PlayerInteractionState.Attacking);
                // Generate bullet
                if (GameObject.FindGameObjectsWithTag("PlayerProjectile").Length < 1) {
                    GameObject playerBullet = (GameObject)Instantiate(bullet, bulletPos, Quaternion.identity);
                    playerBullet.tag = "PlayerProjectile";
                    playerBullet.GetComponent<BulletScript>().speedX = (Inputs.ShootingHorizontalInput * 0.5f);
                    playerBullet.GetComponent<BulletScript>().speedY = (Inputs.ShootingVerticalInput * -0.5f);
                }
            }
        }
    }

    protected void HandlePlayerMovement()
    {
        Vector2 velocity = new Vector2(Inputs.HorizontalInput, Inputs.VerticalInput) * MovementSpeed;
        
        MoveObjectToNewPosition(velocity);
    }

}
