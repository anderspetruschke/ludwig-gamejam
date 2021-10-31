using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
    [SerializeField] private Rigidbody2D saveable;

    private void Awake()
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
        var saveData = new SaveData
        {
            position = saveable.position, velocity = saveable.velocity, angularVelocity = saveable.angularVelocity
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
}