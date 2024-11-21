using System.Runtime.CompilerServices;
using UnityEngine;

public class Logger : MonoBehaviour
{
    public static Logger Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keep it alive across scenes
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    public void Log(string message, [CallerMemberName] string callerName = "")
    {
        Debug.Log($"Called from: {callerName} - Message: {message}");
    }
}