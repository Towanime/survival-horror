using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour
{
    public Texture2D crosshairTexture;
    public Texture2D crosshairActiveTexture;

    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 5);
    }

    void OnGUI()
    {   
        UnityEngine.Cursor.visible = false;

        Texture2D activeTexture;
        activeTexture = crosshairTexture;
        if (activeTexture != null)
        {
            Rect position = new Rect((Screen.width - activeTexture.width) / 2, (Screen.height - activeTexture.height) / 2, activeTexture.width, activeTexture.height);
            GUI.DrawTexture(position, activeTexture);
        }
    }
}