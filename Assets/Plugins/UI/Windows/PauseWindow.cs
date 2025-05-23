using UnityEngine;

namespace GameJam.Plugins.UI.Windows
{
	public class PauseWindow : WindowWithHotkey
	{
		protected virtual bool InvertPause => false;
		public override void Show()
		{
			Time.timeScale = InvertPause ? 1 : 0;
			base.Show();
		}

		public override void Hide()
		{
			Time.timeScale = InvertPause ? 0 : 1;
			base.Hide();
		}
	}
}