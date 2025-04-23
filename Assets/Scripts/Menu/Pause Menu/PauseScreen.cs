using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    public GameObject PausePanel;
    public Button PauseButton;
    public Button ContinueButton;

    // Start is called before the first frame update
    void Start()
    {
        PausePanel.SetActive(false);
        PauseButton.onClick.AddListener(OnPauseButtonClick);
        ContinueButton.onClick.AddListener(OnContinueButtonClick);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnPauseButtonClick()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0f;
    }
    private void OnContinueButtonClick()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1f;
    }



}
