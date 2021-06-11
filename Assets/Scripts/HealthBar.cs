using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	public Slider slider;
	public Gradient gradient;
	public Image fill;
	public TextMeshProUGUI currentHealth;
	public TextMeshProUGUI changeAmount;

	public void SetMaxHealth(int health)
	{
		slider.maxValue = health;
		slider.value = health;

		fill.color = gradient.Evaluate(1f);

		currentHealth.text = $"{slider.value}/{slider.maxValue}";
	}

    public void SetHealth(int health)
	{
		slider.value = health;

		fill.color = gradient.Evaluate(slider.normalizedValue);
		
		currentHealth.text = $"{slider.value}/{slider.maxValue}";
	}

    /*changeAmount.DOKill();
    changeAmount.DOFade(0, 0);
		
    changeAmount.DOFade(0, 0f);
    changeAmount.DOFade(1, .5f);*/
    
}
