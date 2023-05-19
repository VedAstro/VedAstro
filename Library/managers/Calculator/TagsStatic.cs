using System;


namespace VedAstro.Library
{
    /// <summary>
    /// FOR STATIC TABLE BASED FUNCTIONS, STORING DATA
    /// </summary>
    public static partial class AstronomicalCalculator
    {
        /// <summary>
        /// keywords or tag related to a house
        /// </summary>
        public static string GetHouseTags(int house)
        {
            switch (house)
            {
                case 1: return "beginning of life, childhood, health, environment, personality, the physical body and character";
                case 2: return "family, face, right eye, food, wealth, literary gift, and manner and source of death, self-acquisition and optimism";
                case 3: return "brothers and sisters, intelligence, cousins and other immediate relations";
                case 4: return "peace of mind, home life, mother, conveyances, house property, landed and ancestral properties, education and neck and shoulders";
                case 5: return "children, grandfather, intelligence, emotions and fame";
                case 6: return "debts, diseases, enemies, miseries, sorrows, illness and disappointments";
                case 7: return "wife, husband, marriage, urinary organs, marital happiness, sexual diseases, business partner, diplomacy, talent, energies and general happiness";
                case 8: return "longevity, legacies and gifts and unearned wealth, cause of death, disgrace, degradation and details pertaining to death";
                case 9: return "father, righteousness, preceptor, grandchildren, intuition, religion, sympathy, fame, charities, leadership, journeys and communications with spirits";
                case 10: return "occupation, profession, temporal honours, foreign travels, self-respect, knowledge and dignity and means of livelihood";
                case 11: return "means of gains, elder brother and freedom from misery";
                case 12: return "losses, expenditure, waste, extravagance, sympathy, divine knowledge, Moksha and the state after death";
                default: throw new Exception("House details not found!");
            }
        }

        /// <summary>
        /// These details would be highly useful in the delineation of
        /// character and mental disposition
        /// Source:Hindu Predictive Astrology pg.16
        /// </summary>
        public static string GetSignTags(ZodiacName zodiacName)
        {
            switch (zodiacName)
            {
                case ZodiacName.Aries:
                    return @"movable, odd, masculine, cruel, fiery, of short ascension, rising by hinder part, powerful during the night";
                case ZodiacName.Taurus:
                    return @"fixed, even, feminine, mild,earthy, fruitful, of short ascension, rising by hinder part";
                case ZodiacName.Gemini:
                    return @"common, odd, masculine, cruel, airy, barren, of short ascension, rising by the head.";
                case ZodiacName.Cancer:
                    return @"even, movable, feminine, mild, watery, of long ascension, rising by the hinder part and fruitful.";
                case ZodiacName.Leo:
                    return @"fixed, odd, masculine, cruel, fiery, of long ascension, barren, rising by the head.";
                case ZodiacName.Virgo:
                    return @"common, even, feminine, mild, earthy, of long ascension, rising by the head.";
                case ZodiacName.Libra:
                    return @"movable, odd, masculine, cruel, airy, of long ascension, rising by the head.";
                case ZodiacName.Scorpio:
                    return @"fixed, even, feminine, mild, watery, of long ascension, rising by the head.";
                case ZodiacName.Sagittarius:
                    return @"common, odd, masculine, cruel, fiery, of long ascension, rising by the hinder part.";
                case ZodiacName.Capricornus:
                    return @"movable, even, feminine, mild, earthy, of long ascension, rising by hinder part";
                case ZodiacName.Aquarius:
                    return @"fixed, odd, masculine, cruel, fruitful, airy, of short ascension, rising by the head.";
                case ZodiacName.Pisces:
                    return @"common, feminine, water, even, mild, of short ascension, rising by head and hinder part.";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [API("Tags")]
        public static string GetPlanetTags(PlanetName lordOfHouse)
        {
            switch (lordOfHouse.Name)
            {
                case PlanetName.PlanetNameEnum.Sun:
                    return "Father, masculine, malefic, copper colour, philosophical tendency, royal, ego, sons, patrimony, self reliance, political power, windy and bilious temperament, month, places of worship, money-lenders, goldsmith, bones, fires, coronation chambers, doctoring capacity";
                case PlanetName.PlanetNameEnum.Moon:
                    return "Mother, feminine, mind, benefic when waxing, malefic when waning, white colour, women, sea-men, pearls, gems, water, fishermen, stubbornness, romances, bath-rooms, blood, popularity, human responsibilities";
                case PlanetName.PlanetNameEnum.Mars:
                    return "Brothers, masculine, blood-red colour, malefic, refined taste, base metals, vegetation, rotten things, orators, ambassadors, military activities, commerce, aerial journeys, weaving, public speakers.";
                case PlanetName.PlanetNameEnum.Mercury:
                    return "Profession, benefic if well associated, hermaphrodite, green colour, mercantile activity, public speakers, cold nervous, intelligence";
                case PlanetName.PlanetNameEnum.Jupiter:
                    return "Children, masculine, benefic, bright yellow colour, devotion, truthfulness, religious fervour, philosophical wisdom, corpulence";
                case PlanetName.PlanetNameEnum.Venus:
                    return "Wife, feminine, benefic, mixture of all colours, love affairs, sensual pleasure, family bliss, harems of ill-fame, vitality";
                case PlanetName.PlanetNameEnum.Saturn:
                    return "Longevity, malefic, hermaphrodite, dark colour, stubbornness, impetuosity, demoralisation, windy diseases, despondency, gambling";
                case PlanetName.PlanetNameEnum.Rahu:
                    return "Maternal relations, malefic, feminine, renunciation, corruption, epidemics";
                case PlanetName.PlanetNameEnum.Ketu:
                    return "Paternal relations, Hermaphrodite, malefic, religious, sectarian principles, pride, selfishness, occultism, mendicancy";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        /// <summary>
        /// Source: Hindu Predictive Astrology pg.17
        /// </summary>
        public static string GetHouseType(int houseNumber)
        {
            //Quadrants (kendras) are l, 4, 7 and 10.
            //Trines(Trikonas) are 5 and 9.
            //Cadent houses (Panaparas) are 2, 5, 8 and 11
            //Succeedent houses (Apoklimas) are 3, 6, 9 and 12 (9th being a trikona must be omitted)
            //Upachayas are 3, 6, 10 and 11.

            var returnString = "";

            switch (houseNumber)
            {
                //Quadrants (kendras) are l, 4, 7 and 10.
                case 1:
                case 4:
                case 7:
                case 10:
                    returnString += @"Quadrants (kendras)";
                    break;
                //Trines(Trikonas) are 5 and 9.
                case 5:
                case 9:
                    returnString += @"Trines (Trikonas)";
                    break;
            }

            switch (houseNumber)
            {
                //Cadent (Panaparas) are 2, 5, 8 and 11
                case 2:
                case 5:
                case 8:
                case 11:
                    returnString += @"Cadent (Panaparas)";
                    break;
                //Succeedent (Apoklimas) are 3, 6, 9 and 12 (9th being a trikona must be omitted)
                case 3:
                case 6:
                case 9:
                case 12:
                    returnString += @"Succeedent (Apoklimas)";
                    break;
            }

            switch (houseNumber)
            {
                //Upachayas are 3, 6, 10 and 11.
                case 3:
                case 6:
                case 10:
                case 11:
                    returnString += @"Upachayas";
                    break;

            }

            return returnString;
        }

        /// <summary>
        /// Astrology For Beginners - 1992 - B.V. Raman
        /// </summary>


        /// <summary>
        /// Get general planetary info for person's dasa (hardcoded table)
        /// It is intended to be used to intpreate dasa predictions
        /// as such should be displayed next to dasa chart.
        /// This method is direct translation from the book.
        /// Similar to method GetPlanetDasaNature
        /// Data from pg 80 of Key-planets for Each Sign in Hindu Predictive Astrology
        /// </summary>
        public static string GetDasaInfoForAscendant(ZodiacName acesendatName)
        {
            //As soon as tbc Dasas and Bhuktis are determined, the next
            //step would be to find out the good and evil planets for each
            //ascendant so that in applying the principles to decipher the
            //future history of man, the student may be able to carefully
            //analyse the intensilty or good or evil combinations and proceed
            //further with his predictions when applying the results of
            //Dasas and other combinations.

            switch (acesendatName)
            {
                case ZodiacName.Aries:
                    return @"
                        Aries - Saturn, Mercury and Venus are ill-disposed.
                        Jupiter and the Sun are auspicious. The mere combination
                        of Jupiler and Saturn produces no beneficial results. Jupiter
                        is the Yogakaraka or the planet producing success. If Venus
                        becomes a maraka, he will not kill the native but planets like
                        Saturn will bring about death to the person.
                        ";
                case ZodiacName.Taurus:
                    return @"
                        Taurus - Saturn is the most auspicious and powerful
                        planet. Jupiter, Venus and the Moon are evil planets. Saturn
                        alone produces Rajayoga. The native will be killed in the
                        periods and sub-periods of Jupiter, Venus and the Moon if
                        they get death-inflicting powers.
                        ";
                case ZodiacName.Gemini:
                    return @"
                        Gemini - Mars, Jupiter and the Sun are evil. Venus alone
                        is most beneficial and in conjunction with Saturn in good signs
                        produces and excellent career of much fame. Combination
                        of Saturn and Jupiter produces similar results as in Aries.
                        Venus and Mercury, when well associated, cause Rajayoga.
                        The Moon will not kill the person even though possessed of
                        death-inflicting powers.
                        ";
                case ZodiacName.Cancer:
                    return @"
                        Cancer - Venus and Mercury are evil. Jupiter and Mars
                        give beneficial results. Mars is the Rajayogakaraka
                        (conferor of name and fame). The combination of Mars and Jupiter
                        also causes Rajayoga (combination for political success). The
                        Sun does not kill the person although possessed of maraka
                        powers. Venus and other inauspicious planets kill the native.
                        Mars in combination with the Moon or Jupiter in favourable
                        houses especially the 1st, the 5th, the 9th and the 10th
                        produces much reputation.
                        ";
                case ZodiacName.Leo:
                    return @"
                        Leo - Mars is the most auspicious and favourable planet.
                        The combination of Venus and Jupiter does not cause Rajayoga
                        but the conjunction of Jupiter and Mars in favourable
                        houses produce Rajayoga. Saturn, Venus and Mercury are
                        evil. Saturn does not kill the native when he has the maraka
                        power but Mercury and other evil planets inflict death when
                        they get maraka powers.
                        ";
                case ZodiacName.Virgo:
                    return @"
                        Virgo - Venus alone is the most powerful. Mercury and
                        Venus when combined together cause Rajayoga. Mars and
                        the Moon are evil. The Sun does not kill the native even if
                        be becomes a maraka but Venus, the Moon and Jupiter will
                        inflict death when they are possessed of death-infticting power.
                        ";
                case ZodiacName.Libra:
                    return @"
                        Libra - Saturn alone causes Rajayoga. Jupiter, the Sun
                        and Mars are inauspicious. Mercury and Saturn produce good.
                        The conjunction of the Moon and Mercury produces Rajayoga.
                        Mars himself will not kill the person. Jupiter, Venus
                        and Mars when possessed of maraka powers certainly kill the
                        nalive.
                        ";
                case ZodiacName.Scorpio:
                    return @"
                        Scorpio - Jupiter is beneficial. The Sun and the Moon
                        produce Rajayoga. Mercury and Venus are evil. Jupiter,
                        even if be becomes a maraka, does not inflict death. Mercury
                        and other evil planets, when they get death-inlflicting powers,
                        do not certainly spare the native.
                        ";
                case ZodiacName.Sagittarius:
                    return @"
                        Sagittarius - Mars is the best planet and in conjunction
                        with Jupiter, produces much good. The Sun and Mars also
                        produce good. Venus is evil. When the Sun and Mars
                        combine together they produce Rajayoga. Saturn does not
                        bring about death even when he is a maraka. But Venus
                        causes death when be gets jurisdiction as a maraka planet.
                        ";
                case ZodiacName.Capricornus:
                    return @"
                        Capricornus - Venus is the most powerful planet and in
                        conjunction with Mercury produces Rajayoga. Mars, Jupiter
                        and the Moon are evil.
                        ";
                case ZodiacName.Aquarius:
                    return @"
                        Aquarius - Venus alone is auspicious. The combination of
                        Venus and Mars causes Rajayoga. Jupiter and the Moon are
                        evil.
                        ";
                case ZodiacName.Pisces:
                    return @"
                        Pisces - The Moon and Mars are auspicious. Mars is
                        most powerful. Mars with the Moon or Jupiter causes Rajayoga.
                        Saturn, Venus, the Sun and Mercury are evil. Mars
                        himself does not kill the person even if he is a maraka.
                        ";
                default:
                    throw new ArgumentOutOfRangeException(nameof(acesendatName), acesendatName, null);
            }

        }



    }

}
