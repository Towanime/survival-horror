using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour
{
    public Camera crosshairCamera;
    public Texture2D crosshairTexture;
    public Texture2D crosshairActiveTexture;

    void Update()
    {
        if (isCameraEnabled())
        {
            Debug.DrawRay(crosshairCamera.transform.position, crosshairCamera.transform.forward * 5);
        }
    }

    void OnGUI()
    {   
        UnityEngine.Cursor.visible = false;

        Texture2D activeTexture;
        activeTexture = crosshairTexture;
        if (activeTexture != null && isCameraEnabled())
        {
            Rect position = new Rect((Screen.width - activeTexture.width) / 2, (Screen.height - activeTexture.height) / 2, activeTexture.width, activeTexture.height);
            GUI.DrawTexture(position, activeTexture);
        }
    }

    private bool isCameraEnabled()
    {
        return crosshairCamera.isActiveAndEnabled && crosshairCamera.gameObject.activeInHierarchy;
    }
}