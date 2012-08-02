// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Finds the Child of a Game Object by Name and/or Tag. Use this to find attach points etc. NOTE: This action will search recursively through all children and return the first match; To find a specific child use Find Child.")]
	public class GetChild : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;
		
		public FsmString childName;
		
		[UIHint(UIHint.Tag)]
		public FsmString withTag;
		
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmGameObject storeResult;

		public override void Reset()
		{
			gameObject = null;
			childName = "";
			withTag = "Untagged";
			storeResult = null;
		}

		public override void OnEnter()
		{
			storeResult.Value = DoGetChildByName(Fsm.GetOwnerDefaultTarget(gameObject), childName.Value, withTag.Value);

			Finish();
		}

		static GameObject DoGetChildByName(GameObject root, string name, string tag)
		{
			if (root == null)
			{
				return null;
			}

			foreach (Transform child in root.transform)
			{
				if (!string.IsNullOrEmpty(name))
				{
					if (child.name == name)
					{
						if (!string.IsNullOrEmpty(tag))
						{
							if (child.tag.Equals(tag))
							{
								return child.gameObject;
							}
						}
						else
						{
							return child.gameObject;
						}
					}
				}
				else if (!string.IsNullOrEmpty((tag)))
				{
					if (child.tag == tag)
					{
						return child.gameObject;
					}
				}

				// search recursively

				var returnObject = DoGetChildByName(child.gameObject, name, tag);
				if(returnObject != null)
				{
					return returnObject;
				}
			}

			return null;
		}

		public override string ErrorCheck()
		{
			if (string.IsNullOrEmpty(childName.Value) && string.IsNullOrEmpty(withTag.Value))
			{
				return "Specify Child Name, Tag, or both.";
			}
			return null;
		}

	}
}