using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    private Button Button;


    private void Start()
    {
        Button = GetComponent<Button>();
        Button.onClick.AddListener(QuitGame);
    }
    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
Application.Quit();
#endif
    }
}
