using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine.UI;

public class PlayerStatsView : MonoBehaviour
{
    // Content root
    public Transform contentRoot = null;

    // Content UI
    public Text usernameText = null;
    //
    public Text crystalsCountText = null;
    public Image crystalsIcon = null;

    void OnEnable()
    {
        // Hide
        this.Hide();

        // Trigger news show if logged in
        if (PlayFab.PlayFabAuthenticationAPI.IsEntityLoggedIn() == true)
        {
            // Trigger immediate update
            this.UpdateView();
        }
    }

    // Update View
    public void UpdateView()
    {
        this.UpdateUserAccountInfos();
        this.UpdateUserStats();
    }

    // Show / Hide
    void Show()
    {
        if (this.contentRoot != null)
            this.contentRoot.gameObject.SetActive(true);
    }

    void Hide()
    {
        if (this.contentRoot != null)
            this.contentRoot.gameObject.SetActive(false);
    }

    // Update Account Infos
    private void UpdateUserAccountInfos()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), this.OnUpdateUserAccountInfosSuccess, this.OnUpdateUserAccountInfosError);
    }

    private void OnUpdateUserAccountInfosSuccess(GetAccountInfoResult getAccountInfoResult)
    {
        // Update user
        if (this.usernameText != null)
            this.usernameText.text = getAccountInfoResult.AccountInfo.Username;

        // Show
        this.Show();
    }

    private void OnUpdateUserAccountInfosError(PlayFabError playFabError)
    {
        Debug.LogError("PlayerStatsView.OnUpdateUserAccountInfosError() - Error: " + playFabError.ToString());
    }

    // Update User Stats
    private void UpdateUserStats()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), this.OnGetUserInventorySuccess, this.OnGetUserInventoryError);
    }

    private void OnGetUserInventorySuccess(GetUserInventoryResult getUserInventoryResult)
    {
        int crystalsCount = 0;

        // Get crystals from data
        if (getUserInventoryResult != null && getUserInventoryResult.VirtualCurrency != null && getUserInventoryResult.VirtualCurrency.ContainsKey("CR") == true)
        {
            crystalsCount = getUserInventoryResult.VirtualCurrency["CR"];
        }

        // Update crystals count
        {
            if (this.crystalsCountText != null)
            {
                this.crystalsCountText.gameObject.SetActive(true);
                this.crystalsCountText.text = crystalsCount.ToString();
            }

            if (this.crystalsIcon != null)
                this.crystalsIcon.gameObject.SetActive(true);
        }

        // Show
        this.Show();
    }

    private void OnGetUserInventoryError(PlayFabError playFabError)
    {
        // Log
        Debug.LogError("PlayerStatsView.OnGetUserInventoryError() - Error: " + playFabError.ToString());


        // Update crystals count
        {
            if (this.crystalsCountText != null)
            {
                this.crystalsCountText.gameObject.SetActive(true);
                this.crystalsCountText.text = "???";
            }

            if (this.crystalsIcon != null)
                this.crystalsIcon.gameObject.SetActive(true);
        }
    }
}
