using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private float _buoyancy = 1.0f;
	private float _buoyancyMultiplier = 4.0f;
	private float _air = 1.0f;
	private float _surface = 1.25f;
	private float _waterDrag = 5f;
	//private float _airDrag = 0.5f;
	private float _gravity = -3.4f;
	private float _surfaceFalloffDist = 0.01f;
	// Use this for initialization
	private float _falloff, _dist;

	private float _swimStrength = 50.4f;
	private bool _swimdown = false;
	private bool _swimup = false;
	private float _swimTime = 1.0f;
	private float _swamTime = 0.0f;


	void Start () {
	
	}
	
	void AboveWaterUpdate () {
		// gravity
		rigidbody.drag = _waterDrag;
	}

	void UnderwaterUpdate () {

		// Float according to current buoyancy
		// And fall off depending on how close to the surface
		_falloff = 1.0f;
		_dist = _surface - transform.position.y;
		_buoyancy = 1.0f - (_dist / 100.0f);

		if( _dist < _surfaceFalloffDist )
		{
			_falloff =  (_surface - transform.position.y) / _surfaceFalloffDist;
		}

		rigidbody.AddForce(new Vector3(0, _air*_buoyancy*_buoyancyMultiplier*_falloff, 0));
		rigidbody.drag = _waterDrag;
	}

	void Update() {
		// Can only control when under or near surface 
		if (transform.position.y < _surface + 0.1f)
		{
			
			if (Input.GetMouseButtonUp(0))
			{
				_swimup = false;
				_swimdown = false;
				_swamTime = 0.0f;
			}
			
			if (Input.GetMouseButtonDown(0))
			{
				float ypos = (Input.mousePosition.y / Screen.height);
				if (ypos < 0.5)
				{
					_swimdown = true;
					_swamTime = 0.0f;
				}
				else 
				{
					_swimup = true;
					_swamTime = 0.0f;
				}
			}
		}
		else 
		{
			_swimup = false;
			_swimdown = false;
			_swamTime = 0.0f;
		}
	}

	// Update is called once per frame
	void FixedUpdate () {

		if (_swimup || _swimdown)
		{
			_swamTime += Time.deltaTime;
			if (_swamTime > _swimTime)
			{
				_swimup = _swimdown = false;
				_swamTime = 0.0f;
			}
			if (_swimdown)
			{
				rigidbody.AddForce(new Vector3(0, -_swimStrength*Time.deltaTime, 0));
			} 
			if (_swimup)
			{
				rigidbody.AddForce(new Vector3(0, _swimStrength*Time.deltaTime, 0));
			}
		}

		// gravity
		rigidbody.AddForce(new Vector3(0, _gravity, 0));

		if (transform.position.y < _surface)
		{
			UnderwaterUpdate();
		} else {
			AboveWaterUpdate();
		}
	}
}

