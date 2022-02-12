using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsObject : MonoBehaviour
{
    [SerializeField] IntVariable ScoreVariable;
    [SerializeField] GameObject HealthBarPrefab;
    GameObject healthBarObject;
    Image healthBar;

    //Object Values
    float maxHealth;
    float currentHealth;
    float size;
    int scoreValue;

    public bool IsDestroyed { get { return currentHealth <= 0; } }
    
    Mesh mesh;
    Rigidbody rig;

    public static float TotalObjects = 0;
    public static float TotalDestroyedObjects = 0;

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
        size = area;

        maxHealth = size;
        currentHealth = maxHealth;

        scoreValue = Mathf.RoundToInt(Mathf.Sqrt(size) * 10) * 10;

        healthBarObject = Instantiate(HealthBarPrefab, GetComponent<Renderer>().bounds.center, Quaternion.identity, transform);
        healthBar = healthBarObject.GetComponentInChildren<Image>();
        healthBarObject.SetActive(false);
    }

    public void TakeDamage(float damage)
    {
        healthBarObject.SetActive(true);
        currentHealth -= damage / Mathf.Sqrt(size);
        healthBar.fillAmount = currentHealth / maxHealth;

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
        Destroy(healthBarObject);
        rig.isKinematic = false;
        ScoreVariable.Value += scoreValue;
    }
}
