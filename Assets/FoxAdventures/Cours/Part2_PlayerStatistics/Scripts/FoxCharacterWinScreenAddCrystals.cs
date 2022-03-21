using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
using PlayFab;
using PlayFab.ClientModels;
using System;

public class FoxCharacterWinScreenAddCrystals : FoxCharacterWinScreen
{
    protected override void OnWin()
    {
        // Use player stats to register virtual currency increase
        FoxCharacterInventory foxCharacterInventory = this.FoxPlayer.GetComponentInChildren<FoxCharacterInventory>();
        if (foxCharacterInventory != null)
        {
            int crystalsCount = foxCharacterInventory.jewelsCount;
            float levelDuration = Time.timeSinceLevelLoad;
            //
            PlayFabClientAPI.AddUserVirtualCurrency(new AddUserVirtualCurrencyRequest()
            {
                VirtualCurrency = "CR",
                Amount = crystalsCount,
            }, this.OnAddUserVirtualCurrencySuccess, this.OnAddUserVirtualCurrencyError);
        }

        // Base call
        base.OnWin();
    }

    private void OnAddUserVirtualCurrencySuccess(ModifyUserVirtualCurrencyResult modifyUserVirtualCurrencyResult)
    {
        Debug.LogWarning("FoxCharacterWinScreenAddCrystals.OnAddUserVirtualCurrencySuccess() - Success: " + modifyUserVirtualCurrencyResult.ToString());
    }

    private void OnAddUserVirtualCurrencyError(PlayFabError playFabError)
    {
        Debug.LogError("FoxCharacterWinScreenAddCrystals.OnAddUserVirtualCurrencyError() - Error: " + playFabError.ToString());
    }
}
