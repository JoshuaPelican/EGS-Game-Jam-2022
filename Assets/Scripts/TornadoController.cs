using System.Collections.Generic;
using UnityEngine;

public class TornadoController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float Speed = 10;
    [SerializeField] [Range(0, 1)] float AccelerationSpeed = 0.5f;

    [Header("Force Settings")]
    [SerializeField] float TowardsForce = 40;
    [SerializeField] float PerpendicularForce = 15;
    [SerializeField] float UpForce = 30;

    [Header("Damage Settings")]
    [SerializeField] float Damage = 1;

    [Header("Score Settings")]
    [SerializeField] IntVariable ScoreVariable;
    [SerializeField] float ScoreSizeRatio = 0.0001f;


    float Size { get { return Mathf.Sqrt((ScoreVariable.Value * ScoreSizeRatio) + 1); } }

    List<PhysicsObject> objectsInRange = new List<PhysicsObject>();

    Collider col;

    private void Start()
    {
        col = GetComponent<Collider>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0, vertical) * Speed * Size * Time.deltaTime;

        transform.position = Vector3.Slerp(transform.position, transform.position + movement, AccelerationSpeed);
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
        transform.localScale = Vector3.Lerp(transform.localScale,  Vector3.one * Size, 0.1f);

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

            float distanceToTornado = Vector3.Distance(transform.position, obj.transform.position);

            Vector3 proportionalTornadoForce = Vector3.Slerp(Vector3.zero, tornadoForce, (distanceToTornado / col.bounds.extents.magnitude) * Size);

            obj.AddForce(proportionalTornadoForce);
        }
        else
        {
            obj.TakeDamage(Damage * Size * Time.deltaTime);
        }
    }
}
