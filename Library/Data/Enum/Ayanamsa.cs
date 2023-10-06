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
        Lahiri = 1,
        Lahiri1940 = 43,
        LahiriVp285 = 44,
        LahiriIcrc = 46,

        /// <summary>
        /// Krishnamurti: This is another popular ayanamsa in Vedic astrology, especially among the followers of the Krishnamurti Paddhati (KP) system. It is based on the position of the star Ashwini at 0 degrees Aries, as proposed by K.S. Krishnamurti.
        /// It is also known as KP Ayanamsa, and it has been widely
        /// used for horary astrology and stellar astrology
        /// </summary>
        Krishnamurti = 5,
        KrishnamurtiVp291 = 45,

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


        SsRevati = 25,
        SsCitra = 26,
        TrueCitra = 27,
        TrueRevati = 28,
        TruePushya = 29,
        Suryasiddhanta = 21,
        SuryasiddhantaMsun = 22,
        Deluce = 2,
        Ushashashi = 4,
        DjwhalKhul = 6,
        JnBhasin = 8,
        Aldebaran15Tau = 14,
        Hipparchos = 15,
        Sassanian = 16,
        Galcent0Sag = 17,
        J1900 = 19,
        B1950 = 20,
        Aryabhata = 23,
        AryabhataMsun = 24,
        GalcentRgbrand = 30,
        GalequIau1958 = 31,
        GalequTrue = 32,
        GalequMula = 33,
        GalalignMardyks = 34,
        TrueMula = 35,
        GalcentMulaWilhelm = 36,
        Aryabhata522 = 37,
        BabylBritton = 38,
        TrueSheoran = 39,
        GalcentCochrane = 40,
        GalequFiorenza = 41,
        ValensMoon = 42,
    }
}
