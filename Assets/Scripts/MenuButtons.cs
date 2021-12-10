using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] private SceneLoader sceneLoader;
    
    public GameObject deactivateOnWebGL;

    public Button continueButton;

    public static bool loadSave = false;
    
    private void Start()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            deactivateOnWebGL.SetActive(false);  
        }

        continueButton.interactable = PlayerPrefs.HasKey("Save");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        loadSave = false;
        sceneLoader.SetFadeInFillIn();
        sceneLoader.LoadSceneFadeOutFillIn("Game");
    }

    public void Continue()
    {
        loadSave = true;
        sceneLoader.SetFadeInFillIn();
        sceneLoader.LoadSceneFadeOutFillIn("Game");
    }
}
