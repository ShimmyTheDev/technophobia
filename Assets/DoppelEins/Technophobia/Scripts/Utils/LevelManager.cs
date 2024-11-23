using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;
using UnityEditor.SceneManagement;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace DoppelEins.Technophobia.Scripts.Utils
{
    public class LevelManager : MonoBehaviour
    {
        [Header("Scene Management")]
        [SerializeField] int sceneIndex;
        private static readonly Random random = new();

        [Header("Gameplay settings")] public CameraManager[] cameras;

        public List<KeyValuePair<string, CameraManager>> SecretCodes = new();

        public bool SecrectsReady { get; private set; }

        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded();
            Cursor.lockState = CursorLockMode.Locked;
        }

        private UnityAction<Scene, LoadSceneMode> OnSceneLoaded()
        {
            throw new System.NotImplementedException();
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
}