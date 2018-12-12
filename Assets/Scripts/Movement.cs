using UnityEngine;

public class Movement : MonoBehaviour {

    public float forwardSpeed;
    public float turningSpeed;
    public Transform[] sensors;
    public float sensorRange;
    private Network net;
    private bool initialized = false;
    private float timeSurvived = 0f;
	
	void Update ()
    {
        if(initialized)
        {
            timeSurvived += Time.deltaTime;
            transform.Translate(Vector3.forward * Time.deltaTime);
            transform.Rotate(transform.up * turningSpeed * Time.deltaTime * net.GetOutput(SensorInput())[0]);
        }
	}

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Wall")
        {
            initialized = false;
            net.SetFitness(timeSurvived*10);
            gameObject.SetActive(false);
        }
    }

    private float[] SensorInput()
    {
        float[] distances = new float[sensors.Length];

        for (int i=0; i<sensors.Length; i++)
        {
            RaycastHit hit;
            if(Physics.Raycast(sensors[i].position, sensors[i].forward, out hit,sensorRange))
            {
                distances[i] = hit.distance;
                Debug.DrawRay(sensors[i].position, sensors[i].TransformDirection(Vector3.forward) * hit.distance, Color.red);
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

}
