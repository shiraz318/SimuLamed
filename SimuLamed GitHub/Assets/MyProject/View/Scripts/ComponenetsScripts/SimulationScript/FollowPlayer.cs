using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowPlayer : MonoBehaviour
{
	public Transform objectToFollow;

	public Vector3 offset; // different for each camera;
	public float followSpeed = 10;
	public float lookSpeed = 10;

	// Set the angle to the car.
	public void LookAtTarget()
	{
		Vector3 _lookDirection = objectToFollow.position - transform.position;
		Quaternion _rot = Quaternion.LookRotation(_lookDirection, Vector3.up);
		transform.rotation = Quaternion.Lerp(transform.rotation, _rot, lookSpeed * Time.deltaTime);
	}


	// Follow the movement of the car.
	public void MoveToTarget()
	{
		Vector3 _targetPos = objectToFollow.position +
							 objectToFollow.forward * offset.z +
							 objectToFollow.right * offset.x +
							 objectToFollow.up * offset.y;
		transform.position = Vector3.Lerp(transform.position, _targetPos, followSpeed * Time.deltaTime);
	}

	private void FixedUpdate()
	{
		LookAtTarget();
		MoveToTarget();
	}

	
}