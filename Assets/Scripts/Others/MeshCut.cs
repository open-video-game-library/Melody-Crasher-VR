using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeshCut : MonoBehaviour
{

    private MeshFilter attachedMeshFilter;
    private Mesh attachedMesh;
    private bool coliBool = false;
    private double delta = 0.000000001f;
    private float skinWidth = 0.05f;
    private bool returnBool = false;
    private bool returnBool2 = false;
    private bool returnBool3 = false;

    void Start()
    {
        //�A���Ő؂��Ă��܂�Ȃ��悤�ɏ����x�点��B�K�X��������
        Invoke("BoolOn", 0.2f);
        //mesh�̎擾
        attachedMeshFilter = GetComponent<MeshFilter>();
        attachedMesh = attachedMeshFilter.mesh;
    }

    void BoolOn()
    {
        coliBool = true;
    }

    public void Cut(Plane cutPlane)
    {
        //colibool��false�̎��͉������Ȃ�
        if (coliBool == false)
        {
            return;
        }
        returnBool = false;

        //���낢��AVector3�͐��x�̂��߂�double�ň�����悤�ɂ���DVector3���g�p����class����
        DVector3 p1, p2, p3;
        bool p1Bool, p2Bool, p3Bool;
        var uvs1 = new List<Vector2>();
        var uvs2 = new List<Vector2>();
        var vertices1 = new List<DVector3>();
        var vertices2 = new List<DVector3>();
        var triangles1 = new List<int>();
        var triangles2 = new List<int>();
        var normals1 = new List<Vector3>();
        var normals2 = new List<Vector3>();
        var crossVertices = new List<DVector3>();

        //�J�b�g�������I�u�W�F�N�g�̃��b�V�����g���C�A���O�����Ƃɏ���
        for (int i = 0; i < attachedMesh.triangles.Length; i += 3)
        {
            //���b�V����3�̒��_���擾
            p1 = new DVector3(transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i]]));
            p2 = new DVector3(transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 1]]));
            p3 = new DVector3(transform.TransformPoint(attachedMesh.vertices[attachedMesh.triangles[i + 2]]));

            //���_���J�b�g����ʂ̂ǂ��瑤�ɂ��邩
            p1Bool = DVector3.Dot(new DVector3(cutPlane.normal), p1) + (double)cutPlane.distance > 0 ? true : false;
            p2Bool = DVector3.Dot(new DVector3(cutPlane.normal), p2) + (double)cutPlane.distance > 0 ? true : false;
            p3Bool = DVector3.Dot(new DVector3(cutPlane.normal), p3) + (double)cutPlane.distance > 0 ? true : false;

            //3�̒��_���������ɂ���ꍇ�͂��̂܂ܑ���A���_���J�b�g����ꍇ�͂��̏������s��
            if (p1Bool && p2Bool && p3Bool)
            {
                //3�̒��_���������ɂ���A���̂܂܂��ꂼ���1�ɑ��
                for (int k = 0; k < 3; k++)
                {
                    vertices1.Add(new DVector3(attachedMesh.vertices[attachedMesh.triangles[i + k]]));
                    uvs1.Add(attachedMesh.uv[attachedMesh.triangles[i + k]]);
                    normals1.Add(attachedMesh.normals[attachedMesh.triangles[i + k]]);
                    triangles1.Add(vertices1.Count - 1);
                }

            }
            else if (!p1Bool && !p2Bool && !p3Bool)
            {
                //3�̒��_���������ɂ���A���̂܂܂��ꂼ��̂Q�ɑ��
                for (int k = 0; k < 3; k++)
                {
                    vertices2.Add(new DVector3(attachedMesh.vertices[attachedMesh.triangles[i + k]]));
                    uvs2.Add(attachedMesh.uv[attachedMesh.triangles[i + k]]);
                    normals2.Add(attachedMesh.normals[attachedMesh.triangles[i + k]]);
                    triangles2.Add(vertices2.Count - 1);
                }
            }
            else
            {
                //3�̒��_���������ɂȂ��ꍇ�̏����P�A�ȉ����ԊO��̒��_��p,����ȊO��c�Ƃ���
                DVector3 p, c1, c2;
                int n1, n2, n3;
                if ((p1Bool && !p2Bool && !p3Bool) || (!p1Bool && p2Bool && p3Bool))
                {
                    p = p1;
                    c1 = p2;
                    c2 = p3;
                    n1 = 0;
                    n2 = 1;
                    n3 = 2;

                }
                else if ((!p1Bool && p2Bool && !p3Bool) || (p1Bool && !p2Bool && p3Bool))
                {
                    p = p2;
                    c1 = p3;
                    c2 = p1;
                    n1 = 1;
                    n2 = 2;
                    n3 = 0;

                }
                else
                {
                    p = p3;
                    c1 = p1;
                    c2 = p2;
                    n1 = 2;
                    n2 = 0;
                    n3 = 1;

                }

                //�J�b�g�����ʂɐ�����V�������_���v�Z�A�J�b�g���镽�ʂ̖@�������ɑ΂���p��c�̋����̔䂩��c-p�̒��������߂�
                DVector3 cross1 = p + (c1 - p) * (((double)cutPlane.distance + DVector3.Dot(new DVector3(cutPlane.normal), p)) / DVector3.Dot(new DVector3(cutPlane.normal), p - c1));
                DVector3 cross2 = p + (c2 - p) * (((double)cutPlane.distance + DVector3.Dot(new DVector3(cutPlane.normal), p)) / DVector3.Dot(new DVector3(cutPlane.normal), p - c2));

                //�V�������_��uv���v�Z�Ap��c�̊ԂŐ��`���
                Vector2 cross1Uv = Vector2.Lerp(attachedMesh.uv[attachedMesh.triangles[i + n1]], attachedMesh.uv[attachedMesh.triangles[i + n2]], (float)System.Math.Sqrt((cross1 - p).sqrMagnitude / (p - c1).sqrMagnitude));
                Vector2 cross2Uv = Vector2.Lerp(attachedMesh.uv[attachedMesh.triangles[i + n1]], attachedMesh.uv[attachedMesh.triangles[i + n3]], (float)System.Math.Sqrt((cross2 - p).sqrMagnitude / (p - c2).sqrMagnitude));

                //�{����DVector3���ł�肽�����悭�킩��Ȃ��̂�Vector3��InverseTransfromPoint���g�p
                cross1 = new DVector3(transform.InverseTransformPoint(cross1.ToVector3()));
                cross2 = new DVector3(transform.InverseTransformPoint(cross2.ToVector3()));

                //�f�ʂ����邽�߂Ɏ���Ă���
                crossVertices.Add(cross1);
                crossVertices.Add(cross2);


                //p�̂Q�ʂ�̏����A�J�b�g����ʂɑ΂��Ăǂ���ɂ��邩�ňقȂ�
                if ((p1Bool && !p2Bool && !p3Bool) || (!p1Bool && p2Bool && !p3Bool) || (!p1Bool && !p2Bool && p3Bool))
                {

                    //p���̃��b�V����ǉ�
                    vertices1.Add(cross1);
                    uvs1.Add(cross1Uv);
                    normals1.Add(attachedMesh.normals[attachedMesh.triangles[i + n1]]);
                    triangles1.Add(vertices1.Count - 1);

                    vertices1.Add(cross2);
                    uvs1.Add(cross2Uv);
                    normals1.Add(attachedMesh.normals[attachedMesh.triangles[i + n1]]);
                    triangles1.Add(vertices1.Count - 1);

                    vertices1.Add(new DVector3(attachedMesh.vertices[attachedMesh.triangles[i + n1]]));
                    uvs1.Add(attachedMesh.uv[attachedMesh.triangles[i + n1]]);
                    normals1.Add(attachedMesh.normals[attachedMesh.triangles[i + n1]]);
                    triangles1.Add(vertices1.Count - 1);

                    //c���̃��b�V����ǉ��P
                    vertices2.Add(cross2);
                    uvs2.Add(cross2Uv);
                    normals2.Add(attachedMesh.normals[attachedMesh.triangles[i + n1]]);
                    triangles2.Add(vertices2.Count - 1);

                    vertices2.Add(new DVector3(attachedMesh.vertices[attachedMesh.triangles[i + n2]]));
                    uvs2.Add(attachedMesh.uv[attachedMesh.triangles[i + n2]]);
                    normals2.Add(attachedMesh.normals[attachedMesh.triangles[i + n2]]);
                    triangles2.Add(vertices2.Count - 1);

                    vertices2.Add(new DVector3(attachedMesh.vertices[attachedMesh.triangles[i + n3]]));
                    uvs2.Add(attachedMesh.uv[attachedMesh.triangles[i + n3]]);
                    normals2.Add(attachedMesh.normals[attachedMesh.triangles[i + n3]]);
                    triangles2.Add(vertices2.Count - 1);

                    //c���̃��b�V����ǉ�2
                    vertices2.Add(cross2);
                    uvs2.Add(cross2Uv);
                    normals2.Add(attachedMesh.normals[attachedMesh.triangles[i + n1]]);
                    triangles2.Add(vertices2.Count - 1);

                    vertices2.Add(cross1);
                    triangles2.Add(vertices2.Count - 1);
                    uvs2.Add(cross1Uv);
                    normals2.Add(attachedMesh.normals[attachedMesh.triangles[i + n1]]);

                    vertices2.Add(new DVector3(attachedMesh.vertices[attachedMesh.triangles[i + n2]]));
                    uvs2.Add(attachedMesh.uv[attachedMesh.triangles[i + n2]]);
                    normals2.Add(attachedMesh.normals[attachedMesh.triangles[i + n2]]);
                    triangles2.Add(vertices2.Count - 1);
                }
                else
                {
                    //p���̃��b�V����ǉ�
                    vertices2.Add(cross1);
                    triangles2.Add(vertices2.Count - 1);
                    uvs2.Add(cross1Uv);
                    normals2.Add(attachedMesh.normals[attachedMesh.triangles[i + n1]]);

                    vertices2.Add(cross2);
                    triangles2.Add(vertices2.Count - 1);
                    uvs2.Add(cross2Uv);
                    normals2.Add(attachedMesh.normals[attachedMesh.triangles[i + n1]]);

                    vertices2.Add(new DVector3(attachedMesh.vertices[attachedMesh.triangles[i + n1]]));
                    uvs2.Add(attachedMesh.uv[attachedMesh.triangles[i + n1]]);
                    normals2.Add(attachedMesh.normals[attachedMesh.triangles[i + n1]]);
                    triangles2.Add(vertices2.Count - 1);

                    //c���̃��b�V����ǉ��P
                    vertices1.Add(cross2);
                    triangles1.Add(vertices1.Count - 1);
                    uvs1.Add(cross2Uv);
                    normals1.Add(attachedMesh.normals[attachedMesh.triangles[i + n1]]);

                    vertices1.Add(new DVector3(attachedMesh.vertices[attachedMesh.triangles[i + n2]]));
                    uvs1.Add(attachedMesh.uv[attachedMesh.triangles[i + n2]]);
                    normals1.Add(attachedMesh.normals[attachedMesh.triangles[i + n2]]);
                    triangles1.Add(vertices1.Count - 1);

                    vertices1.Add(new DVector3(attachedMesh.vertices[attachedMesh.triangles[i + n3]]));
                    uvs1.Add(attachedMesh.uv[attachedMesh.triangles[i + n3]]);
                    normals1.Add(attachedMesh.normals[attachedMesh.triangles[i + n3]]);
                    triangles1.Add(vertices1.Count - 1);

                    //c���̃��b�V����ǉ�2
                    vertices1.Add(cross2);
                    triangles1.Add(vertices1.Count - 1);
                    uvs1.Add(cross2Uv);
                    normals1.Add(attachedMesh.normals[attachedMesh.triangles[i + n1]]);

                    vertices1.Add(cross1);
                    triangles1.Add(vertices1.Count - 1);
                    uvs1.Add(cross1Uv);
                    normals1.Add(attachedMesh.normals[attachedMesh.triangles[i + n1]]);

                    vertices1.Add(new DVector3(attachedMesh.vertices[attachedMesh.triangles[i + n2]]));
                    uvs1.Add(attachedMesh.uv[attachedMesh.triangles[i + n2]]);
                    normals1.Add(attachedMesh.normals[attachedMesh.triangles[i + n2]]);
                    triangles1.Add(vertices1.Count - 1);
                }
            }
        }

        //mesh�����炷�������s���B(�f�ʈȊO�̏���)�A���Ɋ֐�����
        reduceMesh(ref vertices1, ref uvs1, ref normals1, cutPlane);
        //reduceMesh����Cut�̒��f�𔻒f�����ꍇ��return����
        if (returnBool)
        {
            return;
        }
        reduceMesh(ref vertices2, ref uvs2, ref normals2, cutPlane);
        if (returnBool)
        {
            return;
        }


        //�f�ʂ����鏈��
        if (crossVertices.Count != 0)
        {
            //�f�ʂŒ��_�����炷�����A������ɂ��钸�_��2�݂̂ɂ���
            for (int i = 0; i < crossVertices.Count; i += 2)
            {
                for (int k = i + 2; k < crossVertices.Count; k += 2)
                {
                    //4�̒��_���꒼����ɂ��邩�A�܂�2�̃x�N�g�������s���ǂ���
                    if (System.Math.Abs(DVector3.Dot((crossVertices[i] - crossVertices[i + 1]).normalized, (crossVertices[k] - crossVertices[k + 1]).normalized)) > 1 - delta)
                    {
                        //����̒��_�������ǂ������꒼����ɂ���
                        if ((crossVertices[i] - crossVertices[k]).sqrMagnitude < delta || (crossVertices[i] - crossVertices[k + 1]).sqrMagnitude < delta
                                                    || (crossVertices[i + 1] - crossVertices[k]).sqrMagnitude < delta || (crossVertices[i + 1] - crossVertices[k + 1]).sqrMagnitude < delta)
                        {
                            //�ȉ��d�Ȃ�_�ɉ����������A���[���c���Č������
                            if ((crossVertices[i] - crossVertices[k]).sqrMagnitude < (crossVertices[i + 1] - crossVertices[k]).sqrMagnitude)
                            {
                                crossVertices.Add(crossVertices[i + 1]);
                                if ((crossVertices[i] - crossVertices[k]).sqrMagnitude < (crossVertices[i] - crossVertices[k + 1]).sqrMagnitude)
                                {
                                    crossVertices.Add(crossVertices[k + 1]);
                                }
                                else
                                {
                                    crossVertices.Add(crossVertices[k]);
                                }
                                crossVertices.RemoveRange(k, 2);
                                crossVertices.RemoveRange(i, 2);
                            }
                            else
                            {
                                crossVertices.Add(crossVertices[i]);
                                if ((crossVertices[i + 1] - crossVertices[k]).sqrMagnitude < (crossVertices[i + 1] - crossVertices[k + 1]).sqrMagnitude)
                                {
                                    crossVertices.Add(crossVertices[k + 1]);
                                }
                                else
                                {
                                    crossVertices.Add(crossVertices[k]);
                                }
                                crossVertices.RemoveRange(k, 2);
                                crossVertices.RemoveRange(i, 2);

                            }
                            i -= 2;
                            break;
                        }

                    }

                }
            }

            //�f�ʂ̎O�p�`����鏈��

            //�������_������
            for (int i = 0; i < crossVertices.Count; i++)
            {
                for (int j = i + 1; j < crossVertices.Count; j++)
                {
                    if ((crossVertices[i] - crossVertices[j]).sqrMagnitude < delta)
                    {
                        crossVertices.RemoveAt(j);
                        i--;
                        break;
                    }
                }
            }

            //�O���̒��_����ёւ��AcrossVertices[0]��[1]����Ƃ��Ă��ꂼ��̓_����ёւ���
            for (int i = 2; i < crossVertices.Count; i++)
            {
                for (int j = i + 1; j < crossVertices.Count; j++)
                {
                    if (System.Math.Acos(DVector3.Dot((crossVertices[0] - crossVertices[1]).normalized, (crossVertices[0] - crossVertices[i]).normalized)) >= System.Math.Acos(DVector3.Dot((crossVertices[0] - crossVertices[1]).normalized, (crossVertices[0] - crossVertices[j]).normalized)))
                    {
                        //�p�x���������A�꒼����ɂ���ꍇ�B�{���͂Ȃ��͂������E�E�E
                        if (System.Math.Acos(DVector3.Dot((crossVertices[0] - crossVertices[1]).normalized, (crossVertices[0] - crossVertices[i]).normalized)) == System.Math.Acos(DVector3.Dot((crossVertices[0] - crossVertices[1]).normalized, (crossVertices[0] - crossVertices[j]).normalized)))
                        {
                            //���ς���
                            if ((crossVertices[0] - crossVertices[i]).sqrMagnitude > (crossVertices[0] - crossVertices[j]).sqrMagnitude)
                            {
                                crossVertices.Insert(0, crossVertices[j]);
                                crossVertices.RemoveAt(j + 1);
                            }
                            else
                            {
                                crossVertices.Insert(0, crossVertices[i]);
                                crossVertices.RemoveAt(i + 1);
                            }

                            i = 1;
                            break;
                        }
                        //���ёւ�
                        crossVertices.Insert(i, crossVertices[j]);
                        crossVertices.RemoveAt(j + 1);
                        i = 1;
                        break;
                    }
                }
            }



            for (int i = 1; i < crossVertices.Count - 1; i++)
            {
                //�f�ʂ�normal��uv�̐ݒ�Buv����ʂɐݒ肷��ꍇ�͕ς��Ă�������
                for (int j = 0; j < 3; j++)
                {
                    normals1.Add(-cutPlane.normal);
                    uvs1.Add(new Vector2(0, 0));
                    normals2.Add(cutPlane.normal);
                    uvs2.Add(new Vector2(0, 0));

                }
                //�f�ʂ̎O�p�`��ǉ�����@�ʂ̕\�̕������������Ȃ�悤�ɔ��f���Ēǉ�
                if (Vector3.Dot(transform.TransformDirection(DVector3.Cross((crossVertices[i] - crossVertices[0]).normalized, (crossVertices[i + 1] - crossVertices[i]).normalized).ToVector3()), cutPlane.normal) < delta)
                {
                    vertices1.Add(crossVertices[0]);
                    vertices1.Add(crossVertices[i]);
                    vertices1.Add(crossVertices[i + 1]);

                    vertices2.Add(crossVertices[i]);
                    vertices2.Add(crossVertices[0]);
                    vertices2.Add(crossVertices[i + 1]);
                }
                else
                {
                    vertices1.Add(crossVertices[i]);
                    vertices1.Add(crossVertices[0]);
                    vertices1.Add(crossVertices[i + 1]);

                    vertices2.Add(crossVertices[0]);
                    vertices2.Add(crossVertices[i]);
                    vertices2.Add(crossVertices[i + 1]);
                }
            }



        }
        //�ЂƂ�triangle�ɂ��Ă��ꂼ��3���̒��_������Ă��邽�ߍŌ�ɏ��Ԓʂ�ɂ����
        triangles1.Clear();
        for (int i = 0; i < vertices1.Count; i++)
        {
            triangles1.Add(i);
        }
        triangles2.Clear();
        for (int i = 0; i < vertices2.Count; i++)
        {
            triangles2.Add(i);
        }

        //DVector3��ʏ��Vector3�ɒ����A�����ƌ�����肩�������肻��
        var list1 = new List<Vector3>();
        for (int i = 0; i < vertices1.Count; i++)
        {
            list1.Add(vertices1[i].ToVector3());
        }
        var list2 = new List<Vector3>();
        for (int i = 0; i < vertices2.Count; i++)
        {
            list2.Add(vertices2[i].ToVector3());
        }


        //�J�b�g��̃I�u�W�F�N�g�����A���낢��Ƃ����
        GameObject obj = new GameObject("cut obj", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider), typeof(Rigidbody), typeof(MeshCut));
        var mesh = new Mesh();
        mesh.vertices = list1.ToArray();
        mesh.triangles = triangles1.ToArray();
        mesh.uv = uvs1.ToArray();
        mesh.normals = normals1.ToArray();
        obj.GetComponent<MeshFilter>().mesh = mesh;
        //obj.GetComponent<MeshRenderer>().materials = GetComponent<MeshRenderer>().materials;
        for(int i =0; i < GetComponent<MeshRenderer>().materials.Length; i++)
        {
            Debug.Log("materials��" + i + "�Ԗ�:\n"+ GetComponent<MeshRenderer>().materials[i]);
        }
        
        obj.GetComponent<MeshCollider>().sharedMesh = mesh;
        obj.GetComponent<MeshCollider>().cookingOptions = MeshColliderCookingOptions.CookForFasterSimulation;
        obj.GetComponent<MeshCollider>().convex = true;
        Debug.Log("material" + GetComponent<Collider>().material);
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        obj.transform.localScale = transform.localScale;
        obj.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
        obj.GetComponent<Rigidbody>().angularVelocity = GetComponent<Rigidbody>().angularVelocity;
        obj.GetComponent<MeshCut>().skinWidth = skinWidth;

        GameObject obj2 = new GameObject("cut obj", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider), typeof(Rigidbody), typeof(MeshCut));
        var mesh2 = new Mesh();
        mesh2.vertices = list2.ToArray();
        mesh2.triangles = triangles2.ToArray();
        mesh2.uv = uvs2.ToArray();
        mesh2.normals = normals2.ToArray();
        obj2.GetComponent<MeshFilter>().mesh = mesh2;
        obj2.GetComponent<MeshRenderer>().materials = GetComponent<MeshRenderer>().materials;
        obj2.GetComponent<MeshCollider>().sharedMesh = mesh2;
        obj2.GetComponent<MeshCollider>().cookingOptions = MeshColliderCookingOptions.CookForFasterSimulation;
        obj2.GetComponent<MeshCollider>().convex = true;
        obj2.GetComponent<MeshCollider>().material = GetComponent<Collider>().material;
        obj2.transform.position = transform.position;
        obj2.transform.rotation = transform.rotation;
        obj2.transform.localScale = transform.localScale;
        obj2.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
        obj2.GetComponent<Rigidbody>().angularVelocity = GetComponent<Rigidbody>().angularVelocity;
        obj2.GetComponent<MeshCut>().skinWidth = skinWidth;

        //���̃I�u�W�F�N�g���f�X�g���C
        Destroy(gameObject);

    }

    [SerializeField] Material material;

    void reduceMesh(ref List<DVector3> vertices, ref List<Vector2> uvs, ref List<Vector3> normals, Plane cutPlane)
    {

        var verticeIndices = new List<int>();
        var pVertices = new List<DVector3>();
        var pNormals = new List<Vector3>();
        var pUvs = new List<Vector2>();

        for (int i = 0; i < vertices.Count; i += 3)
        {
            //�܂��͓��ꕽ�ʏ�ɂ���O�p�`�������o�����A��ƂȂ�i���Ƃ肠�����ǉ�
            verticeIndices.Clear();
            verticeIndices.Add(i);

            for (int j = i + 3; j < vertices.Count; j += 3)
            {

                //����̕��ʏ�ɂ���O�p�`���ǂ����Bdelta�Œ���
                if (DVector3.Dot(DVector3.Cross((vertices[i + 1] - vertices[i]).normalized, (vertices[i + 2] - vertices[i + 1]).normalized).normalized,
                 DVector3.Cross((vertices[j + 1] - vertices[j]).normalized, (vertices[j + 2] - vertices[j + 1]).normalized).normalized) > 1 - delta)
                {
                    verticeIndices.Add(j);
                }

                //�S�Ă̎O�p�`�ɂ��Čv�Z���ă��[�v�̍Ō�ɍs������
                if (j == vertices.Count - 3)
                {
                    //�O�p�`��1�̏ꍇ�͉�������K�v�Ȃ�
                    if (verticeIndices.Count > 1)
                    {
                        //���ʏ�̎O�p�`��2�ȏ゠��ꍇ
                        //p�̓��ꕨ�ɎO�p�`��3�̒����ɂ��Ă����
                        for (int k = 0; k < verticeIndices.Count; k++)
                        {
                            for (int l = 0; l < 3; l++)
                            {
                                pVertices.Add(vertices[verticeIndices[k] + l]);
                                pNormals.Add(normals[verticeIndices[k] + l]);
                                pUvs.Add(uvs[verticeIndices[k] + l]);

                                pVertices.Add(vertices[verticeIndices[k] + numRep(l + 1)]);
                                pNormals.Add(normals[verticeIndices[k] + numRep(l + 1)]);
                                pUvs.Add(uvs[verticeIndices[k] + numRep(l + 1)]);
                            }


                        }



                        //�����������������i�ʂ̊O���łȂ����������j
                        int sameLineCount = 0;
                        for (int k = 0; k < pVertices.Count; k += 2)
                        {
                            for (int l = k + 2; l < pVertices.Count; l += 2)
                            {
                                if (((pVertices[l + 1] - pVertices[k]).sqrMagnitude < delta) && ((pVertices[l] - pVertices[k + 1]).sqrMagnitude < delta))
                                {
                                    sameLineCount++;
                                    pVertices.RemoveRange(l, 2);
                                    pVertices.RemoveRange(k, 2);
                                    pNormals.RemoveRange(l, 2);
                                    pNormals.RemoveRange(k, 2);
                                    pUvs.RemoveRange(l, 2);
                                    pUvs.RemoveRange(k, 2);
                                    k -= 2;

                                    break;
                                }
                            }
                        }
                        //���ꕽ�ʏ��n�̗אڂ���O�p�`�ɂ�n-1�̓���������������͂����A�Ȃ��ꍇ(�����܂ł̏��������܂������Ă��Ȃ��ꍇ)�͏�������߂�
                        //������ւ�͌������@���@�V���������ǉ�
                        if (sameLineCount != verticeIndices.Count - 1)
                        {
                            //���ꕽ�ʏ�ɂ��邪�O�p�`���אڂ��Ă��Ȃ��ꍇ�i�{�����ꕽ�ʏ�Ɣ��肳���͂��̎O�p�`��
                            //���ꕽ�ʏ�Ɣ��肳��Ȃ�����(�ؒf�ʂ̒��_�����߂�ۂɂ���2�_���قړ����l������Ă��܂����ꍇ)�j
                            //��cutPlane��������Ƃ��炵�Ă�����x�v�Z������B

                            //��x��������������悤�Ɉȉ��̕���ŃS�j���S�j��
                            if (returnBool2)
                            {
                                returnBool = true;
                                returnBool3 = true;
                                return;
                            }
                            returnBool2 = true;
                            //���炷�ʂ͎b��I
                            Cut(new Plane(cutPlane.normal, -cutPlane.normal * cutPlane.distance + new Vector3(0.02f, 0.02f, 0.02f)));
                            if (returnBool3)
                            {
                                returnBool = false;
                                returnBool2 = false;
                            }
                            else
                            {
                                returnBool = true;
                            }
                            return;

                        }
                        for (int l = 0; l < pVertices.Count; l += 2)
                        {
                            for (int k = l + 2; k < pVertices.Count; k += 2)
                            {
                                //4�̒��_���꒼����ɂ��邩�A2�̃x�N�g�������s���ǂ���
                                if (System.Math.Abs(DVector3.Dot((pVertices[l] - pVertices[l + 1]).normalized, (pVertices[k] - pVertices[k + 1]).normalized)) > 1 - delta)
                                {
                                    //����̓_�������꒼����ɂ���
                                    if ((pVertices[l] - pVertices[k]).sqrMagnitude < delta || (pVertices[l] - pVertices[k + 1]).sqrMagnitude < delta
                                                || (pVertices[l + 1] - pVertices[k]).sqrMagnitude < delta || (pVertices[l + 1] - pVertices[k + 1]).sqrMagnitude < delta)
                                    {

                                        //�ȉ��d�Ȃ�_�ɉ����������A���[���c���Č������
                                        if ((pVertices[l] - pVertices[k]).sqrMagnitude < (pVertices[l + 1] - pVertices[k]).sqrMagnitude)
                                        {
                                            pVertices.Add(pVertices[l + 1]);
                                            pNormals.Add(pNormals[l + 1]);
                                            pUvs.Add(pUvs[l + 1]);

                                            if ((pVertices[l] - pVertices[k]).sqrMagnitude < (pVertices[l] - pVertices[k + 1]).sqrMagnitude)
                                            {
                                                pVertices.Add(pVertices[k + 1]);
                                                pNormals.Add(pNormals[k + 1]);
                                                pUvs.Add(pUvs[k + 1]);

                                            }
                                            else
                                            {
                                                pVertices.Add(pVertices[k]);
                                                pNormals.Add(pNormals[k]);
                                                pUvs.Add(pUvs[k]);

                                            }
                                            pVertices.RemoveRange(k, 2);
                                            pVertices.RemoveRange(l, 2);
                                            pNormals.RemoveRange(k, 2);
                                            pNormals.RemoveRange(l, 2);
                                            pUvs.RemoveRange(k, 2);
                                            pUvs.RemoveRange(l, 2);
                                        }
                                        else
                                        {
                                            pVertices.Add(pVertices[l]);
                                            pNormals.Add(pNormals[l]);
                                            pUvs.Add(pUvs[l]);
                                            if ((pVertices[l + 1] - pVertices[k]).sqrMagnitude < (pVertices[l + 1] - pVertices[k + 1]).sqrMagnitude)
                                            {
                                                pVertices.Add(pVertices[k + 1]);
                                                pNormals.Add(pNormals[k + 1]);
                                                pUvs.Add(pUvs[k + 1]);
                                            }
                                            else
                                            {
                                                pVertices.Add(pVertices[k]);
                                                pNormals.Add(pNormals[k]);
                                                pUvs.Add(pUvs[k]);
                                            }
                                            pVertices.RemoveRange(k, 2);
                                            pVertices.RemoveRange(l, 2);
                                            pNormals.RemoveRange(k, 2);
                                            pNormals.RemoveRange(l, 2);
                                            pUvs.RemoveRange(k, 2);
                                            pUvs.RemoveRange(l, 2);
                                        }

                                        l -= 2;
                                        break;
                                    }

                                }
                            }


                        }


                        //�������_������
                        for (int k = 0; k < pVertices.Count; k++)
                        {
                            for (int l = k + 1; l < pVertices.Count; l++)
                            {
                                if ((pVertices[k] - pVertices[l]).sqrMagnitude < delta)
                                {
                                    pVertices.RemoveAt(l);
                                    pNormals.RemoveAt(l);
                                    pUvs.RemoveAt(l);
                                }
                            }
                        }

                        //�O����̓_�����Ԃɕ��ёւ���.pVertices[0],[1]����Ƃ��ĕ��ёւ�
                        for (int k = 2; k < pVertices.Count; k++)
                        {
                            for (int l = k + 1; l < pVertices.Count; l++)
                            {
                                if (System.Math.Acos(DVector3.Dot((pVertices[0] - pVertices[1]).normalized, (pVertices[0] - pVertices[k]).normalized)) >= System.Math.Acos(DVector3.Dot((pVertices[0] - pVertices[1]).normalized, (pVertices[0] - pVertices[l]).normalized)))
                                {
                                    //�������Ȃ��Ă��܂��ꍇ
                                    if (System.Math.Acos(DVector3.Dot((pVertices[0] - pVertices[1]).normalized, (pVertices[0] - pVertices[k]).normalized)) == System.Math.Acos(DVector3.Dot((pVertices[0] - pVertices[1]).normalized, (pVertices[0] - pVertices[l]).normalized)))
                                    {
                                        if ((pVertices[0] - pVertices[k]).sqrMagnitude > (pVertices[0] - pVertices[l]).sqrMagnitude)
                                        {
                                            pVertices.Insert(0, pVertices[l]);
                                            pVertices.RemoveAt(l + 1);
                                            pNormals.Insert(0, pNormals[l]);
                                            pNormals.RemoveAt(l + 1);
                                            pUvs.Insert(0, pUvs[l]);
                                            pUvs.RemoveAt(l + 1);
                                        }
                                        else
                                        {
                                            pVertices.Insert(0, pVertices[k]);
                                            pVertices.RemoveAt(k + 1);
                                            pNormals.Insert(0, pNormals[k]);
                                            pNormals.RemoveAt(k + 1);
                                            pUvs.Insert(0, pUvs[k]);
                                            pUvs.RemoveAt(k + 1);
                                        }
                                        k = 1;
                                        break;
                                    }
                                    //���ёւ�
                                    pVertices.Insert(k, pVertices[l]);
                                    pVertices.RemoveAt(l + 1);
                                    pNormals.Insert(k, pNormals[l]);
                                    pNormals.RemoveAt(l + 1);
                                    pUvs.Insert(k, pUvs[l]);
                                    pUvs.RemoveAt(l + 1);
                                    k = 1;
                                    break;
                                }
                            }
                        }


                        //�O����̕��ёւ���ꂽ���_��S�ĎO�p�`�Ō��Ԃ悤�ɒǉ�
                        for (int k = 1; k < pVertices.Count - 1; k++)
                        {
                            vertices.Insert(0, pVertices[k + 1]);
                            normals.Insert(0, pNormals[k + 1]);
                            uvs.Insert(0, pUvs[k + 1]);

                            vertices.Insert(0, pVertices[k]);
                            normals.Insert(0, pNormals[k]);
                            uvs.Insert(0, pUvs[k]);


                            vertices.Insert(0, pVertices[0]);
                            normals.Insert(0, pNormals[0]);
                            uvs.Insert(0, pUvs[0]);
                        }

                        //�ǉ������̂ŌÂ��̂�����
                        for (int k = verticeIndices.Count - 1; k >= 0; k--)
                        {
                            vertices.RemoveRange(verticeIndices[k] + 3 * (pVertices.Count - 2), 3);
                            normals.RemoveRange(verticeIndices[k] + 3 * (pVertices.Count - 2), 3);
                            uvs.RemoveRange(verticeIndices[k] + 3 * (pVertices.Count - 2), 3);
                        }
                        //���������O�p�`���l�����ă��[�v�̈ʒu�𒲐�
                        i += 3 * (pVertices.Count - 3);
                        //���������Ă���
                        pVertices.Clear();
                        pNormals.Clear();
                        pUvs.Clear();

                        break;
                    }


                }

            }


        }
    }


    //���[�v�Ŏg�����������֐�
    int numRep(int i)
    {
        if (i % 3 == 0)
        {
            return 0;
        }
        else if (i % 3 == 1)
        {
            return 1;
        }
        else if (i % 3 == 2)
        {
            return 2;
        }
        else
        {
            return 0;
        }
    }

}

//Vector3��double�Ŏg���N���X�A�g���@�\�̂�
public class DVector3
{
    public double x;
    public double y;
    public double z;


    public DVector3(Vector3 a)
    {
        x = a.x;
        y = a.y;
        z = a.z;
    }
    public DVector3(double a, double b, double c)
    {
        x = a;
        y = b;
        z = c;
    }
    public double sqrMagnitude
    {
        get { return x * x + y * y + z * z; }
    }

    public Vector3 ToVector3()
    {
        return new Vector3((float)x, (float)y, (float)z);
    }

    public override string ToString()
    {
        return string.Format("({0:0.00000}, {1:0.00000}, {2:0.00000})", x, y, z);
    }
    public DVector3 normalized
    {
        get { return new DVector3(x / System.Math.Sqrt(this.sqrMagnitude), y / System.Math.Sqrt(this.sqrMagnitude), z / System.Math.Sqrt(this.sqrMagnitude)); }
    }

    public static double Dot(DVector3 a, DVector3 b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }

    public static DVector3 Cross(DVector3 a, DVector3 b)
    {
        return new DVector3(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
    }

    public static DVector3 operator -(DVector3 a, DVector3 b)
    {
        return new DVector3(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    public static DVector3 operator +(DVector3 a, DVector3 b)
    {
        return new DVector3(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public static DVector3 operator *(DVector3 a, double b)
    {
        return new DVector3(a.x * b, a.y * b, a.z * b);
    }

    public static DVector3 operator /(DVector3 a, double b)
    {
        return new DVector3(a.x / b, a.y / b, a.z / b);
    }

}


