using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunCrystalMeter : MonoBehaviour {
    public GameObject sunCrystalMeter;
    public GameObject indicator;
    public GameObject successArea;
    public GameObject meterBackground;
    public float speed = 0.01f;
    //
    private float barWidth;
    private float indicatorInitialX;
    private RectTransform indicatorRectangleTransform;
    private RectTransform sucessAreaRectangleTransform;
    // bar min x and y
    private float minimum;
    private float maximum;
    private float t;

	// Use this for initialization
	void Start () {
        barWidth = meterBackground.GetComponent<RectTransform>().rect.width;
        indicatorRectangleTransform = indicator.GetComponent<RectTransform>();
        indicatorInitialX = indicatorRectangleTransform.rect.x;
        minimum = indicatorRectangleTransform.localPosition.x;
        maximum = minimum + barWidth;
    }
	
	// Update is called once per frame
	void Update () {
        indicatorRectangleTransform.localPosition = new Vector3(Mathf.Lerp(minimum, maximum, t), 
            indicatorRectangleTransform.localPosition.y, 
            indicatorRectangleTransform.localPosition.z);
        //indicatorRectangleTransform.Translate(Mathf.Lerp(minimum, maximum, t), 0, 0);
        t += speed * Time.deltaTime;
        if (t > 1.0f)
        {
            float temp = maximum;
            maximum = minimum;
            minimum = temp;
            t = 0.0f;
        }
    }

    /// <summary>
    /// Checks if the activation is successful depending on where there is in the indicator.
    /// </summary>
    public bool Activate()
    {
        return false;
    }


}
