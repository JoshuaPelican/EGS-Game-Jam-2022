using System.Collections;
using UnityEngine;

public class ReactiveNPC : MonoBehaviour
{
    [Header("Reaction Settings")]
    [SerializeField] float RunSpeed = 4f;
    [SerializeField] [Range(0f, 1f)] float TurningSpeed = 0.05f;
    [SerializeField] float TimeToCalm = 3f;
    [SerializeField] LayerMask BlocksLineOfSight;
    [Space]
    [SerializeField] Personality IdlePersonality;

    [Header("Component References")]
    [SerializeField] Transform Root;
    [SerializeField] Animator Animator;
    [SerializeField] Rigidbody Rig;
    [SerializeField] PhysicsObject PhysicsObject;
    [Space]
    [SerializeField] BoxCollider Collider;

    enum Personality
    {
        Simple,
        Neutral,
        PhoneCallFred,
        Searching,
        TextSavvy,
        Friendly,
        Argumentative,
        Bored,
        Dancing
    }

    bool fearful;
    bool destroyed;
    Vector3 runDirection;

    private void Start()
    {
        SetIdle();
    }

    private void OnEnable()
    {
        PhysicsObject.OnObjectDestroyed += OnDestroyed;
        PhysicsObject.OnObjectSleep += Splat;
    }

    private void OnDisable()
    {
        PhysicsObject.OnObjectDestroyed -= OnDestroyed;
        PhysicsObject.OnObjectSleep -= Splat;
    }

    private void FixedUpdate()
    {
        if (!fearful || destroyed)
            return;

        Vector3 movement = runDirection * RunSpeed;

        Rig.velocity = movement;
        Root.forward = runDirection;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.TryGetComponent(out Fearsome fearsome))
            return;

        if (Physics.Raycast(transform.position, (fearsome.transform.position - transform.position).normalized, Vector3.Distance(transform.position, fearsome.transform.position), BlocksLineOfSight, QueryTriggerInteraction.Ignore))
            return;

        Debug.DrawLine(transform.position, fearsome.transform.position, Color.red, Time.deltaTime);

        runDirection = Vector3.Slerp(runDirection, (transform.position - fearsome.transform.position).normalized, 0.01f);
        runDirection = new Vector3(runDirection.x, 0, runDirection.z);

        OnFearful();
    }

    void Splat()
    {
        Root.SetPositionAndRotation(new Vector3(transform.position.x, -0.8f, transform.position.z), Quaternion.Euler(Quaternion.identity.eulerAngles.x, Root.rotation.eulerAngles.y, Quaternion.identity.eulerAngles.z));
        Rig.isKinematic = true;

        Animator.SetBool("Grounded", true);
    }

    void OnCalm()
    {
        fearful = false;
        Animator.SetBool("Fearful", false);
    }

    void OnFearful()
    {
        fearful = true;
        Animator.SetBool("Fearful", true);
        StopAllCoroutines();
        StartCoroutine(CalmOverTime(TimeToCalm));
    }

    void OnDestroyed()
    {
        destroyed = true;
        Animator.SetBool("Destroyed", true);
        Animator.SetBool("Grounded", false);
        SetColliderSize(true);
    }

    void SetColliderSize(bool falling)
    {
        if (falling)
            Collider.size = new Vector3(90f, 30f, 183f);
        else
            Collider.size = new Vector3(90f, 183f, 30f);
    }

    IEnumerator CalmOverTime(float time)
    {
        yield return new WaitForSeconds(time);

        OnCalm();
    }

    void SetIdle()
    {
        Animator.SetFloat("IdleIndex", (int)IdlePersonality);
    }
}
