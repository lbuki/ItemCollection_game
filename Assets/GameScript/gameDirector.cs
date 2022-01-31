using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class gameDirector : MonoBehaviour
{

    [Tooltip("重力の強さ")]
    public Vector3 gravityPower;

    //-------------------------



    //-------------------------

    [Tooltip("スタミナのテキストオブジェクトを入れるとこ")]
    public TextMeshProUGUI staminaText;
    string staminaValue;
    float maxTime = 0;

    //-------------------------

    int frameCount = 0;
    float lapTime = 0;
    [SerializeField,Tooltip("fpsのテキストオブジェクトを入れるとこ")]
    private TextMeshProUGUI fpsText;
    //public GameObject fpsObject = null;
    string fpsRate;

    //-------------------------

    Color lightColor;//RGBは0~1の間の数値
    float R;
    float G;
    float B;
    float A;
    //chase = falseの時の色
    const float setR_f = 70f;
    const float setG_f = 110f;
    const float setB_f = 170f;
    //chase == trueの時の色
    const float setR_t = 110f;
    const float setG_t = 30f;
    const float setB_t = 40f;
    const float frame = 60f;

    //-------------------------

    GameObject staminaGauge;
    GameObject staminaGauge_back;
    GameObject player;
    GameObject enemy;
    GameObject D_light;
    GameObject effect;
    charaController charaScript;
    void Start()
    {
        this.staminaGauge = GameObject.Find("staminaGauge");
        this.staminaGauge_back = GameObject.Find("staminaGauge_back");
        this.player = GameObject.Find("SD_unitychan_humanoid");
        this.enemy = GameObject.Find("Misaki_win_humanoid");
        this.D_light = GameObject.Find("Directional Light");
        this.effect = GameObject.Find("effect");
        this.charaScript = player.GetComponent<charaController>();
        this.staminaValue = null;
        this.lightColor = D_light.GetComponent<Light>().color;
        R = setR_f;
        G = setG_f;
        B = setB_f;
        A = 255f;
    }

    // Update is called once per frame
    void Update()
    {
        fpsDisplay();
        staminaDisplay();
        staminaGaugeDisplay();
        colorChanger();
    }
    void fpsDisplay()
    {
        this.frameCount++;
        this.lapTime += Time.deltaTime;
        if (lapTime >= 0.5f) //0.5秒毎にfps計算
        {
            float fps = 1.0f * frameCount / lapTime;
            this.fpsRate = $"FPS:{fps.ToString("F2")}";
            this.fpsText.text = fpsRate;
            this.frameCount = 0;
            this.lapTime = 0f;
        }
    }
    void staminaGaugeDisplay()
    {
        this.staminaGauge.GetComponent<Image>().fillAmount = charaScript.stamina / 100.0f;//スタミナゲージの更新
        if(charaScript.stamina >= 100)
        {
            this.maxTime += Time.deltaTime;
        }
        else
        {
            this.maxTime = 0f;
        }
        if(maxTime > 2.0f)
        {
            hideGauge();
        }
        else
        {
            showGauge();
        }
    }
    public void hideGauge()
    {
        this.staminaGauge.GetComponent<Image>().enabled = false;
        this.staminaGauge_back.GetComponent<Image>().enabled = false;
    }
    void showGauge()
    {
        this.staminaGauge.GetComponent<Image>().enabled = true;
        this.staminaGauge_back.GetComponent<Image>().enabled = true;
    }
    void staminaDisplay()
    {
        this.staminaValue = $"energy:{charaScript.stamina.ToString("F0")}";
        this.staminaText.text = staminaValue;
    }
    void colorChanger()
    {
        if (enemy.GetComponent<enemyController>().chase == false)
        {
            if (R > setR_f)
            {
                R -= (Mathf.Abs(setR_f - setR_t) / frame);//1秒くらいで均一に色変更が終わるようにする
            }
            else
            {
                R = setR_f;
            }
            if (G < setG_f)
            {
                G += (Mathf.Abs(setG_f - setG_t) / frame);
            }
            else
            {
                G = setG_f;
            }
            if (B < setB_f)
            {
                B += (Mathf.Abs(setB_f - setB_t) / frame);
            }
            else
            {
                B = setB_f;
            }
        }
        else
        {
            if (R < setR_t)
            {
                R += (Mathf.Abs(setR_f - setR_t) / frame);
            }
            else
            {
                R = setR_t;
            }
            if (G > setG_t)
            {
                G -= (Mathf.Abs(setG_f - setG_t) / frame);
            }
            else
            {
                G = setG_t;
            }
            if (B > setB_t)
            {
                B -= (Mathf.Abs(setB_f - setB_t) / frame);
            }
            else
            {
                B = setB_t;
            }
        }
        lightColor.r = R;
        lightColor.g = G;
        lightColor.b = B;
        lightColor.a = A;
        lightColor /= 255f;//0~1の間に変換
        //Debug.Log("R:"+R+"G:"+G+"B:"+B);
        this.D_light.GetComponent<Light>().color = lightColor;
    }
    public void playEffect(Vector3 playPos)
    {
        this.effect.transform.position = playPos;
        this.effect.GetComponent<ParticleSystem>().Play();
    }
    public void attachGravity(Rigidbody rigid)//重力
    {
        if (rigid.useGravity == true)
        {
            rigid.useGravity = false;
        }
        rigid.AddForce(gravityPower, ForceMode.Acceleration);//質量に関わらず均一に力を加える
    }
}
