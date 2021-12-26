using System;

namespace LevelSystem
{
    public interface ILevelable
    {
        int level { get; }
        event Action levelChanged;
        event Action currentExperienceChanged;
        int currentExperience { get; set; }
        int requiredExperience { get; }
        bool isInitialized { get; }
        event Action initialized;
        event Action willUninitialize;
        event Action loaded;
    }
}