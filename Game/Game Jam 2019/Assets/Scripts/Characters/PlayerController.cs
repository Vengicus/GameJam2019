using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviorExtended
{
    private const int MAX_AMMO = 3;

    public int Health = 3;
    public int Ammo = 3;

    protected float MovementSpeed = 3;
    Dictionary<string, bool> keys = new Dictionary<string, bool>();
    
    public PlayerWeapon MirrorShield
    {
        get
        {
            return transform.GetComponentInChildren<PlayerWeapon>();
        }
    }

    private PlayerInventory inventory;
    public PlayerInventory Inventory
    {
        get
        {
            if(inventory == null)
            {
                inventory = GetComponent<PlayerInventory>();
            }
            return inventory;
        }
    }

    private LevelManager levelManager;
    public LevelManager LevelManager
    {
        get
        {
            if(levelManager == null)
            {
                levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
            }
            return levelManager;
        }
    }


    void Start()
    {
        PlayerState.InitializePlayerStates();
        // init key list
        keys.Add("white", false);
    }
    
    void FixedUpdate()
    {
        UpdatePlayerAura();
        HandlePlayerInteraction();

        // No need to run this logic if player isn't even moving
        if(PlayerState.PlayerMovementState != PlayerMovementState.Idle)
            HandlePlayerMovement();
    }

    protected void UpdatePlayerAura()
    {
        GameObject currentCenteredCollider = Physics2D.OverlapPoint(transform.position)?.gameObject;
        if (currentCenteredCollider != null && (currentCenteredCollider.tag == Tags.TILE || currentCenteredCollider.tag == Tags.RELOAD_ZONE))
        {
            LevelManager.Level.EmitPlayerAura(currentCenteredCollider.GetComponent<Tile>());
        }
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
                // Only shoot bullet once, once the player already shot allow corouting to handle when player can shoot again
                if(PlayerState.PlayerInteractionState != PlayerInteractionState.Attacking)
                    ShootBullet();
            }
        }
    }

    protected void HandlePlayerMovement()
    {
        Vector2 velocity = new Vector2(Inputs.HorizontalInput, Inputs.VerticalInput) * MovementSpeed;
        MoveObjectToNewPosition(velocity);
        
    }

    protected void ShootBullet()
    {
        if (Ammo > 0)
        {
            PlayerState.UpdatePlayerState(PlayerInteractionState.Attacking);

            GameObject playerBullet = Instantiate(FileLoader.PlayerBulletPrefab, transform.position, Quaternion.identity);
            playerBullet.tag = "PlayerProjectile";
            playerBullet.transform.parent = transform;
            BulletScript script = playerBullet.GetComponent<BulletScript>();
            script.Velocity = new Vector2(Inputs.ShootingHorizontalInput, -Inputs.ShootingVerticalInput);
            Ammo--;
        }
    }

    public void TakeDamage(int damage)
    {
        if(PlayerState.PlayerStatusState == PlayerStatusState.Fine)
        {
            PlayerState.PlayerStatusState = PlayerStatusState.Hurt;
            Health -= damage;
            if (Health <= 0)
            {
                KillPlayer();
            }
            StartCoroutine(TriggerTempInvulnerability());
        }
    }

    private void KillPlayer()
    {
        GlobalGameManager.PlayerLives--;
        if (GlobalGameManager.PlayerLives > 0)
        {
            // allow user to restart, or return to main menu, for now just auto restart
            LevelManager.LoadNewLevel();
        }
        else
        {
            // send back to menu, show game over etc
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Key")
        {
            Inventory.AddItemToInventory(FileLoader.KeyPrefab);
            GlobalGameManager.DestroyObject(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Door")
        {
            if(Inventory.HasItemInInventory(FileLoader.KeyPrefab))
            {
                Inventory.RemoveItemFromInventory(FileLoader.KeyPrefab);
                GameObject parent = collision.transform.parent.gameObject;
                parent.SetActive(false);
                StartCoroutine(StartNewLevel());
            }
            
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Tags.RELOAD_ZONE)
        {
            Ammo = MAX_AMMO;
        }
    }

    IEnumerator TriggerTempInvulnerability()
    {
        yield return new WaitForSeconds(1f);
        PlayerState.PlayerStatusState = PlayerStatusState.Fine;
    }


    IEnumerator StartNewLevel()
    {
        yield return new WaitForSeconds(1f);
        LevelManager.LoadNewLevel();
    }

}
