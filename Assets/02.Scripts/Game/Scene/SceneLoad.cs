using UnityEngine;

public class SceneLoad : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        Managers.Scene.ChangeScene<PhnMyUnitScene>(Managers.Scene.PhnMyUnitScene);
    }
}
