using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class Shop : MonoBehaviour
{
    public string shopName = "";
    public GameObject shopEntryPrefab = null;
    //
    private List<ShopEntry> shopEntries = new List<ShopEntry>();

    // Start is called before the first frame update
    void OnEnable()
    {
        // Deactivate prefab in case
        if (this.shopEntryPrefab != null)
            this.shopEntryPrefab.gameObject.SetActive(false);

        // Refresh prefab
        this.RefreshLeaderboard();
    }

    // Refresh
    public void RefreshLeaderboard()
    {
        // Check prefab
        if (this.shopEntryPrefab == null)
            return;

        // Trigger news show if logged in
        if (PlayFab.PlayFabAuthenticationAPI.IsEntityLoggedIn() == true)
        {
            PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest()
            {
                CatalogVersion = this.shopName,
            }, this.OnGetCatalogItemsSuccess, this.OnGetCatalogItemsError);
        }
    }

    private void OnGetCatalogItemsSuccess(GetCatalogItemsResult getCatalogItemsResult)
    {
        // Clear existing entries
        this.ClearExistingEntries();

        // Go through scores
        if (getCatalogItemsResult != null && getCatalogItemsResult.Catalog != null)
        {
            for (int i = 0; i < getCatalogItemsResult.Catalog.Count; i++)
            {
                CatalogItem catalogItem = getCatalogItemsResult.Catalog[i];
                if (catalogItem != null && catalogItem.VirtualCurrencyPrices != null && catalogItem.VirtualCurrencyPrices.ContainsKey("CR") == true)
                {
                    // Instantiate object copy
                    GameObject shopEntryGameobjectCopy = GameObject.Instantiate(this.shopEntryPrefab, this.shopEntryPrefab.transform.parent);
                    if (shopEntryGameobjectCopy != null)
                    {
                        // Activate at our prefab is deactivated
                        shopEntryGameobjectCopy.gameObject.SetActive(true);

                        // set name
                        shopEntryGameobjectCopy.name = ("ShopItemEntry (" +catalogItem.DisplayName + ")");

                        // Get leaderboard entry
                        ShopEntry shopEntry = shopEntryGameobjectCopy.GetComponent<ShopEntry>();
                        if (shopEntry != null)
                        {
                            // Set value
                            shopEntry.SetValue(catalogItem, "CR");

                            // Add to list
                            if (this.shopEntries == null)
                                this.shopEntries = new List<ShopEntry>();
                            this.shopEntries.Add(shopEntry);
                        }
                        // Else, destroy object we just spawned
                        else
                        {
                            GameObject.Destroy(shopEntryGameobjectCopy);
                        }
                    }
                }
            }
        }
    }

    private void OnGetCatalogItemsError(PlayFabError playFabError)
    {
        Debug.LogError("Shop.OnGetCatalogItemsError() - Error: " + playFabError.ToString());
    }

    // Clear existing entries
    public void ClearExistingEntries()
    {
        if (this.shopEntries != null)
        {
            while (this.shopEntries.Count > 0)
            {
                if (this.shopEntries[0] != null)
                {
                    GameObject.Destroy(this.shopEntries[0].gameObject);
                }

                // Remove first entry
                this.shopEntries.RemoveAt(0);
            }
        }
    }
}
