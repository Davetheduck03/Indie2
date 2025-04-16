using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Button StartGameButton;
    public Button SettingButton;

    void Start()
    {
        StartGameButton.onClick.AddListener(StartGame);
        SettingButton.onClick.AddListener(Setting);
    }

    // Update is called once per frame
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
