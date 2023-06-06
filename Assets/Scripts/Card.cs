using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Card : MonoBehaviour {

	private GameManager manager;
	private Player player;

	public Vector2 defaultPos = new Vector2(0, -4);
	public GameObject thisPrefab;

	public int cost = 1;
	public int damage;
	public int defence;
	public int heal;
	public int draw;

	public bool isHeld = false;
	public bool discard = false;
	public bool finishedDrawing = true;
	public bool clickDone = false;
	public bool canChooseTarget = false;

	private void Start() {
		manager = FindObjectOfType<GameManager>();
		player = FindObjectOfType<Player>();
	}

	void Update() {
		if (Input.GetMouseButtonDown(1)) {
			isHeld = false;
			clickDone = false;
		}

		if (discard) {
			transform.position = Vector2.Lerp(transform.position, new Vector2(12, -4), Time.deltaTime * 7.5f);
			if (transform.position.x > 9.888f && finishedDrawing) {
				Destroy(gameObject);
			}
		} else if (!isHeld) {
			transform.position = Vector2.Lerp(transform.position, defaultPos, Time.deltaTime * 5);
		} else {
			//Sans la variable clickDone, la carte est jouée le frame après que la carte soit choisie.
			if (!clickDone && Input.GetMouseButtonUp(0)) {
				clickDone = true;
			}
			if (clickDone && Input.GetMouseButtonDown(0) && (manager.energy - cost) >= 0) {
				if (!canChooseTarget) {
					Play();
				} else {
					DetectEnemy();
				}
			}
			transform.position = Vector2.Lerp(transform.position, new Vector2(0, -0.5f), Time.deltaTime * 5);
		}
	}

	private void OnMouseDown() {
		if (!isHeld) {
			isHeld = true;
			manager.SetHeldCard(gameObject);
		} else {
			isHeld = false;
			clickDone = false;
			manager.SetHeldCard(null);
		}
	}

	public void Play(GameObject target = null) {
		Discard();
		manager.energy -= cost;
		if (target != null) {
			target.GetComponent<Enemy>().TakeDamage(damage);
		} else {
			Enemy[] enemies = FindObjectsOfType<Enemy>();
			for (int i = 0; i < enemies.Length; i++) {
				enemies[i].TakeDamage(damage);
			}
		}
		player.GainDefence(defence);
		player.Heal(heal);
		if (draw != 0) {
			finishedDrawing = false;
			StartCoroutine(manager.Draw(draw, this));
			Debug.Log("Drawing" + draw.ToString() + "cards");
		}
	}

	public void Discard() {
		discard = true;
		manager.RemoveCard(gameObject);
	}

	private void DetectEnemy() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit)) {
			if (hit.transform.CompareTag("Enemy")) {
				Play(hit.transform.gameObject);
			}
		}
	}
}