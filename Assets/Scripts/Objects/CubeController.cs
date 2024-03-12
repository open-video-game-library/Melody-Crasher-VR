using UnityEngine;

public class CubeController : MonoBehaviour
{
    [System.NonSerialized]
    public Vector3 startPoint;

    [System.NonSerialized]
    public Vector3 speed;

    private new Rigidbody rigidbody;

    Vector3 defaultCubeScale;

    // Start is called before the first frame update
    void Start()
    {

        defaultCubeScale = this.transform.localScale;
        rigidbody = GetComponent<Rigidbody>();

        transform.position = startPoint;

        // デフォルトのspeedに、CubeParametersで設定した倍率をかける
        rigidbody.velocity = CubeParameters.speed * new Vector3(0f, 0f, -1.0f);
        transform.localScale = defaultCubeScale * CubeParameters.size;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
