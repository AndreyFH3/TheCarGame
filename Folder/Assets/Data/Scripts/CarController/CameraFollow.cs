using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField] private PrometeoCarController carTransform;
    [SerializeField] private Transform target;
	[Range(1, 10)]
	[SerializeField] private float followSpeed = 2;
	[Range(1, 10)]
    [SerializeField] private float lookSpeed = 5;
    [SerializeField] private float smoothTime = .5f;
	private Vector3 initialCameraPosition;
	private Vector3 initialCarPosition;
    private Vector3 absoluteInitCameraPosition;


	void Start(){
		initialCameraPosition = gameObject.transform.position;
		initialCarPosition = carTransform.transform.position;
		absoluteInitCameraPosition = initialCameraPosition - initialCarPosition;
	}

	void FixedUpdate()
	{
		if (carTransform is null) 
			return;

		transform.position = target.position * (1- smoothTime)+ transform.position * smoothTime;
		transform.LookAt(carTransform.transform.position + Vector3.up);
		smoothTime = Mathf.Abs(carTransform.carSpeed) >= 150 ? Mathf.Abs(Mathf.Abs(carTransform.carSpeed)/150 - .85f) : .45f;
    }

	public void Init(PrometeoCarController carTransform, float followSpeed = 2, float lookSpeed = 5)
	{
		this.carTransform = carTransform;
		target = Instantiate(new GameObject(), carTransform.transform).transform;
		target.localPosition = new Vector3(0, 3.5f, -6.5f);
		this.followSpeed = followSpeed;
		this.lookSpeed = lookSpeed;
	}
}
