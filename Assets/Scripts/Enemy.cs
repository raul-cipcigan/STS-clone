using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour {

	private Player player;

	private GameManager manager;

	public GameObject attackIntent;
	public GameObject defendIntent;
	public GameObject defenceIcon;

	public TextMeshPro intent;
	public TextMeshPro healthText;
	public TextMeshPro defenceText;

	public int health = 50;
	public int maxHealth = 50;
	public int defence = 0;
	public int damage = 5;
	public int block = 5;

	public int nextMove;

	public List<int> pastMoves = new List<int>();

	private void Start() {
		manager = FindObjectOfType<GameManager>();
		player = FindObjectOfType<Player>();
		healthText.text = health.ToString() + "/" + maxHealth.ToString();
		defenceText.text = "";
	}

	public void Attack() {
		player.TakeDamage(damage);
	}

	public void GainDefence() {
		defence += block;
		defenceIcon.SetActive(true);
		defenceText.text = defence.ToString();
	}

	public void TakeDamage(int amount) {
		if (defence >= amount) {
			defence -= amount;
			defenceText.text = defence.ToString();
		} else {
			amount -= defence;
			defence = 0;
			health -= amount;

			healthText.text = health.ToString() + "/" + maxHealth.ToString();
			defenceText.text = "";
			defenceIcon.SetActive(false);

			if (health <= 0) {
				manager.EnemyDeath(this);
				Destroy(gameObject);
			}
		}
	}

	public void ResetDefence() {
		defence = 0;
		defenceText.text = "";
		defenceIcon.SetActive(false);
	}
	
	public void ChooseMove() {
		int countAttacks = 0;
		
		foreach (int move in pastMoves) {
			if (move != 0) {
				countAttacks++;
			}
		}

		//Ceci fait en sorte que chaque attaque peut être répété un maximum de 3 fois, pour garder la variété
		if (countAttacks != 0 && countAttacks < 3) {
			nextMove = Random.Range(0, 3);
		} else if (countAttacks == 0) {
			nextMove = 1;
		} else {
			nextMove = 0;
		}
		
		if (pastMoves.Count == 3) {
			pastMoves.RemoveAt(0);
		}

		pastMoves.Add(nextMove);

		if (nextMove != 0) {
			intent.text = damage.ToString();
			defendIntent.SetActive(false);
			attackIntent.SetActive(true);
		} else {
			intent.text = block.ToString();
			attackIntent.SetActive(false);
			defendIntent.SetActive(true);
		}
	}
}