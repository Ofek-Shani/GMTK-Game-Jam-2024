using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarController : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField] Sprite selected, unselected;
    [SerializeField] Sprite[] projectileIcons;
    int currentlySelected = 0;

    float iconDimensions;

    public void ChangeSelection(int newSelect)
    {
        currentlySelected = newSelect;
        UpdateIcons();
    }

    void UpdateIcons()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Image>().sprite = i == currentlySelected ? selected : unselected;
            transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = projectileIcons[i];
            transform.GetChild(i).GetChild(0).GetComponent<Image>().color = gameManager.ammoRemaining[i] > 0 ? Color.white: Color.black;

            transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition = new Vector3(i == 0 ? 5 * iconDimensions : (i) * iconDimensions, 0, 0);
        }
    }

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        iconDimensions = transform.GetChild(0).GetComponent<RectTransform>().rect.width;
        UpdateIcons();
    }
}
