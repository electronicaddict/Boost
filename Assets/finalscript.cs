using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Add this for Button

public class finalscript : MonoBehaviour
{
    public TextMeshProUGUI dialogueText; // Assign in Inspector
    [TextArea] public string fullText;   // The dialogue to display
    public float typeSpeed = 0.05f;      // Seconds per character

    public Button button1; // Assign in Inspector
    public Button button2; // Assign in Inspector

    void Start()
    {
        // Make sure buttons are inactive at start
        if (button1 != null) button1.gameObject.SetActive(false);
        if (button2 != null) button2.gameObject.SetActive(false);

        StartCoroutine(TypeDialogue());
    }

    IEnumerator TypeDialogue()
    {
        dialogueText.text = "";
        foreach (char c in fullText)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        // Activate buttons when done
        if (button1 != null) button1.gameObject.SetActive(true);
        if (button2 != null) button2.gameObject.SetActive(true);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(0); // Loads the first scene (index 0)
    }

    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
