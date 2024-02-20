using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    private List<SpawnPoint> spawnPointList;
    private List<Character> characters;
    private bool hasSpawned;
    public Collider _collider;
    public UnityEvent onAllSpawnedCharacterEliminated;
    private void Awake()
    {
        var spawnPointArray = transform.parent.GetComponentsInChildren<SpawnPoint>();
        spawnPointList = new List<SpawnPoint>(spawnPointArray);
        characters = new List<Character>();
    }
    private void Update()
    {
        if (!hasSpawned || characters.Count == 0)
        {
            return;
        }
        bool allSpawnedAreDead = true;
        foreach (Character _c in characters)
        {
            if (_c.currentState != Character.CharacterState.Dead)
            {
                allSpawnedAreDead = false;
                break;
            }
        }
        if (allSpawnedAreDead)
        {
            if (onAllSpawnedCharacterEliminated != null)
            {
                onAllSpawnedCharacterEliminated.Invoke();
            }
            characters.Clear();
        }
    }
    public void spawnCharacters()
    {        
        if (hasSpawned)
        {
            return;
        }
        hasSpawned = true;
        AudioManager.instance.playCombatBGM();
        foreach (SpawnPoint point in spawnPointList)
        {
            if (point.EnemyToSpawn != null)
            {
                GameObject spawnedGameObject = Instantiate(point.EnemyToSpawn,point.transform.position, point.transform.rotation);
                characters.Add(spawnedGameObject.GetComponent<Character>());
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {            
            spawnCharacters();            
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, _collider.bounds.size);
    }
}
