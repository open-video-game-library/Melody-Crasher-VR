using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutterMove : MonoBehaviour
{
    private Vector3 prePos = Vector3.zero;
    private Vector3 prePos2 = Vector3.zero;

    public Transform parentGameObject;

    void FixedUpdate ()
    {
       prePos = prePos2;
       prePos2 = transform.position;
        var cutPlane = new Plane(Vector3.Cross(transform.forward.normalized, prePos - transform.position).normalized, transform.position);
        Vector3 a = cutPlane.normal.normalized;
        //Debug.Log("a:" + a);
        //Debug.DrawRay(transform.position, a, Color.yellow, 100000f);


    }

    void OnTriggerEnter(Collider other)
    {
        //var meshCut = other.gameObject.GetComponent<MeshCut>();
        var sliceTest = other.gameObject.GetComponent<SliceController>();
        if (sliceTest == null) { return; }
        //ˆê•ûŒü‚Ì‚İ‚ÅØ’f‚·‚é•û–@A•ûŒü‚É‚Â‚¢‚Ä‚Í“K‹X•ÏX
        //var cutPlane = new Plane(transform.right, transform.position);
        //“®‚«‚ÅØ’f‚·‚éê‡
        var cutPlane = new Plane (Vector3.Cross(-parentGameObject.forward.normalized, prePos - transform.position).normalized, transform.position);
        Vector3 a = cutPlane.normal.normalized;
        //sliceTest.Cut(other.gameObject, transform.position, transform.right);
        sliceTest.Cut(other.gameObject, transform.position, a);
        //meshCut.Cut(cutPlane);
    }
}
