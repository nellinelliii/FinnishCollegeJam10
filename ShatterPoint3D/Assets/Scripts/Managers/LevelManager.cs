using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public string mainMenuScene = "MainMenu";

    public static LevelManager Instance;

    private void Awake()
    {


        if (Instance == null)
        {
            Debug.Log("LevelManager created!", this.gameObject);
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogError("LevelManager exists already! Destroying duplicate.");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadLevel(mainMenuScene);
    }



    // Ladataan uusi kentt‰ annetulla nimell‰
    public void LoadLevel(string levelName)
    {
        //Varmistetaan ett‰ annettu levelnimi on oikea tai muiden ongelmien sattuessa
        try
        {
            SceneManager.LoadScene(levelName);
            //Voidaan laittaa tarkistus onko ladattava kentt‰ main menu

            //JOs MainMenu ladataan, n‰ytet‰‰n MainMenu UI paneeli UIManagerin avulla
            if (levelName == mainMenuScene)
            {
                UIManager.Instance.ShowMainMenuPanel();
            }
            else
            {
                UIManager.Instance.ShowHUDpanel();
            }
        }
        catch (System.Exception ex)
        {
            //Mik‰li tulee erroreita, logataan ne konsoliin
            Debug.LogError("Cannot change into scene '" + levelName + "'");
            Debug.LogError(ex.Message);
        }
    }

}
