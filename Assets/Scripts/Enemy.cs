using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	private Player player;

	public int health = 50;
	public int defence = 0;
	public int damage = 5;

	public int nextMove;

	private void Start() {
		player = FindObjectOfType<Player>();
		ChooseMove();
	}

	private void Update() {
		if (health <= 0) {
			Destroy(gameObject);
		}
	}

	public void Attack() {
		player.TakeDamage(damage);
	}

	public void GainDefence(int amount) {
		defence += amount;
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

	public void ChooseMove() {
		nextMove = Random.Range(0, 4);
	}
}