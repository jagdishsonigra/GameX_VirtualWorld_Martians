using UnityEngine;
using UnityEngine.Events;

public class Puzzle1Check : MonoBehaviour
{
    public Transform object1;
    public Transform object2;

    public float targetY1;
    public float targetY2;

    public float threshold = 1f;
    public float holdTime = 1f;

    public UnityEvent onPuzzleComplete;

    // Chest objects
    public GameObject closedChest;
    public GameObject openChest;
    public GameObject Mask;

    // Audio
    public AudioSource chestAudioSource;

    private bool puzzleSolved = false;
    private float timer = 0f;

    void Update()
    {
        if (puzzleSolved) return;

        float currentY1 = object1.eulerAngles.y;
        float currentY2 = object2.eulerAngles.y;

        if (IsWithinThreshold(currentY1, targetY1) &&
            IsWithinThreshold(currentY2, targetY2))
        {
            timer += Time.deltaTime;

            if (timer >= holdTime)
            {
                CompletePuzzle();
            }
        }
        else
        {
            timer = 0f;
        }
    }

    void CompletePuzzle()
    {
        puzzleSolved = true;
        Debug.Log("✅ Puzzle 1 Complete!");

        // Play sound
        if (chestAudioSource != null)
            chestAudioSource.Play();

        // Open chest
        if (closedChest != null)
            closedChest.SetActive(false);

        if (openChest != null)
            openChest.SetActive(true);

        Mask.SetActive(true);
    }

    bool IsWithinThreshold(float current, float target)
    {
        float diff = Mathf.Abs(Mathf.DeltaAngle(current, target));
        return diff <= threshold;
    }
}
