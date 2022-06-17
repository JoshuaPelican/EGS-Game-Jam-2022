using System.Collections.Generic;
using UnityEngine;

public class Forcer : MonoBehaviour
{
    [SerializeField] float ForceStrength = 10f;

    List<Rigidbody> objectsInRange = new List<Rigidbody>();
    TornadoMovement tornado;
    bool tornadoInRange = false;

    private void OnTriggerEnter(Collider other)
    {
        //Add object to the list
        if (other.TryGetComponent(out Rigidbody obj))
            objectsInRange.Add(obj);

        else if (other.transform.parent.TryGetComponent(out TornadoMovement move))
        {
            tornado = move;
            tornadoInRange = true;
        }
    }

    private void FixedUpdate()
    {
        //Affect objects in the list
        foreach (var obj in objectsInRange)
            ApplyPhysics(obj);

        if (tornadoInRange)
            ApplyPhysics(tornado);
    }

    private void OnTriggerExit(Collider other)
    {
        //Remove object from the list
        if (other.TryGetComponent(out Rigidbody obj))
            objectsInRange.Remove(obj);

        else if (other.transform.parent.TryGetComponent(out TornadoMovement move))
        {
            tornado = null;
            tornadoInRange = false;
        }
    }

    void ApplyPhysics(Rigidbody obj)
    {
        float distanceToThis = Vector3.Distance(obj.transform.position, transform.position);
        float forceFactor = (distanceToThis / transform.localScale.magnitude) * ForceStrength;
        obj.AddForce(forceFactor * transform.forward * Time.deltaTime);
    }

    void ApplyPhysics(TornadoMovement obj)
    {
        float distanceToThis = Vector3.Distance(obj.transform.position, transform.position);
        float forceFactor = (distanceToThis / transform.localScale.magnitude) * ForceStrength;
        obj.Move(forceFactor * Time.deltaTime, transform.forward);
    }
}
