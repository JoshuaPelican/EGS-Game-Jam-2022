using UnityEngine;

public class ReactiveNPC : MonoBehaviour
{
    [SerializeField] Animator animtator;
    [SerializeField] PhysicsObject physicsObject;

    private void OnEnable()
    {
        physicsObject.OnObjectDestroyed += OnDestroyed;
    }

    private void OnDisable()
    {
        physicsObject.OnObjectDestroyed -= OnDestroyed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Fearsome fearsome))
            return;

        OnFearful();
    }

    void OnFearful()
    {
        animtator.SetBool("Fearful", true);
    }

    void OnDestroyed()
    {
        animtator.SetBool("Destroyed", true);
    }
}
