using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RaycastLaser : MonoBehaviour
{
    public Transform firePoint;   // where laser starts
    public float maxDistance = 50f;

    public Material redMaterial;
    public Material greenMaterial;

    private LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;

        // Set default laser color to red
        if (redMaterial != null)
            line.material = redMaterial;
    }

    void Update()
    {
        RaycastHit hit;
        Vector3 endPoint;

        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, maxDistance))
        {
            endPoint = hit.point;
        }
        else
        {
            endPoint = firePoint.position + firePoint.forward * maxDistance;
        }

        line.SetPosition(0, firePoint.position);
        line.SetPosition(1, endPoint);
    }
        // ✅ This will be called by Puzzle Manager
    public void SetLaserGreen()
    {
        if (greenMaterial != null)
        {
            line.material = greenMaterial;
            Debug.Log("🟢 Laser turned GREEN!");
        }
    }
}
