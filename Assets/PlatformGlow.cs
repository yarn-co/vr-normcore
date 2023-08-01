using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGlow : MonoBehaviour
{
    private Renderer rend;
    private Color baseColor;
    public Color targetColor; // Set this color in the inspectorwas

    private float glowTime = 0.0f;  // Time tracking for linear glow change
    private float transitionTime = 0.0f;

    public float glowSpeed = 1.0f;  // Speed of glow effect
    public float colorChangeSpeed = 0.5f;  // Speed of color transition
    public float maxGlowIntensity = 1.0f;  // Maximum intensity for the glow
    public float minGlowIntensity = 0.0f;  // Minimum intensity for the glow

    private bool playerIsOn = false;
    private bool returningToBase = false;
    private bool waiting = false;
    private Color startTransitionColor; // The starting color for the transition
    private Color endTransitionColor;
    private float emissionIntensity;

    /*float GetIntensity(Color color)
    {
        return 0.299f * color.r + 0.587f * color.g + 0.114f * color.b;  // luminance
    }*/

    void Start()
    {
        rend = GetComponent<Renderer>();
        baseColor = rend.material.GetColor("_EmissionColor");
        Debug.Log("baseColor: " + baseColor + " baseColor.r " + baseColor.r);
    }

    void UpdateGlow()
    {
        if (!playerIsOn && !returningToBase && !waiting)
        {
            glowTime += Time.deltaTime * glowSpeed;
            if (glowTime > 2.0f) // Reset after oscillating between min and max intensities
                glowTime = 0.0f;

            emissionIntensity = Mathf.Lerp(minGlowIntensity, maxGlowIntensity, Mathf.PingPong(glowTime, 1.0f));
            rend.material.SetColor("_EmissionColor", baseColor * emissionIntensity);
        }
    }

    void UpdateColorChange()
    {
        if (playerIsOn)
        {
            transitionTime += Time.deltaTime * colorChangeSpeed;
            Color currentColor = Color.Lerp(startTransitionColor, targetColor, transitionTime);
            rend.material.SetColor("_EmissionColor", currentColor);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Enter");
        if (collision.gameObject.CompareTag("Player"))
        {
            StopCoroutine(ReturnToGlow());
            playerIsOn = true;
            returningToBase = false;
            startTransitionColor = rend.material.GetColor("_EmissionColor");  // Set the start transition color to the current emission color
            transitionTime = 0.0f;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        Debug.Log("Time: " + Time.time + "Collision Exit: " + collision.gameObject);
        if (collision.gameObject.CompareTag("Player"))
        {
            playerIsOn = false;
            endTransitionColor = rend.material.GetColor("_EmissionColor");
            waiting = true;
            StartCoroutine(DelayedExitLogic());
        }
    }

    IEnumerator DelayedExitLogic()
    {
        yield return new WaitForSeconds(0.1f); // Wait for 0.1 seconds
        if (!playerIsOn) // Only proceed if the player is still off the platform after the delay
        {
            Debug.Log("Starting return to glow coroutine...");
            StartCoroutine(ReturnToGlow());
        }
        waiting = false;
    }

    System.Collections.IEnumerator ReturnToGlow()
    {
        returningToBase = true;
        float elapsed = 0.0f;

        int counter = 0;
        while (elapsed < 1.0f)
        {
            elapsed += Time.deltaTime * colorChangeSpeed;

            // Lerp color
            Color currentColor = Color.Lerp(endTransitionColor, baseColor * maxGlowIntensity, elapsed);
            rend.material.SetColor("_EmissionColor", currentColor);

            counter++;
            if (counter % 300 == 0)
            {
                Debug.Log($"Elapsed: {elapsed}, Current Color: {currentColor}, Start color: {endTransitionColor}, Target color: {baseColor * maxGlowIntensity}");
            }
            yield return null;
        }
        glowTime = 1.0f;
        returningToBase = false;
    }

    void Update()
    {
        UpdateGlow();
        UpdateColorChange();
    }
}
