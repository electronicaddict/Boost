using UnityEngine;
using UnityEngine.SceneManagement;
public class RocketScript : MonoBehaviour
{
    public float thrustForce = 5f;
    public float rotationSpeed = 200f;

    private Rigidbody rb;
    [SerializeField] private ParticleSystem thrustParticles, expParticles; 
    [SerializeField] private AudioSource thrustAudio, expAudio, successAudio; 
    bool movementEnabled = true;
    void Start()
    {
        successAudio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!movementEnabled) return;
        float rotateInput = -Input.GetAxis("Horizontal"); 
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, 0, rotateInput * rotationSpeed * Time.deltaTime));

        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(gameObject.transform.up * thrustForce);
            thrustParticles.Play();

            if (!thrustAudio.isPlaying)
                thrustAudio.Play();
        }
        else
        {
            if (thrustParticles.isPlaying)
                thrustParticles.Stop();

            if (thrustAudio.isPlaying)
                thrustAudio.Stop();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("ground"))
        {
            AudioSource expAudioInstance = Instantiate(expAudio, transform.position, Quaternion.identity);
            expAudioInstance.Play();

            thrustAudio.Stop();
            Instantiate(expParticles, transform.position, Quaternion.identity);
            foreach (var r in GetComponentsInChildren<Renderer>())
                r.enabled = false;
            foreach (var c in GetComponentsInChildren<Collider>())
                c.enabled = false;
            StartCoroutine(LoadLevelAfterDelay(3f, 0));
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("check"))
        {
            movementEnabled = false;
            rb.isKinematic = true;
            successAudio.Play();
            StartCoroutine(LoadLevelAfterDelay(3f, SceneManager.GetActiveScene().buildIndex + 1));
        }
    }

    private System.Collections.IEnumerator LoadLevelAfterDelay(float delay, int levelIndex = 0)
    {
        yield return new WaitForSeconds(delay);
        if (levelIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(levelIndex);
        else
            SceneManager.LoadScene(0); 
    }
}
