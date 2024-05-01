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
    private float targetSpeed;
    private float currentVelocity;
    private Vector3 inverseScale = new Vector3(1, -1, 1);

    public float DistancePorcentage { get => distancePorcentage; }
    public float Speed { get => speed; set => speed = value; }
    public int CUY_ID { get => cuyID; }
    public float TargetSpeed { set => targetSpeed = value; }

    void Start()
    {
        pathLength = path.CalculateLength();
    }


    void Update()
    {
        if (!active) return;

        //Se suaviza la velocidad del cuy
        speed = Mathf.SmoothDamp(speed, targetSpeed, ref currentVelocity, 1);

        distancePorcentage += speed * Time.deltaTime / pathLength;

        //Se evalúa el punto de posición del cuy y uno próximo para determina su rotación
        Vector3 currentPosition = path.EvaluatePosition(distancePorcentage);
        Vector3 pointerPosition = path.EvaluatePosition(distancePorcentage + 0.01f);
        transform.position = currentPosition;

        Rotate_Cuy(pointerPosition-currentPosition);

        // Si el cuy llega al final del camino, se detiene
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

        //Se invierte la escala en ciertos tramos para asegurar una correcta orientación
        if (distancePorcentage >= 0.3f && distancePorcentage < 0.75f) 
        {
            transform.localScale = inverseScale;
        }
        else if(distancePorcentage < 0.3f || distancePorcentage >= 0.75f) 
        {
            transform.localScale = Vector3.one;
        }
    }

    public void Reset_Position() 
    {
        speed = 0f;
        distancePorcentage = 0f;
        transform.position = path.EvaluatePosition(distancePorcentage);
        active = true;

    }
}
