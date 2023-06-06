using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public int health = 100;
	public int defence = 0;

	public void GainDefence(int amount) {
		defence += amount;
	}

	public void Heal(int amount) {
		if (100 - health >= amount) {
			health += amount;
		} else {
			health = 100;
		}
	}

	public void TakeDamage(int amount) {
		if (defence >= amount) {
			defence -= amount;
		} else {
			amount -= defence;
			health -= amount;
			defence = 0;
		}
	}

	public void ResetDefence() {
		defence = 0;
	}
}
