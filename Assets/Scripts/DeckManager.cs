using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckManager : MonoBehaviour {
	public List<GameObject> deck;
	public List<GameObject> drawPile;
	public List<GameObject> discardPile;

	private GameManager gameManager;

	private System.Random rng = new System.Random();

	private void Start() {
		DontDestroyOnLoad(gameObject);

		gameManager = FindObjectOfType<GameManager>();
		Shuffle(deck);
		StartCoroutine(gameManager.Draw(4));
	}

	public GameObject NextCard() {
		if (drawPile.Count == 0 && discardPile.Count == 0) {
			return null;
		} else if (gameManager.drawnCards.Count < 7) {
			if (drawPile.Count == 0 && discardPile.Count > 0) {
				Shuffle(discardPile);
				discardPile.Clear();
			}
			GameObject nextCard = drawPile[0];
			drawPile.Remove(nextCard);
			nextCard.GetComponent<CardChoice>().enabled = false;
			nextCard.GetComponent<Card>().enabled = true;
			return nextCard;
		} else {
			return null;
		}
	}

	public void DiscardCard(GameObject card) {
		discardPile.Add(card);
	}

	public void CombatEnd() {
		drawPile.Clear();
		discardPile.Clear();
	}

	private void Shuffle(List<GameObject> source) {
		drawPile = source.OrderBy(a => rng.Next()).ToList();
	}
}