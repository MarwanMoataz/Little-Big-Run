using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static bool isPlayerLocked = false; // Global flag to lock/unlock player controls

    public static void LockPlayer()
    {
        isPlayerLocked = true;
        Debug.Log("Player controls locked.");
    }

    public static void UnlockPlayer()
    {
        isPlayerLocked = false;
        Debug.Log("Player controls unlocked.");
    }
}
