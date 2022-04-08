using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonManager : MonoBehaviour
{
    GameObject PCverButton;
    GameObject iPhoneverButton;

    public static int DeviceType = 0;
    void Start()
    {
        PCverButton = GameObject.Find("PCverButton");
        iPhoneverButton = GameObject.Find("iPhoneverButton");
        iPhoneverButton.SetActive(true);
        PCverButton.SetActive(false);
    }
    public void playButtonDown()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void changeDevice()
    {
        if(DeviceType == 0)
        {
            DeviceType = 1;
            PCverButton.SetActive(true);
            iPhoneverButton.SetActive(false);
        }
        else if (DeviceType == 1)
        {
            DeviceType = 0;
            iPhoneverButton.SetActive(true);
            PCverButton.SetActive(false);
        }
    }
}
