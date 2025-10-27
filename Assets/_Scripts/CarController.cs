using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Rigidbody carRB;
    [SerializeField] private Transform[] rayPoints;
    [SerializeField] private LayerMask drivable;


    [Header("JointSuspension2D Setting")]
    [SerializeField] private float springStiffness;
    [SerializeField] private float damperStiffness;
    [SerializeField] private float restLenght;
    [SerializeField] private float springTrevel;
    [SerializeField] private float wheelRadius;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        carRB = GetComponent<Rigidbody>();
    }
    void Update()
    {
        suspension();
    }
    
    private void suspension()
    {
        foreach (Transform rayPoints in rayPoints)
        {
            RaycastHit hit;
            float maxLength = restLenght + springTrevel;

            if(Physics.Raycast(rayPoints.position, -rayPoints.up, out hit, maxLength + wheelRadius, drivable))
            {
                float currentSpringLength = hit.distance - wheelRadius;
                float springCompression = (restLenght - currentSpringLength) / springTrevel;

                float springVelocity = Vector3.Dot(carRB.GetPointVelocity(rayPoints.position), rayPoints.up);
                float dampForce = damperStiffness * springVelocity;
                float springForse = springStiffness * springCompression;
                float netForce = springForse - dampForce;

                carRB.AddForceAtPosition(netForce * rayPoints.up, rayPoints.position);
                Debug.DrawLine(rayPoints.position, hit.point, Color.red);
            }
            else
            {
                Debug.DrawLine(rayPoints.position, rayPoints.position + (wheelRadius + maxLength) * -rayPoints.up, Color.green);
            }
        }
    }
}
