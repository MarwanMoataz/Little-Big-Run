using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetgame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("All PlayerPrefs cleared.");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
