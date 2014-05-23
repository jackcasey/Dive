using UnityEngine;
using System.Collections;

public class ProgressBar : MonoBehaviour {
	public float barDisplay; //current progress
	public Vector2 pos = new Vector2(20,40);
	public Vector2 size = new Vector2(60,20);
	public Texture2D emptyTex;
	public Texture2D fullTex;

	public PlayerController playController;

	public GUIStyle progressStyle;
	
	void OnGUI() {
		//draw the background:
		GUI.depth = 2;
		GUI.BeginGroup(new Rect((Screen.width-size.x)/2.0f, pos.y, size.x, size.y));
		GUI.Box(new Rect(0,0, size.x, size.y), emptyTex, progressStyle);
		
		//draw the filled-in part:
		GUI.BeginGroup(new Rect(0,0, size.x * barDisplay, size.y));
		GUI.Box(new Rect(0,0, size.x, size.y), fullTex, progressStyle);
		GUI.EndGroup();
		GUI.EndGroup();
	}
	
	void Update() {
		//for this example, the bar display is linked to the current time,
		//however you would set this value based on your desired display
		//eg, the loading progress, the player's health, or whatever.
		barDisplay = playController.oxygen / playController.oxygenMax;
		//   barDisplay = MyControlScript.staticHealth;
	}
}