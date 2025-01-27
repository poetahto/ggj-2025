using DefaultNamespace;
using UnityEngine;

public class RespawnSetter : MonoBehaviour
{
    public string respawnId;
    public string respawnScene;
    
    private void Start()
    {
        GlobalState state = GlobalState.GetInstance();
        state.RefillPower();
        
        if (state.RespawnId != respawnId)
        {
            state.textBox.SetText("NEW B.A.R.T. FABRICATOR SELECTED", 1);
            state.RespawnId = respawnId;
            state.RespawnScene = respawnScene;
        }
    }
}
