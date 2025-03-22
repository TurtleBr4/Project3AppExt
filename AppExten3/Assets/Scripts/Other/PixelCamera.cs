using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PixelCamera : MonoBehaviour
{
    public int pixelSize = 8;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // Create a temporary RenderTexture to hold the pixelated image
        RenderTexture temp = RenderTexture.GetTemporary(source.width / pixelSize, source.height / pixelSize, 0);

        // Set the filter mode to point to create a pixelated look
        temp.filterMode = FilterMode.Point;

        // Blit the source texture to the temporary pixelated texture
        Graphics.Blit(source, temp);

        // Blit the pixelated texture back to the destination
        Graphics.Blit(temp, destination);

        // Release the temporary texture
        RenderTexture.ReleaseTemporary(temp);
    }
}