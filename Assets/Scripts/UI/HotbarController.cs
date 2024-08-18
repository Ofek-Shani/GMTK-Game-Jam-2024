using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotbarController : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField] Sprite selected, unselected;
    [SerializeField] Sprite[] projectileIcons;
    int currentlySelected = 0;

    float iconDimensions;

    string[] infoTexts = new string[5]
    {
        "Delivery Comet: Collects Debris it flies through -- send this to the client!",
        "Basic Asteroid: Can destroy planets, but breaks on impact.",
        "Piercer Asteroid: Like the Basic Asteroid, but doesn't break on impact.",
        "Pusher Asteroid: Pushes planets instead of destroying them.",
        "Blaster Asteroid: Blows up when hitting a planet, pushing it away and destroying it."
    };

    // increases/decreases gap between icons in hotbar.
    const float ICON_HORIZONTAL_DISP_MULTIPLIER = 0.9f;

    public void ChangeSelection(int newSelect)
    {
        currentlySelected = newSelect;
        transform.GetChild(transform.childCount - 1).GetComponent<TMP_Text>().text = infoTexts[newSelect];
        UpdateIcons();
    }

    public void UpdateIcons()
    {
        for(int i = 0; i < transform.childCount-1; i++)
        {
            Transform icon = transform.GetChild(i);
            //border
            icon.GetComponent<Image>().sprite = i == currentlySelected ? selected : unselected;
            // projectile icon
            icon.GetChild(0).GetComponent<Image>().sprite = projectileIcons[i];
            // blot out if cannot shoot it
            icon.GetChild(0).GetComponent<Image>().color = gameManager.ammoRemaining[i] > 0 ? Color.white: Color.black;
            //position of everything
            icon.GetComponent<RectTransform>().anchoredPosition = new Vector3(
                i == 0 ? 5 * iconDimensions*ICON_HORIZONTAL_DISP_MULTIPLIER : (i) * iconDimensions*ICON_HORIZONTAL_DISP_MULTIPLIER, 
                0, 0);
            // key text
            // casting is hacky but at this point idgaf
            icon.GetChild(1).GetComponentInChildren<TMP_Text>().text = ""+i;
            // count text (if applicable)
            icon.GetChild(2).GetComponent<TMP_Text>().text = gameManager.maxAmmo[i] > 0 ? gameManager.ammoRemaining[i] + "/" + gameManager.maxAmmo[i] : "";
        }

        
    }

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        iconDimensions = transform.GetChild(0).GetComponent<RectTransform>().rect.width;
        UpdateIcons();
    }
}
