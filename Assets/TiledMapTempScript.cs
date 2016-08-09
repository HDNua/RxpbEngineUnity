using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;



/// <summary>
/// 
/// </summary>
public class TiledMapTempScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            SceneManager.LoadScene("04");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            SceneManager.LoadScene("05");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            SceneManager.LoadScene("06");
        }
    }
}
