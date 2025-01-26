using System.Collections;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Playables;

public class WarpLocation : MonoBehaviour
{
    public string id;
    public PlayableDirector warpCutscene;

    public void SetRespawnPoint()
    {
        GlobalState state = GlobalState.GetInstance();
        state.RespawnId = id;
        state.RespawnScene = gameObject.scene.name;
    }

    public bool HasWarpCutscene()
    {
        return warpCutscene == null;
    }

    public IEnumerator PlayCutsceneCoroutine()
    {
        warpCutscene.Play();

        while (warpCutscene.state == PlayState.Playing)
            yield return null;
    }
}