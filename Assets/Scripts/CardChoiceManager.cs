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
		int iter = 0;
		int initCount = cards.Count;

		for (int i = initCount; i > initCount - 3; i--) {
			int rand = Random.Range(0, i);
			cardChoice.Add(cards[rand]);
			cards.RemoveAt(rand);
			cardChoice[iter].GetComponent<Card>().enabled = false;
			cardChoice[iter].GetComponent<CardChoice>().enabled = true;
			GameObject card = Instantiate(cardChoice[iter], new Vector3((iter - 1) * 3.5f, 0, 0), Quaternion.identity);
			card.GetComponent<CardChoice>().thisPrefab = cardChoice[iter];
			iter++;
		}
	}

	public void Continue() {
		SceneManager.LoadScene(0);
	}
}