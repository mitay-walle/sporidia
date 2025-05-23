namespace GameJam.Plugins.UI.Windows
{
	public interface IPanel
	{
		bool IsVisible { get; }
		void Show();
		void Hide();
		void Toggle();
		void Toggle(bool value);
	}
}