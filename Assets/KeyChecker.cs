using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 
/// </summary>
public class KeyChecker : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        string[] joystickNames = Input.GetJoystickNames();

        foreach (string name in joystickNames)
        {
            print(name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        for (int i=0; i<20; ++i)
        {
            string buttonName = string.Format("Joysick {0} button {1}", 0, i);
            if (Input.GetKeyDown(buttonName))
            {
                print(string.Format("{0} is pressed", buttonName));
            }
        }
        */
    }
}
