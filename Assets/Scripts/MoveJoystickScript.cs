using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveJoystickScript : MonoBehaviour, IDragHandler, IEndDragHandler {

	public PlayerScript player;

	Vector3 initPos;
	Vector3 direction;
	public bool isDragging = false;
	public float rotateSpeed = 20f;
	public Transform panMovement;

	// Use this for initialization
	void Start () 
	{
		initPos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		direction = this.transform.position - initPos;
		//direction.Normalize();

		if(direction.normalized.y != 0.0f)
		{
			player.currSpeed += direction.normalized.y * 40;
			if(player.currSpeed > player.maxSpeed)
			{
				player.currSpeed = player.maxSpeed;
			}
			else if(player.currSpeed < player.minSpeed)
			{
				player.currSpeed = player.minSpeed;
			}
		}
	}

	void LateUpdate()
	{
		panMovement.Rotate(Vector3.up * direction.normalized.x * Time.deltaTime * rotateSpeed);
	}

	public void OnDrag(PointerEventData eventData)
	{
		if(!isDragging) isDragging = true;

		this.transform.position = eventData.position;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		isDragging = false;

		this.transform.position = initPos;
		direction = Vector3.zero;
	}
}
