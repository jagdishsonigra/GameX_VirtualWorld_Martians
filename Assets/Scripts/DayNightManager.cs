using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class DayNightManager : MonoBehaviour
{
    public GameObject dayEnvironment;
    public GameObject nightEnvironment;

    public Renderer maskRenderer;

    [Header("Hint System")]
    public BoxHintSystem boxHintSystem;

    public float hideDelay = 1f; // wait time before hiding mask

    [Header("Audios")]
    public AudioSource dayAudio;
    public AudioSource nightAudio;
    public AudioSource Mask2;

    private bool firstTimeNight = true;

   
    void Start()
    {
        SwitchToDay();
        PlayDayAudio();
    }

    // 🌙 NIGHT = wait → hide mask → hint ON
    public void SwitchToNight()
    {
        if (dayEnvironment != null)
            dayEnvironment.SetActive(false);

        if (nightEnvironment != null)
            nightEnvironment.SetActive(true);

        // Hint ON
        if (boxHintSystem != null)
            boxHintSystem.HintOn();

        // Start delayed hide
        StartCoroutine(HideMaskAfterDelay());

        if (firstTimeNight)
        {
            firstTimeNight = false;
            StartCoroutine(PlayMaskThenNight());
        }
        else
        {
            PlayNightAudio();
        }

    }

    // ☀️ DAY = show mask instantly → hint OFF
    public void SwitchToDay()
    {
        StopAllCoroutines(); // stop any pending hide

        if (dayEnvironment != null)
            dayEnvironment.SetActive(true);

        if (nightEnvironment != null)
            nightEnvironment.SetActive(false);

        // Show mask instantly
        if (maskRenderer != null)
            maskRenderer.enabled = true;

        // Hint OFF
        if (boxHintSystem != null)
            boxHintSystem.HintOff();

        PlayDayAudio();
    }

    IEnumerator HideMaskAfterDelay()
    {
        yield return new WaitForSeconds(hideDelay);

        if (maskRenderer != null)
            maskRenderer.enabled = false;
    }


    // 🎵 Audio Helpers
    void PlayDayAudio()
    {
        if (nightAudio != null && nightAudio.isPlaying)
            nightAudio.Stop();

        if (dayAudio != null && !dayAudio.isPlaying)
            dayAudio.Play();
    }

    void PlayNightAudio()
    {
        if (dayAudio != null && dayAudio.isPlaying)
            dayAudio.Stop();

        if (nightAudio != null && !nightAudio.isPlaying)
            nightAudio.Play();
    }
    IEnumerator PlayMaskThenNight()
    {
        if (Mask2 != null)
            Mask2.Play();

        // wait until mask instruction finishes
        yield return new WaitWhile(() => Mask2 != null && Mask2.isPlaying);

        PlayNightAudio();
    }


}
