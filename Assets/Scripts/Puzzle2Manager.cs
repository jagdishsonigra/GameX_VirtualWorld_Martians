using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Puzzle2Manager : MonoBehaviour
{
    [Header("Total number of emblems")]
    public int totalEmblems = 6;

    [Header("Puzzle Complete Event")]
    public UnityEvent onPuzzleComplete;

    [Header("Secret Door Settings")]
    public Transform secretDoor;      
    public float openAngleX = -90f;    // X axis rotation
    public float openSpeed = 1f;      

    [Header("Door Audio")]
    public AudioSource movingSound;   

    private bool[] emblemPlaced;
    private bool puzzleCompleted = false;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Awake()
    {
        emblemPlaced = new bool[totalEmblems];
    }

    void Start()
    {
        if (secretDoor != null)
        {
            closedRotation = secretDoor.localRotation;

            // Rotate ONLY on X axis
            openRotation = Quaternion.Euler(
                openAngleX,
                secretDoor.localEulerAngles.y,
                secretDoor.localEulerAngles.z
            );
        }
    }

    public void MarkEmblemPlaced(int id)
    {
        if (puzzleCompleted) return;

        int index = id - 1;
        if (index < 0 || index >= totalEmblems) return;

        emblemPlaced[index] = true;
        CheckPuzzleComplete();
    }

    public void MarkEmblemRemoved(int id)
    {
        if (puzzleCompleted) return;

        int index = id - 1;
        if (index < 0 || index >= totalEmblems) return;

        emblemPlaced[index] = false;
    }

    void CheckPuzzleComplete()
    {
        for (int i = 0; i < totalEmblems; i++)
        {
            if (!emblemPlaced[i])
                return;
        }

        PuzzleComplete();
    }

    void PuzzleComplete()
    {
        puzzleCompleted = true;
        Debug.Log("✅ Puzzle 2 Complete!");

        onPuzzleComplete.Invoke();

        if (secretDoor != null)
            StartCoroutine(OpenSecretDoor());
    }

    IEnumerator OpenSecretDoor()
    {
        if (movingSound != null)
        {
            movingSound.loop = true;
            movingSound.Play();
        }

        float t = 0f;
        Quaternion startRot = secretDoor.localRotation;

        while (t < 1f)
        {
            t += Time.deltaTime * openSpeed;
            secretDoor.localRotation = Quaternion.Slerp(startRot, openRotation, t);
            yield return null;
        }

        secretDoor.localRotation = openRotation;

        if (movingSound != null)
            movingSound.Stop();

        Debug.Log("🚪 Secret Door Opened!");
    }
}
