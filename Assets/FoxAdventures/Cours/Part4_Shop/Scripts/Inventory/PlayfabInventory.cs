using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.Events;

public class PlayfabInventory : MonoBehaviour
{
    private static PlayfabInventory instance = null;
    public static PlayfabInventory Instance
    {
        get
        {
            //if (instance == null)
            //    instance = FindObjectOfType<PlayfabInventory>();
            return instance;
        }
    }

    [Header("Inventory")]
    /// <summary>
    /// Array of inventory items belonging to the user.
    /// </summary>
    public List<ItemInstance> Inventory;
    /// <summary>
    /// Array of virtual currency balance(s) belonging to the user.
    /// </summary>
    public Dictionary<string, int> VirtualCurrency;
    /// <summary>
    /// Array of remaining times and timestamps for virtual currencies.
    /// </summary>
    public Dictionary<string, VirtualCurrencyRechargeTime> VirtualCurrencyRechargeTimes;

    [Header("Events")]
    public UnityEvent OnInventoryUpdateSuccess = new UnityEvent();
    public UnityEvent OnInventoryUpdateError = new UnityEvent();

    // Start is called before the first frame update
    void OnEnable()
    {
        // Only keep one singleton if another is already set
        if (PlayfabInventory.Instance != null && PlayfabInventory.Instance != this)
        {
            GameObject.Destroy(this.gameObject);
        }
        // No singleton set or its us
        else
        {
            // Set ourselves as the singleton
            PlayfabInventory.instance = this;

            // Keep cross scene ?
            DontDestroyOnLoad(this.gameObject);

            // Update Inventory
            this.UpdateInventory();
        }
    }

    public void UpdateInventory()
    {
        // Trigger news show if logged in
        if (PlayFab.PlayFabAuthenticationAPI.IsEntityLoggedIn() == true)
        {
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest() { }, this.OnGetUserInventorySuccess, this.OnGetUserInventoryError);
        }
        else
        {
            // Try to update inventory later on if not logged in
            this.Invoke("UpdateInventory", 1.0f);
        }
    }

    private void OnGetUserInventorySuccess(GetUserInventoryResult getUserInventoryResult)
    {
        this.Inventory = getUserInventoryResult.Inventory;
        this.VirtualCurrency = getUserInventoryResult.VirtualCurrency;
        this.VirtualCurrencyRechargeTimes = getUserInventoryResult.VirtualCurrencyRechargeTimes;

        // Callback
        if (this.OnInventoryUpdateSuccess != null)
            this.OnInventoryUpdateSuccess.Invoke();
    }

    private void OnGetUserInventoryError(PlayFabError playFabError)
    {
        Debug.LogError("PlayfabInventory.OnGetUserInventoryError() - Error: " + playFabError.ToString());

        // Callback
        if (this.OnInventoryUpdateError != null)
            this.OnInventoryUpdateError.Invoke();
    }

    // Accessor
    public bool Possess(string catalogItemID)
    {
        if (string.IsNullOrWhiteSpace(catalogItemID) == false && this.Inventory != null)
        {
            for (int i = 0; i < this.Inventory.Count; i++)
            {
                if (this.Inventory[i].ItemId == catalogItemID)
                    return true;
            }
        }
        return false;
    }

    public bool Possess(CatalogItem catalogItem)
    {
        if (catalogItem != null && this.Inventory != null)
        {
            for (int i=0; i<this.Inventory.Count; i++)
            {
                if (this.Inventory[i].ItemId == catalogItem.ItemId) 
                    return true;
            }
        }
        return false;
    }
}
