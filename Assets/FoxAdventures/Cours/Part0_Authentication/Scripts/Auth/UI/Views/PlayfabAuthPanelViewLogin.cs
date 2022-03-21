using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class PlayfabAuthPanelViewLogin : PlayfabAuthPanelView
{
    [Header("Login View")]
    [SerializeField] protected InputField inputFieldEmail = null;
    [SerializeField] protected InputField inputFieldPassword = null;
    [SerializeField] protected Toggle toggleRemember = null;

    // Editor only
#if UNITY_EDITOR
    [Header("Editor only")]
    public bool automaticLogin = false;
#endif


    void OnEnable()
    {
        // Load previously saved data
        {
            if (PlayerPrefs.HasKey(PlayfabAuth.PlayfabAuthPlayerPrefsKeyEmail) == true)
            {
                if (this.inputFieldEmail != null)
                    this.inputFieldEmail.text = PlayerPrefs.GetString(PlayfabAuth.PlayfabAuthPlayerPrefsKeyEmail);
            }

            if (PlayerPrefs.HasKey(PlayfabAuth.PlayfabAuthPlayerPrefsKeyPassword) == true)
            {
                if (this.inputFieldPassword != null)
                    this.inputFieldPassword.text = PlayerPrefs.GetString(PlayfabAuth.PlayfabAuthPlayerPrefsKeyPassword);
            }
        }

#if UNITY_EDITOR
        this.TryLogin();
#endif
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) == true)
            this.TryLogin();
    }

    public void OnLoginButtonClicked()
    {
        this.TryLogin();
    }

    private void TryLogin()
    {
        // Check setup
        if (this.inputFieldEmail == null || this.inputFieldPassword == null)
            return;

        // Remember ?
        bool remember = (this.toggleRemember != null ? this.toggleRemember.isOn : false);

        // Get input
        string email = this.inputFieldEmail.text;
        string password = this.inputFieldPassword.text;

        // Check input
        if (string.IsNullOrWhiteSpace(email) == false && string.IsNullOrWhiteSpace(password) == false)
        {
            // Save Data
            if (remember == true)
            {
                PlayerPrefs.SetString(PlayfabAuth.PlayfabAuthPlayerPrefsKeyEmail, email);
                PlayerPrefs.SetString(PlayfabAuth.PlayfabAuthPlayerPrefsKeyPassword, password);
                PlayerPrefs.Save();
            }

            // Call API
            PlayfabAuth.TryLoginWithEmail(email, password, this.OnLoginSuccess, this.OnLoginError);
        }
    }

    private void OnLoginSuccess(LoginResult loginResult)
    {
        Debug.LogWarning("PlayfabAuthPanelViewLogin.OnLoginSuccess() - " + loginResult.ToString());

        // Hide auth panel
        if (this.PlayfabAuthPanel != null)
            this.PlayfabAuthPanel.HideAll();
    }

    private void OnLoginError(PlayFabError loginError)
    {
        Debug.LogError("PlayfabAuthPanelViewLogin.OnLoginError() - Error: " + loginError.ToString());
        if (loginError != null && (loginError.Error == PlayFabErrorCode.AccountNotFound || loginError.ErrorMessage.Equals("User not found") == true))
        {
            // Show registration view
            if (this.PlayfabAuthPanel != null)
                this.PlayfabAuthPanel.ShowRegistration();
        }
    }
}
