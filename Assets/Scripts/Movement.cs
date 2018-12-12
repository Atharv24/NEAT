using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have
    public Transform[] sensors;
    public float sensorRange;
    private Network net;
    private bool initialized = false;
    public float timeSurvived = 0f;
    public float distanceCovered = 0f;
    private Rigidbody rgbd;

    void Start()
    {
        rgbd = GetComponent<Rigidbody>();
    }

    void Update ()
    {
        if(initialized)
        {
            timeSurvived += Time.deltaTime;
            distanceCovered += Vector3.Dot(rgbd.velocity, transform.forward) * Time.deltaTime;

            float[] output = net.GetOutput(SensorInput());
            float motor = maxMotorTorque * output[0];
            float steering = maxSteeringAngle * output[1];

            foreach (AxleInfo axleInfo in axleInfos)
            {
                if (axleInfo.steering)
                {
                    axleInfo.leftWheel.steerAngle = steering;
                    axleInfo.rightWheel.steerAngle = steering;
                }
                if (axleInfo.motor)
                {
                    axleInfo.leftWheel.motorTorque = motor;
                    axleInfo.rightWheel.motorTorque = motor;
                }
            }
        }
	}

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Wall")
        {
            initialized = false;
            net.SetFitness(distanceCovered * distanceCovered / timeSurvived);
            gameObject.SetActive(false);
        }
    }

    private float[] SensorInput()
    {
        float[] distances = new float[sensors.Length];

        for (int i=0; i<sensors.Length; i++)
        {
            RaycastHit hit;
            if(Physics.Raycast(sensors[i].position, sensors[i].forward, out hit, sensorRange))
            {
                if(hit.collider.tag=="Wall")
                {
                    distances[i] = hit.distance;
                    Debug.DrawRay(sensors[i].position, sensors[i].TransformDirection(Vector3.forward) * hit.distance, Color.red);
                }
            }
            else
            {
                distances[i] = sensorRange;
            }
        }

        return distances;
    }

    public void SetNetwork(Network net)
    {
        this.net = net;
    }

    public void Init()
    {
        initialized = true;
    }

    [System.Serializable]
    public class AxleInfo
    {
        public WheelCollider leftWheel;
        public WheelCollider rightWheel;
        public bool motor; // is this wheel attached to motor?
        public bool steering; // does this wheel apply steer angle?
    }

}
