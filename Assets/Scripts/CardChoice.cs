using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardChoice : MonoBehaviour {
	public DeckManager deckManager;
	public CardChoiceManager cardChoiceManager;

	public GameObject thisPrefab;

	private void Start() {
		deckManager = FindObjectOfType<DeckManager>();
	}

	private void OnMouseDown() {
		if (enabled) {
			deckManager.deck.Add(thisPrefab);
			cardChoiceManager.Continue();
		}
	}
}