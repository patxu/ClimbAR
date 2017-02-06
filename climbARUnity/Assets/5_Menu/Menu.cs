using System;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

    // declare menu items here
    public Dictionary<string, GameObject> menuItems = new Dictionary<string, GameObject>()
    {
        { SceneUtils.SceneNames.rocManGamePlay, null },
        { SceneUtils.SceneNames.musicGame, null },
        //{ SceneUtils.SceneNames.exampleGame, null },
    };

    private GameObject[] holds;

	// Use this for initialization
	void Start () {
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
        holds = GameObject.FindGameObjectsWithTag("Hold");
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
        foreach (string menuItem in menuItems.Keys) {
            GameObject menuHold = menuItems[menuItem];
            if (menuHold == null)
            {
              Debug.Log("No hold for menu item " + menuItem);
            }
            else
            {
                MenuHold menuHoldScript = menuHold.AddComponent<MenuHold>();
                menuHoldScript.setup(menuItem);
                menuHold.GetComponent<LineRenderer>()
                    .startColor = UnityEngine.Color.cyan;
                menuHold.GetComponent<LineRenderer>()
                    .endColor = UnityEngine.Color.cyan;
            }
        }
    }

    private void OnDisable()
    {
        foreach (GameObject hold in holds)
        {
            MenuHold script = hold.GetComponent<MenuHold>();
            Destroy(script);
        }
    }

}
