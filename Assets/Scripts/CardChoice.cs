using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardChoice : MonoBehaviour {
	public DeckManager deckManager;
	private CardChoiceManager cardChoiceManager;

	public GameObject thisPrefab;

	public bool add = true;

	private void Start() {
		deckManager = FindObjectOfType<DeckManager>();
		cardChoiceManager = FindObjectOfType<CardChoiceManager>();
	}

	private void OnMouseDown() {
		if (enabled) {
			if (add) {
				deckManager.deck.Add(thisPrefab);
			} else {
				deckManager.deck.Remove(thisPrefab);
				deckManager.StopShowingDeck(false);
			}
			cardChoiceManager.Continue();
		}
	}
}