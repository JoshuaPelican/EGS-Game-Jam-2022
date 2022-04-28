using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsObject : MonoBehaviour
{
    [Header("Variable References")]
    [SerializeField] IntVariable ScoreVariable;

    public bool IsDestroyed = false;
    public delegate void ObjectEvent();
    public ObjectEvent OnObjectDestroyed;

    float size;
    public float Size { get { return size; } }
    [Header("Object Settings")]
    [SerializeField] float SizeModifier = 1f;
      
    int scoreValue;
    
    Mesh mesh;
    Rigidbody rig;

    public static float TotalObjects = 0;
    public static float TotalDestroyedObjects = 0;

    float sleepTimer = 0;
    [SerializeField] float TimeToSleep = 3f;
    bool sleeping;
    public ObjectEvent OnObjectSleep;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        rig = GetComponent<Rigidbody>();

        Initialize();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (sleeping)
            return;
        if (collision.gameObject.layer != LayerMask.NameToLayer("Ground"))
            return;

        sleepTimer += Time.deltaTime;
        if(sleepTimer >= TimeToSleep)
        {
            sleeping = true;
            sleepTimer = 0;
            OnObjectSleep?.Invoke();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Ground"))
            return;
        sleepTimer = 0;
    }

    void Initialize()
    {
        TotalObjects++;

        rig.isKinematic = true;

        //Size Calculation
        size = (mesh.bounds.size.x * transform.localScale.x) * (mesh.bounds.size.y * transform.localScale.y) * (mesh.bounds.size.z * transform.localScale.z) * SizeModifier;

        //Score Caluclation
        scoreValue = Mathf.RoundToInt(Mathf.Sqrt(Size) * 10) * 10;
    }

    public void CheckDestruction(float otherSize)
    {
        float scaledSize = otherSize * otherSize;

        Debug.Log(scaledSize + " : " + Size);

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
        sleeping = false;
        Debug.Log($"{name} was Destroyed!");
        IsDestroyed = true;
        TotalDestroyedObjects++;
        rig.isKinematic = false;
        ScoreVariable.Value += scoreValue;
        OnObjectDestroyed?.Invoke();
    }
}
