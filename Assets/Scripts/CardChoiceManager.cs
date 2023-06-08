using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardChoiceManager : MonoBehaviour {
	public List<GameObject> cards = new List<GameObject>();

	public List<GameObject> cardChoice = new List<GameObject>();

	private void Start() {
		cards = Resources.LoadAll<GameObject>("Cards").ToList();
		cardChoice.Add(cards[0]);
		cardChoice[0].GetComponent<Card>().enabled = false;
		cardChoice[0].GetComponent<CardChoice>().enabled = true;
		cardChoice[0].GetComponent<CardChoice>().cardChoiceManager = this;
		GameObject card = Instantiate(cardChoice[0], new Vector3(0, 0, 0), Quaternion.identity);
		card.GetComponent<CardChoice>().thisPrefab = cardChoice[0];
	}

	public void Continue() {
		SceneManager.LoadScene(0);
	}
}