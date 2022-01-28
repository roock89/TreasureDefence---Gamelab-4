using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
	// Level variables
	public LevelWaveSequence LevelData;
	[SerializeField] private int currentWave;
	[SerializeField] Transform EnemyParent;
	
	// Individual wave variables
	bool waveIsInProgress;
	
	public bool waveIsPlaying => waveIsInProgress;
	
	private int waveProgress;

	// Timer variables
	private float cooldownTimer = 0;
	private float currentCooldown = 0;
	private int repeatSpawn = -1;

	void Start()
	{
		EnemyParent = GameObject.FindGameObjectWithTag("EnemyHolder").transform;
	}

	// Update is called once per frame
	void  Update()
	{
		if(waveIsInProgress)
		{
			if(waveProgress == getWaveLength())
			{
				waveIsInProgress = false;
				EndOfWave();
			}
			
			if(cooldownTimer > currentCooldown)
			{
				GameManager.instance.enemies.Add(SpawnNextEnemy());
			}
			else
				cooldownTimer += Time.deltaTime;
		}		
	}
	public void nextWave()
	{
		waveIsInProgress = true;
	}
	
	private int getWaveLength()
	{
		return LevelData.waves[currentWave].waveData.Length;
	}
	
	private void EndOfWave()
	{
		currentWave++;
		waveProgress = 0;
		cooldownTimer = 0;
		currentCooldown = 0;
		repeatSpawn = -1;
	}
	
	Enemy SpawnNextEnemy()
	{
		GameObject spawn = null;
		
		// Debug.Log("wave progress");
		if(repeatSpawn == -1)
		{
			repeatSpawn = getRepeatSpawn();
			// Debug.Log("new repeat spawn");
		}
		else if(repeatSpawn > -1)
		{
			// update repeat spawning
			repeatSpawn--;
			
			// Spawn the next enemy
			spawn = Instantiate(getEnemyPrefab());
			spawn.transform.SetParent(EnemyParent, true);
			
			currentCooldown = getCooldown();
			cooldownTimer = 0;
			
			// update wave progress		
			if(repeatSpawn == -1)
				waveProgress++;
			// Debug.Log("decrease spawn repeat");
		}
		if(spawn != null)
			return spawn.GetComponent<Enemy>();
		// Debug.Log(" no enemy was spawned");
		return null;
	}
	
	GameObject getEnemyPrefab()
	{
		return LevelData.waves[currentWave].waveData[waveProgress].enemyPfb;
	}
	
	private float getCooldown()
	{
		return LevelData.waves[currentWave].waveData[waveProgress].Cooldown;
	}
	
	private int getRepeatSpawn()
	{
		int n = Mathf.Clamp(LevelData.waves[currentWave].waveData[waveProgress].repeat -1, 0, 100000);
		return n;	
	}
}