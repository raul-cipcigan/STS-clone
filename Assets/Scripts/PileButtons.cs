using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PileButtons : MonoBehaviour {
	private Action<List<GameObject>> clickBehaviour;
	public DeckManager deckManager;

	public bool discard;

	private void Start() {
		deckManager = FindObjectOfType<DeckManager>();
		if (discard) {
			deckManager.discardPileText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
		} else {
			deckManager.drawPileText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
		}
		clickBehaviour = deckManager.ShowDeck;
	}

	public void ClickBehaviour() {
		if (discard) {
			clickBehaviour.Invoke(deckManager.discardPile);
		} else {
			clickBehaviour.Invoke(deckManager.drawPile);
		}
	}
}
