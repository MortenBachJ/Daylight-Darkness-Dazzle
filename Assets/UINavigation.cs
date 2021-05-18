using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UINavigation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playGame()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    public void viewControls()
    {
        SceneManager.LoadScene("ControlsScene", LoadSceneMode.Single);
    }

    public void viewMenu()
    {
        SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
