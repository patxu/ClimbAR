using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

    string[] menuItems = new string[1] { SceneUtils.Names.menuExampleGame };
    GameObject[] holds;

	// Use this for initialization
	void Start () {

        holds = GameObject.FindGameObjectsWithTag("Hold");

        foreach (string menuItem in menuItems) {
            Debug.Log(menuItem);
            GameObject menuHold = getHoldForMenu();
            if (menuHold == null)
            {
                Debug.Log("no valid hold for menu");
            }
            else
            {
                Debug.Log("init menu hold");
                ClimbingHold climbingHoldScript = menuHold.GetComponent<ClimbingHold>();
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
	
	// Update is called once per frame
	void Update () {
		
	}

    GameObject getHoldForMenu()
    {
        int numHolds = holds.Length;
        if (holds.Length == 0)
        {
            return null;
        }
        GameObject menuHold = holds[Random.Range(0, holds.Length)]; // TODO smarter picking of the menu hold

        return menuHold;
    }
}
