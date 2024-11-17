using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    private static readonly Random random = new();

    [Header("Gameplay settings")] public CameraManager[] cameras;

    public List<KeyValuePair<string, CameraManager>> SecretCodes = new();
    public static GameManager Instance { get; private set; }

    public bool SecrectsReady { get; private set; }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        GenerateSecretCodes();
    }

    private void GenerateSecretCodes()
    {
        const string chars = "0123456789";

        for (var i = 0; i < cameras.Length; i++)
        {
            var code = new string(Enumerable.Repeat(chars, 4)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            SecretCodes.Add(new KeyValuePair<string, CameraManager>(code, cameras[i]));
        }

        SecrectsReady = true;
    }
}