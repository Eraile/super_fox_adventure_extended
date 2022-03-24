using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HigherJumpPowerup : MonoBehaviour
{
    private FoxCharacterController foxCharacterController = null;
    public FoxCharacterController FoxCharacterController
    {
        get
        {
            if (this.foxCharacterController == null)
                this.foxCharacterController = this.GetComponentInChildren<FoxCharacterController>();
            return this.foxCharacterController;
        }
    }
    // Initial status
    private float foxCharacterControllerDefaultJumpForce = 0.0f;
    //
    public string catalogItemID = string.Empty;
    public float jumpBoost = 200f;

    void OnEnable()
    {
        // Register to some variables (like initial state fo the jump)
        if (this.FoxCharacterController != null)
        {
            this.foxCharacterControllerDefaultJumpForce = this.FoxCharacterController.jumpForce;
        }

        // Inventory is setup?
        if (PlayfabInventory.Instance != null)
        {
            // Register to events
            PlayfabInventory.Instance.OnInventoryUpdateSuccess.AddListener(this.OnInventoryUpdateSuccess);
            PlayfabInventory.Instance.OnInventoryUpdateError.AddListener(this.OnInventoryUpdateError);
        }

        // Update view to init
        this.UpdatePowerupStatus();
    }

    void OnDisable()
    {
        // Inventory is setup?
        if (PlayfabInventory.Instance != null)
        {
            // Unregister to events
            PlayfabInventory.Instance.OnInventoryUpdateSuccess.RemoveListener(this.OnInventoryUpdateSuccess);
            PlayfabInventory.Instance.OnInventoryUpdateError.RemoveListener(this.OnInventoryUpdateError);
        }
    }

    private void OnInventoryUpdateSuccess()
    {
        this.UpdatePowerupStatus();
    }

    private void OnInventoryUpdateError()
    {
        this.UpdatePowerupStatus();
    }

    // Update powerup
    private bool poweredUp = false;
    private void UpdatePowerupStatus()
    {
        this.poweredUp = false;
        if (PlayfabInventory.Instance != null && PlayfabInventory.Instance.Possess(this.catalogItemID) == true)
            this.poweredUp = true;

        if (this.FoxCharacterController != null)
        {
            // If powered up, Apply
            if (this.poweredUp == true)
            {
                this.FoxCharacterController.jumpForce = this.foxCharacterControllerDefaultJumpForce + this.jumpBoost;
            }
            // Else, Revert
            else
            {
                this.FoxCharacterController.jumpForce = this.foxCharacterControllerDefaultJumpForce;
            }
        }
    }
}
