using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Splines;
using static UnityEditor.FilePathAttribute;

public class Cuy : MonoBehaviour
{
    [SerializeField] private SplineContainer path;
    [SerializeField] private float speed;

    private float distabcePorcentage = 0f;
    private float pathLength;
    private bool active = true;
    private Vector3 inverseScale = new Vector3(1, -1, 1);

    void Start()
    {
        pathLength = path.CalculateLength();
    }

    // Update is called once per frame
    void Update()
    {
        if (!active) return;

        distabcePorcentage += speed * Time.deltaTime / pathLength;

        Vector3 currentPosition = path.EvaluatePosition(distabcePorcentage);
        Vector3 pointerPosition = path.EvaluatePosition(distabcePorcentage + 0.01f);
        transform.position = currentPosition;

        Rotate_Cuy(pointerPosition-currentPosition);

        if(distabcePorcentage > 1f)
        {
            active = false;
        }
    }

    private void Rotate_Cuy(Vector3 dir) 
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (distabcePorcentage >= 0.3f && distabcePorcentage < 0.75f) 
        {
            transform.localScale = inverseScale;
        }
        else if(distabcePorcentage < 0.3f || distabcePorcentage >= 0.75f) 
        {
            transform.localScale = Vector3.one;
        }
    }
}
