using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PileButtons : MonoBehaviour {
	private Action<List<GameObject>, bool> clickBehaviour;
	public DeckManager deckManager;

	public int deck;

	private void Start() {
		deckManager = FindObjectOfType<DeckManager>();
		if (deck == 0) {
			deckManager.discardPileText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
		} else if (deck == 1) {
			deckManager.drawPileText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
		} 
		clickBehaviour = deckManager.ShowDeck;
	}

	public void ClickBehaviour() {
		if (deck == 0) {
			clickBehaviour.Invoke(deckManager.discardPile, false);
		} else if (deck == 1) {
			clickBehaviour.Invoke(deckManager.drawPile, false);
		} else {
			clickBehaviour.Invoke(deckManager.deck, true);
		}
	}
}