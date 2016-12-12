using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public AudioClip ShieldHit;
	public AudioClip SlashAttack;
	public AudioClip ThrustAttack;
	public AudioClip a4;

	/*AudioClip ShieldHit2;
	AudioClip SlashAttack2;
	AudioClip ThrustAttack2;*/

	AudioSource audio;
	// Use this for initialization
	void Start () {
		audio=gameObject.GetComponent<AudioSource>();
		/*ShieldHit2=ShieldHit;
		SlashAttack2=SlashAttack;
		ThrustAttack2=ThrustAttack;*/
	}
	
	// Update is called once per frame
	void Update () {
	}
	public void playSlashAttackSound(){
		audio.PlayOneShot(SlashAttack);
	}
	public void playThrustAttackSound(){
		audio.PlayOneShot(ThrustAttack);
	}
	public void playShieldHitSound(){
		audio.PlayOneShot(ShieldHit);
	}
	/*public void play(string nazwa, bool gracz){
		if (gracz){
			switch (nazwa){
				case "ShieldHit": audio.PlayOneShot(ShieldHit); break;
				case "SlashAttack": audio.PlayOneShot(SlashAttack); break;
				case "ThrustAttack": audio.PlayOneShot(ThrustAttack); break;
				//case "ShieldHit": audio.PlayOneShot(ShieldHit); break;
				default: break;
			}
		}
		else{
			switch (nazwa){
			case "ShieldHit": audio.PlayOneShot(ShieldHit2); break;
			case "SlashAttack": audio.PlayOneShot(SlashAttack2); break;
			case "ThrustAttack": audio.PlayOneShot(ThrustAttack2); break;
				//case "ShieldHit": audio.PlayOneShot(ShieldHit); break;
			default: break;
			}
		}
	}*/

}
