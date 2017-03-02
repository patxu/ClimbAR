using System;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{

    // declare menu items here
    public Dictionary<string, GameObject> menuItems = new Dictionary<string, GameObject>()
    {
        { SceneUtils.SceneNames.rocManGamePlay, null },
        { SceneUtils.SceneNames.musicGame, null },
        { SceneUtils.SceneNames.exampleGame, null },
    };

    private GameObject[] holds;
    public static Sprite customHoldSprite0;
    public static Sprite customHoldSprite1;
    public static float spriteXScale;
    public static float spriteYScale;

    // Use this for initialization
    void Start()
    {
        customHoldSprite0 = Resources.Load<Sprite>("customHold0");
        customHoldSprite1 = Resources.Load<Sprite>("customHold1");
        if (customHoldSprite0 == null || customHoldSprite1 == null)
        {
            Debug.Log("Could not find both custom hold sprites necessary in Resources folder");
        }
        pairMenuItemsWithHolds(menuItems);
        attachMenuHoldToHold(menuItems);
        //Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("Skeleton")); // don't show skeleton
    }

    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (Application.isEditor)
            {
                Debug.Log("Cannot quit the application (Application is editor).");
            }
            else
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }
    }

    void pairMenuItemsWithHolds(Dictionary<string, GameObject> menuItems)
    {
        holds = ClimbARHandhold.GetValidClimbingHolds();
        List<string> keys = new List<string>(menuItems.Keys);
        if (holds.Length < keys.Count)
        {
            Debug.Log("Not enough handholds for the number of menu items");
        }

        // right now, just pair them arbitrarily
        for (int i = 0; i < Math.Min(holds.Length, keys.Count); i++)
        {
            menuItems[keys[i]] = holds[i];
        }
    }

    void attachMenuHoldToHold(Dictionary<string, GameObject> menuItems)
    {
        foreach (string menuItem in menuItems.Keys)
        {
            GameObject menuHold = menuItems[menuItem];
            if (menuHold == null)
            {
                Debug.Log("No hold for menu item " + menuItem);
            }
            else
            {
                if (customHoldSprite0 != null && menuItem.Equals(SceneUtils.SceneNames.rocManGamePlay))
                {
                    ClimbARHandhold.DrawHoldSprite(menuHold.GetComponent<SpriteRenderer>(),
                        spriteXScale, spriteYScale);
                }
                else
                {
                    menuHold.GetComponent<LineRenderer>().startColor = UnityEngine.Color.cyan;
                    menuHold.GetComponent<LineRenderer>().endColor = UnityEngine.Color.cyan;
                }

                GameObject holdText = new GameObject();
                HoldText holdTextScript = holdText.AddComponent<HoldText>();
                holdTextScript.setup(menuItem, holdText, menuHold);

                MenuHold menuHoldScript = menuHold.AddComponent<MenuHold>();
                menuHoldScript.setup(menuItem);
            }
        }
    }

    private void OnDisable()
    {
        foreach (GameObject hold in holds)
        {
            // Which components do we want to destroy?
            MenuHold mHoldScript = hold.GetComponent<MenuHold>();
            HoldText hTextScript = hold.GetComponent<HoldText>();
            // Hide the rendered sprite
            hold.GetComponent<SpriteRenderer>().enabled = false;
            // Ugly - that we need to rescale the hold upon scene change
            hold.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            // Reset line renderer to uniform color
            hold.GetComponent<LineRenderer>().startColor = UnityEngine.Color.cyan;
            hold.GetComponent<LineRenderer>().endColor = UnityEngine.Color.cyan;
            Destroy(mHoldScript);
            Destroy(hTextScript);
        }
    }
}
