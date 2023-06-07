using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour {

	public TextMeshPro healthText;
	public TextMeshPro defenceText;

	public GameObject defenceIcon;

	public int health = 80;
	public int defence = 0;

	private void Start() {
		healthText.text = health.ToString() + "/80";
		defenceText.text = "";
	}

	public void GainDefence(int amount) {
		defence += amount;
		if (defence != 0) {
			defenceIcon.SetActive(true);
			defenceText.text = defence.ToString();
		}
	}

	public void Heal(int amount) {
		if (80 - health >= amount) {
			health += amount;
		} else {
			health = 80;
		}
	}

	public void TakeDamage(int amount) {
		if (defence >= amount) {
			defence -= amount;
			defenceText.text = defence.ToString();
		} else {
			amount -= defence;
			defence = 0;
			health -= amount;
			healthText.text = health.ToString() + "/80";
			defenceText.text = "";
			defenceIcon.SetActive(false);
		}
	}

	public void ResetDefence() {
		defence = 0;
		defenceText.text = "";
		defenceIcon.SetActive(false);
	}
}
