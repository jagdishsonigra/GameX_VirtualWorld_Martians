using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;


[RequireComponent(typeof(CharacterController))]
public class XRJump : MonoBehaviour
{
    [Header("jump")]
    public float jumpForce = 3f;
    public float gravity = -9.8f;

    InputDevice rightHand;
    CharacterController controller;
    float yVelocity;
    bool lastButtonState;

    [Header("sprint")]
    public float normalSpeed = 2f;
    public float sprintSpeed = 5f;

    InputDevice leftHand;
    bool isSprinting;
    
    public DynamicMoveProvider moveProvider;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        bool jumpPressed = false;

        // Quest Controller (A button)
        if (rightHand.isValid)
        {
            if (rightHand.TryGetFeatureValue(CommonUsages.primaryButton, out bool aPressed))
            {
                jumpPressed = aPressed && !lastButtonState;
                lastButtonState = aPressed;
            }
        }
        else
        {
            rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        }

        // Simulator / Keyboard fallback (Space key)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpPressed = true;
        }

        if (jumpPressed)
        {
            yVelocity = jumpForce;
        }

        if (yVelocity < 0)
            yVelocity = -2f;

        yVelocity += gravity * Time.deltaTime;
        controller.Move(Vector3.up * yVelocity * Time.deltaTime);
        // Detect joystick press for sprint (Quest)
       // ---------- SPRINT (Hold Left Joystick Button) ----------
        bool sprintHeld = false;

        if (leftHand.isValid)
        {
            if (leftHand.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool clickPressed))
            {
                sprintHeld = clickPressed;
            }
        }
        else
        {
            leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        }

        // Editor fallback (hold Left Shift)
        #if UNITY_EDITOR
        if (Input.GetKey(KeyCode.LeftShift))
        {
            sprintHeld = true;
        }
        #endif

        // Apply speed while holding
        if (moveProvider != null)
        {
            moveProvider.moveSpeed = sprintHeld ? sprintSpeed : normalSpeed;
        }

    }
}
