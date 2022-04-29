using System.Collections;
using UnityEngine;

public class ReactiveNPC : MonoBehaviour
{
    [Header("Reaction Settings")]
    [SerializeField] float TimeToCalm = 3f;

    [Header("Component References")]
    [SerializeField] Transform Root;
    [SerializeField] Animator Animator;
    [SerializeField] Rigidbody Rig;
    [SerializeField] PhysicsObject PhysicsObject;
    [Space]
    [SerializeField] BoxCollider Collider;

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

    private void OnTriggerStay(Collider other)
    {
        if (!other.TryGetComponent(out Fearsome fearsome))
            return;

        OnFearful();
    }

    void Splat()
    {
        Root.rotation = Quaternion.Euler(Quaternion.identity.eulerAngles);
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        Rig.isKinematic = true;

        Animator.SetBool("Grounded", true);
    }

    void OnCalm()
    {
        Animator.SetBool("Fearful", false);
    }

    void OnFearful()
    {
        Animator.SetBool("Fearful", true);
        StopAllCoroutines();
        StartCoroutine(CalmOverTime(TimeToCalm));
    }

    void OnDestroyed()
    {
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
}
