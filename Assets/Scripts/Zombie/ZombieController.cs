using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
	public enum ZombieState
	{
		Idle,
		Chase,
		Attack
	}

	public ZombieState currentState = ZombieState.Idle;

	public float chaseRange = 10f;
	public float attackRange = 1.5f;

	public float attackTime = 1.0f;
	public float attackDamage = 5.0f;

	private GameObject[] players;
	private Transform targetPlayer;

	private NavMeshAgent navMeshAgent;
	private Animator animator;
	private bool canAttack = true;
	private Coroutine handleAttackRoutine = null;

	void Start()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();

		SetState(ZombieState.Idle);
	}

	void FindPlayers()
	{
		players = GameObject.FindGameObjectsWithTag("Player");
	}

	void UpdateTarget()
	{
		FindPlayers();

		int closestPlayer = 0;
		float distanceToClosestPlayer = Mathf.Infinity;

		for (int i = 0; i < players.Length; i++)
		{
			float distance = (transform.position - players[i].transform.position).sqrMagnitude;
			if (distance < distanceToClosestPlayer)
			{
				closestPlayer = i;
				distanceToClosestPlayer = distance;
			}
		}

		targetPlayer = players[closestPlayer].transform;
	}

	void Update()
	{
		if(targetPlayer == null)
		{
			SetState(ZombieState.Idle);
		}

		switch (currentState)
		{
			case ZombieState.Idle:
				IdleState();
				break;
			case ZombieState.Chase:
				ChaseState();
				break;
			case ZombieState.Attack:
				AttackState();
				break;
		}
	}

	void SetState(ZombieState newState)
	{
		currentState = newState;

		switch(currentState)
		{
			case ZombieState.Idle:
				animator.SetBool("Chase", false);
				break;

			case ZombieState.Chase:
				animator.SetBool("Chase", true);
				break;
		}
	}

	void IdleState()
	{
		UpdateTarget();

		if (Vector3.Distance(transform.position, targetPlayer.position) < chaseRange)
		{
			SetState(ZombieState.Chase);
		}

		canAttack = true;
	}

	void ChaseState()
	{
		if(handleAttackRoutine != null)
		{
			StopCoroutine(handleAttackRoutine);
			canAttack = true;
		}

		float distance = Vector3.Distance(transform.position, targetPlayer.position);

		if (distance < attackRange)
		{
			SetState(ZombieState.Attack);
			return;
		}
		else if (distance > chaseRange)
		{
			SetState(ZombieState.Idle);
			return;
		}

		navMeshAgent.isStopped = false;
		navMeshAgent.SetDestination(targetPlayer.position);
	}

	void AttackState()
	{
		if (Vector3.Distance(transform.position, targetPlayer.position) > attackRange)
		{
			SetState(ZombieState.Idle);
			return;
		}

		if (!canAttack)
			return;

		navMeshAgent.isStopped = true;

		animator.SetTrigger("Attack");
		handleAttackRoutine = StartCoroutine(HandleAttack());
	}

	IEnumerator HandleAttack()
	{
		canAttack = false;
		yield return new WaitForSeconds(attackTime / 2);
		Debug.Log("Attack");
		//targetPlayer.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, attackDamage);
		yield return new WaitForSeconds(attackTime / 2);
		canAttack = true;
		handleAttackRoutine = null;
	}
}
