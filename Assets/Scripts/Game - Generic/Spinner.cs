using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    public float spinRate = 0f;
    void Update()
    {
        transform.Rotate(0, 0, spinRate);
    }
}
