using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;


public class MaskAttach : MonoBehaviour
{
    public Transform faceAttachPoint;
    public float snapDistance = 0.2f;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;
    private Rigidbody rb;

    private bool isAttached = false;

    public Renderer maskRenderer;


    void Start()
    {
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        grab.selectEntered.AddListener(OnGrab);
        maskRenderer = GetComponent<Renderer>();
        maskRenderer.enabled = true;
    }

    void Update()
    {
        if (!grab.isSelected || isAttached) return;

        float dist = Vector3.Distance(transform.position, faceAttachPoint.position);

        if (dist <= snapDistance)
        {
            AttachToFace();
        }
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        // If already attached and user grabs it → detach
        if (isAttached)
        {
            DetachFromFace();
        }
    }

    void AttachToFace()
    {
        isAttached = true;

        // Parent to face
        transform.SetParent(faceAttachPoint);

        // Snap position
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        // Physics off
        rb.isKinematic = true;
        rb.useGravity = false;

        Debug.Log("🎭 Mask Attached");
        StartCoroutine(HideAfterDelay());

    }
    IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        maskRenderer.enabled = false;
        Debug.Log("👻 Mask hidden after 2 seconds");
    }


    void DetachFromFace()
    {
        isAttached = false;

        // Unparent
        transform.SetParent(null);

        // Physics back on
        rb.isKinematic = false;
        rb.useGravity = true;

        Debug.Log("❌ Mask Detached");
        maskRenderer.enabled = true;
    }
}
