using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace DoppelEins.Technophobia.Scripts
{
    public class LevelManager
    {
        [Header("Level settings")]
        Transform playerSpawnPoint;
        [SerializeField] GameObject playerPrefab;
        private static readonly Random random = new();
        Dictionary<string, CameraManager> securityCameras = new Dictionary<string, CameraManager>();

        public void InitializeLevel()
        {
            
        }
        
        // Find all cameras and setup security codes
        void SetupSecurityCameras()
        {
            Debug.Log($"Creating security cameras");
            securityCameras.Clear();
            GameObject[] _cameras = GameObject.FindGameObjectsWithTag("SecurityCamera");

            const string chars = "0123456789";
            foreach (GameObject camera in _cameras)
            {
                var code = new string(Enumerable.Repeat(chars, 4)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
                securityCameras.Add(code, camera.GetComponent<CameraManager>());
                camera.SetActive(true);
            }
        }
        
        // Spawn Player
        void SpawnPlayer()
        {
            Debug.Log("Spawning player");
            playerSpawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform;
            GameObject.Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation);
        }
    }
}