using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Design.Tags
{
	[Serializable]
	public class TagContainer : IEnumerable<Tag>
	{
		[SerializeField] private List<Tag> Tags = new();
		public int Count => Tags.Count;

		public bool Contains(Tag tag) => Tags.Contains(tag);
		public Tag GetAnySame(TagContainer other) => Tags.Find(other.Contains);
		public Tag GetAnySame(IEnumerable<Tag> other)
		{
			foreach (Tag tag in other)
			{
				if (Tags.Contains(tag)) return tag;
			}

			return null;
		}
		public bool ContainsAny(IEnumerable<Tag> other)
		{
			bool result = false;

			foreach (Tag tag in other)
			{
				if (Tags.Contains(tag)) result = true;
			}

			return result;
		}
		public bool Contains(TagContainer other) => !other.Tags.Exists(tag => !Tags.Contains(tag));
		public bool ContainsAny(TagContainer other) => Tags.Exists(other.Contains);

		public bool Contains<T>() where T : Tag => Tags.Exists(tag => tag is T);

		public void ReplaceTag<T>(T value) where T : Tag
		{
			if (Tags == null)
			{
				Tags = new List<Tag>();
			}

			var index = Tags.FindIndex(t => t is T);
			if (index >= 0) Tags.RemoveAt(index);

			Tags.Add(value);
		}

		public void Add(Tag tag) => Tags.Add(tag);
		public void AddRange(IEnumerable<Tag> tags, bool allowDublicates = false)
		{
			if (tags == null) return;

			foreach (Tag tag in tags)
			{
				if (!Tags.Contains(tag) || allowDublicates)
				{
					Tags.Add(tag);
				}
			}
		}
		public void AddRange(TagContainer tags, bool allowDublicates = false)
		{
			if (tags == null) return;

			tags.Tags.ForEach(tag =>
			{
				if (!Tags.Contains(tag) || allowDublicates)
				{
					Tags.Add(tag);
				}
			});
		}

		public void RemoveAll(TagContainer tags)
		{
			if (tags == null) return;

			foreach (Tag tag in tags.Tags)
			{
				if (Tags.Contains(tag))
				{
					Tags.Remove(tag);
				}
			}
		}
		public void Remove(Tag tag)
		{
			if (tag == null) return;

			if (Tags.Contains(tag)) Tags.Remove(tag);
		}

		public IEnumerator<Tag> GetEnumerator() => Tags.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		public Tag this[int index] => Tags[index];

	}
}