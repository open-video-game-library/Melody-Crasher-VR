using UnityEngine;
using EzySlice; // Ezy-Slice �t���[�����[�N�𗘗p���邽�߂ɕK�v
using System.Collections.Generic;

public class SliceController : MonoBehaviour
{
    // �ؒf�ʂ̐F
    public Material MaterialAfterSlice;

    // �ؒf���郌�C���[
    public LayerMask sliceMask;
    [System.NonSerialized]
    public Vector3 startPoint;

    public float radious;
    public float force;
    
    private bool isSliced;
    [SerializeField] GameObject audioObject;
    [SerializeField] float velocity_deceleration;

    [SerializeField]
    Anime anime = Anime.cutting;


    bool cutted;
    List <Transform> childObject = new List<Transform>();
    float i = 1f;
    Vector3 defaultScale = new Vector3(0f, 0f, 0f);

    [SerializeField]
    GameObject psObject;

    enum Anime
    {
        cutting,
        breaking,
        animate
    }

    

    private void Start()
    {

    }

    void Update()
    {
        if (!cutted) return;
        foreach(Transform a in childObject)
        {
            /*
            Color c = a.GetComponent<MeshRenderer>().material.color;
            foreach(Material m in a.GetComponent<MeshRenderer>().materials)
            {
                m.color = new Color(c.r, c.g, c.b, i);
            }
            */

            if(a.transform.localScale.x <= 0)
            {
                this.transform.localScale = new Vector3(0, 0, 0);
            }
            else
            {
                a.transform.localScale = new Vector3(defaultScale.x * i, defaultScale.y * i, defaultScale.z * i);
            }
        }
        if (this.transform.localScale.x <= 0)
        {
            this.transform.localScale = new Vector3(0 ,0 ,0);
        }
        else
        {
            this.transform.localScale = new Vector3(defaultScale.x * i, defaultScale.y * i, defaultScale.z * i);
        }

        Destroy(this.GetComponent<BoxCollider>());
        i -= Time.deltaTime * 2f;
    }

    // �ؒf���ɐ�������I�u�W�F�N�g��Ԃ�
    private SlicedHull SliceObject(GameObject obj, Vector3 position, Vector3 right, Material crossSectionMaterial = null)
    {
        // Ezy-Slice �t���[�����[�N �𗘗p���ăX���C�X���Ă���
        return obj.Slice(position, right, crossSectionMaterial);
    }

    // �I�u�W�F�N�g��������MeshCollider��Rigidbody���A�^�b�`����
    private void MakeItPhysical(GameObject obj, Material mat = null)
    {
        // MeshCollider �� Convex �� true �ɂ��Ȃ��ƁC���蔲���Ă��܂��̂Œ���
        // MeshCollider��mesh��Enable to read/write �ɂ��Ă��Ȃ���Mesh�̐ؒf�������ł��܂���I 
        obj.AddComponent<MeshCollider>().convex = true;
        obj.AddComponent<Rigidbody>();
        obj.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, velocity_deceleration);
            //this.GetComponent<Rigidbody>().velocity;
    }

    public void Cut(GameObject objectToSlice, Vector3 position, Vector3 right)
    {
        if (isSliced) { return; }
        isSliced = true;
        if (GameStateManager.GameState == GameState.Play) { Result.hits += 1; }
        else { Result.hits = 0; }

        switch (anime){
            case Anime.cutting:
                Instantiate(audioObject, this.transform.position, Quaternion.identity);
                Destroy(this.gameObject, 3.0f);
                cutted = true;
                break;

            case Anime.breaking:
                Instantiate(audioObject, this.transform.position, Quaternion.identity);


                foreach (Transform a in this.transform)
                {
                    a.gameObject.AddComponent<Rigidbody>();
                    a.GetComponent<BoxCollider>().isTrigger = false;
                    Explosion(a.GetComponent<Rigidbody>(), position);
                    
                    childObject.Add(a.transform);
                    a.gameObject.layer = 8;
                    Destroy(a.gameObject, 3.0f);
                }
                foreach (Transform a in childObject)
                {
                    //defaultScale = gameObject.GetComponent<MeshRenderer>().bounds.size;
                    a.transform.parent = null;
                    defaultScale = a.transform.localScale;
                    a.transform.localScale = new Vector3(defaultScale.x * i, defaultScale.y * i, defaultScale.z * i);
                }
                
                Destroy(this.gameObject, 3.0f);
                cutted = true;
                return;

            case Anime.animate:
                Instantiate(audioObject, this.transform.position, Quaternion.identity);
                this.gameObject.AddComponent<Rigidbody>();
                this.GetComponent<SphereCollider>().isTrigger = false;
                defaultScale = this.transform.localScale;

                this.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, velocity_deceleration);

                cutted = true;
                GameObject ps = Instantiate(psObject, this.transform.position, Quaternion.identity);
                Destroy(ps, 1.0f);
                Destroy(this.gameObject, 3.0f);
                if (GetComponentInChildren<Animator>())
                {
                    GetComponentInChildren<Animator>().SetInteger("HP", 0);
                }
                //this.GetComponentInChildren<Animator>().SetBool
                return;
        }

        

        SlicedHull slicedObject = SliceObject(objectToSlice.gameObject, position, right, MaterialAfterSlice);

        if (slicedObject != null)
        {
            // ��ʑ��̃I�u�W�F�N�g�̐���
            GameObject upperHullGameobject = slicedObject.CreateUpperHull(objectToSlice.GetComponent<Collider>().gameObject, MaterialAfterSlice);
            MakeItPhysical(upperHullGameobject);
            Explosion(upperHullGameobject.GetComponent<Rigidbody>(), position);
            upperHullGameobject.layer = 8;
            Destroy(upperHullGameobject, 3.0f);
            //upperHullGameobject.GetComponent<MeshCollider>().isTrigger = true;

            // ���ʑ��̃I�u�W�F�N�g�̐���
            GameObject lowerHullGameobject = slicedObject.CreateLowerHull(objectToSlice.GetComponent<Collider>().gameObject, MaterialAfterSlice);
            MakeItPhysical(lowerHullGameobject);
            Explosion(lowerHullGameobject.GetComponent<Rigidbody>(), position);
            lowerHullGameobject.layer = 8;
            Instantiate(audioObject, this.transform.position, Quaternion.identity);
            Destroy(lowerHullGameobject, 3.0f);
            //lowerHullGameobject.GetComponent<MeshCollider>().isTrigger = true;
        }

        
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "OnDestroy")
        {
            if (!isSliced)
            {
                Destroy(this.gameObject);
                isSliced = true;
                if (GameStateManager.GameState == GameState.Play) { Result.miss++; }
                else { Result.miss = 0; }
            }
        }
        //entry = other.ClosestPointOnBounds(this.transform.position);
    }

    public void Explosion(Rigidbody a, Vector3 position)
    {
        //m_particle.Play();
        //m_position = m_particle.transform.position;

        // �͈͓���Rigidbody��AddExplosionForce
        //Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, radious);
        a.AddExplosionForce(force, position, radious, 0.0f, ForceMode.Impulse);
    }
}