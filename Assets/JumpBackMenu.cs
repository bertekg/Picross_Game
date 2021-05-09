using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBackMenu : MonoBehaviour
{
    public void JumpBackM()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
