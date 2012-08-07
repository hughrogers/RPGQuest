using UnityEngine;
using System.Collections;

public class Knight : MonoBehaviour 
{
	private float _blinkTimeLeft;
	
	/// <summary>
	/// Reference to the knight Smooth Moves bone animation
	/// </summary>
	public SmoothMoves.BoneAnimation knight;
	
	/// <summary>
	/// Reference to the audio clip to play when the sword swishes
	/// </summary>
	public AudioSource swishSound;
	
	/// <summary>
	/// Reference to the audio clip to play when the sword hits something
	/// </summary>
	public AudioSource hitSound;
	
	/// <summary>
	/// The min and max blink times govern how often the knight blinks
	/// </summary>
	public float minBlinkTime;
	public float maxBlinkTime;
	
	/// <summary>
	/// The old material is the material to swap from, 
	/// the new material is the material to swap to
	/// </summary>
	public Material oldMaterial;
	public Material newMaterial;
	
	/// <summary>
	/// Speed that the knight moves
	/// </summary>
	public float speed;
	
	/// <summary>
	/// Reference to the transform of the knight shadow.
	/// </summary>
	public Transform knightShadow;
	
	/// <summary>
	/// Reference to the Smooth Moves sparks animation to play
	/// when the knight hits something
	/// </summary>
	public SmoothMoves.BoneAnimation sparks;
	
	// Use this for initialization
	void Start () 
	{
		// switch our knight to a new material
		knight.SwapMaterial(oldMaterial, newMaterial);
		
		// register a delegate for when the collisions occur
		knight.RegisterColliderTriggerDelegate(ColliderTrigger);
		
		// register a delegate for when user triggers occur
		knight.RegisterUserTriggerDelegate(UserTrigger);
		
		// swap out the sword texture on the "Weapon" bone with the axe texture across all animations.
		//
		// Note: We could also call SwapAnimationBoneTexture on each of our animations (or only the ones
		// that we want to change), but this is much quicker since we want the change to 
		// apply to all animations.
		//
		// Also Note: we are swapping one weapon with another from the same atlas in this example (Weapons),
		// but you can swap textures across different atlases if you wish. The only requirement for this
		// is that both atlases are referenced in your bone animation at design time so Smooth Moves can look
		// up the atlas by name.
		//
		// Caution: Atlas and texture names are case sensitive. Texture names are not referenced with their
		// file extensions.
		knight.SwapBoneTexture("Weapon", "Weapons", "sword", "Weapons", "axe");
		
		// swap out the sword_swish texture across all bones and animations with the axe texture
		//
		// Note: we could have used the same method above (SwapBoneTexture), but this demonstrates
		// that you can swap a texture across every bone and animation without having to 
		// iterate through them all. This is a bit redundant in the case of the sword_swish
		// texture since it only occurs on the Weapon bone anyway, but is useful for illustration.
		knight.SwapTexture("Weapons", "sword_swish", "Weapons", "axe");
		
		// move the shadow to be a child of the knight
		//
		// Note: we could have put this as a sibling to the knight under a common parent
		// object. This just shows you how you can move children to a bone animation
		// at runtime. Placing children under the bone animation should not be done at
		// design time because these objects will be wiped out when the animation is
		// re-created.
		knightShadow.parent = knight.GetBoneTransform("Root");
		knightShadow.localPosition = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		
		//Enhanced Animation and Movement by Kyle LeMaster, Enigma Factory Games
        //If either movement key is released, crossfade to Stand animation
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            knight.CrossFade("Stand");
        }
       
        if (Input.GetKey(KeyCode.RightArrow))
        {
            //If the opposite direction is held, crossfade Stand and exit
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                knight.CrossFade("Stand");
                return;
            }
            //Check if Walk is playing, if it is not, play it
            if (!knight.IsPlaying("Walk"))
            {
                knight["Walk"].speed = 1.0f;
                knight.CrossFade("Walk");
            }
            //Move the player character
            knight.mLocalTransform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }
 
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //If the opposite direction is held, crossfade Stand and exit
            if (Input.GetKey(KeyCode.RightArrow))
            {
                knight.CrossFade("Stand");
                return;
            }
            //Check if Walk is playing, if it is not, play it
            if (!knight.IsPlaying("Walk"))
            {
                knight["Walk"].speed = -1.0f;
                knight.CrossFade("Walk");
            }
            //Move the player character
            knight.mLocalTransform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        }
	
		// Attack
		if (Input.GetKeyDown(KeyCode.A))
		{
			knight.CrossFade("Attack");
		}

		// switch to the axe
		if (Input.GetKeyDown(KeyCode.X))
		{
			// note that we reference the original texture "sword" and "sword_swish". 
			// This way we don't have to keep track of the current texture, only 
			// what is was initially.
			knight.SwapBoneTexture("Weapon", "Weapons", "sword", "Weapons", "axe");
			knight.SwapBoneTexture("Weapon", "Weapons", "sword_swish", "Weapons", "axe");
		}
		
		// switch to the mace
		if (Input.GetKeyDown(KeyCode.M))
		{
			// note that we reference the original texture "sword" and "sword_swish". 
			// This way we don't have to keep track of the current texture, only 
			// what is was initially.
			knight.SwapBoneTexture("Weapon", "Weapons", "sword", "Weapons", "mace");
			knight.SwapBoneTexture("Weapon", "Weapons", "sword_swish", "Weapons", "mace");
		}

		// switch to the sword
		if (Input.GetKeyDown(KeyCode.S))
		{
			// the sword was the original weapon set up in the animation,
			// so we just restore to the original instead of replacing
			knight.RestoreBoneTexture("Weapon");
		}
		
		// Hide and Show the weapon
		if (Input.GetKeyDown(KeyCode.H))
		{
			// this shows how to get the current hidden state of a bone with IsBoneHidden
			knight.HideBone("Weapon", !knight.IsBoneHidden("Weapon"));
		}
		
		// make the knight blink
		_blinkTimeLeft -= Time.deltaTime;
		if (_blinkTimeLeft <= 0)
		{
			knight.Play("Blink");
			
			_blinkTimeLeft = UnityEngine.Random.Range(minBlinkTime, maxBlinkTime);
		}
	}
	
	/// <summary>
	/// This function will be called whenever a collider is processed by the knight. We assign this 
	/// function in the Awake of this example.
	/// </summary>
	/// <param name='triggerEvent'>
	/// The event to process
	/// </param>
	public void ColliderTrigger(SmoothMoves.ColliderTriggerEvent triggerEvent)
	{
		// filter down the event to only capture when something hits the Weapon bone on the ENTER event type
		if (triggerEvent.boneName == "Weapon" && triggerEvent.triggerType == SmoothMoves.ColliderTriggerEvent.TRIGGER_TYPE.Enter)
		{
			// play the hit sound since this was a collision
			hitSound.Play();
			
			// move sparks to the nearest location of the hit point and then play the sparks animation
			sparks.mLocalTransform.position = triggerEvent.otherColliderClosestPointToBone;
			sparks.Play("Hit");
		}
	}
	
	/// <summary>
	/// This function will be called whenever a user trigger is encountered in the animation. We assign this
	/// function in the Awake of this example.
	/// </summary>
	/// <param name='triggerEvent'>
	/// The event to process
	/// </param>
	public void UserTrigger(SmoothMoves.UserTriggerEvent triggerEvent)
	{
		// filter down the events to only look at user triggers on the Weapon bone
		if (triggerEvent.boneName == "Weapon")
		{
			// play the swish sound for this event
			swishSound.Play();
		}
	}
}
