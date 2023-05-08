using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatBar : MonoBehaviour
{
    public Image healthImage;
    public Image healthDelayImage;//红色血条
    public Image powerImage;
    private Character currentCharacter;
    private bool isRecovering;


    /// <summary>
    /// 接收Health的变更百分比
    /// </summary>
    /// <param name="percentage">百分比：Current/Max</param>
    /// 
    private void Update()
    {
        if(isRecovering)
        {
            float persentage = currentCharacter.currentPower / currentCharacter.maxPower;
            powerImage.fillAmount = persentage;
            if(persentage >= 1)
            {
                isRecovering = false;
                return;
            }
        }


        if (healthDelayImage.fillAmount > healthImage.fillAmount)
        {
            healthDelayImage.fillAmount -= Time.deltaTime;
            //healthDelayImage.fillAmount -= Time.deltaTime * speed;
            //可以通过speed调整速度 
        }
    }
    public void OnHealthChange(float percentage)
    {
        healthImage.fillAmount = percentage;
    }

    public void OnPowerChange(Character character)
    {
        isRecovering = true;
        currentCharacter = character;
    }


}
