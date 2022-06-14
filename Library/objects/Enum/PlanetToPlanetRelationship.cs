namespace Genso.Astrology.Library
{

    /// <summary>
    /// Planetary Relations - By friendship
    /// we mean that the rays of the one planet will be
    /// intensified by those of the other, declared as his
    /// friend, while the same rays will be counteracted
    /// by a planet who is an enemy.
    /// </summary>
    public enum PlanetToPlanetRelationship
    {
        /// <summary>
        /// Same planet
        /// </summary>
        Own,

        /// <summary>
        /// Intimate friend
        /// </summary>
        AdhiMitra,

        /// <summary>
        /// Friend
        /// </summary>
        Mitra,

        /// <summary>
        /// Bitter enemy
        /// </summary>
        AdhiSatru,

        /// <summary>
        /// Enemy
        /// </summary>
        Satru,

        /// <summary>
        /// Neutral
        /// </summary>
        Sama
    }
}