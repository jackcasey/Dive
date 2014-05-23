using UnityEngine;
using System.Collections;

public class Fader : MonoBehaviour {
	public float fade; //current progress
	public float speed = 1.0f;
	public Texture2D texture;	
	public GUIStyle style;
	static Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);

	private bool fadeIn = false;
	private bool fadeOut = false;

	void OnGUI() {
		//draw the background:
		GUI.depth = 1;
		GUI.BeginGroup(screenRect);
		GUI.color = new Color(1,1,1,fade);
		GUI.DrawTexture(screenRect, texture, ScaleMode.StretchToFill);
		GUI.EndGroup();
	}

	public void FadeIn()
	{
		fadeIn = true;
		fadeOut = false;
	}

	public void FadeOut()
	{
		fadeIn = false;
		fadeOut = true;
	}


	void Update() {
		if (fadeIn)
		{
			fade = Mathf.Lerp(fade, 0.0f, Time.deltaTime * speed);
			if (fade < 0.001)
			{
				fade = 0.0f;
				fadeIn = false;
			}
		}
		else if (fadeOut)
		{
			fade = Mathf.Lerp(fade, 1.0f, Time.deltaTime * speed);
			if (fade > 0.99)
			{
				fade = 1.0f;
				fadeOut = false;
			}
		}
	}
}