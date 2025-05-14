using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class ResetPlayerPrefs : MonoBehaviour
{
    [MenuItem("Window/PlayerPrefs 초기화")]
    private static void ResetPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs has been reset.");
    }

}
#endif
