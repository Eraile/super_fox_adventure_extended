using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
using PlayFab;
using PlayFab.ClientModels;
using System;

public class FoxCharacterWinScreenLeaderboard : FoxCharacterWinScreenAddCrystals
{
    protected override void OnWin()
    {
        // Use player stats to register virtual currency increase
        FoxCharacterInventory foxCharacterInventory = this.FoxPlayer.GetComponentInChildren<FoxCharacterInventory>();
        if (foxCharacterInventory != null)
        {
            int crystalsCount = foxCharacterInventory.jewelsCount;
            float levelDuration = Time.timeSinceLevelLoad;

            // Statistic updates
            StatisticUpdate crystalsStatisticUpdate = new StatisticUpdate()
            {
                StatisticName = "level1_crystals",
                Value = crystalsCount,
            };

            StatisticUpdate speedrunStatisticUpdate = new StatisticUpdate()
            {
                StatisticName = "level1_speedrun",
                Value = Mathf.FloorToInt(levelDuration * 100.0f),
            };

            // Register leaderboard
            PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest()
            {
                Statistics = new List<StatisticUpdate>()
                {
                    crystalsStatisticUpdate,
                    speedrunStatisticUpdate,
                }
            }, this.OnUpdatePlayerStatisticsRequestSuccess, this.OnUpdatePlayerStatisticsRequestError);
        }

        // base win
        base.OnWin();
    }

    private void OnUpdatePlayerStatisticsRequestSuccess(UpdatePlayerStatisticsResult updatePlayerStatisticsResult)
    {
    }

    private void OnUpdatePlayerStatisticsRequestError(PlayFabError playFabError)
    {
        Debug.LogError("FoxCharacterWinScreenLeaderboard.OnUpdatePlayerStatisticsRequestError() - Error: " + playFabError.ToString());
    }
}
