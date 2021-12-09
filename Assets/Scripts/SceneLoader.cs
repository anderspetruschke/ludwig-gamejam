using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


    public class SceneLoader : MonoBehaviour
    {
        public bool setThisAsActiveScene = true;

        public string mainSceneName = "Main";

        [Tooltip("the scene manager will wait for this value to be true, before loading the scene")]
        public bool isReady = true;

        [SerializeField] private List<GameObject> activateOnSceneLoad;

        private void Awake()
        {
            foreach (var obj in activateOnSceneLoad)
            {
                obj.SetActive(false);
            }
        }

        private void Start()
        {
            LoadMainScene();

            if (setThisAsActiveScene)
            {
                UnityEngine.SceneManagement.SceneManager.SetActiveScene(this.gameObject.scene);
            }
        }

        private void OnEnable()
        {
            SceneManager.onSceneLoadComplete += SceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.onSceneLoadComplete -= SceneLoaded;
        }

        private void SceneLoaded()
        {
            foreach (var obj in activateOnSceneLoad)
            {
                obj.SetActive(true);
            }
        }

        private void LoadMainScene()
        {
            var mainIsLoaded = false;
            for (var i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++) {
                var scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
                if (scene.name == mainSceneName && scene.isLoaded)
                {
                    mainIsLoaded = true;
                }
            }

            if (!mainIsLoaded)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(mainSceneName);
            }
        }
        
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, SceneManager.FadingType.None);
        }
        
        public void LoadSceneFadeOutOpacity(string sceneName)
        {
            SceneManager.LoadScene(sceneName, SceneManager.FadingType.Opacity);
        }
        
        public void LoadSceneFadeOutDown(string sceneName)
        {
            SceneManager.LoadScene(sceneName, SceneManager.FadingType.Down);
        }
        
        public void LoadSceneFadeOutUp(string sceneName)
        {
            SceneManager.LoadScene(sceneName, SceneManager.FadingType.Up);
        }
        
        public void LoadSceneFadeOutLeft(string sceneName)
        {
            SceneManager.LoadScene(sceneName, SceneManager.FadingType.Left);
        }
        
        public void LoadSceneFadeOutRight(string sceneName)
        {
            SceneManager.LoadScene(sceneName, SceneManager.FadingType.Right);
        }
        
        public void LoadSceneFadeOutFillOut(string sceneName)
        {
            SceneManager.SetFadingFillPosition();
            SceneManager.LoadScene(sceneName, SceneManager.FadingType.FillOut);
        }
        
        public void LoadSceneFadeOutFillIn(string sceneName)
        {
            SceneManager.SetFadingFillPosition();
            SceneManager.LoadScene(sceneName, SceneManager.FadingType.FillIn);
        }
        
        public void SetFadeInNone()
        {
            SceneManager.SetFadeInType(SceneManager.FadingType.None);
        }
        
        public void SetFadeInOpacity()
        {
            SceneManager.SetFadeInType(SceneManager.FadingType.Opacity);
        }
        
        public void SetFadeInDown()
        {
            SceneManager.SetFadeInType(SceneManager.FadingType.Down);
        }
        
        public void SetFadeInUp()
        {
            SceneManager.SetFadeInType(SceneManager.FadingType.Up);
        }
        
        public void SetFadeInLeft()
        {
            SceneManager.SetFadeInType(SceneManager.FadingType.Left);
        }
        
        public void SetFadeInRight()
        {
            SceneManager.SetFadeInType(SceneManager.FadingType.Right);
        }
        
        public void SetFadeInFillOut()
        {
            SceneManager.SetFadingFillPosition();
            SceneManager.SetFadeInType(SceneManager.FadingType.FillOut);
        }
        
        public void SetFadeInFillIn()
        {
            SceneManager.SetFadingFillPosition();
            SceneManager.SetFadeInType(SceneManager.FadingType.FillIn);
        }
    }
