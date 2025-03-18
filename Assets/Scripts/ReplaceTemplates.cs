using UnityEngine;

public class ReplaceTemplates : MonoBehaviour
{   
    public GameObject[] enemyTemplates;
    public GameObject enemyPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyTemplates = GameObject.FindGameObjectsWithTag("Templates");
        ReplaceEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ReplaceEnemies() {
        foreach (var i in enemyTemplates) {
            Vector2 position = i.transform.position;
            Quaternion rotation = i.transform.rotation;
            GameObject newEnemy = Instantiate(enemyPrefab, position, rotation);
            Destroy(i);
        }
    }
}
