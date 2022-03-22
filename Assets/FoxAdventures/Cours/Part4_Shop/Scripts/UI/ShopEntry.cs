using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopEntry : MonoBehaviour
{
    public Image itemSprite = null;
    public Text itemNameText = null;
    public Text itemValueText = null;

    public void SetValue(string imageURL, string itemNameText, string itemValueText)
    {
        if (this.itemSprite != null)
        {
            Sprite sprite = (string.IsNullOrWhiteSpace(imageURL) == false ? Resources.Load<Sprite>(imageURL) : null);
            if (sprite != null)
                this.itemSprite.sprite = sprite;
            else
                this.itemSprite.sprite = null;
        }

        if (this.itemNameText != null)
            this.itemNameText.text = itemNameText;
        if (this.itemValueText != null)
            this.itemValueText.text = itemValueText;
    }
}
