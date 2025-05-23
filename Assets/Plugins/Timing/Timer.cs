using System;
using TriInspector;
using UnityEngine;

namespace GameJam.Plugins.Timing
{
	[Serializable, InlineProperty]
	public struct Timer : IComparable<float>
	{
		[LabelText(" "), LabelWidth(8)] public float duration;
        private float endTime;

		[ShowInInspector, HideLabel] private float DrawElapsedTime => GetTimeElapsed();

		public Timer(float duration)
		{
			this.duration = duration;
			endTime = 0;
		}
        public void SetDuration(float newDuration)
        {
            this.duration = newDuration;
            Restart(newDuration);
        }

        public bool CheckAndRestart()
		{
			bool isComplete = IsReady();
			if (isComplete)
				Restart();
			return isComplete;
		}

        public void Cancel()
        {
	        endTime = float.MaxValue;
        }
        
		public bool CheckAndCancel()
		{
			bool isComplete = IsReady();
			if (isComplete)
				Cancel();
			return isComplete;
		}

		public bool IsReady() => Time.time >= endTime;
		public void Restart() => endTime = Time.time + duration;

		public void Restart(float time)
		{
			this.duration = time;
			endTime = Time.time + time;
		}

		public void Reset() => endTime = Time.time;
		public void Reset(float time) => endTime = Time.time + time;
		public float GetTimeLeft() => Mathf.Max(0, endTime - Time.time);
		public float GetTimeElapsed() => Mathf.Max(0, duration - GetTimeLeft());
		public float GetStartTime() => endTime - duration;
		public void Complete() => endTime = float.MinValue;

		public static explicit operator float(Timer source) => source.duration;

#if UNITY_EDITOR

		//[OnInspectorGUI]
		private void OnInspectorGUI()
		{
			if (!Application.isPlaying) return;
			//GUIHelper.RequestRepaint();
			//EditorGUILayout.Slider(GetTimeElapsed(), 0, duration);
		}
#endif
		public int CompareTo(float other)
		{
			return duration.CompareTo(other);
		}
	}
}