using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public CameraManager[] cameras;
    public List<KeyValuePair<string, CameraManager>> SecretCodes = new List<KeyValuePair<string, CameraManager>>();
    private static System.Random random = new System.Random();
    void Awake()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        GenerateSecretCodes();
    }

    void GenerateSecretCodes()
    {
        const string chars = "0123456789";

        for (int i = 0; i < cameras.Length; i++)
        {
            string code = new string(Enumerable.Repeat(chars, 4)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            SecretCodes.Add(new KeyValuePair<string, CameraManager>(code, cameras[i]));
        }
    }


}