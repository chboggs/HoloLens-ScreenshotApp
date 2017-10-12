using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    public float WaitAngle = Mathf.PI/4;
    public float AngularSpeed = 0.1f;
    public float Distance = 2;

    float currentAngle = 0;
    float desiredAngle = 0;

    GameObject cameraReference;

    public float distance;

	// Use this for initialization
	void Start () {
        cameraReference = GameObject.FindGameObjectWithTag("MainCamera");
	}
    private void Update()
    {
        Vector3 cameraDirection = Vector3.ProjectOnPlane(cameraReference.transform.forward, Vector3.up).normalized;
        float cameraAngle = Mathf.Atan2(cameraDirection.y, cameraDirection.x);

        if(Mathf.Abs(DistanceBetweenAngles(cameraAngle, currentAngle) )> WaitAngle){
            desiredAngle = cameraAngle;
        }

        currentAngle = BringAngleCloser(currentAngle, desiredAngle, AngularSpeed);

        transform.position = cameraReference.transform.position + new Vector3(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle), 0) * Distance;

        transform.Rotate(new Vector3(0, 0, currentAngle * Mathf.Rad2Deg));

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
