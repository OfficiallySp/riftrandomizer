using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchGames : MonoBehaviour
{
    public void GotoValo()
    {
        SceneManager.LoadScene("Valo");
    }

    public void GotoLeague()
    {
        SceneManager.LoadScene("League");
    }
    
    public void GoToArena()
    {
        SceneManager.LoadScene("Arena");
    }
}
