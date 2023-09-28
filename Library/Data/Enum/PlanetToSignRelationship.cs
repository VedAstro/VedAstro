namespace VedAstro.Library
{
    public enum PlanetToSignRelationship
    {

        Empty = 0,

        /// <summary>
        /// Swavarga - own varga
        /// </summary>
        OwnVarga = 1,

        /// <summary>
        /// Adhi Mitravarga - Intimate friend varga
        /// </summary>
        BestFriendVarga = 2,

        /// <summary>
        /// Mitravarga - friendly varga
        /// </summary>
        FriendVarga = 3,

        /// <summary>
        /// Sama Varga - neutral's varga
        /// </summary>
        NeutralVarga = 4,

        /// <summary>
        /// Satruvarga - enemy's varga
        /// </summary>
        EnemyVarga = 5,

        /// <summary>
        /// Adhi Satruvarga - Bitter enemy varga
        /// </summary>
        BitterEnemyVarga = 6,

        /// <summary>
        /// Special position Note: use special calculator
        /// </summary>
        Moolatrikona = 7,
    }
}