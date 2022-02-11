using System.Collections.Generic;
using UnityEngine;

public class TornadoController : MonoBehaviour
{
    [SerializeField] float Speed;
    [SerializeField] float MaxSpeed;
    [SerializeField] float TowardsForce;
    [SerializeField] float PerpendicularForce;
    [SerializeField] float UpForce;
    [SerializeField] float Damage;

    List<PhysicsObject> objectsInRange = new List<PhysicsObject>();

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0, vertical) * Speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PhysicsObject obj))
        {
            objectsInRange.Add(obj);
        }
    }

    private void FixedUpdate()
    {
        foreach (PhysicsObject obj in objectsInRange)
        {
            AffectPhysicsObject(obj);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PhysicsObject obj))
        {
            objectsInRange.Remove(obj);
        }
    }

    void AffectPhysicsObject(PhysicsObject obj)
    {
        if (obj.IsDestroyed)
        {
            Vector3 towardsTornado = (transform.position - obj.transform.position).normalized;
            Vector3 perpendicularToTornado = Vector3.Cross(towardsTornado, Vector3.up).normalized;
            Vector3 tornadoForce = (perpendicularToTornado * PerpendicularForce) + (towardsTornado * TowardsForce) + (Vector3.up * UpForce);
            obj.AddForce(tornadoForce);
        }
        else
        {
            obj.TakeDamage(Damage * Time.deltaTime);
        }
    }
}
