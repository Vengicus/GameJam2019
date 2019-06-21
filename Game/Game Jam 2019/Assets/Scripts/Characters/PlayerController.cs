using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviorExtended
{
    protected float MovementSpeed = 3;
    
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
                PlayerState.UpdatePlayerState(PlayerInteractionState.Attacking);
                MirrorShield.Shoot(new Vector2(0, 1));
            }
        }
    }

    protected void HandlePlayerMovement()
    {
        Vector2 velocity = new Vector2(Inputs.HorizontalInput, Inputs.VerticalInput) * MovementSpeed;
        
        MoveObjectToNewPosition(velocity);
    }

}
