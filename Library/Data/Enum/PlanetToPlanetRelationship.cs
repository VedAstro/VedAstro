namespace VedAstro.Library
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
        Empty = 0,

        /// <summary>
        /// Same Planet
        /// </summary>
        SamePlanet,

        /// <summary>
        /// Best Friend
        /// Adhi Mitra
        /// </summary>
        BestFriend,

        /// <summary>
        /// Friend
        /// Mitra
        /// </summary>
        Friend,

        /// <summary>
        /// Bitter Enemy
        /// Adhi Satru
        /// </summary>
        BitterEnemy,

        /// <summary>
        /// Enemy
        /// Satru
        /// </summary>
        Enemy,

        /// <summary>
        /// Neutral
        /// Sama
        /// </summary>
        Neutral,
    }
}