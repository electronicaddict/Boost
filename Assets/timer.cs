using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class timer : MonoBehaviour
{
    private float elapsedTime = 0f;
    private TextMeshProUGUI text;
    private static timer instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) { elapsedTime = 0f; }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #endif
        }
        // Pause timer if on scene 8
        if (SceneManager.GetActiveScene().buildIndex == 8)
            text.text = (Mathf.Round(elapsedTime * 100f) / 100f).ToString("F2");
        else
            elapsedTime += Time.deltaTime;
            if (text != null)
            {
                float roundedTime = Mathf.Round(elapsedTime * 100f) / 100f;
                text.text = roundedTime.ToString("F2");
            }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject timerObj = GameObject.FindGameObjectWithTag("timer");
        if (timerObj != null)
            text = timerObj.GetComponent<TextMeshProUGUI>();
        else
            text = null;

        if (scene.buildIndex == 0)
            elapsedTime = 0f;
        if (scene.buildIndex == 8)
        {
            AudioSource audio = GetComponent<AudioSource>();
            if (audio != null)
                Destroy(audio);
        }
    }
}
