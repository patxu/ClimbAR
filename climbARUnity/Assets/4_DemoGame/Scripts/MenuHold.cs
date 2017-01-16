using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHold : MonoBehaviour
{

    private float startTime;

    void OnStart()
    {
        startTime = 0;
    }

    void OnUpdate()
    {

    }

    void OnTriggerExit2D(Collider2D col)
    {
        this.startTime = 0;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //this.startTime += Time.deltaTime;
        //if (this.startTime >= 2)
        // {
        SceneManager.LoadScene("4_DemoGame");
        //}
    }

    void OnMouseDown()
    {
        Destroy(gameObject);
    }
}
