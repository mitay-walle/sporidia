using System;
using System.Collections.Generic;
using GameJam.Player;
using GameJam.Plugins.Combat.Damage;
using GameJam.Plugins.Timing;
using GameJam.Scripts.EnemySystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameJam.Scripts.ResourcesSystem;
using GameJam.Upgrades;
using GameJam.Scripts.Weapons;
using Unity.AI.Navigation;

namespace GameJam.Plugins.GameLoop
{
	public enum GameplayState
	{
		Initial,
		Day,
		Twighlight,
		Night,
		Lose,
	}

	public class GameplayEvents : MonoBehaviour
	{
		private const string PATH_TO_UPGRADES = "PseudoScriptableObjects";

		[SerializeField] private GameObject _day;
		[SerializeField] private GameObject _twilights;
		[SerializeField] private GameObject _night;

		public Action<DamageInfo> OnDamageAny;
		public Action<DamageInfo> OnDeathAny;
		public Action<GameplayState> OnStateChanged;
		public Action<eResourceType, int> OnResourceCountChanged;
		public Action<eResourceType, int> OnResourceDebugAdd;

		public Action OnLose;
		public Action OnWin;

		public Timer GetDayTime => _dayTimer;
		public Timer GetNightTime => _nightTimer;
		public Timer GetTwilightTime => _twilightTimerTEMP;

		[SerializeField] private Timer _dayTimer = new(20);
		[SerializeField] private Timer _nightTimer = new(60);
		[SerializeField] private Timer _twilightTimerTEMP = new(10);

		[ShowInInspector, ReadOnly] public GameplayState State { get; private set; }
		[field: SerializeField] public PlayerAsist PlayerAsist { get; private set; }

		private Dictionary<GameplayState, IGameStateBlender> _toStateBlenders = new Dictionary<GameplayState, IGameStateBlender>();
		private GatherableResourceSpawnController _resourceSpawnController;
		private EnemySpawnerController _enemySpawnController;

		private void Awake()
		{
			FindAnyObjectByType<NavMeshSurface>().BuildNavMesh();
			_resourceSpawnController =
				FindAnyObjectByType<GatherableResourceSpawnController>(FindObjectsInactive.Include);
			_enemySpawnController =
				FindAnyObjectByType<EnemySpawnerController>(FindObjectsInactive.Include);	
		
			_enemySpawnController.Init();
			
			_toStateBlenders.Add(GameplayState.Day, _day.GetComponent<IGameStateBlender>());
			_toStateBlenders.Add(GameplayState.Twighlight, _twilights.GetComponent<IGameStateBlender>());
			_toStateBlenders.Add(GameplayState.Night, _night.GetComponent<IGameStateBlender>());

			foreach (var blender in _toStateBlenders)
			{
				blender.Value.OnBlendDone += SetState;
				blender.Value.Init(this);
			}

			SetState(GameplayState.Initial);

			foreach (UpgradableParams upgradeable in Resources.LoadAll<UpgradableParams>(PATH_TO_UPGRADES))
			{
				upgradeable.ResetIndex();
			}

            foreach (PurchasableItemParams purchasable in Resources.LoadAll<PurchasableItemParams>(PATH_TO_UPGRADES))
            {
				if (purchasable.Type == eUpgradeItemType.RocketLauncher || purchasable.Type == eUpgradeItemType.Tesla)
				{
					purchasable.IsPurchased = false;
				}
			}
		}

		private void Update()
		{
			switch (State)
			{
				case GameplayState.Day:
					{
						UpdateDay();
						break;
					}

				case GameplayState.Twighlight:
					{
						//UpdateTwilight();
						break;
					}

				case GameplayState.Night:
					{
						UpdateNight();
						break;
					}

				case GameplayState.Initial:
				case GameplayState.Lose:
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void UpdateDay()
		{
			if (_dayTimer.IsReady())
			{
				_toStateBlenders[GameplayState.Day].StartBlend(GameplayState.Day, GameplayState.Twighlight);
			}
		}

		private void UpdateTwilight()
		{
			if (_twilightTimerTEMP.IsReady())
			{
				_toStateBlenders[GameplayState.Twighlight].StartBlend(GameplayState.Twighlight, GameplayState.Night);
			}
		}

		private void UpdateNight()
		{
			if (_nightTimer.IsReady())
			{
				_toStateBlenders[GameplayState.Night].StartBlend(GameplayState.Night, GameplayState.Day);
			}
		}

		public void OnUpgradesFinished()
		{
			_toStateBlenders[GameplayState.Twighlight].StartBlend(GameplayState.Twighlight, GameplayState.Night);
		}

		[Button] private void Night() => SetState(GameplayState.Night);
		[Button] private void Twighlight() => SetState(GameplayState.Twighlight);
		
		[PropertySpace, Button] private void BecomeRich() => OnResourceDebugAdd?.Invoke(eResourceType.parts, 2000 ); 
		private void SetState(GameplayState state)
		{
			State = state;
			switch (state)
			{
				case GameplayState.Day:
				{
					_dayTimer.Restart();
					_day.SetActive(true);
					_twilights.SetActive(false);
					_night.SetActive(false);
					_resourceSpawnController.BeginNextDay();
					break;
				}

				case GameplayState.Twighlight:
				{
					_day.SetActive(false);
					_twilights.SetActive(true);
					_night.SetActive(false);
					break;
				}

				case GameplayState.Night:
				{
					_nightTimer.Restart();
					_day.SetActive(false);
					_twilights.SetActive(false);
					_night.SetActive(true);


						
						WeaponBase[] weapons = PlayerAsist.NightHouse.GetComponentsInChildren<WeaponBase>(true);

                        for (int i = 0; i < weapons.Length; i++)
                        {
							weapons[i].ActualizeUnlock();
						}
						GetComponents<WeaponBase>();

					PlayerAsist.NightHouse.GetComponent<PlayerHouse>().OnNightStart();
						if (!_enemySpawnController.CheckWin())
							_enemySpawnController.BeginNextDay();
						else
						{
							Debug.Log($"GameplayEvents.SetStateNight It's a win");
							OnWin?.Invoke();
						}
					break;
				}

				case GameplayState.Initial:
				{
					_day.SetActive(false);
					_twilights.SetActive(false);
					_night.SetActive(false);
					break;
				}

				case GameplayState.Lose:
				{
					_day.SetActive(false);
					_twilights.SetActive(false);
					_night.SetActive(false);
					break;
				}

				default:
					throw new ArgumentOutOfRangeException(nameof(state), state, null);
			}

			OnStateChanged?.Invoke(state);
		}

		public void FirstDayInit()
		{
			SetState(GameplayState.Day);
		}

		public void Win()
		{
			Debug.Log("win");
			OnWin?.Invoke();
		}

		private void Lose()
		{
			Debug.Log("lose");
			SetState(GameplayState.Lose);
			OnLose?.Invoke();
		}

		public void Restart()
		{
			Debug.Log("restart");
			string currentSceneName = SceneManager.GetActiveScene().name;
			SceneManager.LoadScene(currentSceneName);
		}

		public void OnDeathAnyInvoke(DamageInfo info)
		{
			OnDeathAny?.Invoke(info);
			if (info.Target.GetComponent<PlayerHouse>())
			{
				OnPlayerDeath(info);
			}
		}

		public void OnDamageAnyInvoke(DamageInfo info)
		{
			OnDamageAny?.Invoke(info);
		}

		private void OnPlayerDeath(DamageInfo info)
		{
			Lose();
		}

		public void PlayNext()
		{
			int index = SceneManager.GetActiveScene().buildIndex + 1;
			Debug.Log($"play next {index}");
			SceneManager.LoadScene(index);
		}
	}
}