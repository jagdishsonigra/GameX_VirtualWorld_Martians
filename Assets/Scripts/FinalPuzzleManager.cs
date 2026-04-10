using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FinalPuzzleManager : MonoBehaviour
{
    // ================= CAMERA =================
    public Transform xrCamera;
    private Vector3 correctCameraPosition = new Vector3(57.32f, -6.23f, -0.18f);
    public float threshold = 1.5f;

    private bool areaTriggered = false;
    private bool puzzleEnded = false;

    // ================= CHESTS =================
    public HingeJoint correctChest;
    public HingeJoint[] wrongChests;
    public float openAngle = 60f;

    // ================= AUDIO =================
    public AudioSource audioSource;
    public AudioClip player_1;

    public AudioClip instructionClip;
    public AudioClip twoMinLeftClip;
    public AudioClip oneMinLeftClip;
    public AudioClip countdownClip;

    public AudioClip winAudioClip;
    public AudioClip loseAudioClip;

    // ================= EFFECTS =================
    public GameObject winEffectObject;
    public GameObject loseEffectObject;

    // ================= SCENES =================
    public string winSceneName = "WinScene";
    public string loseSceneName = "GameOverScene";

    public GameObject confettiPrefab;   // Drag your prefab here
    public int spawnCount = 5;

    public float upwardForce = 5f;
    public float sideForce = 2f;
    // ================= COROUTINE HANDLE =================
    private Coroutine timerCoroutine;

    [Header("PLAYER 2 AUDIO TRIGGER")]
    public AudioClip player_2;
    public Vector3 player2Position;   // Set in Inspector
    public float player2Threshold = 1f;

    private bool player2Played = false;

    [Header("Level 3 Audio Trigger Positions")]
    public AudioClip Game3_1;
    public AudioClip Game3_end;
    public Vector3 game3_1_Position;
    public float game3_1_Threshold = 1f;
    private bool game3_1_Played = false;

    public Vector3 game3_end_Position;
    public float game3_end_Threshold = 1f;
    private bool game3_end_Played = false;



    void Start()
    {
        if (winEffectObject) winEffectObject.SetActive(false);
        if (loseEffectObject) loseEffectObject.SetActive(false);
        StartCoroutine(PlayIntroAudioWhenReady());
    }

    void LateUpdate()
    {
        if (puzzleEnded) return;

        CheckCameraArea();
        CheckPlayer2AudioTrigger();
        CheckGame3_1_AudioTrigger();
        CheckGame3_End_AudioTrigger();
        CheckChests();
    }

    // ================= CAMERA CHECK =================
    void CheckCameraArea()
    {
        if (areaTriggered || xrCamera == null) return;
        //Debug.Log("XR Camera world position: " + xrCamera.transform.position);

        if (Vector3.Distance(xrCamera.position, correctCameraPosition) <= threshold)
        {
            areaTriggered = true;
            Debug.Log("✅ Camera entered correct area");

            timerCoroutine = StartCoroutine(TimerRoutine());
        }
    }

    // ================= CHEST CHECK =================
    void CheckChests()
    {
        if (puzzleEnded) return;

        if (IsChestOpened(correctChest))
        {
            puzzleEnded = true;
            StopTimer();
            StartCoroutine(WinRoutine());
            return;
        }

        foreach (HingeJoint chest in wrongChests)
        {
            if (IsChestOpened(chest))
            {
                puzzleEnded = true;
                StopTimer();
                StartCoroutine(LoseRoutine());
                return;
            }
        }
    }

    bool IsChestOpened(HingeJoint hinge)
    {
        if (hinge == null) return false;
        return Mathf.Abs(hinge.angle) >= openAngle;
    }

    // ================= TIMER =================
    IEnumerator TimerRoutine()
    {
        audioSource.PlayOneShot(instructionClip);

        yield return new WaitForSeconds(60f);
        audioSource.PlayOneShot(twoMinLeftClip);

        yield return new WaitForSeconds(60f);
        audioSource.PlayOneShot(oneMinLeftClip);

        yield return new WaitForSeconds(50f);
        audioSource.PlayOneShot(countdownClip);

        yield return new WaitForSeconds(10f);

        if (!puzzleEnded)
        {
            puzzleEnded = true;
            StartCoroutine(LoseRoutine());
        }
    }

    // ================= STOP TIMER =================
    void StopTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }

    // ================= WIN =================
    IEnumerator WinRoutine()
    {
        Debug.Log("🎉 YOU WIN!");

        if (winEffectObject) winEffectObject.SetActive(true);
        if (winAudioClip) audioSource.PlayOneShot(winAudioClip);

        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene(winSceneName);
    }

    // ================= LOSE =================
    IEnumerator LoseRoutine()
    {
        Debug.Log("💀 YOU LOSE!");

        if (loseEffectObject) loseEffectObject.SetActive(true);
        if (loseAudioClip) audioSource.PlayOneShot(loseAudioClip);

        yield return new WaitForSeconds(15f);

        SceneManager.LoadScene(loseSceneName);
    }
    public void LaunchConfetti()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            // Spawn prefab
            GameObject confetti = Instantiate(confettiPrefab, transform.position, Random.rotation);

            // Get Rigidbody
            Rigidbody rb = confetti.GetComponent<Rigidbody>();

            if (rb != null)
            {
                // Random direction like confetti
                Vector3 force = new Vector3(
                    Random.Range(-sideForce, sideForce),
                    Random.Range(upwardForce * 0.5f, upwardForce),
                    Random.Range(-sideForce, sideForce)
                );

                rb.AddForce(force, ForceMode.Impulse);
            }

            // Optional: destroy after 5 seconds
            Destroy(confetti, 5f);
        }
    }
    IEnumerator PlayIntroAudioWhenReady()
    {
        // Wait until scene fully loads
        yield return new WaitUntil(() => SceneManager.GetActiveScene().isLoaded);

        // Wait one frame so XR camera stabilizes
        yield return null;

        // Optional: small delay for safety (recommended for Quest)
        yield return new WaitForSeconds(0.3f);

        if (audioSource != null && player_1 != null)
        {
            audioSource.PlayOneShot(player_1);
        }
    }

    void CheckPlayer2AudioTrigger()
    {
        if (player2Played || xrCamera == null) return;

        float dist = Vector3.Distance(xrCamera.position, player2Position);

        if (dist <= player2Threshold)
        {
            player2Played = true;

            Debug.Log("🔊 Player 2 audio triggered");

            if (audioSource != null && player_2 != null)
            {
                audioSource.PlayOneShot(player_2);
            }
        }
    }
    void CheckGame3_1_AudioTrigger()
    {
        if (game3_1_Played || xrCamera == null) return;

        if (Vector3.Distance(xrCamera.position, game3_1_Position) <= game3_1_Threshold)
        {
            game3_1_Played = true;

            if (audioSource != null && Game3_1 != null)
            {
                audioSource.PlayOneShot(Game3_1);
                Debug.Log("🔊 Game3_1 audio played");
            }
        }
    }
    void CheckGame3_End_AudioTrigger()
    {
        if (game3_end_Played || xrCamera == null) return;

        if (Vector3.Distance(xrCamera.position, game3_end_Position) <= game3_end_Threshold)
        {
            game3_end_Played = true;

            if (audioSource != null && Game3_end != null)
            {
                audioSource.PlayOneShot(Game3_end);
                Debug.Log("🔊 Game3_end audio played");
            }
        }
    }
}
