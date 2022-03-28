using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class GameDirector : MonoBehaviour
{
    ObjNameList nameList;
    [Tooltip("0はパソコン, 1はスマホで遊ぶ")]
    public int DeviceType = 0;

    [SerializeField]
    AudioClip[] clips = new AudioClip[5];//[0]=キャラ消滅, [1]=アイテム回収, [2]=静かな曲, [3]=うるさい曲, [4]=キャラボイス
    AudioSource[] sources = new AudioSource[3];//[1]=SE,  [2],[3]=BGM
    //protected AudioSource source2;
    //protected AudioSource source3;

    //-------------------------

    [Tooltip("重力の強さ")]
    [SerializeField]
    internal Vector3 gravityPower;

    //-------------------------

    [System.NonSerialized]
    internal float wasCollected = 0f;
    [System.NonSerialized]
    internal int itemAmount;

    //-------------------------
    GameObject canvas;
    //-------------------------

    [Tooltip("スタミナのテキストオブジェクトを入れるとこ")]
    [SerializeField]
    internal TextMeshProUGUI staminaText;
    string staminaValue;
    float maxTime = 0;
    internal bool hideSwitch = false;

    //-------------------------

    int frameCount = 0;
    float lapTime = 0;
    [SerializeField,Tooltip("fpsのテキストオブジェクトを入れるとこ")]
    TextMeshProUGUI fpsText;
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

    int BGM_count = 1;
    const float BGM_Volume = 0.15f;
    float countTime = 0;

    //-------------------------
    //Image collectGauge_back;
    Image collectGauge;
    Image staminaGauge;
    Image staminaGaugeBack;
    GameObject player;
    StampController stampScript;
    EnemyController enemyScript;
    Light directionalLight;
    ParticleSystem effectScript;
    CharaController playerScript;
    void Awake()
    {
        nameList = GetComponent<ObjNameList>();
    }
    void Start()
    {
        canvas = nameList.canvas;
        player = nameList.player;
        enemyScript = nameList.enemyScript;
        directionalLight = nameList.lightScript;
        
        staminaValue = null;

        effectScript = nameList.effectScript;
        playerScript = nameList.playerScript;
        stampScript = nameList.stampScript;
        lightColor = nameList.lightScript.color;
        collectGauge = nameList.collectGauge.GetComponent<Image>();
        staminaGauge = nameList.staminaGauge.GetComponent<Image>();
        staminaGaugeBack = nameList.staminaGaugeBack.GetComponent<Image>();
        sources = GetComponents<AudioSource>();
        AudioSource[] audio = GetComponents<AudioSource>();
        sources[0].volume = 0.4f;
        sources[1].volume = 0.1f;
        sources[2].volume = 0.1f;
        //collectGauge_back = GameObject.Find("collectGauge_back").GetComponent<Image>();

        if(gravityPower == null || gravityPower.y == 0)
        {
            gravityPower = new Vector3(0, -10, 0);
        }

        GameObject[] itemList = GameObject.FindGameObjectsWithTag("item");
        itemAmount = itemList.Length;
        if (DeviceType == 0 || DeviceType == 1)
        {
            if(DeviceType == 0)
            {
                GameObject[] buttonList = GameObject.FindGameObjectsWithTag("TapButton");
                for(int i = 0; i < buttonList.Length; ++i)
                {
                    buttonList[i].SetActive(false);
                }
            }
        }
        else
        {
            DeviceType = 0;
        }
        R = setR_f;
        G = setG_f;
        B = setB_f;
        A = 255f;
    }

    // Update is called once per frame
    void Update()
    {
        fpsDisplay();
        itemGaugeDisplay();
        staminaDisplay();
        staminaGaugeDisplay();
        colorChanger();
        BGM_changer();
        if(hideSwitch == true)
        {
            hideGauge();
        }
    }
    void fpsDisplay()
    {
        frameCount++;
        lapTime += Time.deltaTime;
        if (lapTime >= 0.5f) //0.5秒毎にfps計算
        {
            float fps = 1.0f * frameCount / lapTime;
            fpsRate = $"FPS:{fps.ToString("F2")}";
            fpsText.text = fpsRate;
            frameCount = 0;
            lapTime = 0f;
        }
    }
    void itemGaugeDisplay()
    {
        collectGauge.fillAmount = (wasCollected / itemAmount);
    }
    void staminaGaugeDisplay()
    {
        staminaGauge.fillAmount = playerScript.stamina / 100.0f;//スタミナゲージの更新
        if(playerScript.stamina >= 100)
        {
            maxTime += Time.deltaTime;
        }
        else
        {
            maxTime = 0f;
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
    internal void hideGauge()
    {
        staminaGauge.enabled = false;
        staminaGaugeBack.enabled = false;
        hideSwitch = false;
    }
    void showGauge()
    {
        staminaGauge.enabled = true;
        staminaGaugeBack.enabled = true;
    }
    void staminaDisplay()
    {
        staminaValue = $"energy:{playerScript.stamina.ToString("F0")}";
        staminaText.text = staminaValue;
    }
    void colorChanger()
    {
        if (enemyScript.chase == false)
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
        directionalLight.color = lightColor;
    }
    void BGM_changer()
    {
        if(enemyScript.chase == true)
        {
            if (BGM_count == 0)//追いかけ始めた時に音楽変える
            {
                sources[2].clip = clips[3];
                sources[2].Play();
                BGM_count = 1;
                countTime = 0;
            }
            countTime += 0.1f * Time.deltaTime;
            if (sources[2].volume < BGM_Volume)
            {
                sources[2].volume = countTime / BGM_Volume;
            }
            if (sources[1].volume > 0)
            {
                sources[1].volume = BGM_Volume - countTime;
            }
        }
        else
        {
            if (BGM_count == 1)
            {
                sources[1].clip = clips[2];
                sources[1].Play();
                BGM_count = 0;
                countTime = 0;
            }
            countTime += 0.1f * Time.deltaTime;
            if (sources[1].volume < BGM_Volume)
            {
                sources[1].volume = countTime / BGM_Volume;
            }
            if (sources[2].volume > 0)
            {
                sources[2].volume = BGM_Volume - countTime;
            }
        }
    }
    internal void playEffect(Vector3 playPos)
    {
        SE_delete();
        this.effectScript.transform.position = playPos;
        this.effectScript.Play();
    }
    internal void attachGravity(Rigidbody rigid)//重力
    {
        if (rigid.useGravity == true)
        {
            rigid.useGravity = false;
        }
        rigid.AddForce(gravityPower, ForceMode.Acceleration);//質量に関わらず均一に力を加える
    }
    IEnumerator volumeChanger()
    {
        while(sources[1].volume > 0)
        {
            sources[1].volume = (BGM_Volume - Time.deltaTime) /BGM_Volume;
        }
        while(sources[1].volume < BGM_Volume)
        {
            sources[1].volume = Time.deltaTime / BGM_Volume;
        }
        yield return null;
    }
    IEnumerator moveScene()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("TitleScene");
    }
    IEnumerator clearAndMove()
    {
        
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("TitleScene");
    }
    IEnumerator SE_charaVoice()
    {
        yield return new WaitForSeconds(0.5f);
        sources[0].volume = 1f;
        sources[0].pitch = 1f;
        sources[0].PlayOneShot(clips[4], 1f);
    }
    void SE_delete()
    {
        this.sources[0].pitch = 1f;
        this.sources[0].PlayOneShot(clips[0], 0.8f);
    }
    internal void SE_collect()
    {
        sources[0].pitch = 1f;
        sources[0].PlayOneShot(clips[1], 1f);
    }
    internal void playVoice()
    {
        StartCoroutine(SE_charaVoice());
    }
    internal void moveTitle()
    {
        StartCoroutine(moveScene());
    }
}
