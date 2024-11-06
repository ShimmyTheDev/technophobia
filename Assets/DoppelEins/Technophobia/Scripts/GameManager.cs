using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public List<KeyValuePair<string, bool>> SecretCodes = new List<KeyValuePair<string, bool>>();
    private static System.Random random = new System.Random();
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
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
        int iterations = 10;
        const string chars = "0123456789";

        for (int i = 0; i < iterations; i++)
        {
            string code = new string(Enumerable.Repeat(chars, 4)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            SecretCodes.Add(new KeyValuePair<string, bool>(code, false));
        }
    }


}