using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Helper enum for PlayerMovementState
/// </summary>
public enum PlayerMovementState
{
    Idle,
    Walking
}

/// <summary>
/// Helper enum for PlayerInteractionState
/// </summary>
public enum PlayerInteractionState
{
    None,
    UsingSomething,
    Attacking,
    Reloading
}

/// <summary>
/// Helper enum for PlayerStatusState
/// </summary>
public enum PlayerStatusState
{
    Fine,
    Hurt,
    Dead
}

public static class PlayerState
{
    public static PlayerMovementState PlayerMovementState
    {
        get; set;
    }

    public static PlayerInteractionState PlayerInteractionState
    {
        get; set;
    }

    public static PlayerStatusState PlayerStatusState
    {
        get; set;
    }
    
    public static void InitializePlayerStates()
    {
        PlayerMovementState = PlayerMovementState.Idle;
        PlayerInteractionState = PlayerInteractionState.None;
        PlayerStatusState = PlayerStatusState.Fine;
    }

    /// <summary>
    /// Update Player Status state
    /// </summary>
    /// <param name="statusState"></param>
    public static void UpdatePlayerState(PlayerStatusState statusState)
    {
        UpdatePlayerState(PlayerMovementState, PlayerInteractionState, statusState);
    }

    /// <summary>
    /// Update Player Movement state
    /// </summary>
    /// <param name="movementState"></param>
    public static void UpdatePlayerState(PlayerMovementState movementState)
    {
        UpdatePlayerState(movementState, PlayerInteractionState, PlayerStatusState);
    }

    /// <summary>
    /// Update Player Interaction state
    /// </summary>
    /// <param name="interactionState"></param>
    public static void UpdatePlayerState(PlayerInteractionState interactionState)
    {
        UpdatePlayerState(PlayerMovementState, interactionState, PlayerStatusState);
    }

    /// <summary>
    /// (DO NOT CALL DIRECTLY) Handle Player state updates
    /// </summary>
    /// <param name="movementState"></param>
    /// <param name="interactionState"></param>
    /// <param name="statusState"></param>
    private static void UpdatePlayerState(PlayerMovementState movementState, PlayerInteractionState interactionState, PlayerStatusState statusState)
    {
        PlayerMovementState = movementState;        //Update movement
        PlayerInteractionState = interactionState;  //Update interaction
        PlayerStatusState = statusState;            //Update status
    }

}
