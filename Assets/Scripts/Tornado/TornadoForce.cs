using System.Collections.Generic;
using UnityEngine;

public class TornadoForce : MonoBehaviour
{
    [Header("Force Settings")]
    [SerializeField] float TowardsForce = 40;
    [SerializeField] float PerpendicularForce = 15;
    [SerializeField] float UpForce = 30;
    [SerializeField] float ForceGrowthFactor = 1.1f;

    [Header("Size Settings")]
    [SerializeField] IntVariable ScoreVariable;
    [SerializeField] float SizeGrowthFactor = 0.1f;

    float Size { get { return (Mathf.Sqrt((ScoreVariable.Value * SizeGrowthFactor) / 1000f)) + 1; } }

    List<PhysicsObject> objectsInRange = new List<PhysicsObject>();

    private void OnTriggerEnter(Collider other)
    {
        //Add object to the list
        if (other.TryGetComponent(out PhysicsObject obj))
            objectsInRange.Add(obj);
    }

    private void FixedUpdate()
    {
        //Scale size over time
        transform.parent.localScale = Vector3.Lerp(transform.localScale, Vector3.one * Size, 0.1f);

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


    void ApplyPhysics(PhysicsObject obj)
    {
        if (!obj)
        {
            return;
        }

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
            float forceFactor = Mathf.Pow((distanceToTornado / transform.localScale.magnitude) * Size + obj.Size, ForceGrowthFactor);
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
