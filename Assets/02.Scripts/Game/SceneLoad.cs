using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Managers.Scene.ChangeScene<GameScene>(Managers.Scene.GameScene);
    }


}
