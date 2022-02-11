using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsObject : MonoBehaviour
{
    //Object Values
    float maxHealth;
    float currentHealth;
    float size;
    int scoreValue;

    public bool IsDestroyed { get { return currentHealth <= 0; } }
    
    Mesh mesh;
    Rigidbody rig;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        rig = GetComponent<Rigidbody>();

        Initialize();
    }

    void Initialize()
    {
        rig.isKinematic = true;

        float area = mesh.bounds.size.x * mesh.bounds.size.y * mesh.bounds.size.z;
        size = Mathf.Sqrt(area);

        maxHealth = size;
        currentHealth = maxHealth;

        scoreValue = Mathf.RoundToInt(size) * 100;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            Destroy();
        }
    }

    public void AddForce(Vector3 forceToAdd)
    {
        rig.AddForce(forceToAdd / Mathf.Sqrt(Mathf.Sqrt(size)), ForceMode.Force);
    }

    void Destroy()
    {
        rig.isKinematic = false;
        ScoreManager.Instance.AddScore(scoreValue);
    }
}
