using System.Collections;
using UnityEngine;

public class ReactiveNPC : MonoBehaviour
{
    [Header("Reaction Settings")]
    [SerializeField] float RunSpeed = 4f;
    [SerializeField] float TimeToCalm = 3f;

    [Header("Component References")]
    [SerializeField] Transform Root;
    [SerializeField] Animator Animator;
    [SerializeField] Rigidbody Rig;
    [SerializeField] PhysicsObject PhysicsObject;
    [Space]
    [SerializeField] BoxCollider Collider;

    bool fearful;
    bool destroyed;
    Vector3 runDirection;
    int randIdleIndex = 0;

    private void OnEnable()
    {
        PhysicsObject.OnObjectDestroyed += OnDestroyed;
        PhysicsObject.OnObjectSleep += Splat;

        SetRandomIdle();
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

        runDirection = (transform.position - fearsome.transform.position).normalized;
        runDirection = new Vector3(runDirection.x, 0, runDirection.z);

        OnFearful();
    }

    void Splat()
    {
        Root.rotation = Quaternion.Euler(Quaternion.identity.eulerAngles.x, Root.rotation.eulerAngles.y, Quaternion.identity.eulerAngles.z);
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        Rig.isKinematic = true;

        Animator.SetBool("Grounded", true);
    }

    void OnCalm()
    {
        fearful = false;
        SetRandomIdle();
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

    void SetRandomIdle()
    {
        randIdleIndex = Random.Range(0, 8);
        Animator.SetFloat("IdleIndex", randIdleIndex);
    }
}