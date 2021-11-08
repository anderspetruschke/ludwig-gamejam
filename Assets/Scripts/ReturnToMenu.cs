using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
    [SerializeField] private Rigidbody2D saveable;
    [SerializeField] private List<Coin> coins;
    
    private void Start()
    {
        if (MenuButtons.loadSave && PlayerPrefs.HasKey("Save"))
        {
            var savedDataJson = PlayerPrefs.GetString("Save");
            var savedData = JsonUtility.FromJson<SaveData>(savedDataJson);

            saveable.position = savedData.position;
            saveable.velocity = savedData.velocity;
            saveable.angularVelocity = savedData.angularVelocity;

            var cameraTransform = Camera.main.transform;
            
            var position = transform.position;
            position.z = cameraTransform.position.z;
            cameraTransform.transform.position = position;

            for (var index = 0; index < savedData.coins.Length; index++)
            {
                var coinCollected = savedData.coins[index];
                if (coinCollected)
                {
                    coins[index].RestoreCollected();
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKey("escape"))
        {
            ToMenu();
        }
    }

    public void ToMenu()
    {
        var collectedCoins = new bool[coins.Count];

        for (var index = 0; index < coins.Count; index++)
        {
            var coin = coins[index];
            collectedCoins[index] = coin.WasCollected();
        }

        var saveData = new SaveData
        {
            position = saveable.position, velocity = saveable.velocity, angularVelocity = saveable.angularVelocity, coins = collectedCoins
        };

        var json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString("Save", json);
        
        SceneManager.LoadScene(0);
    }
}

[Serializable]
public class SaveData
{
    public Vector3 position;
    public Vector3 velocity;
    public float angularVelocity;
    public bool[] coins;
}