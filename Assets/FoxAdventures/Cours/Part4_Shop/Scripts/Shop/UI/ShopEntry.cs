using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class ShopEntry : MonoBehaviour
{
    [Header("Data")]
    public CatalogItem catalogItem = null;
    public string virtualCurrencyPriceListing = "CR";

    [Header("UI Elements")]
    public Image itemSprite = null;
    public Text itemNameText = null;
    public Text itemValueText = null;
    public Image itemValueSprite = null;

    // OnEnable / OnDisable
    void OnEnable()
    {
        // Inventory is setup?
        if (PlayfabInventory.Instance != null)
        {
            // Register to events
            PlayfabInventory.Instance.OnInventoryUpdateSuccess.AddListener(this.OnInventoryUpdateSuccess);
            PlayfabInventory.Instance.OnInventoryUpdateError.AddListener(this.OnInventoryUpdateError);
        }

        //// Update view to init
        //this.UpdateView();
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

    // Update View
    public void SetValue(CatalogItem _catalogItem, string _virtualCurrencyPriceListing)
    {
        // Save catalog item
        this.catalogItem = _catalogItem;
        this.virtualCurrencyPriceListing = _virtualCurrencyPriceListing;

        // Update view
        this.UpdateView();
    }

    public void UpdateView()
    {
        // Check item is set
        if (this.catalogItem != null && string.IsNullOrWhiteSpace(this.virtualCurrencyPriceListing) == false
            && this.catalogItem.VirtualCurrencyPrices != null && this.catalogItem.VirtualCurrencyPrices.ContainsKey(this.virtualCurrencyPriceListing) == true)
        {
            // Determine some data from the catalog item itself
            bool isUnique = (this.catalogItem.Tags.Contains("unique") == true);
            bool isPossessed = false;

            // If unique & already in inventoryk, specific view
            if (isUnique == true && PlayfabInventory.Instance.Possess(this.catalogItem) == true)
            {
                // Mark as possessed
                isPossessed = true;
            }

            // Get data
            string itemImageURL = this.catalogItem.ItemImageUrl;
            string itemName = this.catalogItem.DisplayName;
            uint itemPrice = this.catalogItem.VirtualCurrencyPrices[this.virtualCurrencyPriceListing];

            // Update sprite
            if (this.itemSprite != null)
            {
                Sprite sprite = (string.IsNullOrWhiteSpace(itemImageURL) == false ? Resources.Load<Sprite>(itemImageURL) : null);
                if (sprite != null)
                    this.itemSprite.sprite = sprite;
                else
                    this.itemSprite.sprite = null;
            }

            // Update name
            if (this.itemNameText != null)
                this.itemNameText.text = itemName;

            // If we have bough the item
            if (PlayfabInventory.Instance != null && PlayfabInventory.Instance.Inventory != null)
            {
                // If already possessed,
                if (isPossessed == true)
                {
                    // Hide item price image
                    if (this.itemValueSprite != null)
                        this.itemValueSprite.gameObject.SetActive(false);

                    // Update value
                    if (this.itemValueText != null)
                    {
                        //this.itemValueText.alignment = TextAnchor.MiddleCenter;
                        this.itemValueText.text = "Owned";
                    }
                }
                else
                {
                    // Show item price image
                    if (this.itemValueSprite != null)
                        this.itemValueSprite.gameObject.SetActive(true);

                    // Update value
                    if (this.itemValueText != null)
                    {
                        //this.itemValueText.alignment = TextAnchor.MiddleLeft;
                        this.itemValueText.text = itemPrice.ToString();
                    }
                }
            }
        }
    }

    // UI Interactions
    public void OnBuyButtonClick()
    {
        this.TryBuyItem();
    }

    // Buy item
    public void TryBuyItem()
    {
        // Check item
        if (this.catalogItem == null 
            && this.catalogItem.VirtualCurrencyPrices != null 
            && this.catalogItem.VirtualCurrencyPrices.ContainsKey("CR") == true)
        {
            return;
        }

        // Determine some data from the catalog item itself
        bool isUnique = (this.catalogItem.Tags.Contains("unique") == true);
        bool isPossessed = false;

        // If already in inventory
        if (PlayfabInventory.Instance.Possess(this.catalogItem) == true)
        {
            // Mark as possessed
            isPossessed = true;
        }

        // If unique & already possessed, prevent buy
        if (isUnique == true && isPossessed == true)
        {
            Debug.LogWarning("ShopEntry.TryBuyItem() - " + this.gameObject.name + ": Prevent buy as it's unique & already possessed");
            return;
        }

        // Trigger item purchasing
        if (PlayFab.PlayFabAuthenticationAPI.IsEntityLoggedIn() == true)
        {
            PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest()
            {
                ItemId = this.catalogItem.ItemId,
                Price = (int)this.catalogItem.VirtualCurrencyPrices["CR"],
                VirtualCurrency = "CR",
            }, this.OnPurchaseItemSuccess, this.OnPurchaseItemError);
        }
    }

    private void OnPurchaseItemSuccess(PurchaseItemResult purchaseItemResult)
    {
        if (purchaseItemResult != null)
        {
            // Update inventory
            if (PlayfabInventory.Instance != null)
                PlayfabInventory.Instance.UpdateInventory();
        }
    }

    private void OnPurchaseItemError(PlayFabError playFabError)
    {
        Debug.LogError("PlayerStatsView.OnUpdateUserAccountInfosError() - Error: " + playFabError.ToString());
    }

    // Playfab Inventory events we register to
    private void OnInventoryUpdateSuccess()
    {
        this.UpdateView();
    }

    private void OnInventoryUpdateError()
    {
        this.UpdateView();
    }
}
