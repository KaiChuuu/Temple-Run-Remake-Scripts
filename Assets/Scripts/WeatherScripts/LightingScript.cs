using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingScript : MonoBehaviour
{
    bool secret = false;

    float duration = 5.0f;
    Color color_0 = Color.red;
    Color color_1 = Color.blue;

    Light currentLight;

    // Start is called before the first frame update
    void Start()
    {
        currentLight = GetComponent<Light>();

        if (WeatherHandler.Instance.gameWeather == WeatherType.NIGHT)
        {
            currentLight.color = new Color32(94, 91, 81, 255);
        }
        else if(WeatherHandler.Instance.gameWeather == WeatherType.SECRET)
        {
            secret = true;
        }
        else
        {
            currentLight.color = new Color32(229, 200, 200, 255);
        }
    }

    void Update()
    {
        if (secret)
        {
            colorCycle();
        }
    }

    private void colorCycle()
    {
        float t = Mathf.PingPong(Time.time, duration) / duration;
        currentLight.color = Color.Lerp(color_0, color_1, t);
    }
}
