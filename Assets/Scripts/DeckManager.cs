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
			return nextCard;
		} else {
			return null;
		}
	}

	public void DiscardCard(GameObject card) {
		//Lorsque la carte est détruite, l'objet ajoutée à discardPile est "Missing Object".
		//Pour contourner ceci, je sauvegarde la position de chaque carte dans le deck par défaut pour retrouver son prefab.
		discardPile.Add(card);
	}

	private void Shuffle(List<GameObject> source) {
		drawPile = source.OrderBy(a => rng.Next()).ToList();
	}
}
