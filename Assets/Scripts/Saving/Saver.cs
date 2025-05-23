using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Saving
{
	[CreateAssetMenu]
	public class Saver : ScriptableObject
	{
		string fileExtension => $"sv";
		string fileName => $"Save";
		string fileNameSettings => $"Settings";
		string path => $"{Application.persistentDataPath}/{fileName}.{fileExtension}";
		string pathSettings => $"{Application.persistentDataPath}/{fileNameSettings}.{fileExtension}";

		[SerializeField] InputActionAsset input;
		[SerializeField] Save save;
		[SerializeField] Save DefaultSave;
		[SerializeField] SaveSettings settings;
		[SerializeField] SaveSettings defaultSettings;

		public Save GetSave() => save;
		public SaveSettings GetSettings() => settings;

		[Button]
		public void SaveSettings()
		{
			if (input)
			{
				settings.gamepadOverrides = input.SaveBindingOverridesAsJson();
			}

			string data = JsonUtility.ToJson(!File.Exists(pathSettings) ? defaultSettings : settings);
			File.WriteAllText(pathSettings, data);
		}
		[Button]
		public void Save()
		{
			string data = JsonUtility.ToJson(!File.Exists(path) ? DefaultSave : save);
			File.WriteAllText(path, data);
		}

		[Button]
		public void Load()
		{
			if (!File.Exists(path)) Save();
			if (!File.Exists(pathSettings)) SaveSettings();
			string dataSettings = File.ReadAllText(pathSettings);
			JsonUtility.FromJsonOverwrite(dataSettings, settings);

			LoadBindingOverridesFromJsonInternal(input,settings.gamepadOverrides);
			
			string data = File.ReadAllText(path);
			JsonUtility.FromJsonOverwrite(data, save);
		}


		[Button]
		public void Open() => Application.OpenURL($"file://{path}");

		[Button]
		public void Delete()
		{
			input.RemoveAllBindingOverrides();
			if (File.Exists(path)) File.Delete(path);
		}
		
		private static void LoadBindingOverridesFromJsonInternal(IInputActionCollection2 actions, string json)
		{
			if (string.IsNullOrEmpty(json))
				return;

			var overrides = JsonUtility.FromJson<BindingOverrideListJson>(json);
			foreach (var entry in overrides.bindings)
			{
				// Try to find the binding by ID.
				if (!string.IsNullOrEmpty(entry.id))
				{
					var guid = Guid.Parse(entry.id);
					var bindingIndex = actions.FindBinding(new InputBinding { id = guid }, out var action);
					if (bindingIndex == -1) continue;
					
					action.ApplyBindingOverride(bindingIndex, new InputBinding
					{
						overridePath = entry.path,
						overrideInteractions = entry.interactions,
						overrideProcessors = entry.processors,
					});
				}
			}
		}
		
		[Serializable]
		internal struct BindingOverrideListJson
		{
			public List<BindingOverrideJson> bindings;
		}

		[Serializable]
		internal struct BindingOverrideJson
		{
			// We save both the "map/action" path of the action as well as the binding ID.
			// This gives us two avenues into finding our target binding to apply the override
			// to.
			public string action;
			public string id;
			public string path;
			public string interactions;
			public string processors;

			public static BindingOverrideJson FromBinding(InputBinding binding)
			{
				return new BindingOverrideJson
				{
					action = binding.action,
					//id = binding.m_Id,
					path = binding.overridePath,
					interactions = binding.overrideInteractions,
					processors = binding.overrideProcessors,
				};
			}
		}
	}
}