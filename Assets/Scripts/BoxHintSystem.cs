using UnityEngine;

public class BoxHintSystem : MonoBehaviour
{
    [Header("Assign cubes in row order")]
    public GameObject[] boxes;

    [Header("Materials")]
    public Material normalMaterial;
    public Material hintMaterial;

    private int rows = 8;
    private int cubesPerRow = 3;

    private int[] correctCubeIndex;

    void Awake()
    {
        // Initialize early to prevent NullReferenceException
        correctCubeIndex = new int[rows];
    }

    void Start()
    {
        RandomizeCorrectCubes();
        HintOff(); // start in Day mode
    }

    // 🎲 Random correct cube per row
    public void RandomizeCorrectCubes()
    {
        for (int row = 0; row < rows; row++)
        {
            int start = row * cubesPerRow;
            correctCubeIndex[row] = Random.Range(start, start + cubesPerRow);
        }
    }

    // 🔴 NIGHT MODE (Hint ON)
    public void HintOn()
    {
        if (boxes == null) return;

        for (int i = 0; i < boxes.Length; i++)
        {
            if (boxes[i] == null) continue;

            Collider col = boxes[i].GetComponent<Collider>();
            Renderer rend = boxes[i].GetComponent<Renderer>();

            // Disable all colliders
            if (col != null)
                col.enabled = false;

            // Wrong cubes glow red
            if (rend != null)
            {
                if (IsCorrectCube(i))
                    rend.material = normalMaterial;
                else
                    rend.material = hintMaterial;
            }
        }
    }

    // 🟢 DAY MODE (Hint OFF)
    public void HintOff()
    {
        if (boxes == null) return;

        for (int i = 0; i < boxes.Length; i++)
        {
            if (boxes[i] == null) continue;

            Collider col = boxes[i].GetComponent<Collider>();
            Renderer rend = boxes[i].GetComponent<Renderer>();

            // Only correct cubes enabled
            if (col != null)
                col.enabled = IsCorrectCube(i);

            if (rend != null)
                rend.material = normalMaterial;
        }
    }

    // ✅ Check correct cube
    bool IsCorrectCube(int index)
    {
        if (correctCubeIndex == null) return false;

        for (int r = 0; r < rows; r++)
        {
            if (correctCubeIndex[r] == index)
                return true;
        }
        return false;
    }

    // ⚠ Reshuffle when player falls
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RandomizeCorrectCubes();
            HintOff();
        }
    }
}
