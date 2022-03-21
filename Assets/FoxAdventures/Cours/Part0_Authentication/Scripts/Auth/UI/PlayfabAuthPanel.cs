using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfabAuthPanel : MonoBehaviour
{

    void Start()
    {
        //// Hide all
        //this.HideAll();
    }

    void OnEnable()
    {
        if (PlayFab.PlayFabAuthenticationAPI.IsEntityLoggedIn() == false)
        {
            // Show Login
            this.ShowLogin();
        }
        else
        {
            this.HideAll();
        }
    }

    // Show / Hide all
    public void ShowAll()
    {
        this.gameObject.SetActive(true);
    }
    public void HideAll()
    {
        this.gameObject.SetActive(false);
    }

    // Login
    public PlayfabAuthPanelViewLogin loginView = null;
    // Register
    public PlayfabAuthPanelViewRegister registerView = null;
    //
    private bool loginShown = true;

    // Views
    public void ShowLogin()
    {
        this.loginShown = true;
        this.ReorderTabs();
    }
    public void ShowRegistration()
    {
        this.loginShown = false;
        this.ReorderTabs();
    }

    // Reorder
    public void ReorderTabs()
    {
        if (this.loginShown == false)
        {
            this.loginView.transform.SetSiblingIndex(0);
            this.registerView.transform.SetSiblingIndex(1);
        }
        else
        {
            this.registerView.transform.SetSiblingIndex(0);
            this.loginView.transform.SetSiblingIndex(1);
        }
    }
}
