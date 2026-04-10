using UnityEngine;

public class FinalXRCameraAreaChecker : MonoBehaviour
{
    [Header("XR Main Camera")]
    public Transform xrCamera;

    // Static correct camera position (from your log)
    private Vector3 correctCameraPosition = new Vector3(57.32f, -6.23f, -0.18f);

    // Threshold in each direction (±1.5 meters)
    private float threshold = 1.5f;

    private bool triggered = false;

    void LateUpdate() // LateUpdate = after XR tracking updates
    {
        if (triggered || xrCamera == null) return;

        Vector3 camPos = xrCamera.position;

        bool insideX = camPos.x >= correctCameraPosition.x - threshold &&
                       camPos.x <= correctCameraPosition.x + threshold;

        bool insideY = camPos.y >= correctCameraPosition.y - threshold &&
                       camPos.y <= correctCameraPosition.y + threshold;

        bool insideZ = camPos.z >= correctCameraPosition.z - threshold &&
                       camPos.z <= correctCameraPosition.z + threshold;

        if (insideX && insideY && insideZ)
        {
            triggered = true;
            OnCameraInsideCorrectArea();
        }
    }

    void OnCameraInsideCorrectArea()
    {
        Debug.Log("✅ Camera is inside correct area (±1.5m in all directions)");
    }
}