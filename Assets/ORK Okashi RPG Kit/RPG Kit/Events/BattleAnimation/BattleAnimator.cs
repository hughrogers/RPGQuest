
using UnityEngine;
using System.Collections;

public class BattleAnimator : MonoBehaviour
{
	private string idleAnimation = "";
	private string runAnimation = "";
	
	public BattleAnimator()
	{
		
	}

	public void StartAnimation(string idle, string run, bool playIdle)
	{
		if(this.animation)
		{
			this.idleAnimation = idle;
			this.runAnimation = run;
			
			if("" != this.runAnimation) this.animation[this.runAnimation].layer = -1;
			if("" != this.idleAnimation) this.animation[this.idleAnimation].layer = -2;
			
			if(playIdle)
			{
				this.animation.Stop();
				if("" != this.idleAnimation) this.animation.Play(this.idleAnimation);
			}
		}
	}

	void Update ()
	{
		if(this.animation && "" != this.runAnimation)
		{
			CharacterController controller = (CharacterController)GetComponent(typeof(CharacterController));
			float currentSpeed = new Vector3(controller.velocity.x, 0, controller.velocity.z).magnitude;
			
			if(currentSpeed > 0.1)
			{
				animation.CrossFade(this.runAnimation, 0.1f);
			}
			// Fade out walk and run
			else
			{
				this.StartCoroutine(this.FadeOut(this.runAnimation, 0.1f));
			}
		}
	}
	
	private IEnumerator FadeOut(string name, float time)
	{
		this.animation.Blend(name, 0, time);
		if(time > 0) yield return new WaitForSeconds(time);
		this.animation.Stop(name);
	}
}