using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestCatmullZ : MonoBehaviour
{
    [SerializeField] CatmullRomSpline2D line;
    [SerializeField] float speed = 0.1f;
    [SerializeField] float distance = 0;
    private void Update()
    {
        distance += speed * Time.deltaTime;
        transform.position = line.GetPointByDistance(distance);


    }
}
