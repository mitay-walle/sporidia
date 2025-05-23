using System;
using UnityEngine;

namespace GameJam.Plugins.UI.Legacy.ACV.Runtime
{
	public class ACV_GameObjectLine : ACV_Line<GameObject>
	{
		protected override GameObject GetTarget(Transform target) => target.gameObject;
		protected override bool GetState(GameObject target) => target.activeSelf;
		protected override void SetState(GameObject target, bool state) => target.SetActive(state);
	}
}