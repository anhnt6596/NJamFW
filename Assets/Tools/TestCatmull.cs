using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestCatmull : MonoBehaviour
{
    [SerializeField] CatmullRomSpline line;
    [SerializeField] float speed = 1;
    float distance = 0;
    private void Update()
    {
        distance += speed * Time.deltaTime;
        transform.position = line.GetPointByDistance(distance);
    }
}
