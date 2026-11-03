using System;
using UnityEngine;
using DG.Tweening;

public class LastBearerBossBehaviour : BossBehaviour
{

public override void Start()
	{
		base.Start();

		enemyStatus.EnemyHealthChangedEvent += CallBossHealthChanged;

		//Combat test: start fighting immediately instead of waiting for an intro/dialogue trigger
		chasingPlayer = true;
	}

public override void OnEnable()
	{
		//Intentionally does not call base.OnEnable(): that subscribes using enemyStatus before Start() assigns it.
		//Subscribed in Start() instead.
	}

	public override void OnDisable()
	{
		if (enemyStatus != null)
			enemyStatus.EnemyHealthChangedEvent -= CallBossHealthChanged;
	}


	public enum LastBearerAttack { none = 0, attack1 = 1, attack2 = 2, attack3 = 3, attack4 = 4 }

	public LastBearerAttack currentAttack;
	public LastBearerAttack lastAttack;

	[Header("VFX")]
	public GameObject groundWaveVFX;

	
[Header("Movement Settings")]
	public float keepDistance = 5f;

	
[Header("Attack 1 Settings")]
	public float attack1Distance = 2f;
	public float attack1DamageMultiplier = 1f;
	public string attack1AnimParameterName = "Attack1Trigger";

	[Header("Attack 2 Settings")]
	public float attack2Distance = 2f;
	public float attack2DamageMultiplier = 1f;
	public string attack2AnimParameterName = "Attack2Trigger";

	[Header("Attack 3 Settings")]
	public float attack3Distance = 2f;
	public float attack3DamageMultiplier = 1f;
	public string attack3AnimParameterName = "Attack3Trigger";

	[Header("Attack 4 Settings")]
	public float attack4Distance = 2f;
	public float attack4DamageMultiplier = 1f;
	public string attack4AnimParameterName = "Attack4Trigger";
		public GameObject[] summonPrefabs;
		public float summonDistanceBehind = 2f;
		public float summonLifetime = 8f;

public override void ChooseAndAct()
	{
		if (paused || enemyStatus.IsDead || IsBeingTamed() || !chasingPlayer)
			return;

		moveAmount = Vector2.zero;

		if (currentAttack == LastBearerAttack.none)
		{
			currentAttack = ChooseNextAttack(lastAttack);

			switch (currentAttack)
			{
				case LastBearerAttack.attack1:
					attackDistance = attack1Distance;
					break;
				case LastBearerAttack.attack2:
					attackDistance = attack2Distance;
					break;
				case LastBearerAttack.attack3:
					attackDistance = attack3Distance;
					break;
				case LastBearerAttack.attack4:
					attackDistance = attack4Distance;
					break;
			}
		}

		target = Player.Instance.gameObject;

		bool isBusy = anim.GetCurrentAnimatorStateInfo(0).IsTag(Settings.enemyAttackTag) || anim.GetCurrentAnimatorStateInfo(0).IsTag(Settings.enemyHitTag);

		if (readyToAttack)
		{
			//Close the distance to melee range
			if (!isBusy && Vector2.Distance(transform.position, target.transform.position) >= minDistance)
			{
				MoveToTarget();
			}

			if (!anim.GetCurrentAnimatorStateInfo(0).IsTag(Settings.enemyAttackTag) && Vector2.Distance(transform.position, target.transform.position) < attackDistance)
			{
				Attack();
			}
		}
		else
		{
			//Attack on cooldown: since every attack is melee, back off to a safe distance instead of staying glued to the player
			if (!isBusy && Vector2.Distance(transform.position, target.transform.position) < keepDistance)
			{
				targetPositionBetweenAttacks = target.transform.position + ((transform.position - target.transform.position).normalized * keepDistance);
				MoveToPoint(targetPositionBetweenAttacks);
			}
		}

		//Animation stuff
		anim.SetFloat("Speed", moveAmount.magnitude);
		if (moveAmount.x != 0)
		{
			artTransform.localScale = new Vector3(Mathf.Sign(moveAmount.x), 1, 1);
		}
		else if (anim.GetCurrentAnimatorStateInfo(0).IsTag(Settings.enemyAttackTag))
		{
			artTransform.localScale = new Vector3(Mathf.Sign(target.transform.position.x - transform.position.x), 1, 1);
		}
	}

	public override void MoveToTarget()
	{
		MoveToPoint(target.transform.position);
	}

	public LastBearerAttack ChooseNextAttack(LastBearerAttack previousAttack)
	{
		var values = Enum.GetValues(typeof(LastBearerAttack));
		int random = UnityEngine.Random.Range(1, values.Length);

		LastBearerAttack newAttack = (LastBearerAttack)values.GetValue(random);

		if (newAttack == previousAttack)
		{
			random += 1;
			if (random >= values.Length)
				random = 1;

			newAttack = (LastBearerAttack)values.GetValue(random);
		}

		return newAttack;
	}

	public override void Attack()
	{
		switch (currentAttack)
		{
			case LastBearerAttack.attack1:
				anim.SetTrigger(attack1AnimParameterName);
				break;
			case LastBearerAttack.attack2:
				anim.SetTrigger(attack2AnimParameterName);
				break;
			case LastBearerAttack.attack3:
				anim.SetTrigger(attack3AnimParameterName);
				break;
			case LastBearerAttack.attack4:
				anim.SetTrigger(attack4AnimParameterName);
				break;
		}

		readyToAttack = false;

		readyToAttackTween.Kill();
		readyToAttackTween = DOVirtual.DelayedCall(timeBetweenAttacks, EnableReadyToAttack, false);

		lastAttack = currentAttack;
		currentAttack = LastBearerAttack.none;
	}

	//Called from an Animation Event on Attack1-4
	public void DealMeleeDamage()
	{
		float damageMultiplier = 1f;
		switch (lastAttack)
		{
			case LastBearerAttack.attack1: damageMultiplier = attack1DamageMultiplier; break;
			case LastBearerAttack.attack2: damageMultiplier = attack2DamageMultiplier; break;
			case LastBearerAttack.attack3: damageMultiplier = attack3DamageMultiplier; break;
			case LastBearerAttack.attack4: damageMultiplier = attack4DamageMultiplier; break;
		}

		Vector2 point = (Vector2)transform.position + new Vector2(closeCombatColliderOffset.x * Mathf.Sign(target.transform.position.x - transform.position.x), closeCombatColliderOffset.y);
		PlayerStatus[] playerStatusArray = HelperMethods.GetComponentsAtBoxLocationNonAlloc<PlayerStatus>(4, point, closeCombatColliderSize, 0f);
		for (int i = playerStatusArray.Length - 1; i >= 0; i--)
		{
			if (playerStatusArray[i] != null)
			{
				Vector2 push = (Vector2)(playerStatusArray[i].transform.position - transform.position).normalized * pushForce;
				playerStatusArray[i].DealDamage(enemyStatus.strength.GetStatValue() * damageMultiplier, push);
			}
		}
	}

//Called from an Animation Event near the end of Attack1_1: rolls whether the combo continues into Attack1_2
	public void RollContinueAttack1()
	{
		anim.SetBool("ContinueAttack1", UnityEngine.Random.value < 2f / 3f);
	}

	//Called from an Animation Event near the end of Attack1_2: rolls whether the combo continues into Attack1_3
	public void RollContinueAttack1_2()
	{
		anim.SetBool("ContinueAttack1_2", UnityEngine.Random.value < 0.5f);
	}

//Called from an Animation Event on every attack clip: lunges towards the target with the given impulse force
	public void LungeTowardsTarget(float force)
	{
		Vector2 dirToTarget = target.transform.position - transform.position;
		enemyStatus.PushEnemy(dirToTarget.normalized * force);
	}

//Called from an Animation Event on Attack4: summons a random enemy from summonPrefabs behind the boss
	public void SummonRandomEnemy()
	{
		if (summonPrefabs == null || summonPrefabs.Length == 0) return;

		GameObject prefab = summonPrefabs[UnityEngine.Random.Range(0, summonPrefabs.Length)];
		if (prefab == null) return;

		//Boss faces the target, so "behind" is the opposite side
		float facing = target != null ? Mathf.Sign(target.transform.position.x - transform.position.x) : artTransform.localScale.x;
		Vector3 spawnPos = transform.position + new Vector3(-facing * summonDistanceBehind, 0f, 0f);
		GameObject summoned = Instantiate(prefab, spawnPos, Quaternion.identity);

		//Summoned enemies die after summonLifetime seconds
		EnemyStatus summonedStatus = summoned.GetComponent<EnemyStatus>();
		if (summonedStatus != null)
			DOVirtual.DelayedCall(summonLifetime, () => { if (summonedStatus != null && !summonedStatus.IsDead) summonedStatus.KillEnemy(); }, false);
	}

	//Called from an Animation Event on Attack3: activates and plays the ground wave VFX
	public void PlayGroundWaveVFX()
	{
		if (groundWaveVFX == null) return;

		groundWaveVFX.SetActive(true);

		var ps = groundWaveVFX.GetComponent<ParticleSystem>();
		if (ps != null) ps.Play();
	}




	//Called from animation
	public void DoScreenshake()
	{
		PlayerCamera.Instance.Shake(Settings.hoeAttackScreenshakeAmplitudeMult, Settings.hoeAttackScreenshakeFrequencyMult, 0.15f);
	}
}
