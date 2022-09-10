using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager GM;

    private Ground[] allGroundPieces;

    public GameObject LevelCompleteUI;

    void Start()
    {
        SetupNewLevel();
        
    }

    private void SetupNewLevel()
    {
        allGroundPieces = FindObjectsOfType<Ground>();

    }

    //making GM to work as a singleton by this awake 
    private void Awake()
    {
        if(GM == null)
        {
            GM = this;
        }
        else if (GM != this )
        {
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        
    }

    private void OnLevelFinishedLoading(Scene scene , LoadSceneMode mode)
    {
        SetupNewLevel();
    }

    public void CheckComplete()
    {
        bool isFinished = true;

        for (int i=0; i<allGroundPieces.Length;i++)
        {
            if (allGroundPieces[i].isColored == false)
            {
                isFinished = false;
                break;
            }
        }

        if(isFinished)
        {
           
            NextLevel();
        }
    }

    
    private void NextLevel()
    {
        LevelCompleteUI.SetActive(true);
       
    }

    public void NextLevelButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Game Closed");
        Application.Quit();

    }


}

