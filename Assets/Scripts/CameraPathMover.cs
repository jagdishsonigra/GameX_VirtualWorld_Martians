using UnityEngine;
using System.Collections;

public class CameraPathMover : MonoBehaviour
{
    [Header("Path Points (Start → Middle → End)")]
    public Transform[] pathPoints;   // assign points in order

    [Header("Movement Settings")]
    public float moveSpeed = 2f;

    private int currentIndex = 0;

    void Start()
    {
        if (pathPoints == null || pathPoints.Length < 2)
        {
            Debug.LogWarning("CameraPathMover: Not enough path points assigned.");
            return;
        }

        // Start at first point
        transform.position = pathPoints[0].position;

        // Begin movement automatically
        StartCoroutine(MoveAlongPath());
    }

    IEnumerator MoveAlongPath()
    {
        while (currentIndex < pathPoints.Length)
        {
            Transform target = pathPoints[currentIndex];

            while (Vector3.Distance(transform.position, target.position) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    target.position,
                    moveSpeed * Time.deltaTime
                );

                yield return null;
            }

            currentIndex++;
            yield return null;
        }

        Debug.Log("📷 Camera movement finished");
    }
}
