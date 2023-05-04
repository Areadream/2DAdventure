using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatBar : MonoBehaviour
{
    public Image healthImage;
    public Image healthDelayImage;//��ɫѪ��
    public Image powerImage;

    /// <summary>
    /// ����Health�ı���ٷֱ�
    /// </summary>
    /// <param name="percentage">�ٷֱȣ�Current/Max</param>
    /// 
    private void Update()
    {
        if (healthDelayImage.fillAmount > healthImage.fillAmount)
        {
            healthDelayImage.fillAmount -= Time.deltaTime;
            //healthDelayImage.fillAmount -= Time.deltaTime * speed;
            //����ͨ��speed�����ٶ� 
        }
    }
    public void OnHealthChange(float percentage)
    {
        healthImage.fillAmount = percentage;
    }

}
