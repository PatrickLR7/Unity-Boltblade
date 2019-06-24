using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClicks : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        Wave_Spawner.currentLevel = 1;
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
        //EditorApplication.isPlaying = false;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void GoToInstructions()
    {
        SceneManager.LoadScene(6);
    }
}
