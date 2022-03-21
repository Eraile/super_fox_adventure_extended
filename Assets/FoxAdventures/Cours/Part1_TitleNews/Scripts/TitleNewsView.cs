using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class TitleNewsView : MonoBehaviour
{
    // Content root
    public Transform contentRoot = null;

    // Content UI
    public Text contentText = null;

    // Show on
    public bool automaticShowLatestNews = false;

    void OnEnable()
    {
        // Hide by default
        this.HideView();

        // Show latest news
        if (this.automaticShowLatestNews == true)
            this.ShowLatestNews();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            this.HideView();
        }
    }

    public void ShowLatestNews()
    {
        // Trigger news show if logged in
        if (PlayFab.PlayFabAuthenticationAPI.IsEntityLoggedIn() == true)
        {
            GetTitleNewsRequest getTitleNewsRequest = new GetTitleNewsRequest();
            PlayFabClientAPI.GetTitleNews(getTitleNewsRequest, this.OnGetTitleNewsSuccess, this.OnGetTitleNewsError);
        }
    }

    private void OnGetTitleNewsSuccess(GetTitleNewsResult getTitleNewsResult)
    {
        // News found
        if (getTitleNewsResult != null && getTitleNewsResult.News != null && getTitleNewsResult.News.Count > 0)
        {
            // Update Content
            if (this.contentText != null)
            {
                string newsContent = string.Empty;
                for (int i = 0; i < getTitleNewsResult.News.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(newsContent) == false)
                        newsContent += "\n\n";

                    // Fill content with our news
                    newsContent += "- " + getTitleNewsResult.News[i].Timestamp.ToLongDateString() + " -";
                    newsContent += "\n<color=orange>" + getTitleNewsResult.News[i].Title + "</color>";
                    newsContent += "\n" + getTitleNewsResult.News[i].Body;
                }

                // Update view
                this.contentText.text = newsContent;
            }

            // Show
            this.ShowView();
        }
        // No news
        else
        {
            // Hide view immediately
            this.HideView();
        }
    }

    private void OnGetTitleNewsError(PlayFabError playFabError)
    {
        // Log
        Debug.LogError("TitleNewsView.OnGetTitleNewsError() - Error: " + playFabError.ToString());

        // Hide view immediately
        this.HideView();
    }


    // Show / Hide content root
    public void ShowView()
    {
        if (this.contentRoot != null)
            this.contentRoot.gameObject.SetActive(true);
    }

    public void HideView()
    {
        if (this.contentRoot != null)
            this.contentRoot.gameObject.SetActive(false);
    }
}
