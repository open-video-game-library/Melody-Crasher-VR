using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    private Vector3 prePos = Vector3.zero;
    private Vector3 prePos2 = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        preVec = this.transform.position;
        prePos = prePos2;
        prePos2 = transform.position;
        var cutPlane = new Plane(Vector3.Cross(transform.forward.normalized, prePos - transform.position).normalized, transform.position);
        Vector3 a = cutPlane.normal.normalized;
        Debug.Log("a:" + a);
        //Debug.DrawRay(transform.position, a, Color.yellow, 100000f);
    }

    Vector3 preVec;

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;

        float dis = GetComponent<Rigidbody>().velocity.magnitude;

        Debug.DrawRay(this.transform.position, (preVec - this.transform.position).normalized, Color.red, (preVec - this.transform.position).magnitude);
        if (Physics.Raycast(this.transform.position, preVec - this.transform.position, out hit)){

            if (hit.collider.GetComponent<Collider>())
            {
                Cut(hit.collider.GetComponent<Collider>());
            }

        };

        preVec = this.transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        Cut(other);
    }

    public void Cut(Collider other)
    {
        //var meshCut = other.gameObject.GetComponent<MeshCut>();
        if (!other.gameObject.GetComponent<SliceController>())
        {

        }
        var sliceTest = other.gameObject.GetComponent<SliceController>();
        if (sliceTest == null) { return; }
        //àÍï˚å¸ÇÃÇ›Ç≈êÿífÇ∑ÇÈï˚ñ@ÅAï˚å¸Ç…Ç¬Ç¢ÇƒÇÕìKãXïœçX
        //var cutPlane = new Plane(transform.right, transform.position);
        //ìÆÇ´Ç≈êÿífÇ∑ÇÈèÍçá
        var cutPlane = new Plane(Vector3.Cross(-GetComponent<Rigidbody>().velocity.normalized, prePos - transform.position).normalized, transform.position);
        Vector3 a = cutPlane.normal.normalized;
        //sliceTest.Cut(other.gameObject, transform.position, transform.right);
        Debug.Log("test");
        sliceTest.Cut(other.gameObject, transform.position, a);
        //meshCut.Cut(cutPlane);

        Destroy(this.gameObject);
    }
}
