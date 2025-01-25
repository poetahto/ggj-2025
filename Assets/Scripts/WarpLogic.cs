using DefaultNamespace;
using UnityEngine;

public class WarpLogic : MonoBehaviour
{
    public string targetId;
    public string targetScene;

    public void Warp()
    {
        GlobalState.GetInstance().Warp(targetScene, targetId);
    }
}
