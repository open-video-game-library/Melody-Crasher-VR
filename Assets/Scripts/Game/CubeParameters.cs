using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CubeParameters : MonoBehaviour
{
    [SerializeField]
    private Slider sizeSlider;
    [SerializeField]
    private TextMeshProUGUI sizeSliderText;

    [SerializeField]
    private Slider speedSlider;
    [SerializeField]
    private TextMeshProUGUI speedSliderText;

    [SerializeField]
    private Slider startPointZSlider;
    [SerializeField]
    private TextMeshProUGUI startPointZSliderText;

    [SerializeField]
    private Slider spanSlider;
    [SerializeField]
    private TextMeshProUGUI spanSliderText;

    public static float size;
    public static float speed;
    public static float startPointZ;
    public static float span;

    

    public static Vector3 StartVector3 { get; internal set; }

    // Start is called before the first frame update
    void Awake()
    {
        ChangeValue();
    }

    private void Update()
    {
        //if (Config.enable)
        //{
        //    GetComponent<CanvasGroup>().alpha = 1.0f;
        //    GetComponent<CanvasGroup>().blocksRaycasts = true;
        //}
        //else
        //{
        //    GetComponent<CanvasGroup>().alpha = 0.0f;
        //    GetComponent<CanvasGroup>().blocksRaycasts = false;
        //}
    }

    // Update is called once per frame
    public void ChangeValue()
    {
        sizeSliderText.text = (sizeSlider.value).ToString("N2");

        speedSliderText.text = (speedSlider.value).ToString("N2");

        startPointZSliderText.text = (startPointZSlider.value).ToString("N2");

        spanSliderText.text = (spanSlider.value).ToString("N2");

        size = sizeSlider.value;
        speed = speedSlider.value;
        startPointZ = startPointZSlider.value;
        span = spanSlider.value;
    }
}
