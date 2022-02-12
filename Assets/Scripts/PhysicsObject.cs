using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsObject : MonoBehaviour
{
    [SerializeField] IntVariable ScoreVariable;

    //Object Values
    float maxHealth;
    float currentHealth;
    float size;
    int scoreValue;

    public bool IsDestroyed { get { return currentHealth <= 0; } }
    
    Mesh mesh;
    Rigidbody rig;

    public static float TotalObjects;
    public static float TotalDestroyedObjects;
    public static float PercentDestroyed = TotalDestroyedObjects / TotalObjects;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        rig = GetComponent<Rigidbody>();

        Initialize();
    }

    void Initialize()
    {
        TotalObjects++;

        rig.isKinematic = true;

        float area = mesh.bounds.size.x * mesh.bounds.size.y * mesh.bounds.size.z;
        size = Mathf.Sqrt(area);

        maxHealth = size;
        currentHealth = maxHealth;

        scoreValue = Mathf.RoundToInt(size * 10) * 10;
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
        TotalDestroyedObjects++;
        rig.isKinematic = false;
        ScoreVariable.Value += scoreValue;
    }
}
