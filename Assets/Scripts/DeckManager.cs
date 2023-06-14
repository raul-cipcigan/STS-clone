using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DeckManager : MonoBehaviour {
	public List<GameObject> deck;
	public List<GameObject> drawPile;
	public List<GameObject> discardPile;

	private List<GameObject> shownPile = null;

	private GameObject shownCardsHolder;

	public GameObject screenCoverPrefab;
	private GameObject screenCover;
	public GameObject cancelButton;

	public GameManager gameManager;
	public Player player;

	public TextMeshProUGUI drawPileText;
	public TextMeshProUGUI discardPileText;

	private System.Random rng = new System.Random();

	public int playerHealth = 80;
	public int wave = 0;

	private void Awake() {
		DontDestroyOnLoad(gameObject);
	}

	private void Update() {
		if (shownPile != null) {
			shownCardsHolder.transform.Translate(0, -Input.mouseScrollDelta.y, 0);
		}
	}

	public GameObject NextCard() {
		if (drawPile.Count == 0 && discardPile.Count == 0) {
			return null;
		} else if (gameManager.drawnCards.Count < 7) {

			if (drawPile.Count == 0 && discardPile.Count > 0) {
				Shuffle(discardPile);
				discardPile.Clear();
				discardPileText.text = "0";
			}

			GameObject nextCard = drawPile[0];
			drawPile.Remove(nextCard);
			nextCard.GetComponent<CardChoice>().enabled = false;
			nextCard.GetComponent<Card>().enabled = true;
			
			drawPileText.text = drawPile.Count.ToString();

			return nextCard;
		} else {
			return null;
		}
	}

	public void DiscardCard(GameObject card) {
		discardPile.Add(card);
		discardPileText.text = discardPile.Count.ToString();
	}

	public void CombatStart() {
		Shuffle(deck);
		discardPileText.text = "0";
		StartCoroutine(gameManager.Draw(4));
	}

	public void CombatEnd() {
		drawPile.Clear();
		discardPile.Clear();
		playerHealth = player.health;
		gameManager = null;
		player = null;
		drawPileText = null;
		discardPileText = null;
		wave++;
	}

	public void ShowDeck(List<GameObject> pile, bool removal) {
		shownPile = pile;
		screenCover = Instantiate(screenCoverPrefab, new Vector3(0, 0, -0.02f), Quaternion.identity);
		cancelButton.SetActive(true);
		int x = -6;
		float y = 2.5f;
		shownCardsHolder = new GameObject("Shown Pile");
		foreach (GameObject card in pile) {
			card.GetComponent<Card>().enabled = false;
			if (!removal) {
				card.GetComponent<CardChoice>().enabled = false;
			} else {
				card.GetComponent<CardChoice>().enabled = true;
				card.GetComponent<CardChoice>().add = false;
			}

			GameObject shownCard = Instantiate(card, new Vector3(x, y, -0.03f), Quaternion.identity);
			shownCard.transform.parent = shownCardsHolder.transform;

			if (removal) {
				//Ceci doit être fait après Instantiate(), puisque autrement, thisPrefab devient le clone GameObject et non le prefab.
				shownCard.GetComponent<CardChoice>().thisPrefab = card;
			}

			if (x == 6) {
				x = -6;
				y -= 3.5f;
			} else {
				x += 3;
			}
		}
	}

	public void StopShowingDeck(bool cardChoice) {
		Destroy(shownCardsHolder);
		foreach (GameObject card in shownPile) {
			card.GetComponent<Card>().enabled = !cardChoice;
			card.GetComponent<CardChoice>().enabled = cardChoice;
		}
		shownPile = null;
		Destroy(screenCover);
		cancelButton.SetActive(false);
	}

	private void Shuffle(List<GameObject> source) {
		drawPile = source.OrderBy(a => rng.Next()).ToList();
	}
}