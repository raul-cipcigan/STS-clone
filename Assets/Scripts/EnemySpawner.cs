using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
	private DeckManager deckManager;

	public GameObject enemy;

	public List<Vector3> twoEnemiesSpawnPos;
	public List<Vector3> fourEnemiesSpawnPos;

	public List<GameObject> newEnemies = new List<GameObject>();

	private List<Sprite> enemySprites;

	private int upgradeTokens;
	private int spawnTokens;

	private void Start() {
		deckManager = FindObjectOfType<DeckManager>();

		enemySprites = Resources.LoadAll<Sprite>("Sprites").ToList<Sprite>();

		//Chaque nouveau ennemi coûte 4 upgrade tokens pour balancer le jeu, et il y a une limite de 4 ennemis.
		//Ce ternary operation détermine le nombre maximum d'ennemis qui peut être créé par le nombre de upgrade tokens qui existent.
		//Chaque wave correspont à un upgrade token de plus.
		int enemyLimit = (Mathf.FloorToInt(deckManager.wave / 4f) <= 3) ? Mathf.FloorToInt(deckManager.wave / 4f) : 3;

		spawnTokens = Random.Range(0, (enemyLimit + 1));
		upgradeTokens = deckManager.wave - (4 * spawnTokens);
		spawnTokens++;

		if (spawnTokens == 1) {
			
			newEnemies.Add(Instantiate(enemy, new Vector3(5, 1), Quaternion.identity));
			newEnemies[0].transform.localScale = new Vector3(3, 3, 1);

		} else if (spawnTokens == 2) {
			for (int i = 0; i < spawnTokens; i++) {
				newEnemies.Add(Instantiate(enemy, twoEnemiesSpawnPos[i], Quaternion.identity));
			}
		} else {
			for (int i = 0; i < spawnTokens; i++) {
				newEnemies.Add(Instantiate(enemy, fourEnemiesSpawnPos[i], Quaternion.identity));
			}
		}

		foreach (GameObject newEnemy in newEnemies) {
			newEnemy.GetComponent<Enemy>().maxHealth = 10 + Mathf.FloorToInt(40 / spawnTokens);

			newEnemy.GetComponent<Enemy>().damage += Mathf.CeilToInt(Mathf.FloorToInt(upgradeTokens / 2) / spawnTokens);
			newEnemy.GetComponent<Enemy>().block += Mathf.CeilToInt(Mathf.FloorToInt(upgradeTokens / 3) / spawnTokens);
			newEnemy.GetComponent<Enemy>().maxHealth += Mathf.CeilToInt(Mathf.FloorToInt(upgradeTokens * 2) / spawnTokens);
			newEnemy.GetComponent<SpriteRenderer>().sprite = enemySprites[Random.Range(0, 2)];
		}
	}
}