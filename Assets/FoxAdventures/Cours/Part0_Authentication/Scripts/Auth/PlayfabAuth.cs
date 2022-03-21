using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public static class PlayfabAuth
{
    // Const - Save email/password
    public const string PlayfabAuthPlayerPrefsKeyUsername = "playfab_auth_username";
    public const string PlayfabAuthPlayerPrefsKeyEmail = "playfab_auth_email";
    public const string PlayfabAuthPlayerPrefsKeyPassword = "playfab_auth_password";

    // Functions
    public static void TryRegisterWithEmail(string email, string password, Action<RegisterPlayFabUserResult> registerResultCallback, Action<PlayFabError> errorCallback)
    {
        PlayfabAuth.TryRegisterWithEmail(email, password, email, registerResultCallback, errorCallback);
    }

    public static void TryRegisterWithEmail(string email, string password, string username, Action<RegisterPlayFabUserResult> registerResultCallback, Action<PlayFabError> errorCallback)
    {
        // Create request
        RegisterPlayFabUserRequest registerPlayFabUserRequest = new RegisterPlayFabUserRequest()
        {
            Email = email,
            Username = username,
            DisplayName = username,
            Password = password,
        };

        // Trigger request
        PlayFabClientAPI.RegisterPlayFabUser(registerPlayFabUserRequest, registerResultCallback, errorCallback);
    }

    public static void TryLoginWithEmail(string email, string password, Action<LoginResult> loginResultCallback, Action<PlayFabError> errorCallback)
    {
        // Create request
        LoginWithEmailAddressRequest loginWithEmailAddressRequest = new LoginWithEmailAddressRequest()
        {
            Email = email,
            Password = password,
        };

        // Trigger request
        PlayFabClientAPI.LoginWithEmailAddress(loginWithEmailAddressRequest, loginResultCallback, errorCallback);
    }
}
