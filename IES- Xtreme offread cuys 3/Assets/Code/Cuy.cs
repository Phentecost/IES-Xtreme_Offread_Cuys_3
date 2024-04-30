using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Splines;

public class Cuy : MonoBehaviour
{
    [SerializeField] private SplineContainer path;
    [SerializeField] private int cuyID;
    private float speed;
    private float distancePorcentage = 0f;
    private float pathLength;
    private bool active = true;
    private Vector3 inverseScale = new Vector3(1, -1, 1);

    public float DistancePorcentage { get => distancePorcentage; }
    public float Speed { get => speed; set => speed = value; }
    public int CUY_ID { get => cuyID; }

    void Start()
    {
        pathLength = path.CalculateLength();
    }

    // Update is called once per frame
    void Update()
    {
        if (!active) return;

        distancePorcentage += speed * Time.deltaTime / pathLength;

        Vector3 currentPosition = path.EvaluatePosition(distancePorcentage);
        Vector3 pointerPosition = path.EvaluatePosition(distancePorcentage + 0.01f);
        transform.position = currentPosition;

        Rotate_Cuy(pointerPosition-currentPosition);

        if(distancePorcentage > 1f)
        {
            GameManager.instance.On_Cuy_Reach_Goal(this);
            active = false;
        }
    }

    private void Rotate_Cuy(Vector3 dir) 
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (distancePorcentage >= 0.3f && distancePorcentage < 0.75f) 
        {
            transform.localScale = inverseScale;
        }
        else if(distancePorcentage < 0.3f || distancePorcentage >= 0.75f) 
        {
            transform.localScale = Vector3.one;
        }
    }
}
