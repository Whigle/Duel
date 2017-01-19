using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gif : MonoBehaviour {

	public double framesPerSecond;
	public Sprite[] frames;
	// Use this for initialization
	void Start () {
		framesPerSecond=System.Convert.ToDouble(frames.Length)*(framesPerSecond*0.1);
	}
	// Update is called once per frame
	void Update () {
		int index=System.Convert.ToInt32(Time.time*framesPerSecond);
		if (frames.Length>0){
			index=index%frames.Length;
			//print(index);
			this.gameObject.GetComponentInChildren<Image>().sprite=frames[index];
		}
	}
}
