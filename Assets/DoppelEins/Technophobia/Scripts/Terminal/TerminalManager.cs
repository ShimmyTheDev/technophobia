using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TerminalManager : MonoBehaviour
{
    public Transform playerPosition;
    [SerializeField] private Canvas TerminalDisplay;
    [SerializeField] private GameObject passwordPrompt;
    [SerializeField] private TMP_Text passwordInput;
    [SerializeField] private TMP_Text tmp;
    [SerializeField] private AudioClip beep;
    [SerializeField] private AudioClip error;
    [SerializeField] private AudioClip success;
    [SerializeField] private string[] terminalMessages;
    [SerializeField] private string secretMessage;
    public CameraManager targetCamera;
    private AudioSource audioSource;

    private List<KeyValuePair<CameraManager, string>> cameras = new();
    private bool CanEnterSolution;
    private string currentInput = "";
    private bool IsTyping;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        passwordInput.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (CanEnterSolution && !IsTyping)
            for (var i = 0; i <= 9; i++)
                if (Input.GetKeyDown(i.ToString()))
                {
                    OnPasswordInput(i.ToString());
                    break;
                }
    }

    public void StartInteraction()
    {
        PlayerInputManager.Instance.IsInteracting = true;
        if (!IsTyping && !CanEnterSolution)
        {
            IsTyping = true;
            StartCoroutine(TypeTextOnScreen());
        }
    }

    public void EndInteraction()
    {
        if (IsTyping) IsTyping = false;
        Camera.main.transform.rotation = Camera.main.transform.parent.rotation;
        PlayerInputManager.Instance.IsInteracting = false;
    }

    private IEnumerator TypeTextOnScreen()
    {
        tmp.text = "";
        foreach (var msg in terminalMessages)
        {
            if (!IsTyping) yield break;

            tmp.text += "\n";
            foreach (var c in msg)
            {
                tmp.text += c;
                audioSource.PlayOneShot(beep);
                yield return new WaitForSeconds(0.05f);
            }

            yield return new WaitForSeconds(0.5f);
        }

        IsTyping = false;
        CanEnterSolution = true;
        passwordPrompt.SetActive(true);
        passwordInput.gameObject.SetActive(true);
        passwordInput.text = "";
        currentInput = "";
    }

    public void OnPasswordInput(string key)
    {
        if (!CanEnterSolution) return;
        if (passwordInput.text.Length == 4)
        {
            currentInput = "";
            passwordInput.text = "";
        }

        passwordInput.color = Color.green;


        currentInput += key;
        passwordInput.text = currentInput;
        audioSource.PlayOneShot(beep);

        if (currentInput.Length == 4)
        {
            CheckSoultion();
            if (targetCamera != null)
            {
                targetCamera.DisableCamera();
                audioSource.PlayOneShot(success);
            }
            else
            {
                passwordInput.color = Color.red;
                audioSource.PlayOneShot(error);
            }
        }
    }

    private void CheckSoultion()
    {
        var cameraPairs = GameManager.Instance.SecretCodes;
        foreach (var pair in cameraPairs)
            if (pair.Key == currentInput)
                targetCamera = pair.Value;
    }
}