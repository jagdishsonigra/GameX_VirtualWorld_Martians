using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class KnobRotateModel : MonoBehaviour
{
    public XRKnob knob;           // Drag your XRKnob here
    public Transform targetModel; // Drag your 3D model here

    public bool invertRotation = false; // ✔ tick this to invert direction

    void Start()
    {
        if (knob != null)
            knob.onValueChange.AddListener(OnKnobValueChanged);
    }

    void OnDestroy()
    {
        if (knob != null)
            knob.onValueChange.RemoveListener(OnKnobValueChanged);
    }

    void OnKnobValueChanged(float value)
    {
        // Invert if checked
        if (invertRotation)
            value = 1f - value;

        // Map knob value (0–1) to rotation (0–180)
        float yAngle = Mathf.Lerp(0f, 180f, value);

        // Apply rotation only on Y axis
        targetModel.localEulerAngles = new Vector3(
            targetModel.localEulerAngles.x,
            yAngle,
            targetModel.localEulerAngles.z
        );
    }
}
