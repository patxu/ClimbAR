using System;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

    // declare menu items here
    public Dictionary<string, GameObject> menuItems = new Dictionary<string, GameObject>()
    {
        { SceneUtils.SceneNames.musicGame, null },
        { SceneUtils.SceneNames.menuExampleGame, null },
    };

	// Use this for initialization
	void Start () {
        Menu.PairMenuItemsWithHolds(menuItems);
        Menu.AttachMenuHoldToHold(menuItems);
        Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("Skeleton")); // don't show skeleton
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

    static void PairMenuItemsWithHolds(Dictionary<string, GameObject> menuItems)
    {
        GameObject[] holds = GameObject.FindGameObjectsWithTag("Hold");
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

    static void AttachMenuHoldToHold(Dictionary<string, GameObject> menuItems)
    {
        foreach (string menuItem in menuItems.Keys) {
            GameObject menuHold = menuItems[menuItem];
            if (menuHold == null)
            {
              Debug.Log("No hold for menu item " + menuItem);
            }
            else
            {
                Debug.Log("init menu hold");
                ClimbingHold climbingHoldScript = menuHold.GetComponent<ClimbingHold>(); // issue 147
                Destroy(climbingHoldScript);

                MenuHold menuHoldScript = menuHold.AddComponent<MenuHold>();
                menuHoldScript.setup(menuItem);
                menuHold.GetComponent<LineRenderer>()
                    .startColor = UnityEngine.Color.cyan;
                menuHold.GetComponent<LineRenderer>()
                    .endColor = UnityEngine.Color.cyan;
            }
        }
    }

}
