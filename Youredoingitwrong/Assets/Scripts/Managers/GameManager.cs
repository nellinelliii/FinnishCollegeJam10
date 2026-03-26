using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Reference to the player GameObject
    public GameObject PlayerObj;

    // Singleton instance of the GameManager
    public static GameManager Instance;


    private void Awake()
    {
        if (Instance == null) //Tarkistetaan onko GameManager-instanssi olemassa
        {
            Debug.Log("GameManager created!", this.gameObject);
            Instance = this; //Asetetaan tämä GameManager-instanssiksi
            
        }
        else
        {
            Debug.LogError("GameManager exists already! Destroying duplicate."); // Virheilmoitus, jos toinen GameManager-instanssi on jo olemassa)
            Destroy(gameObject); // Tuhoaa tämän GameManager-instanssin, jos toinen on jo olemassa
        }
    }

}
