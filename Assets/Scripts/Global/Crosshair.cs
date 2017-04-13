using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour
{
    public float lineWidth = 2;
    public float lineHeight = 16;
    public int basePadding = 3;
    
    public Camera playerCamera;

    private Texture2D crosshairTexture;
    private Rect rect;

    void Start()
    {
        crosshairTexture = new Texture2D(1, 1); //Creates 2D texture
        crosshairTexture.SetPixel(1, 1, Color.white); //Sets the 1 pixel to be white
        crosshairTexture.Apply();
    }

    void OnGUI()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        float degreesToRotate = Mathf.Lerp(0, 180, 360);//inventory.GetCurrentWeapon().CurrentSpreadRange);
        Quaternion cameraRotation = playerCamera.transform.rotation;
        playerCamera.transform.Rotate(0, degreesToRotate, 0);
        Vector3 direction = playerCamera.transform.forward;
        playerCamera.transform.rotation = cameraRotation;

        Vector3 rangeWorldPoint = playerCamera.transform.position + direction.normalized;

        //Debug.DrawRay(playerCamera.transform.TransformPoint(Vector3.zero), direction.normalized * 3f, Color.red, 0.5f);

        Vector3 rangeScreenPoint = playerCamera.WorldToScreenPoint(rangeWorldPoint);
        float rangePadding = Mathf.Abs(rangeScreenPoint.x - screenCenter.x);

        // top
        rect.Set(screenCenter.x - lineWidth / 2, screenCenter.y - lineHeight - basePadding - rangePadding, lineWidth, lineHeight);
        GUI.DrawTexture(rect, crosshairTexture);
        // right
        rect.Set(screenCenter.x + basePadding + rangePadding, screenCenter.y - lineWidth / 2, lineHeight, lineWidth);
        GUI.DrawTexture(rect, crosshairTexture);
        // bottom
        rect.Set(screenCenter.x - lineWidth / 2, screenCenter.y + basePadding + rangePadding, lineWidth, lineHeight);
        GUI.DrawTexture(rect, crosshairTexture);
        // left
        rect.Set(screenCenter.x - lineHeight - basePadding - rangePadding, screenCenter.y - lineWidth / 2, lineHeight, lineWidth);
        GUI.DrawTexture(rect, crosshairTexture);
    }
}