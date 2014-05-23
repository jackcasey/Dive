using UnityEngine;
using System.Collections;

public class FadeAway : MonoBehaviour {

	public float delay = 0.0f;
	public float duration = 0.0f;
	public float fadeSpeed = 0.5f;

	private float alpha = 1.0f;
	public int state = 0; 
	// 0 waiting, 1 fading in, 2 on, 3 fading out

	// Use this for initialization
	void Start () {
		if (delay > 0.0f) 
		{
			alpha = 0.0f;
			state = 0;
		} 
		else
		{
			alpha = 1.0f;
			state = 2;
		}
	}
	
	// Update is called once per frame
	void Update () {

		// waiting to fade in 
		if (state == 0)
		{
			delay -= Time.deltaTime;
			if (delay < 0.0f)
				state = 1;
		}
		// fading in
		if (state == 1)
		{
			alpha = Mathf.Lerp(alpha, 1.0f, Time.deltaTime);
			if (alpha > 0.99f)
				state = 2;
		}

		//waiting to fade out
		if (state == 2)
		{
			duration -= Time.deltaTime;
			if (duration < 0.0f)
				state = 3;
		}

		// fading out
		if (state == 3)
		{
			alpha = Mathf.Lerp(alpha, 0.0f, Time.deltaTime);
			if (alpha < 0.0f)
				Destroy(gameObject);
		}
		guiText.color = new Color(1,1,1,alpha);
	}
}
