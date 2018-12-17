using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LookJoystickScript : MonoBehaviour, IDragHandler, IEndDragHandler {

	public float rotateSpeed = 20f;
	Vector3 initPos;
	Vector3 direction;
	public Transform rotationAxis;
	public PlayerScript player;

	// Use this for initialization
	void Start () 
	{
		initPos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		direction = this.transform.position - initPos;
	}

	void LateUpdate()
	{
		if(player.canMove)
		{
			rotationAxis.Rotate(Vector3.back * direction.normalized.x * Time.deltaTime * rotateSpeed);

			rotationAxis.Rotate(Vector3.right * direction.normalized.y * Time.deltaTime * rotateSpeed); //correct
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		this.transform.position = eventData.position;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		this.transform.position = initPos;
		direction = Vector3.zero;
	}
}
