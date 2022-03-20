using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FoxCharacterWinScreen : MonoBehaviour
{
    // Health component of player
    [SerializeField] private FoxPlayer foxPlayer = null;
    public FoxPlayer FoxPlayer
    {
        get
        {
            if (this.foxPlayer == null)
                this.foxPlayer = this.gameObject.GetComponentInParent<FoxPlayer>();
            return this.foxPlayer;
        }
    }

    // Root of the view
    [SerializeField] private Transform winScreenRoot = null;

    // View
    public Text levelResultsText = null;

    void Awake()
    {
        // Register to health events
        if (this.FoxPlayer != null)
        {
            this.FoxPlayer.OnWin.AddListener(this.OnWin);
        }

        // Hide
        if (this.winScreenRoot != null)
            this.winScreenRoot.gameObject.SetActive(false);
    }

    // UI Events
    public void OnWinButtonClicked()
    {
        // Go back to Menu (first scene?)
        SceneManager.LoadScene(0);
    }

    // Events
    public void OnWin()
    {
        // Update view
        if (this.levelResultsText != null && this.FoxPlayer != null)
        {
            FoxCharacterInventory foxCharacterInventory = this.FoxPlayer.GetComponentInChildren<FoxCharacterInventory>();
            if (foxCharacterInventory != null)
            {
                this.levelResultsText.text = "You collected:\n"+ 
                                             foxCharacterInventory.jewelsCount.ToString() + " jewels " +
                                             "in " + Time.timeSinceLevelLoad.ToString("0.00") + " seconds";
            }
            else
            {
                this.levelResultsText.text = string.Empty;
            }
        }

        // Show view
        if (this.winScreenRoot != null)
            this.winScreenRoot.gameObject.SetActive(true);
    }
}
