using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsObject : MonoBehaviour
{
    [Header("Variable References")]
    [SerializeField] IntVariable ScoreVariable;

    Mesh mesh;
    Rigidbody rig;
    new Renderer renderer;

    public bool IsDestroyed = false;
    public UnityEvent OnObjectDestroyed;

    [Header("Object Settings")]
    [SerializeField] float SizeModifier = 1f;
    [SerializeField] bool StartStatic = true;

    [Header("Sleep Settings")]
    [SerializeField] float TimeToSleep = 3f;
    float sleepTimer = 0;
    bool sleeping;
    public UnityEvent OnObjectSleep;

    [Header("Destruction Settings")]
    [SerializeField] GameObject DestroyedPrefab; 
    int scoreValue;
  
    float size;
    public float Size { get { return size; } }


    //Static Variables
    public static float TotalObjects = 0;
    public static float TotalDestroyedObjects = 0;


    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        rig = GetComponent<Rigidbody>();
        renderer = GetComponent<Renderer>();

        Initialize();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (sleeping || !IsDestroyed)
            return;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            sleepTimer += Time.deltaTime;
            if (sleepTimer >= TimeToSleep)
            {
                sleepTimer = 0;
                Sleep();
            }
        }
    }

    /*
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Ground"))
            return;
        sleepTimer = 0;
    }
    */

    void Initialize()
    {
        TotalObjects++;

        rig.isKinematic = StartStatic;

        //Size Calculation
        size = (mesh.bounds.size.x * transform.localScale.x) * (mesh.bounds.size.y * transform.localScale.y) * (mesh.bounds.size.z * transform.localScale.z) * SizeModifier;

        //Score Caluclation
        scoreValue = Mathf.RoundToInt(Mathf.Sqrt(Size) * 10) * 10;
    }

    public void CheckDestruction(float otherSize)
    {
        float scaledSize = otherSize * otherSize;

        //Debug.Log(scaledSize + " : " + Size);

        if(scaledSize >= Size)
        {
            Destroy();
        }
    }

    public void AddForce(Vector3 forceToAdd)
    {       
        rig.AddForce(forceToAdd / Mathf.Sqrt(Mathf.Sqrt(Size)), ForceMode.Force);
    }

    void Destroy()
    {
        Debug.Log($"{name} was Destroyed!");

        if (DestroyedPrefab)
        {
            Instantiate(DestroyedPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            sleeping = false;
            IsDestroyed = true;
            rig.isKinematic = false;
        }
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

        TotalDestroyedObjects++;
        ScoreVariable.Value += scoreValue;
        OnObjectDestroyed?.Invoke();
    }

    void Sleep()
    {
        gameObject.isStatic = true;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        rig.Sleep();

        sleeping = true;
        OnObjectSleep?.Invoke();
    }
}
