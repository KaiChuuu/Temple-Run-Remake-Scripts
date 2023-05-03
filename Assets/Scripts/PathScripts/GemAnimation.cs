using System;
using System.Collections;
using UnityEngine;

public class GemAnimation : MonoBehaviour
{
    private Vector3 pos1 = new Vector3(0, 0.1f, 0);
    private Vector3 pos2 = new Vector3(0, -0.4f, 0);

    public int rotationSpeed;

    public float offset;

    // Start is called before the first frame update
    void Start()
    {
        pos1 += transform.position;
        pos2 += transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        transform.position = Vector3.Lerp(pos1, pos2, (Mathf.Sin(Time.time + offset) + 1.0f) / 2.0f);
    }
}
