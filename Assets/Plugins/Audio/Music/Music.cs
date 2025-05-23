using System;
using System.Collections.Generic;
using System.Linq;
using GameJam.Plugins.Audio.Music;
using GameJam.Plugins.Disablers;
using TriInspector;
using UnityEngine;

namespace GameJam.Plugins
{
	public class Music : DisablerListBehaviour
	{
		private SortedDictionary<eMusic, MusicLayer> _layers = new();

		[ShowInInspector, ReadOnly] private List<MusicComponent> _components = new();
		[ShowInInspector, ReadOnly] private eMusic? _current;

		protected void Start()
		{
			_layers.Clear();
			var found = GetComponentsInChildren<MusicLayer>().ToDictionary(l =>
			{
				if (Enum.TryParse(l.name, out eMusic value))
				{
					return value;
				}

				Debug.LogError($"[ Music ] Awake | MusicLayer object name '{l.name}' in invalid!", l);
				return default;
			});

			foreach (var kvp in found)
			{
				_layers.Add(kvp.Key, kvp.Value);
			}

			ValidateCurrentLayer();
		}

		public void Add(MusicComponent component)
		{
			if (!_components.Contains(component))
			{
				_components.Insert(0, component);
				ValidateCurrentLayer();
			}
		}

		public void Remove(MusicComponent component)
		{
			if (!_components.Contains(component)) return;

			_components.Remove(component);
			ValidateCurrentLayer();
		}

		private void ValidateCurrentLayer()
		{
			eMusic last = _components.Count > 0 ? _components[0].Music : eMusic.Discovery;

			if (Logs) Debug.Log($"[ Music ] ValidateCurrentLayer | current {_current} | new {last}");

			if (_current == null || _current != last)
			{
				_current = last;
				foreach (var kvp in _layers)
				{
					if (kvp.Key == last)
					{
						if (Logs) Debug.Log($"[ Music ] ValidateCurrentLayer | enable {last}");
						kvp.Value.DisablerList.Add(this);
						kvp.Value.DisablerList.Remove(this);
					}
					else
					{
						kvp.Value.DisablerList.Add(this);
					}
				}
			}
		}
	}
}