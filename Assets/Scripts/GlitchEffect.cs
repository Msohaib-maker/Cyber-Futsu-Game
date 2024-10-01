using System.Collections;
using TMPro;
using UnityEngine;

public class GlitchEffect : MonoBehaviour
{
    public TMP_Text textMeshPro;      // Reference to the TextMeshPro component
    public float glitchDuration = 0.1f;  // How long each glitch lasts
    public float glitchInterval = 0.3f;  // Time between glitches
    public Vector3 positionJitter = new Vector3(0.5f, 0.5f, 0f); // Position jitter values for the glitch effect
    public Color glitchColor = Color.red; // The color of the glitch effect

    private string originalText;
    private Vector3[] originalVertices;
    private TMP_TextInfo textInfo;

    void Start()
    {
        // Get the TextMeshPro component if not assigned
        if (textMeshPro == null)
        {
            textMeshPro = GetComponent<TMP_Text>();
        }

        // Cache the original text and the text info
        originalText = textMeshPro.text;
        textInfo = textMeshPro.textInfo;

        // Start the glitch effect loop
        StartCoroutine(GlitchLoop());
    }

    IEnumerator GlitchLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(glitchInterval);
            StartCoroutine(ApplyGlitch());
        }
    }

    IEnumerator ApplyGlitch()
    {
        textMeshPro.ForceMeshUpdate(); // Update the text mesh data

        // Store the original vertices of the text mesh
        originalVertices = textInfo.meshInfo[0].vertices;

        // Loop through each character in the text
        for (int i = 0; i < textMeshPro.text.Length; i++)
        {
            // Check if the character is visible (not a space or special char)
            if (textInfo.characterInfo[i].isVisible)
            {
                // Get the index of the character in the mesh
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                // Randomly offset the vertices to create the glitch effect
                Vector3 jitterOffset = new Vector3(
                    Random.Range(-positionJitter.x, positionJitter.x),
                    Random.Range(-positionJitter.y, positionJitter.y),
                    Random.Range(-positionJitter.z, positionJitter.z)
                );

                for (int j = 0; j < 4; j++)
                {
                    textInfo.meshInfo[0].vertices[vertexIndex + j] = originalVertices[vertexIndex + j] + jitterOffset;
                }

                // Randomly change the character color to the glitch color
                Color32[] newVertexColors = textInfo.meshInfo[0].colors32;
                newVertexColors[vertexIndex] = glitchColor;
            }
        }

        // Update the text mesh with the new vertex data
        textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.All);

        // Wait for the glitch duration, then revert to the original state
        yield return new WaitForSeconds(glitchDuration);

        // Restore the original vertices
        for (int i = 0; i < textMeshPro.text.Length; i++)
        {
            if (textInfo.characterInfo[i].isVisible)
            {
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                for (int j = 0; j < 4; j++)
                {
                    textInfo.meshInfo[0].vertices[vertexIndex + j] = originalVertices[vertexIndex + j];
                }
            }
        }

        // Restore the original text mesh
        textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
    }
}
