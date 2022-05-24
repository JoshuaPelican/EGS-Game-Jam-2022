using UnityEngine;

public class TornadoMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float Speed = 10f;
    [SerializeField] float MaxVelocity = 10f;
    [SerializeField][Range(0, 1)] float Friction = 0.05f;

    CharacterController controller;
    Rigidbody rig;

    Vector3 velocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        rig = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector3 direction = CalculateDirection();
        Move(Speed, direction);
    }

    Vector3 CalculateDirection()
    {
        //Input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //Direction Calculation
        return Vector3.ClampMagnitude(new Vector3(horizontal, 0, vertical), 1);
    }

    public void Move(float speed, Vector3 direction)
    {
        //Velocity Calculation
        velocity += Time.deltaTime * speed * (transform.rotation * direction);
        velocity = Vector3.ClampMagnitude(velocity, MaxVelocity);

        //Apply slowing friction force
        if (Mathf.Abs(CalculateDirection().magnitude) <= 0.05f)
        {
            velocity *= Friction;
        }

        Vector3 movement = transform.localScale.x * Time.deltaTime * velocity + (Physics.gravity * Time.deltaTime);

        //Move the tornado according to movement
        controller.Move(movement);
    }
}
