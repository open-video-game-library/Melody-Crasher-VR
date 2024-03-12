using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Config : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Slider slider_w;
    [SerializeField] GameObject bar;
    [SerializeField] GameObject bar2;
    [SerializeField] Toggle toggle;
    [SerializeField] Toggle bothToggle;
    [SerializeField] TMP_Dropdown dropdown_R;
    [SerializeField] TMP_Dropdown dropdown_L;

    [SerializeField] GameObject Sword;
    [SerializeField] GameObject Sword2;
    [SerializeField] GameObject Sword_Blade;
    [SerializeField] GameObject Sword_Blade2;

    [SerializeField] GameObject Hunmer; //righthand sword
    [SerializeField] GameObject Hunmer2; //lefthand sword

    //[SerializeField] Slider playerHeightSlider;
    //[SerializeField] Transform playerHeight;

    [SerializeField] Slider playTime;

    [SerializeField] Slider playerWeaponRotX;
    [SerializeField] Slider playerWeaponRotZ;

    [SerializeField] Transform RightHandAnchor;
    [SerializeField] Transform LeftHandAnchor;
    [SerializeField] Transform Bar; //righthand sword
    [SerializeField] Transform Bar2; //lefthand sword
    [SerializeField] Transform hunmer; //righthand hunmer
    [SerializeField] Transform hunmer_; //righthand hunmer tsuka
    [SerializeField] Transform hunmer2; //lefthand hunmer
    [SerializeField] Transform hunmer2_; //righthand hunmer tsuka

    [SerializeField] Transform Gun;
    [SerializeField] Transform Gun2;

    [SerializeField] Transform HunmerTransform;
    [SerializeField] Transform Hunmer2Transform;

    [SerializeField] SkinnedMeshRenderer R_hand;
    [SerializeField] SkinnedMeshRenderer L_hand;

    //public TextMeshProUGUI tmpro_playerHeight;
    public TextMeshProUGUI tmpro_rotX;
    public TextMeshProUGUI tmpro_rotZ;
    public TextMeshProUGUI tmpro_bar;
    public TextMeshProUGUI tmpro_playTime;

    public float m_force = 20;
    public float m_radius = 5;
    
    Vector3 defaultbar;
    Vector3 defaulthunmer;
    Vector3 defaulthunmer_;
    Vector3 defaultblade;

    public static bool enable;

    [SerializeField] LaserPointer laserPointer;
    // Start is called before the first frame update
    void Start()
    {
        defaultbar = bar.transform.localScale;
        defaulthunmer = hunmer.transform.localScale;
        defaulthunmer_ = hunmer_.transform.localScale;
        defaultblade = Sword_Blade.transform.localScale;
    }

    private void Update()
    {
        
        Result.playTime = playTime.value;
        tmpro_playTime.text = Result.playTime.ToString("N0");

        bar.transform.localScale =
            new Vector3(slider_w.value * defaultbar.x, slider.value * defaultbar.y, slider_w.value * defaultbar.z);
        bar.transform.localPosition =
            new Vector3(0f, (bar.transform.localScale.y / 2f), 0f);
        hunmer.transform.localScale =
            new Vector3(slider_w.value * defaulthunmer.x, slider_w.value * defaulthunmer.y, slider_w.value * defaulthunmer.z);
        hunmer.transform.localPosition =
            new Vector3(0f, (slider.value), 0f);
        hunmer_.transform.localScale =
            new Vector3(0.0094f, 0.005f * slider.value, 0.0094f);
        hunmer_.transform.localPosition =
            new Vector3(0f, slider.value + 0.10f, 0f);
        Sword_Blade.transform.localPosition =
            new Vector3(0f, -0.06232678f + defaultblade.y * slider.value / 2f, -18.08643f);
        Sword_Blade.transform.localScale =
            new Vector3(defaultblade.x, defaultblade.y, defaultblade.z * slider.value);


        bar2.transform.localScale =
            new Vector3(defaultbar.x, slider.value * defaultbar.y, defaultbar.z);
        bar2.transform.localPosition =
            new Vector3(0f, (bar.transform.localScale.y / 2f), 0f);
        hunmer2.transform.localScale =
            new Vector3(slider_w.value * defaulthunmer.x, slider_w.value * defaulthunmer.y, slider_w.value * defaulthunmer.z);
        hunmer2.transform.localPosition =
            new Vector3(0f, (slider.value), 0f);
        hunmer2_.transform.localScale =
            new Vector3(0.0094f, 0.005f * slider.value, 0.0094f);
        hunmer2_.transform.localPosition =
            new Vector3(0f, slider.value + 0.10f, 0f);
        Sword_Blade2.transform.localPosition =
            new Vector3(0f, -0.06232678f + defaultblade.y * slider.value / 2f, -18.08643f);
        Sword_Blade2.transform.localScale =
            new Vector3(defaultblade.x, defaultblade.y , defaultblade.z * slider.value);


        if (slider.value <= 0.1f)
        {
           
            bar.GetComponent<MeshRenderer>().enabled = false;
            bar2.GetComponent<MeshRenderer>().enabled = false;
            bar.transform.parent.GetComponent<MeshRenderer>().enabled = false;
            bar2.transform.parent.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            bar.GetComponent<MeshRenderer>().enabled = true;
            bar2.GetComponent<MeshRenderer>().enabled = true;
            bar.transform.parent.GetComponent<MeshRenderer>().enabled = true;
            bar2.transform.parent.GetComponent<MeshRenderer>().enabled = true;
        }

        if (slider_w.value <= 0.1f)
        {
            bar.GetComponent<MeshRenderer>().enabled = false;
            bar2.GetComponent<MeshRenderer>().enabled = false;
            bar.transform.parent.GetComponent<MeshRenderer>().enabled = false;
            bar2.transform.parent.GetComponent<MeshRenderer>().enabled = false;
            hunmer.GetComponent<MeshRenderer>().enabled = false;
            hunmer2.GetComponent<MeshRenderer>().enabled = false;
            hunmer.transform.parent.GetComponent<MeshRenderer>().enabled = false;
            hunmer2.transform.parent.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            bar.GetComponent<MeshRenderer>().enabled = true;
            bar2.GetComponent<MeshRenderer>().enabled = true;
            bar.transform.parent.GetComponent<MeshRenderer>().enabled = true;
            bar2.transform.parent.GetComponent<MeshRenderer>().enabled = true;
            hunmer.GetComponent<MeshRenderer>().enabled = true;
            hunmer2.GetComponent<MeshRenderer>().enabled = true;
            hunmer.transform.parent.GetComponent<MeshRenderer>().enabled = true;
            hunmer2.transform.parent.GetComponent<MeshRenderer>().enabled = true;
        }


        if (bothToggle.isOn == true)
        {
            Bar.gameObject.SetActive(true);
            Bar2.gameObject.SetActive(true);
        }
        else
        {
            if (toggle.isOn == false)
            {
                Bar.gameObject.SetActive(true);
                Bar2.gameObject.SetActive(false);
            }
            else
            {
                Bar.gameObject.SetActive(false);
                Bar2.gameObject.SetActive(true);
            }
        }

        Bar2.transform.parent = LeftHandAnchor;
        Bar2.transform.localPosition = new Vector3(0f, 0f, 0f);
        Bar2.transform.localEulerAngles = new Vector3(90f + playerWeaponRotX.value, 0f, 0f + playerWeaponRotZ.value);


        Bar.transform.parent = RightHandAnchor;
        Bar.transform.localPosition = new Vector3(0f, 0f, 0f);
        Bar.transform.localEulerAngles = new Vector3(90f + playerWeaponRotX.value, 0f, 0f + playerWeaponRotZ.value);

        tmpro_bar.text = (slider.value).ToString("N1");
        //tmpro_playerHeight.text = playerHeightSlider.value.ToString("N2");
        //tmpro_rotX.text = playerWeaponRotX.value.ToString("N2");
        //tmpro_rotZ.text = playerWeaponRotZ.value.ToString("N2");

        if(dropdown_R.value == 0)
        {
            R_hand.enabled = false;

            Bar.gameObject.SetActive(true);
            HunmerTransform.gameObject.SetActive(false);
            Gun.gameObject.SetActive(false);

            Sword.gameObject.SetActive(false);
        }
        else if(dropdown_R.value == 1)
        {
            R_hand.enabled = true;

            Bar.gameObject.SetActive(false);
            HunmerTransform.gameObject.SetActive(false);
            Gun.gameObject.SetActive(false);

            Sword.gameObject.SetActive(false);
        }
        else if (dropdown_R.value == 2)
        {
            R_hand.enabled = false;

            Bar.gameObject.SetActive(false);
            HunmerTransform.gameObject.SetActive(true);
            Gun.gameObject.SetActive(false);

            Sword.gameObject.SetActive(false);
        }
        else if (dropdown_R.value == 3)
        {
            R_hand.enabled = false;

            Bar.gameObject.SetActive(false);
            HunmerTransform.gameObject.SetActive(false);
            Gun.gameObject.SetActive(true);
            Sword.gameObject.SetActive(false);
        }
        else if (dropdown_R.value == 4)
        {
            R_hand.enabled = false;

            Bar.gameObject.SetActive(false);
            HunmerTransform.gameObject.SetActive(false);
            Gun.gameObject.SetActive(false);
            Sword.gameObject.SetActive(true);
        }

        if (dropdown_L.value == 0)
        {
            L_hand.enabled = false;

            Bar2.gameObject.SetActive(true);
            Hunmer2Transform.gameObject.SetActive(false);
            Gun2.gameObject.SetActive(false);
            Sword2.gameObject.SetActive(false);
        }
        else if (dropdown_L.value == 1)
        {
            L_hand.enabled = true;

            Bar2.gameObject.SetActive(false);
            Hunmer2Transform.gameObject.SetActive(false);
            Gun2.gameObject.SetActive(false);
            Sword2.gameObject.SetActive(false);
        }
        else if (dropdown_L.value == 2)
        {
            L_hand.enabled = false;

            Bar2.gameObject.SetActive(false);
            Hunmer2Transform.gameObject.SetActive(true);
            Gun2.gameObject.SetActive(false);
            Sword2.gameObject.SetActive(false);
        }
        else if (dropdown_L.value == 3)
        {
            L_hand.enabled = false;

            Bar2.gameObject.SetActive(false);
            Hunmer2Transform.gameObject.SetActive(false);
            Gun2.gameObject.SetActive(true);
            Sword2.gameObject.SetActive(false);
        }
        else if (dropdown_L.value == 4)
        {
            L_hand.enabled = false;

            Bar2.gameObject.SetActive(false);
            Hunmer2Transform.gameObject.SetActive(false);
            Gun2.gameObject.SetActive(false);
            Sword2.gameObject.SetActive(true);
        }
    }

    private void LateUpdate()
    {
        //playerHeight.position = new Vector3(0f, playerHeightSlider.value, 0f);
    }
}
