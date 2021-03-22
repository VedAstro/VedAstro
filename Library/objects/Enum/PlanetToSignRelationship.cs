namespace Genso.Astrology.Library
{
    public enum PlanetToSignRelationship
    {
        /// <summary>
        /// Swavarga - own varga
        /// </summary>
        Swavarga = 1,

        /// <summary>
        /// Adhi Mitravarga - Intimate friend varga
        /// </summary>
        AdhiMitravarga = 2,

        /// <summary>
        /// Mitravarga - friendly varga
        /// </summary>
        Mitravarga = 3,

        /// <summary>
        /// Samavarga - neutral's varga
        /// </summary>
        Samavarga = 4,

        /// <summary>
        /// Satruvarga - enemy's varga
        /// </summary>
        Satruvarga = 5,

        /// <summary>
        /// Adhi Satruvarga - Bitter enemy varga
        /// </summary>
        AdhiSatruvarga = 6,

        /// <summary>
        /// Special position
        /// </summary>
        Moolatrikona = 7
    }
}