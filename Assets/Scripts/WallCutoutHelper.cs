using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class WallCutoutHelper : MonoBehaviour{
    [Range(0, .5f)]
    public float cutoutSize;
    public float cutoutFade;
    public float ditherSize;
    
    void Awake()
    {
        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
    }

    // Update is called once per frame
    void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera){
        //We want to get the current player position in screen space
        Shader.SetGlobalVector("_PlayerPosition", this.transform.position);
        Shader.SetGlobalFloat("_CutoutSize", cutoutSize);
        Shader.SetGlobalFloat("_CutoutFade", cutoutFade);
        Shader.SetGlobalFloat("_DitherSize", ditherSize);

    }

    void OnDestroy(){
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
    }
}
