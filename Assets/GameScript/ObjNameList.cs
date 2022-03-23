using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjNameList : MonoBehaviour
{
    internal string playerName;
    internal string enemyName;
    internal string lightName;
    internal string cameraName;
    internal string areaName;
    internal string canvasName;
    internal string gameDirectorName;
    internal string collectItemName;
    internal string staminaGaugeName;
    internal string collectGaugeName;
    internal string areaGeneratorName;
    internal string staminaGaugeBackName;
    internal string collectGaugeBackName;
    internal string particleEffectName;

    internal GameObject player;
    internal GameObject enemy;
    internal GameObject canvas;
    internal GameObject directionalLight;
    internal GameObject mainCamera;
    internal GameObject areaGenerator;
    internal GameObject gameDirector;
    internal GameObject collectItem;
    internal GameObject staminaGauge;
    internal GameObject collectGauge;
    internal GameObject staminaGaugeBack;
    internal GameObject collectGaugeBack;
    internal GameObject particleEffect;

    internal CharaController playerScript;
    internal EnemyController enemyScript;
    internal CameraController cameraScript;
    internal AttackController attackScript;
    internal StampController stampScript;
    internal GameDirector UImanager;
    internal ParticleSystem effectScript;
    internal Light lightScript;

    void Awake()
    {
        particleEffectName   = "Effect";
        canvasName           = "Canvas";
        cameraName           = "Main Camera";
        collectItemName      = "CollectItem";
        staminaGaugeName     = "StaminaGauge";
        collectGaugeName     = "CollectGauge";
        gameDirectorName     = "GameDirector";
        areaGeneratorName    = "AreaGenerator";
        staminaGaugeBackName = "StaminaGauge_back";
        collectGaugeBackName = "CollectGauge_back";
        enemyName            = "Misaki_win_humanoid";
        playerName           = "SD_unitychan_humanoid";
        areaName             = "AttackAreaPrefab(Clone)";
        lightName            = "Directional Light";

        player           = GameObject.Find(playerName);
        enemy            = GameObject.Find(enemyName);
        directionalLight = GameObject.Find(lightName);
        canvas           = GameObject.Find(canvasName);
        mainCamera       = GameObject.Find(cameraName);
        gameDirector     = GameObject.Find(gameDirectorName);
        collectItem      = GameObject.Find(collectItemName);
        staminaGauge     = GameObject.Find(staminaGaugeName);
        collectGauge     = GameObject.Find(collectGaugeName);
        areaGenerator    = GameObject.Find(areaGeneratorName);
        particleEffect   = GameObject.Find(particleEffectName);
        staminaGaugeBack = GameObject.Find(staminaGaugeBackName);
        collectGaugeBack = GameObject.Find(collectGaugeBackName);

        UImanager    = GetComponent<GameDirector>();
        stampScript  = GetComponent<StampController>();
        enemyScript  = enemy.GetComponent<EnemyController>();
        playerScript = player.GetComponent<CharaController>();
        cameraScript = mainCamera.GetComponent<CameraController>();
        attackScript = areaGenerator.GetComponent<AttackController>();
        lightScript  = directionalLight.GetComponent<Light>();
        effectScript = particleEffect.GetComponent<ParticleSystem>();
    }
}
