using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    public float WaitAngle = Mathf.PI/4;
    public float AngularSpeed = 0.1f;
    public float Distance = 2;

    
    public float currentAngle = 0;
    public float desiredAngle = 0;

    public float cam;

    GameObject cameraReference;


	// Use this for initialization
	void Start () {
        cameraReference = GameObject.FindGameObjectWithTag("MainCamera");
	}
    private void Update()
    {
        Vector3 cameraDirection = Vector3.ProjectOnPlane(cameraReference.transform.forward, Vector3.up).normalized;
        float cameraAngle = Mathf.Atan2(cameraDirection.z, cameraDirection.x);

        
        if(Mathf.Abs(DistanceBetweenAngles(cameraAngle, currentAngle) )> WaitAngle){
            desiredAngle = cameraAngle;
            //Debug.Log("setting desired");
        }
        else
        {
            //Debug.LogFormat("camerangle: {0}, currentangle: {1}", cameraAngle, currentAngle);
        }

        currentAngle = BringAngleCloser(currentAngle, desiredAngle, AngularSpeed);

        transform.position = cameraReference.transform.position + new Vector3(Mathf.Cos(currentAngle),0, Mathf.Sin(currentAngle)) * Distance;

        transform.eulerAngles = new Vector3(0, 90-currentAngle * Mathf.Rad2Deg,0);

    }

    float BringAngleCloser(float a, float b, float amount)
    {
        float d = DistanceBetweenAngles(a, b);
        if (Mathf.Abs(d) < amount)
        {
            return b;
        }
        else if (d > 0)
        {
            return a + amount;
        }
        else
        {
            return a - amount;
        }
    }

    float DistanceBetweenAngles(float a, float b)
    {
        float c = b - a;
        while (c < -Mathf.PI)
        {
            c += Mathf.PI * 2;
        }
        while (c > Mathf.PI)
        {
            c -= Mathf.PI * 2;
        }
        return c;
    }

}
