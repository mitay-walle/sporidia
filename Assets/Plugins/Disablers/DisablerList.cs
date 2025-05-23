using System;
using TriInspector;
using UnityEngine;
using UnityEngine.Rendering;

namespace GameJam.Plugins.Disablers
{
	[Serializable, InlineProperty]
	public class DisablerList
	{
		[ShowInInspector, HideLabel] public bool IsEnabled => Disablers.Count == 0;
		[ShowInInspector, HideLabel] public ObservableList<object> Disablers = new();

		public event Action<bool, object> OnChanged;

		public DisablerList() { }

		public DisablerList(Action<bool> action)
		{
			Disablers.ItemAdded += Changed;
			Disablers.ItemRemoved += Changed;

			void Changed(ObservableList<object> sender, ListChangedEventArgs<object> listChangedEventArgs)
			{
				action?.Invoke(IsEnabled);
				OnChanged?.Invoke(IsEnabled, listChangedEventArgs.item);
			}
		}

		public DisablerList(Behaviour component)
		{
			Disablers.ItemAdded += Changed;
			Disablers.ItemRemoved += Changed;

			void Changed(ObservableList<object> sender, ListChangedEventArgs<object> listChangedEventArgs)
			{
				component.enabled = IsEnabled;
				OnChanged?.Invoke(IsEnabled, listChangedEventArgs.item);
			}
		}

		public void Add(object source)
		{
			if (!Disablers.Contains(source)) Disablers.Add(source);
		}

		public void Remove(object source)
		{
			if (Disablers.Contains(source)) Disablers.Remove(source);
		}

		public void Toggle(object source, bool value)
		{
			bool contains = Disablers.Contains(source);
			if (contains == value) return;

			if (value)
			{
				Disablers.Add(source);
			}
			else
			{
				Disablers.Remove(source);
			}
		}

		[Button]
		private void Toggle()
		{
			Toggle(this, !Disablers.Contains(this));
		}

	}
}