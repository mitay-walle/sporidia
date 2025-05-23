namespace GameJam.Plugins.Disablers
{
	public class DisablerListComponent : DisablerListBehaviour
	{
		protected override void OnChanged()
		{
			enabled = DisablerList.IsEnabled;
		}
	}
}