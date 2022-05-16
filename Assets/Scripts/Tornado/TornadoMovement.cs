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
        Vector3 movement = CalculateMovement(direction);
        ApplyMovement(movement);
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
        //Velocity Calculation
        velocity += Time.deltaTime * Speed * direction;
        velocity = Vector3.ClampMagnitude(velocity, MaxVelocity);

        if(Mathf.Abs(CalculateDirection().magnitude) <= 0.05f)
        {
            velocity *= Friction;
        }

        //Movement Calculation
        return transform.localScale.x * Time.deltaTime * velocity + Physics.gravity;
    }

    public void ApplyMovement(Vector3 movement)
    {
        //Move the tornado according to movement
        controller.Move(movement);
    }
}
