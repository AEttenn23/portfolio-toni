using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Drawing;
using Cinemachine;
using UnityEngine.Rendering;

public class TitanGolunkBossBehaviour : BossBehaviour
{
	public enum TitanGolunkAttack {none = 0, groundSmash = 1, spores = 2, roots = 3, vines = 4}

	public TitanGolunkAttack currentTitanGolunkAttack;
	public TitanGolunkAttack lastTitanGolunkAttack;

	[Header("Ground Smash Settings")]
	public float groundSmashAttackDistance;
	public float groundSmashAttackDamageMultiplayer;
	public string groundSmashAnimParameterName;
	
	[Header("Spores Settings")]
	public float sporesAttackDistance;
	public float sporesAttackDamageMultiplayer;
	public float sporesAttackSporesPerUnitOfTime;
	public float sporesAttackSporesInterval;
	public string sporesAnimParameterName;
	public GameObject sporePrefab;
    public Transform sporesSpawnPoint;

    [Header("Roots Settings")]
	public float rootsAttackDistance;
	public float rootsAttackDamageMultiplayer;
	public string rootsAnimParameterName;
	public GameObject rootPrefab;


	[Header("Vines Settings")]
	public string vinesAnimParameterName;
	public GameObject vinesPrefab;
	public GameObject roomCenterTarget;
	public float vinesPushForce = 60f;
	public GameObject vinesParent;

	private bool introPlayed;

	public GameObject titanNPCGameObject;

	private bool tutorialShown;
	public override void OnEnable()
	{
		introPlayed = false;
		tutorialShown = false;
		CameraTargetSwap.CameraChangeTriggered += CameraSwapTriggered;
		//base.OnEnable();
	}

	public override void OnDisable()
	{
		CameraTargetSwap.CameraChangeTriggered -= CameraSwapTriggered;

		TimeManager.Instance.BossfightActive = false;

		base.OnDisable();
	}

	public override void Start()
	{
		base.Start();
		enemyStatus.Invincible = true;
		enemyStatus.EnemyHealthChangedEvent += BossHealthChanged; //Already done in base on Enable
		enemyStatus.CanBeCaptured = false;
	}

	bool fiftyPercentHealthEventPlayed = false;
	bool zeroPercentHealthEventPlayed = false;

	private void BossHealthChanged(float maxHealth, float currentHealth)
	{
		if(currentHealth/maxHealth <= 0.5f && !fiftyPercentHealthEventPlayed)
		{
			//Trigger special attack
			currentTitanGolunkAttack = TitanGolunkAttack.vines;
			readyToAttack = true;
			fiftyPercentHealthEventPlayed = true;
			enemyStatus.Invincible = true;
			enemyStatus.CanBeCaptured = true;
		}
	}

	public void Update()
	{
		enemyStatus.canBeCapturedPS.SetActive(anim.GetCurrentAnimatorStateInfo(0).IsName("Vines") || anim.GetCurrentAnimatorStateInfo(0).IsName("VinesHeal"));
		if (enemyStatus.health.GetCurrentValue() / enemyStatus.health.GetStatValue() <= 0.5f && !fiftyPercentHealthEventPlayed)
		{
			//Trigger special attack
			currentTitanGolunkAttack = TitanGolunkAttack.vines;
			readyToAttack = true;
			fiftyPercentHealthEventPlayed = true;
			enemyStatus.Invincible = true;
			enemyStatus.CanBeCaptured = true;

			if(!tutorialShown)
			{
				UIManager.Instance.OpenBossDewTutorialPopUp();
				tutorialShown = true;
			}
		}

		if (enemyStatus.health.GetCurrentValue() / enemyStatus.health.GetStatValue() <= 0.0f && !zeroPercentHealthEventPlayed)
		{
			//Trigger special attack
			currentTitanGolunkAttack = TitanGolunkAttack.vines;
			readyToAttack = true;
			zeroPercentHealthEventPlayed = true;
			enemyStatus.Invincible = true;
			enemyStatus.CanBeCaptured = true;
		}
	}

	public override void ChooseAndAct()
	{
		if (paused || enemyStatus.IsDead || IsBeingTamed() || !chasingPlayer)
			return;

		moveAmount = Vector2.zero;

		if(currentTitanGolunkAttack == TitanGolunkAttack.none)
		{
			//Choose Attack
			currentTitanGolunkAttack = ChoseNextAttack(lastTitanGolunkAttack);

			switch (currentTitanGolunkAttack)
			{
				case TitanGolunkAttack.groundSmash:
					attackDistance = groundSmashAttackDistance;
					break;
				case TitanGolunkAttack.spores:
					attackDistance = sporesAttackDistance;
					break;
				case TitanGolunkAttack.roots:
					attackDistance = rootsAttackDistance;
					break;
				case TitanGolunkAttack.vines:
					attackDistance = 0f;
					break;
			}
		}

		if (readyToAttack)
		{
			//Attack coooldown is over
			if(currentTitanGolunkAttack == TitanGolunkAttack.vines)
			{
				target = roomCenterTarget;
			}
			else
			{
				target = Player.Instance.gameObject;
			}

			if (!anim.GetCurrentAnimatorStateInfo(0).IsTag(Settings.enemyAttackTag) && !anim.GetCurrentAnimatorStateInfo(0).IsTag(Settings.enemyHitTag) && Vector2.Distance(transform.position, target.transform.position) >= minDistance)
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
			//Attack cooldown
			//Chase player
			if (!anim.GetCurrentAnimatorStateInfo(0).IsTag(Settings.enemyAttackTag) && !anim.GetCurrentAnimatorStateInfo(0).IsTag(Settings.enemyHitTag) && Vector2.Distance(transform.position, target.transform.position) >= minDistance)
			{
				MoveToTarget();
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


	public TitanGolunkAttack ChoseNextAttack(TitanGolunkAttack lastAttack)
	{
		enemyStatus.Invincible = false;
		enemyStatus.CanBeCaptured = false;

		var values = Enum.GetValues(typeof(TitanGolunkAttack));
		int random = UnityEngine.Random.Range(1, values.Length-1);

		TitanGolunkAttack newAttack = (TitanGolunkAttack)values.GetValue(random);

		if (newAttack == lastAttack)
		{
			random += 1;
			if (random >= values.Length-1)
				random = 1;

			newAttack = (TitanGolunkAttack)values.GetValue(random);
		}

		return (newAttack);
	}

	public override void Attack()
	{
		//Debug.Log(Settings.enemyAttackTag);
		switch (currentTitanGolunkAttack)
		{
			case TitanGolunkAttack.groundSmash:
				anim.SetTrigger(groundSmashAnimParameterName);
				break;
			case TitanGolunkAttack.spores:
				anim.SetTrigger(sporesAnimParameterName);
				break;
			case TitanGolunkAttack.roots:
				anim.SetTrigger(rootsAnimParameterName);
				break;
			case TitanGolunkAttack.vines:
				anim.SetTrigger(vinesAnimParameterName);
				break;
		}

		readyToAttack = false;
		//targetPositionBetweenAttacks = target.transform.position + ((transform.position - target.transform.position).normalized * agroAreaRadius * UnityEngine.Random.Range(0.9f, 1.5f));

		readyToAttackTween.Kill();
		readyToAttackTween = DOVirtual.DelayedCall(timeBetweenAttacks, EnableReadyToAttack, false);

		lastTitanGolunkAttack = currentTitanGolunkAttack;
		currentTitanGolunkAttack = TitanGolunkAttack.none;
		/*

		//Attack info
		Vector2 point = (Vector2)transform.position + (Vector2)(target.transform.position - transform.position).normalized;
		Vector2 size = new Vector2(2, 2);

		//Attack player
		//EnemyStatus[] enemyArray = HelperMethods.GetComponentsAtBoxLocationNonAlloc<EnemyStatus>(20, point, size, 0f);
		PlayerStatus [] playerStatusArray = HelperMethods.GetComponentsAtBoxLocationNonAlloc<PlayerStatus>(4, point, size, 0f);
		for (int i = playerStatusArray.Length - 1; i >= 0; i--)
		{
			if (playerStatusArray[i] != null)
			{
				//Push force
				Vector2 push = (Vector2)(playerStatusArray[i].transform.position - transform.position).normalized * pushForce;

				playerStatusArray[i].DealDamage(enemyStatus.strength, enemyStatus.magic, push);
			}
		}
	    */
	}


	public void InstantiateGroundSmashDamageCollider()
	{
		PlayerStatus[] playerStatusArray = HelperMethods.GetComponentsAtBoxLocationNonAlloc<PlayerStatus>(40, transform.position, Vector2.one * 5f, 0f);
		for (int i = playerStatusArray.Length - 1; i >= 0; i--)
		{
			if (playerStatusArray[i] != null)
			{
				//Push force
				Vector2 push = (Vector2)(playerStatusArray[i].transform.position - transform.position).normalized * pushForce;

				playerStatusArray[i].DealDamage(enemyStatus.strength.GetStatValue(), (target.transform.position - transform.position).normalized * pushForce, true);
			}
		}

		DoScreenshake();
	}

	public void SpawnSporeInRandomDirection()
	{
        Vector3 spawnPosition = sporesSpawnPoint != null ? sporesSpawnPoint.position : transform.position + new Vector3(0, 2f);

        for (int i = 0; i < sporesAttackSporesPerUnitOfTime; i++)
		{
			if(i == sporesAttackSporesPerUnitOfTime)
			{
                //Aim at player
                GameObject newSpore = Instantiate(sporePrefab);
                newSpore.GetComponent<EnemyProjectile>().Init(spawnPosition, ((target.transform.position + (Vector3)Player.Instance.GetPlayerVectorDirection()) - spawnPosition).normalized, enemyStatus.strength.GetStatValue(), enemyStatus.magic.GetStatValue(), pushForce, gameObject);
            }	
			else
			{
				//Aim Randomly
                GameObject newSpore = Instantiate(sporePrefab);
                newSpore.GetComponent<EnemyProjectile>().Init(spawnPosition, UnityEngine.Random.insideUnitCircle, enemyStatus.strength.GetStatValue(), enemyStatus.magic.GetStatValue(), pushForce, gameObject);
			}
		}
	}
			

	public void SpawnRootTowardsPlayer()
	{
		GameObject newRoot = Instantiate(rootPrefab);
		newRoot.GetComponent<TitanAmanitaRootProjectile>().Initialize(transform.position, ((target.transform.position + (Vector3)Player.Instance.GetPlayerVectorDirection()) - transform.position).normalized, enemyStatus.strength.GetStatValue(), enemyStatus.magic.GetStatValue(), pushForce, gameObject, target.transform);
	}

	public void PushPlayerOnVinesStart()
	{
		Vector2 pushDir = Player.Instance.GetPlayerCentrePosition() - transform.position;
		Player.Instance.GetComponent<PlayerStatus>().PushPlayer(pushDir.normalized.normalized * vinesPushForce);
	}

	public void HealFromVines()
	{
		//Recover a 20% of health
		enemyStatus.Invincible = false;
		enemyStatus.health.ModifyValue(enemyStatus.health.GetStatValue() * 0.1f);
		enemyStatus.Invincible = true;

		EventHandler.CallActiveBossHealthChangedEvent(enemyStatus.health.GetCurrentValue(), enemyStatus.health.GetStatValue());
	}

	public void SpawnVinesAround()
	{
		InstantiateInCircle(vinesPrefab, Vector3.zero, 4, 20, gameObject);

		InstantiateInCircle(vinesPrefab, Vector3.zero, 8, 40, gameObject);
	}

	public void DestroyVines()
	{
		//Remove invincibility
		//Destroy spawned vines
		//List<Transform> vines = new List<Transform>(vinesParent.GetComponentsInChildren<Transform>());
		for (int i = vinesParent.transform.childCount - 1; i >= 0; i--)
		{
			//Debug.Log("Destroying vine " + vinesParent.transform.GetChild(i).gameObject);
			Destroy(vinesParent.transform.GetChild(i).gameObject);
		}
	}

	public void InstantiateInCircle(GameObject obj, Vector3 center, float radius, int number, GameObject parent = null)
	{
		for (int i = 0; i < number; i++)
		{
			float angle = i * Mathf.PI * 2f / number;
			Vector3 newPos = center + (new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0));
			Instantiate(obj, newPos, Quaternion.Euler(0, 0, 0)).transform.parent = vinesParent.transform;
		}
	}

	public void CameraSwapTriggered(Cinemachine.CinemachineVirtualCamera newCam)
	{
		//Bosfight camera event
		if (newCam.transform.parent != null && newCam.transform.parent.name == "CameraEvent" && !introPlayed)
		{
			//Trap player with roots
			TitanAmanitaProtoFinal titanAmanitaProtoFinal = FindFirstObjectByType<TitanAmanitaProtoFinal>();
			titanAmanitaProtoFinal.CloseAccessToArena();

			//Disable collider
			//newCam.transform.parent.GetComponent<PolygonCollider2D>().enabled = false;
			introPlayed = true;

			//Start conversation
			DOVirtual.DelayedCall(1f, () => titanNPCGameObject.GetComponent<NPCInteractable>().StartConversation());
		}

		//Post bossfight animation camera event

		if(newCam.transform.parent != null && newCam.transform.parent.name == "CameraEventTotem")
		{

		}
	}


	//Called from animation
	public void StartChasingPlayer()
	{
		//Make boss start chasing the player
		chasingPlayer = true;
		enemyStatus.Invincible = false;

		TimeManager.Instance.BossfightActive = true;

		titanNPCGameObject.GetComponent<NPCInteractable>().enabled = false;
	}

	//Called from animation
	public void DoScreenshake()
	{
		PlayerCamera.Instance.Shake(Settings.hoeAttackScreenshakeAmplitudeMult, Settings.hoeAttackScreenshakeFrequencyMult, 0.15f);
	}

	//Called once dialog is ended
	[Yarn.Unity.YarnCommand("PlayBossIntro")]
	public void PlayBossIntro()
	{
		anim.SetTrigger("PlayIntro");

		FindFirstObjectByType<BossInfoHUD>().ShowBossInfo();

		////AudioManager.Instance.PlayBespokeMusicTrackOnLoop(SoundName.musicMeadowBossfight);
	}	

	//Called from animator
	public void EndBossfightIfNecessary()
	{
		if(zeroPercentHealthEventPlayed)
		{
			//Players has tamed boss fight on last health event
			//Play outro

			//Stop boss
			chasingPlayer = false;

			//Hide Healthbar
			FindFirstObjectByType<BossInfoHUD>().HideBossInfo();

			//White flash
			foreach(SpriteRenderer bodyRend in bodyRenderers)
			{
				bodyRend.material.SetFloat("_HitEffectBlend", 1f);
				DOVirtual.DelayedCall(0.05f, () => bodyRend.material.SetFloat("_HitEffectBlend", 0f), false);
			}

			//Sound
			AudioManager.Instance.PlayOneShot2D(Others_FMODEvents.Instance.BossFight_End);

			//Push player
			PushPlayerOnVinesStart();

			//Play dialog
			StartPostBossFightDialog();

			//Stop boss music and play normal music
			////AudioManager.Instance.StopMusic();
			////AudioManager.Instance.PlaySceneSounds();
		}
	}

	public void StartPostBossFightDialog()
	{
		titanNPCGameObject.GetComponent<NPCInteractable>().StartConversationWithNode("TitanAmanitaPostBossfight");
	}

	//Called from dialog
	[Yarn.Unity.YarnCommand("PlayBondSequence")]
	public void PlayBondSequence()
	{
		//Play totem light animation
		GameObject totemAnim = GameObject.Find("BossfightTameTotemAnimation");

		//TODO: Add fade in and fade out before moving camera in and out
		SceneControllerManager.Instance.FadeInAndBackEffect(1.5f);

		//Enable camera
		CameraTargetSwap cameraSwap = totemAnim.transform.Find("CameraEventTotem").GetComponent<CameraTargetSwap>();

		DOVirtual.DelayedCall(0.75f, cameraSwap.SwapToTargetCamera).SetUpdate(false);

		//Tween eyes
		//Return to previous camera after a delay
		SpriteRenderer eyesSprite = totemAnim.transform.Find("mushroom_boss_statue/titanAmanitaBlueEyes").GetComponent<SpriteRenderer>();
		Tween eyesTween = eyesSprite.DOFade(1, 2f).SetDelay(3f).SetEase(Ease.OutElastic);
		DOVirtual.DelayedCall(7f, () => { SceneControllerManager.Instance.FadeInAndBackEffect(1.5f); DOVirtual.DelayedCall(0.75f, cameraSwap.SwapToPreviousCamera); });//DOVirtual.DelayedCall(1f, () => )

		//Complete totem on Farm Progress Manager
		FarmProgressManager.Instance.titanAmanitaCompleted = true;
	}
	
	//Called from dialog
	[Yarn.Unity.YarnCommand("LeaveArena")]
	public void LeaveArena()
	{
		//Fade in and fade out
		//Destroy bossfight camera
		SceneControllerManager.Instance.FadeInAndBackEffect(1f);
		DOVirtual.DelayedCall(0.75f, () => {
			TitanAmanitaProtoFinal titanAmanitaProtoFinal = FindFirstObjectByType<TitanAmanitaProtoFinal>();
			titanAmanitaProtoFinal.progress = 1;
			titanAmanitaProtoFinal.DestroyBlocker();

			Player.Instance.EnablePlayerInput();
		}, false);
	}

	
}
