using DefaultNamespace;
using UnityEngine;

public class RespawnSetter : MonoBehaviour
{
    public MeshRenderer screen;
    public string respawnId;
    public string respawnScene;

    private Material _lockedMaterial;
    private Material _activeMaterial;
    private Material _inactiveMaterial;
    
    public void OnInteract()
    {
        GlobalState state = GlobalState.GetInstance();

        if (state.UnlockedRespawns.Contains(respawnId))
        {
            // unlocked and not active yet - activate it
            if (state.RespawnId != respawnId)
            {
                state.textBox.SetText("NEW B.A.R.T. FABRICATOR SELECTED", 1);
                state.RespawnId = respawnId;
                state.RespawnScene = respawnScene;
                ChangeScreen(_activeMaterial);
                state.RefillPower();
            }
            else
            {
                // unlocked and already active
                state.textBox.SetText($"WELCOME HOME, B.A.R.T. MODEL {state.DeathCount + 1}\n IT IS GOOD TO SEE YOU.", 1);
            }
        }
        else
        {
            state.textBox.SetText("THIS FABRICATOR HAS BEEN LOCKED : ACCESS CODE \"GAIA\"", 1);
        }
    }
    
    private void Start()
    {
        _lockedMaterial = Resources.Load<Material>("MatLocked");
        _activeMaterial = Resources.Load<Material>("MatEnabled");
        _inactiveMaterial = Resources.Load<Material>("MatDisabled");
        
        GlobalState state = GlobalState.GetInstance();

        if (state.UnlockedRespawns.Contains(respawnId))
            ChangeScreen(state.RespawnId == respawnId ? _activeMaterial : _inactiveMaterial);
        else // hasn't been unlocked yet - locked
            ChangeScreen(_lockedMaterial);
    }
    
    private void ChangeScreen(Material material)
    {
        Material[] materials = screen.sharedMaterials;
        materials[1] = material;
        screen.sharedMaterials = materials;
    }
}
