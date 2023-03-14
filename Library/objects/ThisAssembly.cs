using System;

namespace Genso.Astrology.Library
{
    /// <summary>
    /// This file gets set with correct values by publisher before build
    /// so version number gets nicely embedded into code just before deployment
    /// </summary>
    public record ThisAssembly
    {
        //HARD CODED DEFAULTS
        public const string CommitHash = "1010101010"; 
        public const string CommitNumber = "100"; 
        public const string BranchName = "stable";
        public static readonly string Version = $"{CommitHash}-{CommitNumber}-{BranchName}";
    }
}
