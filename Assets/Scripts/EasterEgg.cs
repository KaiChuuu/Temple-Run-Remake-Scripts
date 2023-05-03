using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEgg : MonoBehaviour
{
    int counter = 10;

    private void OnMouseDown()
    {
        counter--;
        if(counter == 1)
        {
            WeatherHandler.Instance.gameWeather = WeatherType.SECRET;
        }
    }
}
