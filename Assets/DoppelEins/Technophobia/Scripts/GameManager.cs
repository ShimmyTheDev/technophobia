using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static readonly Random random = new();
    private int attempts = 0;
    [Header("Gameplay settings")] 
    public CameraManager[] cameras;

    public GameObject playerPrefab;

    private Transform playerStartPosition;
    
    public List<KeyValuePair<string, CameraManager>> SecretCodes = new();
    public static GameManager Instance { get; private set; }

    public bool SecrectsReady { get; private set; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadLevel();
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy the duplicate
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        playerStartPosition = GameObject.FindGameObjectWithTag("Respawn").transform;
    }

    private void Start()
    {
        GenerateSecretCodes();
    }

    private void GenerateSecretCodes()
    {
        SecretCodes.Clear(); // Clear previous codes in case of reload
        const string chars = "0123456789";

        for (var i = 0; i < cameras.Length; i++)
        {
            var code = new string(Enumerable.Repeat(chars, 4)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            SecretCodes.Add(new KeyValuePair<string, CameraManager>(code, cameras[i]));
        }

        SecrectsReady = true;
    }

    private void ReloadLevel()
    {
        // Reset static variables or other persistent data if necessary
        SecrectsReady = false;

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Optionally, reinitialize the level after the scene loads
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeLevel();

        // Unsubscribe to avoid multiple calls
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void InitializeLevel()
    {
        GenerateSecretCodes();
    }
}
