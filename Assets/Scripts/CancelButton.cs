using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelButton : MonoBehaviour {
	private DeckManager deckManager;
	private Action<bool> clickBehaviour;

	public void Start() {
		deckManager = FindObjectOfType<DeckManager>();
		deckManager.cancelButton = gameObject;
		clickBehaviour = deckManager.StopShowingDeck;
		gameObject.SetActive(false);
	}

	public void ClickBehaviour(bool cardChoice) {
		clickBehaviour.Invoke(cardChoice);
	}
}