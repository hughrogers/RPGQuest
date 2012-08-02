// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Finds a Game Object by Name and/or Tag.")]
	public class FindGameObject : FsmStateAction
	{
		public FsmString objectName;
		[UIHint(UIHint.Tag)]
		public FsmString withTag;
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmGameObject store;

		public override void Reset()
		{
			objectName = "";
			withTag = "Untagged";
			store = null;
		}

		public override void OnEnter()
		{
			Finish();

			if (withTag.Value != "Untagged")
			{
				if (!string.IsNullOrEmpty(objectName.Value))
				{
					GameObject[] possibleGameObjects = GameObject.FindGameObjectsWithTag(withTag.Value);

					foreach (GameObject go in possibleGameObjects)
					{
						if (go.name == objectName.Value)
						{
							store.Value = go;
							return;
						}
					}

					store.Value = null;
					return;
				}

				store.Value = GameObject.FindGameObjectWithTag(withTag.Value);
				return;
			}

			store.Value = GameObject.Find(objectName.Value);
		}

		public override string ErrorCheck()
		{
			if (string.IsNullOrEmpty(objectName.Value) && string.IsNullOrEmpty(withTag.Value))
				return "Specify Name, Tag, or both.";
			return null;
		}

	}
}