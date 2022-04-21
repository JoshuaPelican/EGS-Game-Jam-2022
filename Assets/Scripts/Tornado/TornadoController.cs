using System.Collections.Generic;
using UnityEngine;

public class TornadoController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float Speed = 10;
    [SerializeField] [Range(0, 1)] float AccelerationSpeed = 0.5f;
    [SerializeField] float MaxAreaRadius = 180f;

    [Header("Force Settings")]
    [SerializeField] float TowardsForce = 40;
    [SerializeField] float PerpendicularForce = 15;
    [SerializeField] float UpForce = 30;
    [SerializeField] float ForceGrowthFactor = 1.1f;

    [Header("Size Settings")]
    [SerializeField] IntVariable ScoreVariable;
    [SerializeField] [Range(0.1f, 10)] float SizeGrowthFactor = 2;

    float Size { get { return Mathf.Pow(ScoreVariable.Value / 1000f, 1 / SizeGrowthFactor) + 1; } }

    List<PhysicsObject> objectsInRange = new List<PhysicsObject>();
    Collider col;

    private void Start()
    {
        col = GetComponent<Collider>();
        Debug.Log(PhysicsObject.TotalObjects);
    }

    private void Update()
    {
        Vector3 direction = CalculateDirection();
        Vector3 movement = CalculateMovement(direction);
        ApplyMovement(movement);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Add object to the list
        if (other.TryGetComponent(out PhysicsObject obj))
            objectsInRange.Add(obj);
    }

    private void FixedUpdate()
    {
        //Scale size over time
        transform.localScale = Vector3.Lerp(transform.localScale,  Vector3.one * Size, 0.1f);

        //Affect objects in the list
        foreach (PhysicsObject obj in objectsInRange)
            ApplyPhysics(obj);
    }

    private void OnTriggerExit(Collider other)
    {
        //Remove object from the list
        if (other.TryGetComponent(out PhysicsObject obj))
            objectsInRange.Remove(obj);
    }

    Vector3 CalculateDirection()
    {
        //Input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //Direction Calculation
        return Vector3.ClampMagnitude(new Vector3(horizontal, 0, vertical), 1);
    }

    Vector3 CalculateMovement(Vector3 direction)
    {
        //Movement Calculation
        return direction * Speed * Size * Time.deltaTime;
    }

    void ApplyMovement(Vector3 movement)
    {
        //Move the tornado according to movement
        transform.position = Vector3.Slerp(transform.position, transform.position + movement, AccelerationSpeed);

        //Check if the tornado is out of bounds, if it is then move it back
        if (transform.position.magnitude > MaxAreaRadius)
            transform.position = Vector3.Slerp(transform.position, transform.position - movement, AccelerationSpeed);
    }

    void ApplyPhysics(PhysicsObject obj)
    {
        //If object is destroyed then apply force to it
        if (obj.IsDestroyed)
        {
            //Force calculation
            Vector3 towardsTornado = (transform.position - obj.transform.position).normalized;
            Vector3 perpendicularToTornado = Vector3.Cross(towardsTornado, Vector3.up).normalized;
            Vector3 tornadoForce = (perpendicularToTornado * PerpendicularForce) + (towardsTornado * TowardsForce) + (Vector3.up * UpForce);

            //Distance to object
            float distanceToTornado = Vector3.Distance(transform.position, obj.transform.position);

            //Force proportional to object distance and tonado size
            float forceFactor = Mathf.Pow((distanceToTornado / col.bounds.extents.magnitude) * Size + obj.Size, ForceGrowthFactor);
            Vector3 proportionalTornadoForce = Vector3.Slerp(Vector3.zero, tornadoForce, forceFactor);

            //Debug.Log("Force: " + proportionalTornadoForce);
            obj.AddForce(proportionalTornadoForce);
        }
        //Otherwise check if it needs to be destroyed this frame
        else
        {
            obj.CheckDestruction(Size);
        }
    }
}
