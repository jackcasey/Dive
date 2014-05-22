using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public Animator animator;
	public float maxDepth = 0.0f;
	private float oxygenUsageRate = 1.0f;
	private float oxygenUsageRateMin = 1.0f;
	private float oxygen;
	private float oxygenMax = 300.0f;
	private float oxygenUsageIncreasePerSwim = 0.075f;
	private float oxygenUsageDecayRate = 0.1f;
	public GUIText oxygenLabel;
	public GUIText oxygenRateLabel;

		
	private float _buoyancy = 1.0f;
	private float _buoyancyMultiplier = 4.0f;
	private float _air = 1.0f;

	private float _surface = 0.0f;
	private float _waterDrag = 5f;
	//private float _airDrag = 0.5f;
	private float _gravity = -3.4f;
	private float _surfaceFalloffDist = 0.01f;
	// Use this for initialization
	private float _falloff, _dist;

	private float _swimStrength = 50.4f;
	private bool _swimdown = false;
	private bool _swimup = false;
	private float _swimTime = 1.25f;
	private float _swamTime = 0.0f;




	void Start () {
		maxDepth = 0.0f;
		oxygen = oxygenMax;
		oxygenUsageRate = oxygenUsageRateMin;
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

	void UpdateOxygenUI()
	{
		oxygenLabel.text = "Oxygen: " + oxygen;
		oxygenRateLabel.text = "Rate: " + oxygenUsageRate;
	}


	void UpdateOxygen()
	{
		oxygenUsageRate = Mathf.Lerp(oxygenUsageRate, oxygenUsageRateMin, Time.deltaTime*oxygenUsageDecayRate);
		if (HeadUnderWater())
		{
			oxygen -= oxygenUsageRate * Time.deltaTime;
		} else {
			oxygen = oxygenMax;
		}
		UpdateOxygenUI();
	}

	void Update() {
		UpdateOxygen();

		maxDepth = Mathf.Max(maxDepth, transform.position.y);

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
				if (!_swimup && !_swimdown)
				{
					oxygenUsageRate += oxygenUsageIncreasePerSwim;
				}
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

		UpdateAnimation();
	}

	bool HeadUnderWater()
	{
		return(transform.position.y < _surface - (0.06));
	}


	bool UnderWater()
	{
		return(transform.position.y < _surface);
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

		if (UnderWater())
		{
			UnderwaterUpdate();
		} else {
			AboveWaterUpdate();
		}
	}

	void UpdateAnimation()
	{
		animator.SetBool("Treading", !HeadUnderWater());
		animator.SetBool("Swimming", _swimup || _swimdown);

		if (_swimup)
			animator.SetBool("Direction", false);
		else if (_swimdown)
			animator.SetBool("Direction", true);
		else
			animator.SetBool("Direction", rigidbody.velocity.y < 0.0f);
	}
}

