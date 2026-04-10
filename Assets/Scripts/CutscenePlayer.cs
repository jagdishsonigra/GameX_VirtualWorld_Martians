using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using System.Collections.Generic;

public class CutscenePlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName = "GameScene";

    InputDevice rightHand;
    bool lastButtonState;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void Update()
    {
        bool skipPressed = false;

        // Quest Controller (A button)
        if (rightHand.isValid)
        {
            if (rightHand.TryGetFeatureValue(CommonUsages.primaryButton, out bool aPressed))
            {
                skipPressed = aPressed && !lastButtonState;
                lastButtonState = aPressed;
            }
        }
        else
        {
            rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        }

        // Keyboard fallback (Space key)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            skipPressed = true;
        }

        // If skip pressed → load next scene
        if (skipPressed)
        {
            SkipCutscene();
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        LoadNextScene();
    }

    void SkipCutscene()
    {
        videoPlayer.Stop(); 
        LoadNextScene();
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}