using System.Collections;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Playables;

public class WarpLocation : MonoBehaviour
{
    public string id;
    public PlayableDirector warpCutscene;
    public bool isIntroWarp;

    private void Start()
    {
        GlobalState state = GlobalState.GetInstance();
        
        if (isIntroWarp && state.GameState == GameState.Intro)
        {
            state.GameState = GameState.Playing;
            StartCoroutine(PlayCutsceneCoroutine());
        }
    }

    public void SetRespawnPoint()
    {
        GlobalState state = GlobalState.GetInstance();
        state.RespawnId = id;
        state.RespawnScene = gameObject.scene.name;
    }

    public bool HasWarpCutscene()
    {
        return warpCutscene != null;
    }

    public IEnumerator PlayCutsceneCoroutine()
    {
        warpCutscene.Play();

        while (warpCutscene.state == PlayState.Playing)
            yield return null;
    }
}