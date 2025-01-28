using System.Collections;
using DefaultNamespace;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameUtil : MonoBehaviour
{
    public void EjectBatteries()
    {
        StartCoroutine(EjectBatteryCoroutine());
    }

    public void IgnoreCaptain()
    {
        StartCoroutine(IgnoreCaptainCoroutine());
    }

    private IEnumerator EjectBatteryCoroutine()
    {
        // player death
        float remaining = 2.6f;
        RuntimeManager.PlayOneShot("event:/Robot_Death_Screen");

        while (remaining >= 0)
        {
            float t = 1 - remaining / 2.6f;
            Time.timeScale = Mathf.Lerp(1, 0, t);
            remaining -= Time.unscaledDeltaTime;
            Shader.SetGlobalFloat("_GlitchProgress", t);
            yield return null;
        }

        Time.timeScale = 1;
        Shader.SetGlobalFloat("_GlitchProgress", 0);
        
        // load credits
        SceneManager.LoadScene("credits_leave");
    }

    private IEnumerator IgnoreCaptainCoroutine()
    {
        GlobalState state = GlobalState.GetInstance();
        RuntimeManager.PlayOneShot("event:/Robot_Death_Screen");
        state.textBox.SetText("B.A.R.T. - this is an order, eject the\n[CONNECTION TERMINATED]", 1);
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(state.LoadCredits("credits_stay"));
    }
}
