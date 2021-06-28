using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowPlayer : MonoBehaviour
{
	[SerializeField]
	private Transform objectToFollow;
	[SerializeField]
	private Vector3 offset; // different for each camera;
	
	private float followSpeed = 10;
	private float lookSpeed = 10;

	// Set the angle to the car.
	public void LookAtTarget()
	{
		Vector3 lookDirection = objectToFollow.position - transform.position;
		Quaternion rot = Quaternion.LookRotation(lookDirection, Vector3.up);
		transform.rotation = Quaternion.Lerp(transform.rotation, rot, lookSpeed * Time.deltaTime);
	}


	// Follow the movement of the car.
	public void MoveToTarget()
	{
		Vector3 targetPos = objectToFollow.position +
							 objectToFollow.forward * offset.z +
							 objectToFollow.right * offset.x +
							 objectToFollow.up * offset.y;
		transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
	}

	private void FixedUpdate()
	{
		LookAtTarget();
		MoveToTarget();
	}

	
}