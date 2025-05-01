using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Button StartGameButton;
    public Button SettingButton;

    void Start()
    {
        StartGameButton.onClick.AddListener(StartGame);
        SettingButton.onClick.AddListener(Setting);
    }

    void Update()
    {
        
    }
    private void StartGame()
    {
        SceneManager.LoadScene("Mall Level 1");
    }
    private void Setting()
    {
        Debug.Log("Ditmemay");
    }
}
