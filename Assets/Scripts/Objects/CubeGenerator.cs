using UnityEngine;

public class CubeGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject cubePrefab;

    [SerializeField]
    private Material defaultSky;

    private Vector3 defaultSpeed = new Vector3(0.0f, 0.0f, -1.0f);

    [SerializeField]
    private GameObject notes;

    private float hight;

    private float generateCount;

    [SerializeField]
    private Material sky;

    [SerializeField]
    private GameObject monster;

    [SerializeField]
    private Material sea;

    [SerializeField]
    private GameObject taru;

    public NotesGenerator ng;

    public bool isPlay;

    enum Priset{
        A,
        B,
        C
    }

    [SerializeField]
    private Priset priset = Priset.A;

    // Start is called before the first frame update
    void Start()
    {
        GenerateCube(0.0f, 0.0f);
        notes = cubePrefab;
    }

    public void ChangePrefab(GameObject a, Material b)
    {
        if (a == null) return;
        notes = a;

        if (b == null) return;
        RenderSettings.skybox = b;
    }

    // Update is called once per frame
    void Update()
    {
        //enum test
        switch(priset){
            case Priset.B:
                ChangePrefab(monster, sky);
                break;
            case Priset.A:
                ChangePrefab(cubePrefab, defaultSky);
                break;
            case Priset.C:
                ChangePrefab(taru, sea);
                break;
        }

        if (GameStateManager.GameState == GameState.End) { return; }

        hight = GameStateManager.defauldCenterEyeAnchor.position.y * 8.0f / 9.0f;

        generateCount += Time.deltaTime;

        

        if (generateCount > CubeParameters.span && GameStateManager.GameState != GameState.Play)
        {
            float startPointX = Random.Range(-0.75f * GameStateManager.defauldCenterEyeAnchor.position.y, 0.75f * GameStateManager.defauldCenterEyeAnchor.position.y);
            float startPointY = Random.Range(-0.75f * GameStateManager.defauldCenterEyeAnchor.position.y, 0.75f * GameStateManager.defauldCenterEyeAnchor.position.y);

            GenerateCube(startPointX, startPointY);
            generateCount = 0.0f;
            return;
        }

        if (isPlay)
        {

        }
        
    }

    public void GenerateCube(float startPointX, float startPoiontY)
    {
        float x = Random.Range(-0.75f * GameStateManager.defauldCenterEyeAnchor.position.y, 0.75f * GameStateManager.defauldCenterEyeAnchor.position.y);
        float y = Random.Range(-0.75f * GameStateManager.defauldCenterEyeAnchor.position.y, 0.75f * GameStateManager.defauldCenterEyeAnchor.position.y);
        GameObject cube = Instantiate(notes);

        CubeController cubeController = cube.GetComponent<CubeController>();
        cubeController.startPoint = new Vector3(GameStateManager.defauldCenterEyeAnchor.position.x + x, hight + y, CubeParameters.startPointZ);
        cubeController.speed = defaultSpeed;
    }
}
