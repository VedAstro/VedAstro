namespace VedAstro.Library
{
    /// <summary>
    /// connected to swiss eph
    /// </summary>
    public enum Ayanamsa
    {
        /// <summary>
        /// Lahiri: This is one of the most widely used and accepted ayanamsas in Vedic astrology, especially in India. It is based on the position of the star Spica (Chitra) at 0 degrees Libra, as prescribed by the Surya Siddhanta. It is also known as
        /// Chitra Paksha Ayanamsa, and it has been adopted by
        /// the Indian government for official purposes. 
        /// </summary>
        TraditionalLahiri = 1,

        /// <summary>
        /// 
        /// </summary>
        ChitrapakshaLahiri = 25,

        /// <summary>
        /// Krishnamurti: This is another popular ayanamsa in Vedic astrology, especially among the followers of the Krishnamurti Paddhati (KP) system. It is based on the position of the star Ashwini at 0 degrees Aries, as proposed by K.S. Krishnamurti.
        /// It is also known as KP Ayanamsa, and it has been widely
        /// used for horary astrology and stellar astrology
        /// </summary>
        KrishnamurtiKP = 5,

        /// <summary>
        /// This is a well-known ayanamsa in Vedic astrology, especially among the followers of B.V. Raman. It is based on the position of the star Revati at 0 degrees Pisces, as suggested by B.V. Raman.
        /// It is also known as Revati Paksha Ayanamsa,
        /// and it has been used for natal astrology and mundane astrology
        /// </summary>
        Raman = 3,

        /// <summary>
        /// This is a prominent ayanamsa in Western sidereal astrology, especially among the followers of Cyril Fagan and Donald Bradley. It is based on the position of the star Aldebaran at 15 degrees Taurus, as advocated by Cyril Fagan.
        /// It is also known as Aldebaran 15 Tau Ayanamsa,
        /// and it has been used for tropical-sidereal conversion
        /// and zodiacal releasing.
        /// </summary>
        FaganBradley = 0,

        /// <summary>
        /// J2000: This is a standard ayanamsa in modern astronomy,
        /// especially for celestial coordinate systems. It is based on the position of the vernal equinox at 0 degrees Aries at noon on January 1st, 2000 (J2000 epoch).
        /// It is also known as J2000 Equinox Ayanamsa,
        /// and it has been used for astronomical calculations
        /// and observations.
        /// </summary>
        J2000 = 18,

        /// <summary>
        /// Yukteshwar: This is a spiritual ayanamsa in Vedic astrology, especially for
        /// Yogananda's teachings. It is based on the position of the star Dhanishta at 0 degrees Aquarius,
        /// as proposed by Sri Yukteshwar.
        /// It is also known as Dhanishta Paksha Ayanamsa,
        /// and it has been used for esoteric astrology and cosmic cycles. 
        /// </summary>
        Yukteshwar = 7,


        /// <summary>
        /// BELOW IS SPECIAL DIRECT TO SWISS FOR SUPER ADVANCED USERS
        /// </summary>

        [AdvancedOption]
        FAGAN_BRADLEY = 0,
        [AdvancedOption]
        LAHIRI = 1,
        [AdvancedOption]
        DELUCE = 2,
        [AdvancedOption]
        RAMAN = 3,
        [AdvancedOption]
        USHASHASHI = 4,
        [AdvancedOption]
        KRISHNAMURTI = 5,
        [AdvancedOption]
        DJWHAL_KHUL = 6,
        [AdvancedOption]
        YUKTESHWAR = 7,
        [AdvancedOption]
        JN_BHASIN = 8,
        [AdvancedOption]
        BABYL_KUGLER1 = 9,
        [AdvancedOption]
        BABYL_KUGLER2 = 10,
        [AdvancedOption]
        BABYL_KUGLER3 = 11,
        [AdvancedOption]
        BABYL_HUBER = 12,
        [AdvancedOption]
        BABYL_ETPSC = 13,
        [AdvancedOption]
        ALDEBARAN_15TAU = 14,
        [AdvancedOption]
        HIPPARCHOS = 15,
        [AdvancedOption]
        SASSANIAN = 16,
        [AdvancedOption]
        GALCENT_0SAG = 17,
        [AdvancedOption]
        J1900 = 19,
        [AdvancedOption]
        B1950 = 20,
        [AdvancedOption]
        SURYASIDDHANTA = 21,
        [AdvancedOption]
        SURYASIDDHANTA_MSUN = 22,
        [AdvancedOption]
        ARYABHATA = 23,
        [AdvancedOption]
        ARYABHATA_MSUN = 24,
        [AdvancedOption]
        SS_REVATI = 25,
        [AdvancedOption]
        SS_CITRA = 26,
        [AdvancedOption]
        TRUE_CITRA = 27,
        [AdvancedOption]
        TRUE_REVATI = 28,
        [AdvancedOption]
        TRUE_PUSHYA = 29,
        [AdvancedOption]
        GALCENT_RGBRAND = 30,
        [AdvancedOption]
        GALEQU_IAU1958 = 31,
        [AdvancedOption]
        GALEQU_TRUE = 32,
        [AdvancedOption]
        GALEQU_MULA = 33,
        [AdvancedOption]
        GALALIGN_MARDYKS = 34,
        [AdvancedOption]
        TRUE_MULA = 35,
        [AdvancedOption]
        GALCENT_MULA_WILHELM = 36,
        [AdvancedOption]
        ARYABHATA_522 = 37,
        [AdvancedOption]
        BABYL_BRITTON = 38,
        [AdvancedOption]
        TRUE_SHEORAN = 39,
        [AdvancedOption]
        GALCENT_COCHRANE = 40,
        [AdvancedOption]
        GALEQU_FIORENZA = 41,
        [AdvancedOption]
        VALENS_MOON = 42,
        [AdvancedOption]
        LAHIRI_1940 = 43,
        [AdvancedOption]
        LAHIRI_VP285 = 44,
        [AdvancedOption]
        KRISHNAMURTI_VP291 = 45,
        [AdvancedOption]
        LAHIRI_ICRC = 46,
    }
}
