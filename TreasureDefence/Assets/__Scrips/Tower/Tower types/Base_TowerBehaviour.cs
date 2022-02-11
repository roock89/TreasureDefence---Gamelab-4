using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_TowerBehaviour : TowerBehaviour
{
	override public void Attack(int damage, EnemyBehaviour[] targets)
	{
		if (targets != null)
		{
			Transform _projectile = Instantiate(projectilePrefab, projectileSpawnPos.position, Quaternion.identity).transform;
			attackData newProjectile = getCurrentAttackData(_projectile, targets[0]);
			projectile.Add(newProjectile);
		}
	}
	
	override public void projectileUpdate()
	{
		List<attackData> projectilesHit = new List<attackData>();
		foreach (var currentProjectile in projectile)
		{
			Vector3 pos = new Vector3();
			if(currentProjectile.enemy)
				pos = currentProjectile.UpdateProjectile();
			else
			{
				Destroy(currentProjectile.gameObject);
				projectile.Remove(currentProjectile);
			}
			
			if(!currentProjectile.hit)
				currentProjectile.transform.position = pos;
			else
			{
				projectilesHit.Add(currentProjectile);
			}
		}
		foreach (var hit in projectilesHit)
		{
			Instantiate(ExplotionParticle, hit.transform.position, Quaternion.identity);
			hit.enemy.TakeDamage(hit.projectileDamage);
			projectile.Remove(hit);
			Destroy(hit.gameObject);
		}
	}
	

}