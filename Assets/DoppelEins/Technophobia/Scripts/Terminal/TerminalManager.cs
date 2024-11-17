using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TerminalManager : MonoBehaviour
{
    public Transform playerPosition;
    [SerializeField] Canvas TerminalDisplay;
    [SerializeField] GameObject passwordPrompt;
    [SerializeField] TMP_Text passwordInput;
    [SerializeField] TMP_Text tmp;
    [SerializeField] AudioClip beep;
    [SerializeField] AudioClip error;
    [SerializeField] AudioClip success;
    [SerializeField] string[] terminalMessages;
    [SerializeField] string secretMessage;
    public CameraManager targetCamera;
    private AudioSource audioSource;
    bool IsTyping = false;
    bool CanEnterSolution = false;
    private string currentInput = "";

    private List<KeyValuePair<CameraManager, string>> cameras = new List<KeyValuePair<CameraManager, string>>();

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        passwordInput.gameObject.SetActive(false);

    }

    private void Update()
    {
        if (CanEnterSolution && !IsTyping)
        {
            for (int i = 0; i <= 9; i++)
            {
                if (Input.GetKeyDown(i.ToString()))
                {

                    OnPasswordInput(i.ToString());
                    break;
                }
            }
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
        if (IsTyping)
        {
            IsTyping = false;
        }
        Camera.main.transform.rotation = Camera.main.transform.parent.rotation;
        PlayerInputManager.Instance.IsInteracting = false;
    }

    private IEnumerator TypeTextOnScreen()
    {
        tmp.text = "";
        foreach (string msg in terminalMessages)
        {
            if (!IsTyping) yield break;

            tmp.text += "\n";
            foreach (char c in msg)
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
        List<KeyValuePair<string, CameraManager>> cameraPairs = GameManager.Instance.SecretCodes;
        foreach (KeyValuePair<string, CameraManager> pair in cameraPairs)
        {
            if (pair.Key == currentInput)
            {

                targetCamera = pair.Value;
            }
        }
    }

}
