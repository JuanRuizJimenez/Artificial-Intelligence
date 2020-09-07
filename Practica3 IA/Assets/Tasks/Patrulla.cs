﻿namespace BehaviorDesigner.Samples
{

	using UnityEngine;
	// En versiones antiguas de Unity al parece hacía falta usar este otro espacio de nombres
#if !(UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4)
	using UnityEngine.AI;
#endif
	using BehaviorDesigner.Runtime;
	using BehaviorDesigner.Runtime.Tasks;
	using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

	[TaskCategory("FootBall")]
	[TaskDescription("Queda a la espera hasta que la pelota cruce la mitad del campo.")]
	public class Patrulla : Action
	{
		private Vector3 targetPosition;

		private NavMeshAgent navMeshAgent;

		public float speed = 5f;

		public override void OnAwake()
		{
			targetPosition = GameObject.FindGameObjectWithTag("ball").transform.position;//gameObject.transform.position;
			navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
		}

		public override void OnStart()
		{
			navMeshAgent.enabled = true;
		}

		public override TaskStatus OnUpdate()
		{

			targetPosition = GameObject.FindGameObjectWithTag("ball").transform.position;

			if (Object.FindObjectOfType<GameManager>().goalTime())
			{
				return TaskStatus.Failure;
			}

			if (targetPosition.x > 0)
			{
				return TaskStatus.Success;
			}
			else
			{
				navMeshAgent.SetDestination(new Vector3(30, 0.1f, targetPosition.z));
				return TaskStatus.Running;
			}

		}
	}
}
