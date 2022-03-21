using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitGameButton : MonoBehaviour
{
    [SerializeField] private Button quitButton = null;
    public Button QuitButton
    {
        get
        {
            if (this.quitButton == null)
                this.quitButton = GetComponent<Button>();

            return this.quitButton;
        }
    }

    void OnEnable()
    {
        // Register to events
        if (this.QuitButton != null)
            this.QuitButton.onClick.AddListener(this.OnQuitGameClick);
    }

    void OnDisable()
    {
        // Register to events
        if (this.QuitButton != null)
            this.QuitButton.onClick.RemoveListener(this.OnQuitGameClick);
    }

    private void OnQuitGameClick()
    {
#if UNITY_EDITOR
        Debug.LogError("QuitGameButton.OnQuitGameClick()");
#else
        Application.Quit();
#endif
    }
}
