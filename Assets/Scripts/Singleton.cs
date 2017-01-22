﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour {


	private static Singleton instance = null;
	public static Singleton Instance {
		get { return instance; }
	}
	void Awake() {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}
	// any other methods you need

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}