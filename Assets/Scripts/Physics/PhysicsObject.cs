using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsObject : MonoBehaviour
{
    [Header("Variable References")]
    [SerializeField] IntVariable ScoreVariable;

    public float Size { get { return area; } }
    float area;
    int scoreValue;

    public bool isDestroyed = false;
    
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

        area = (mesh.bounds.size.x * transform.localScale.x) * (mesh.bounds.size.y * transform.localScale.y) * (mesh.bounds.size.z * transform.localScale.z);

        //Score Caluclation
        scoreValue = Mathf.RoundToInt(Mathf.Sqrt(Size) * 10) * 10;
    }

    public void CheckDestruction(float otherSize)
    {
        float scaledSize = otherSize;

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
        isDestroyed = true;
        TotalDestroyedObjects++;
        rig.isKinematic = false;
        ScoreVariable.Value += scoreValue;
    }
}
