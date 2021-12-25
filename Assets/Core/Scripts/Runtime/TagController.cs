using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace Core
{
    public class TagController : MonoBehaviour, ITaggable
    {
        private Dictionary<string, int> m_TagCountMap = new Dictionary<string, int>();
        public event Action<string> tagAdded;
        public event Action<string> tagRemoved;
        public ReadOnlyCollection<string> tags => m_TagCountMap.Keys.ToList().AsReadOnly();

        public bool Contains(string tag)
        {
            return m_TagCountMap.ContainsKey(tag);
        }

        public bool ContainsAny(IEnumerable<string> tags)
        {
            return tags.Any(m_TagCountMap.ContainsKey);
        }

        public bool ContainsAll(IEnumerable<string> tags)
        {
            return tags.All(m_TagCountMap.ContainsKey);
        }

        public bool SatisfiesRequirements(IEnumerable<string> mustBePresentTags, IEnumerable<string> mustBeAbsentTags)
        {
            return ContainsAll(mustBePresentTags) && !ContainsAny(mustBeAbsentTags);
        }

        public void AddTag(string tag)
        {
            if (m_TagCountMap.ContainsKey(tag))
            {
                m_TagCountMap[tag]++;
            }
            else
            {
                m_TagCountMap.Add(tag, 1);
                tagAdded?.Invoke(tag);
            }
        }

        public void RemoveTag(string tag)
        {
            if (m_TagCountMap.ContainsKey(tag))
            {
                m_TagCountMap[tag]--;
                if (m_TagCountMap[tag] == 0)
                {
                    m_TagCountMap.Remove(tag);
                    tagRemoved?.Invoke(tag);
                }
            }
            else
            {
                Debug.LogWarning("Attempting to remove a tag that does not exist!");
            }
        }
    }
}