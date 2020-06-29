using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private Button Button;


    private void Start()
    {
        Button = GetComponent<Button>();
        Button.onClick.AddListener(() => SceneManager.LoadScene("game"));
    }
}
