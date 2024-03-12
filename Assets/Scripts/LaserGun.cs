using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] float speed = 30f;
    [SerializeField] GameObject burrel_R;
    [SerializeField] GameObject burrel_L;
    public bool rightHand;

    [SerializeField]
    LineRenderer R_lr;

    [SerializeField]
    LineRenderer L_lr;

    [SerializeField]
    Transform RedPointer;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit hit;

        if (rightHand)
        {
            var positions = new Vector3[]{
                burrel_R.transform.position,               // 開始点
                burrel_R.transform.right * 100f,        // 終了点
                };

            Vector3 ray = burrel_R.transform.right * 100f - burrel_R.transform.position;

            if (Physics.Raycast(burrel_R.transform.position, ray, out hit, 100f))
            {
                RedPointer.gameObject.SetActive(true);
                RedPointer.transform.position = hit.point;
                positions = new Vector3[]{
                    burrel_R.transform.position,               // 開始点
                    hit.point      // 終了点
                    };
            }
            else
            {
                RedPointer.gameObject.SetActive(false);
            }

            // 線を引く場所を指定する
            R_lr.SetPositions(positions);
        }
        else
        {
            var positions = new Vector3[]{
                burrel_L.transform.position,               // 開始点
                burrel_L.transform.right * 100f,        // 終了点
                };

            Vector3 ray = burrel_L.transform.right * 100f - burrel_L.transform.position;

            if (Physics.Raycast(burrel_L.transform.position, ray, out hit, 100f))
            {
                RedPointer.gameObject.SetActive(true);
                RedPointer.transform.position = hit.point;
                positions = new Vector3[]{
                    burrel_L.transform.position,               // 開始点
                    hit.point         // 終了点
                    };
            }
            else
            {
                RedPointer.gameObject.SetActive(false);
            }

            // 線を引く場所を指定する
            L_lr.SetPositions(positions);
        }

        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger) && rightHand)
        {
            GameObject a = Instantiate(bullet, burrel_R.transform.position, burrel_R.transform.rotation);
            a.GetComponent<Rigidbody>().velocity = burrel_R.transform.right * speed;
            Destroy(a, 3f);
        }

        if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger) && !rightHand)
        {
            GameObject a = Instantiate(bullet, burrel_L.transform.position, burrel_L.transform.rotation);
            a.GetComponent<Rigidbody>().velocity = burrel_L.transform.right * speed;
            Destroy(a, 3f);
        }
    }
}
