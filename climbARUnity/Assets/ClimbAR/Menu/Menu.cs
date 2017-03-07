using System;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{

    // Declare menu items here
    public Dictionary<string, GameObject> menuItems = new Dictionary<string, GameObject>()
    {
        { SceneUtils.SceneNames.rocManGamePlay, null },
        { SceneUtils.SceneNames.musicLoadingScene, null },
    };
    // Associate sprites with specific menu scene items 
    public static Dictionary<string, Sprite> menuSprites = new Dictionary<string, Sprite>()
    {
        { SceneUtils.SceneNames.rocManGamePlay, null },
        { SceneUtils.SceneNames.musicLoadingScene, null },
    };   

    // Set during handhold instantiation, used for rendering menu hold sprites
    public static float camHeight;
    public static float camWidth;

    // Other variables
    private GameObject[] holds;
    public GameObject customHoldSprite;
    public static float spriteXScale;
    public static float spriteYScale;

    // Use this for initialization
    void Start()
    {
        // Find custom hold sprites
        menuSprites[SceneUtils.SceneNames.rocManGamePlay] = Resources.Load<Sprite>("CustomHolds/rocmanGameHold");
        menuSprites[SceneUtils.SceneNames.musicLoadingScene] = Resources.Load<Sprite>("CustomHolds/musicGameHold");

        // Setup menu scene
        pairMenuItemsWithHolds(menuItems);
        attachMenuHoldToHold(menuItems);
        //Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("Skeleton")); // don't show skeleton
    }

    void Update()
    {
    }

    void pairMenuItemsWithHolds(Dictionary<string, GameObject> menuItems)
    {
        holds = GameObject.FindGameObjectsWithTag("Hold");
        List<string> keys = new List<string>(menuItems.Keys);

        GameObject currentHold = RouteGeneration.getStartingHold(holds);

        // right now, just pair them arbitrarily
        for (int i = 0; i < Math.Min(holds.Length, keys.Count); i++)
        {
            menuItems[keys[i]] = currentHold;

            currentHold = RouteGeneration.getNearestHoldAbove(holds, currentHold);
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
                // Draw custom hold sprite for menu hold if present in Resources folder
                if (menuSprites[menuItem] != null)
                {
                    GameObject customSpriteObject = GameObject.Instantiate(customHoldSprite);
                    customSpriteObject.transform.SetParent(menuHold.transform);
                    customSpriteObject.transform.localPosition = new Vector3(0,0,0);

                    float radius = menuHold.GetComponent<CircleCollider2D>().radius;
                    ClimbARHandhold.DrawHoldSprite(customSpriteObject.GetComponent<SpriteRenderer>(), menuSprites[menuItem],
                        radius / camWidth / 2, radius / camWidth / 2);

                }
                // Draw line renderer otherwise
                else
                {
                    ClimbARHandhold.HoldLineRendererActive(menuHold, true);
                    ClimbARHandhold.setHoldColor(menuHold, UnityEngine.Color.cyan);
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
            if (hold != null)
            {
                MenuHold mHoldScript = hold.GetComponent<MenuHold>();
                HoldText hTextScript = hold.GetComponent<HoldText>();

                // Hide the rendered sprite
                hold.GetComponent<SpriteRenderer>().enabled = false;
                ClimbARHandhold.DestroyChildren(hold);
                ClimbARHandhold.HoldLineRendererActive(hold, false);
                hold.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

                // Reset line renderer to uniform color
                hold.GetComponent<LineRenderer>().startColor = UnityEngine.Color.cyan;
                hold.GetComponent<LineRenderer>().endColor = UnityEngine.Color.cyan;

                Destroy(mHoldScript);
                Destroy(hTextScript);
            }
        }
    }
}
