using DefaultNamespace;
using UnityEngine;

public class WarpLocation : MonoBehaviour
{
    public string id;

    public void SetRespawnPoint()
    {
        GlobalState state = GlobalState.GetInstance();
        state.RespawnId = id;
        state.RespawnScene = gameObject.scene.name;
    }
}