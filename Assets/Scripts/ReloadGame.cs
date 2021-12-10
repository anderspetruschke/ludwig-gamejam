using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadGame : MonoBehaviour
{
    [SerializeField] private SceneLoader sceneLoader;

    private void OnEnable()
    {
        sceneLoader.SetFadeInFillIn();
        sceneLoader.LoadScene("Game");
    }
}
