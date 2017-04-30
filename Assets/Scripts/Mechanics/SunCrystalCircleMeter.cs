using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SunCrystalCircleMeter : MonoBehaviour {
    // utility struct for success scale ranges 
    [System.Serializable]
    public struct Range
    {
        public float from;
        public float to;
    }
    // variables
    public GameObject indicator;
    public GameObject outerCircleI;
    public GameObject outerCircleII;
    public GameObject outerCircleIII;
    public Range successRangeCircleI;
    public Range successRangeCircleII;
    public Range successRangeCircleIII;
    public Animator animator;
    [Range(1,3)]
    public int initialCircleState = 1;
    public float indicatorInitialScale = 0.1f;
    public float overgrowBy = 0.5f;
    public float secondsToFill = 2.5f;
    public Color onSuccessColor = Color.green;
    public Color onFailureColor = Color.red;
    public Light crystalLight;
    public float crystalFadingTime = 0.2f;
    public float minimumLightIntensity = 0f;
    public float maximumLightIntensity = 5f;
    // circle scale vairables
    private float currentIndicatorScale;
    private RectTransform indicatorRectangleTransform;
    private float maximumScale;
    private float currentTime;
    private float traversedTime;
    private Range currentRange;
    private Image currentCircleImage;
    // used to know if the circle needs to change color
    private bool activated;
    // activation fade variables
    private Color initialColor; // color to go back to when failed
    private Color activationColor;
    private float currentColorFadeTime;
    private float remainingTimeColor;
    // fade variables for the indicator
    private Image indicatorImage;
    private float currentIndicatorFadeTime;
    private float remainingIndicatorFadingTime;
    private bool fadingIndicator;
    // point light variables
    private bool lit; // true as long as the player keeps clicking at the right time
    private bool justLit; // used to check if a player
    private bool fadingLight;
    private float currentLightTime;
    private float traversedTimeLight;
    // temporal light range
    private float intensityFrom;
    private float intensityTo;


    // Use this for initialization
    void Start () {
        indicatorRectangleTransform = indicator.GetComponent<RectTransform>();
        indicatorImage = indicator.GetComponent<Image>();
        currentCircleImage = outerCircleI.GetComponent<Image>();
        initialColor = currentCircleImage.color;
        SetCircleState(initialCircleState);
    }
	
	// Update is called once per frame
	void Update () {
        traversedTime = currentTime / secondsToFill;
        float scale = Mathf.Lerp(indicatorInitialScale, maximumScale, traversedTime);
        indicatorRectangleTransform.localScale = new Vector3(scale, scale, 0);
        currentTime += Time.deltaTime;

        // if the scale is out of the limit then start fading
         if (indicatorRectangleTransform.localScale.x >= currentRange.to)
        {
            if (remainingIndicatorFadingTime == 0)
            {
                // set remaining time until the overgrow finishes
                remainingIndicatorFadingTime = secondsToFill - currentTime;
            }
            Color temp = indicatorImage.color;
            temp.a = Mathf.Lerp(1, 0, currentIndicatorFadeTime / remainingIndicatorFadingTime);
            indicatorImage.color = temp;
            currentIndicatorFadeTime += Time.deltaTime;
            fadingIndicator = true;
        }
        // indicator is on full scale?
        if (traversedTime > 1.0f)
        {
            traversedTime = 0.0f;
            currentTime = 0.0f;
            currentColorFadeTime = 0.0f;
            currentCircleImage.color = initialColor;//Color.white;
            currentIndicatorFadeTime = 0;
            remainingIndicatorFadingTime = 0;
            activated = false;
            fadingIndicator = false;
            // turn off the light if it wasn't pressed right this time
            if (justLit != lit)
            {
                fadingLight = true;
                FadeOutLight();
                //IsLightFading();
            }
            lit = justLit;
            justLit = false; // reset for next loop
            Invoke("ShowIndicator", 0.1f);
        }
        // fade color if necessary
        FadeColor();

        // fade light
        FadeLight();
    }

    private void FadeColor()
    {
        // color on the circle
        if (activated)
        {
            currentCircleImage.color = Color.Lerp(activationColor, initialColor/*Color.white*/, currentColorFadeTime / remainingTimeColor);
            currentColorFadeTime += Time.deltaTime;
        }
    }

    private void FadeLight()
    {
        if (fadingLight)
        {
            traversedTimeLight = currentLightTime / crystalFadingTime;
            crystalLight.intensity = Mathf.Lerp(intensityFrom, intensityTo, traversedTimeLight);
            currentLightTime += Time.deltaTime;

            if (traversedTimeLight > 1.0f)
            {
                fadingLight = false;
                currentLightTime = 0;
            }
        }
    }

    private void ShowIndicator()
    {
        indicatorImage.color = Color.white;
    }

    private void SetCircleState(int circleState)
    {
        switch (circleState)
        {
            default:
            case 1:
                currentCircleImage.enabled = false;
                currentRange = successRangeCircleI;
                maximumScale = currentRange.to + overgrowBy;
                currentCircleImage = outerCircleI.GetComponent<Image>();
                currentCircleImage.enabled = true;
                break;
            case 2:
                currentCircleImage.enabled = false;
                currentRange = successRangeCircleII;
                maximumScale = currentRange.to + overgrowBy;
                currentCircleImage = outerCircleII.GetComponent<Image>();
                currentCircleImage.enabled = true;
                break;
            case 3:
                currentCircleImage.enabled = false;
                currentRange = successRangeCircleIII;
                maximumScale = currentRange.to + overgrowBy;
                currentCircleImage = outerCircleIII.GetComponent<Image>();
                currentCircleImage.enabled = true;
                break;
        }
        indicatorRectangleTransform.localScale = new Vector3(indicatorInitialScale, indicatorInitialScale, 0);
    }

    /// <summary>
    /// Checks if the activation is successful depending on where there is in the indicator.
    /// </summary>
    public void Activate()
    {
        if (activated) return;

        activated = true;
        remainingTimeColor = secondsToFill - currentTime;
        float currentScale = indicatorRectangleTransform.localScale.x;
        // is between the success scales?
        if (currentScale >= currentRange.from && currentScale <= currentRange.to)
        {
            currentCircleImage.color = onSuccessColor;
            activationColor = onSuccessColor;
            justLit = true;
            lit = true;
            intensityFrom = minimumLightIntensity;
            intensityTo = maximumLightIntensity;
            // trigger success
            animator.SetTrigger("Success");
        }else
        {
            currentCircleImage.color = onFailureColor;
            activationColor = onFailureColor;
            lit = false;
            FadeOutLight();
            /*intensityFrom = crystalLight.intensity;
            intensityTo = minimumLightIntensity;
            // reset light time when fading out
            currentLightTime = 0;*/
        }
        IsLightFading();
    }

    private void FadeOutLight()
    {
        intensityFrom = crystalLight.intensity;
        intensityTo = minimumLightIntensity;
        // reset light time when fading out
        currentLightTime = 0;
    }

    /// <summary>
    /// Check if the light should fade in or out.
    /// </summary>
    private void IsLightFading()
    {
        if (crystalLight.intensity != intensityTo)
        {
            fadingLight = true;
        }
        else
        {
            fadingLight = false;
        }
    }

    public bool IsLit
    {
        get { return lit; }
    }


}
