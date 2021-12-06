using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public bool level2;
    public bool gameOver;
    
    void Start()
    {
        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if (gameOver)
        {
            if (Input.GetKey(KeyCode.R))
            {
                SceneManager.LoadScene("Main");
            }
        }

    }
}
