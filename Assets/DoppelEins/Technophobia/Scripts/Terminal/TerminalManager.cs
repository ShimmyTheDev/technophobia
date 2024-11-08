using System;
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
    private AudioSource audioSource;
    public KeyValuePair<string, bool> terminalSolution;
    public string solutionCode;
    bool IsTyping = false;
    bool CanEnterSolution = false;
    private string currentInput = "";

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        passwordInput.gameObject.SetActive(false);

    }

    private void Update()
    {
        if (solutionCode == "")
        {
            GetSolution();
        }

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

    private void GetSolution()
    {
        terminalSolution = GameManager.Instance.SecretCodes[0];
        solutionCode = terminalSolution.Key;
    }

    public void StartInteraction()
    {
        if (!IsTyping && !CanEnterSolution)
        {
            IsTyping = true;
            PlayerInputManager.Instance.IsInteracting = true;
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
        Debug.Log("Current input: " + currentInput);


        if (currentInput.Length == 4)
        {
            if (IsValidSolution())
            {
                Debug.Log("That's correct, disabling camera");
                audioSource.PlayOneShot(success);
            }
            else
            {
                Debug.Log("Incorrect solution, try again!");
                passwordInput.color = Color.red;
                audioSource.PlayOneShot(error);
            }
        }
    }

    private bool IsValidSolution()
    {
        return currentInput == solutionCode;
    }
}
