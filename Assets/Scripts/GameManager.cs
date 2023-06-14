using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	public List<GameObject> drawnCards = new List<GameObject>();
	private List<Enemy> enemies;
	private Player player;

	public GameObject gameOverScreen;
	public GameObject endTurnButton;

	public TextMeshProUGUI energyText;

	private GameObject heldCard;

	public GameObject deckManagerPrefab;
	private DeckManager deckManager;

	public int energy = 3;
	private bool gameOver = false;

	private void Awake() {
		deckManager = FindObjectOfType<DeckManager>();
		if (deckManager == null) {
			deckManager = Instantiate(deckManagerPrefab).GetComponent<DeckManager>();
		}
	}

	private void Start() {
		deckManager.gameManager = this;
		StartCoroutine(Wait());

		player = FindObjectOfType<Player>();
		energyText.text = "3/3";
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			DrawCard(deckManager.NextCard());
		}

		if (Input.GetMouseButtonDown(1)) {
			heldCard = null;
		}
	}

	private void DrawCard(GameObject card) {
		if (card != null) {
			GameObject newCard = Instantiate(card, new Vector2(-10, -4), Quaternion.identity);

			//Lorsque la carte est d�truite, l'objet ajout�e � discardPile est "Missing Object".
			//Pour contourner ceci, je sauvegarde le prefab de la carte dans la carte elle-m�me pour la r�cup�rer plus tard.
			newCard.GetComponent<Card>().thisPrefab = card;

			drawnCards.Add(newCard);
			Recenter();
		}
	}
	
	public void Recenter() {
		for (int i = 0; i < drawnCards.Count; i++) {
			drawnCards[i].GetComponent<Card>().defaultPos = new Vector2(((drawnCards.Count - i) * 2.5f) - (1.25f * (drawnCards.Count + 1)), -4);
		}
	}

	public void SetHeldCard(GameObject card) {
		if (heldCard != null && heldCard != card) {
			heldCard.GetComponent<Card>().isHeld = false;
			heldCard.GetComponent<Card>().clickDone = false;
		}
		heldCard = card;
	}

	public void RemoveCard(GameObject card) {
		drawnCards.Remove(card);
		deckManager.DiscardCard(card.GetComponent<Card>().thisPrefab);
		Recenter();
	}

	//Dans Enemy.cs, j'aurais pu utiliser manager.enemies.Remove(this), mais il faut que je compte le nombre d'enemis restants.
	public void EnemyDeath(Enemy enemy) {
		enemies.Remove(enemy);

		if (enemies.Count == 0) {
			deckManager.CombatEnd();
			SceneManager.LoadScene(2);
		}
	}

	public void CallEndTurn() {
		StartCoroutine(EndTurn());
	}

	//Arr�te toutes les fonctions du jeu pour montrer le game over screen sans que le joueur peut interagir avec d'autres objets
	public void GameOver() {
		gameOver = true;
		StopAllCoroutines();
		gameOverScreen.SetActive(true);
		energyText.gameObject.SetActive(false);
		player.gameObject.SetActive(false);
		endTurnButton.SetActive(false);

		foreach (Enemy enemy in enemies) {
			enemy.gameObject.SetActive(false);
		}
		
		foreach (GameObject card in drawnCards) {
			Destroy(card);
		}

		Destroy(deckManager);
		
		gameObject.SetActive(false);
	}

	private IEnumerator EndTurn() {
		energy = 3;
		energyText.text = energy.ToString() + "/3";

		yield return StartCoroutine(Discard());

		foreach (Enemy enemy in enemies) {
			enemy.ResetDefence();

			if (enemy.nextMove != 0) {
				enemy.Attack();
				if (gameOver) {
					yield break;
				}
			} else {
				enemy.GainDefence();
			}

			enemy.ChooseMove();
		}

		player.ResetDefence();

		StartCoroutine(Draw(4));
	}

	public IEnumerator Draw(int numOfCards, Card card = null) {
		DrawCard(deckManager.NextCard());
		for (int i = 1; i < numOfCards; i++) {
			yield return new WaitForSeconds(0.1f);
			DrawCard(deckManager.NextCard());
		}

		//StartCoroutine() est reli� au GameObject qui l'a commenc�. Puisque la carte se d�truisait avant de piger toutes les cartes,
		//il faut ce code pour attendre que toutes les cartes sont pig�es avant de la d�truire.
		if (card != null) {
			card.finishedDrawing = true;
		}
	}

	private IEnumerator Discard() {
		if (drawnCards.Count != 0) {
			for (int i = 1; i != drawnCards.Count;) {
				drawnCards[0].GetComponent<Card>().Discard();
				yield return new WaitForSeconds(0.1f);
			}
			drawnCards[0].GetComponent<Card>().Discard();
		}
	}

	private IEnumerator Wait() {
		yield return new WaitForEndOfFrame();

		//Cette fonction serait normalement invoqu�e dans Start(), mais dans PileButtons.cs, drawPileText et discardPileText sont aussi assign�s dans Start().
		//Puisque les cartes ne peuvent pas �tre pig�s sans ce texte (les fonctions sont int�rrompues par le manque du texte),
		//il faut attendre au prochain frame pour commencer � piger les cartes.
		deckManager.CombatStart();

		//Similairement, dans EnemySpawner.cs, il faut attendre pour Start() pour s'assurer que deckManager existe.
		//Ceci signifie qu'il faut attendre pour apr�s Start() pour s'assurer que les ennemis existent.
		enemies = FindObjectsOfType<Enemy>().ToList();

		foreach (Enemy enemy in enemies) {
			enemy.ChooseMove();
		}
	}
}