using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public List<GameObject> drawnCards = new List<GameObject>();
	public Enemy[] enemies;
	public Player player;

	private GameObject heldCard;

	public DeckManager deckManager;

	public int energy = 3;

	private void Start() {
		enemies = FindObjectsOfType<Enemy>();
		foreach (Enemy enemy in enemies) {
			enemy.ChooseMove();
		}
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			DrawCard(deckManager.NextCard());
		}

		if (Input.GetKeyDown(KeyCode.M)) {
			StartCoroutine(Draw(4));
		}

		if (Input.GetMouseButtonDown(1)) {
			heldCard = null;
		}
	}

	private void DrawCard(GameObject card) {
		if (card != null) {
			GameObject newCard = Instantiate(card, new Vector2(-10, -4), Quaternion.identity);
			newCard.GetComponent<Card>().thisPrefab = card;
			drawnCards.Add(newCard);
			Recenter();
		}
	}
	
	public void Recenter() {
		for (int i = 0; i < drawnCards.Count; i++) {
			drawnCards[i].GetComponent<Card>().defaultPos = new Vector2(((drawnCards.Count - i) * 2.5f) - (1.25f * (drawnCards.Count + 1)), -4);
		}
	}

	public void SetHeldCard(GameObject card) {
		if (heldCard != null && heldCard != card) {
			heldCard.GetComponent<Card>().isHeld = false;
			heldCard.GetComponent<Card>().clickDone = false;
		}
		heldCard = card;
	}

	public void RemoveCard(GameObject card) {
		drawnCards.Remove(card);
		deckManager.DiscardCard(card.GetComponent<Card>().thisPrefab);
		Recenter();
	}

	public void CallEndTurn() {
		StartCoroutine(EndTurn());
	}

	private IEnumerator EndTurn() {
		energy = 3;
		yield return StartCoroutine(Discard());

		foreach (Enemy enemy in enemies) {
			enemy.ResetDefence();
			if (enemy.nextMove != 0) {
				enemy.Attack();
			} else {
				enemy.GainDefence(5);
			}
			enemy.ChooseMove();
		}

		player.ResetDefence();

		StartCoroutine(Draw(4));
	}

	public IEnumerator Draw(int numOfCards, Card card = null) {
		DrawCard(deckManager.NextCard());
		for (int i = 1; i < numOfCards; i++) {
			yield return new WaitForSeconds(0.1f);
			DrawCard(deckManager.NextCard());
		}

		//StartCoroutine() est relié au GameObject qui l'a commencé. Puisque la carte se détruisait avant de piger toutes les cartes,
		//il faut ce code pour attendre que toutes les cartes sont pigées avant de la détruire.
		if (card != null) {
			card.finishedDrawing = true;
		}
	}

	private IEnumerator Discard() {
		if (drawnCards.Count != 0) {
			for (int i = 1; i != drawnCards.Count;) {
				drawnCards[0].GetComponent<Card>().Discard();
				yield return new WaitForSeconds(0.1f);
			}
			drawnCards[0].GetComponent<Card>().Discard();
		}
	}
}