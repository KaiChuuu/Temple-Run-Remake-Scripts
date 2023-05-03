using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeatherType
{
    DAY,
    NIGHT,
    SECRET
}

public class WeatherHandler : MonoBehaviour
{
    public static WeatherHandler Instance;

    public WeatherType gameWeather;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        RandomizeWeather();
    }

    public void RandomizeWeather()
    {
        int randomWeather = Random.Range(0, 2);
        
        if(randomWeather == 0)
        {
            gameWeather = WeatherType.DAY;
        }
        else if(randomWeather == 1)
        {
            gameWeather = WeatherType.NIGHT;
        }
        else
        {
            gameWeather = WeatherType.DAY;
        }
    }
}
