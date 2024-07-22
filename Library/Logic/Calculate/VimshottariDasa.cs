using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VedAstro.Library
{

    /// <summary>
    /// Low level logic wrapper for Dasa calculation
    /// stuff that should not be in API layer
    /// </summary>
    public static class VimshottariDasa
    {
        /// <summary>
        /// note: used recursively to generate nested JSON for Dasa
        /// feeds on given allDasaEvents list, till last level
        /// </summary>
        public static JObject GetDasaJson(List<DasaEvent> allDasaEvents, int level, DasaEvent parentDasa = null)
        {
            var parentDasaJson = new JObject();

            //get only events for the current dasa level (type)
            //if not specified than must be 1 level dasa
            var isSpecified = parentDasa != null;
            var dasaEvents = isSpecified
                ? allDasaEvents.FindAll(delegate (DasaEvent dasaEvt)
                {
                    var levelMatch = dasaEvt.DasaLevel == level;
                    var parentMatch = dasaEvt.ParentLord == parentDasa.Lord;

                    //make sure sub dasa are within parent dasa time period (before end of parent)
                    var withinTime = dasaEvt.EndTime.GetStdDateTimeOffset() <= parentDasa.EndTime.GetStdDateTimeOffset();

                    return levelMatch && parentMatch && withinTime;
                })
                : allDasaEvents.FindAll(dasaEvt => dasaEvt.DasaLevel == level);

            //if no events found then max level reached
            if (!dasaEvents.Any()) { return null; }

            foreach (var evt in dasaEvents)
            {

                var dasaDataJson = new JObject
             {
                 { "Type", evt.DasaName },
                 { "Start", evt.StartTime.GetStdDateTimeOffsetText() },
                 { "End", evt.EndTime.GetStdDateTimeOffsetText() },
                 { "DurationHours", evt.Duration },
                 { "DurationText", Tools.TimeDurationToHumanText(evt.Duration) },
                 { "TechnicalName", evt.SourceEvent.Name.ToString() },
                 { "Lord", evt.Lord.ToString() },
                 { "ParentLord", evt.ParentLord.ToString() },
                 { "Description", evt.Description },
                 { "Nature", evt.Nature.ToString() },
             };

                //make the sub dasa data (+1 level down) (will be null once last PD level)
                var subDasaJson = GetDasaJson(allDasaEvents, level + 1, evt);

                //if null means this last PD level, so no more sub dasas
                if (subDasaJson != null) { dasaDataJson.Add("SubDasas", subDasaJson); }

                //place nicely in bigger "SubDasas" object for caller
                parentDasaJson[evt.Lord.ToString()] = dasaDataJson;
            }

            return parentDasaJson;
        }

        public static List<DasaEvent> DasaPeriodsOld(Time birthTime, int levels = 4, int scanYears = 120)
        {

            //based on scan years, set start & end time
            Time startTime = birthTime;
            Time endTime = birthTime.AddYears(scanYears);

            //# set how accurately the start & end time of each event is calculated
            //# exp: setting 1 hour, means given in a time range of 1 day, it will be checked 24 times 
            var precisionInHours = 504;

            //set what dasa levels to include based on input level
            var tagList = new List<EventTag>
     {
         //Dasa > Bhukti > Antaram > Sukshma > Prana > Avi Prana > Viprana
         EventTag.PD1,EventTag.PD2, EventTag.PD3, EventTag.PD4,
     };

            // TEMP hack to place time in Person (wrapped) 
            var johnDoe = new Person("", birthTime, Gender.Empty);

            //do calculation (heavy computation)
            List<Event> eventList = EventManager.CalculateEvents(precisionInHours,
                                                                        startTime,
                                                                        endTime,
                                                                        johnDoe,
                                                                        tagList);


            //convert to Dasa Events
            var result = new List<DasaEvent>();
            foreach (var e in eventList)
            {
                //cast to dasa event
                var dasaEvent = new DasaEvent(e);

                //add to list
                result.Add(dasaEvent);
            }

            return result;

        }

        /// <summary>
        /// Gets the relationship between a mojor period planet and minor period planet based solely on relationship between
        /// the planets and nothing to do with the signs yet
        /// based on cyclic relationship between planets
        /// </summary>
        public static (EventNature eventNature, string desciption) PlanetDasaMajorPlanetAndMinorRelationship(PlanetName majorPlanet, PlanetName minorPlanet)
        {
            //------ Code Poetry ------
            // lets take a moment to appreciate this piece of code
            // it represents mathematically the nueral patern inside the human brain
            // what the brain once did, is now done below
            //-------------------------

            //create place to fill the data in & to detect if null
            var returnVal = (eventNature: EventNature.Empty, desciption: "");

            switch (majorPlanet.Name)
            {
                case Library.PlanetName.PlanetNameEnum.Sun:
                    {
                        switch (minorPlanet.Name)
                        {
                            case Library.PlanetName.PlanetNameEnum.Sun:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Unpleasantness with relatives and superiors, anxieties, headache, pain in the ear, some tendency to urinary or kidney troubles, sickness, fear from rulers and enemies, fear of death, loss of money, danger to father if the Sun is afflicted, stomachache and travels, gains through religious people, mental sufferings, a wandering life in a foreign country.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Moon:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Winning favour from superiors, increase in business, fresh enterprises, troubles through women, eye troubles, many relatives and friends, indulgence in idle pastimes, jaundice and kindred ailments, new clothes and ornaments, will be happy, healthy, good meals, respect among relatives.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mars:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Rheumatic and similar troubles, quarrels, danger of enteric fever, dysentery, troubles to relatives, loss of money by thefts or wasteful expenses, failures, acquisition of wealth in the form of gold and gems, royal favour leading to prosperity, contraction and transmission of bilious and other diseases, mental worries, danger from fire, ill-health, loss of reputation, sorrow.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mercury:
                                {
                                    returnVal.eventNature = EventNature.Neutral;
                                    returnVal.desciption = @"Gain in money, good reputation, acquisition of new clothes and ornaments, new education, trouble through relatives, mental distress, depression of spirits, waste of money and nervous weakness, no comforts, friends becoming enemies, much anxiety and fears, health bad, children ungrateful, disputes and trouble from ruler or judge, suffer disgrace, many short journeys and wanderings.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Jupiter:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Benefits from friends and acquaintances, increase in education, employment in high circles, association with people of high rank, success through obstacles, birth of a child, wealth got through sons (if there is a son), honour to religious people, virtuous acts, good traditional observances, good society and conversations, reputation, gains and court-honours.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Venus:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Gain of money, respect by rulers and gain of vehicles, likelihood of marriage, increase of property, illness, does many good works, acquisition of pearls or other precious stones, fatigue, addiction to immoral females and profitless discussions.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Saturn:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Constant sickness to family members, new enemies, some loss of property, bodily ailment, much unhappiness, displacement from home accidents, quarrels with relatives, loss of money, disease, lacking in energy, ignoble calls, mental worries, loans, danger from thieve and rulers.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Rahu:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Many troubles, changes according to the position of Rahu, family disputes, journeys, fear of death, trouble from relatives and enemies, loss of peace or mental misery, loss of money, sorrows, unsuccessful in all attempts, fear of thieves and reptiles, scandals.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Ketu:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Loss of money, affliction of mind with troubles, fainting or nervous exhaustion, mind full of misgivings, a long journey to a distant place, change of house due to disputes, troubles among relatives and associates, throat disease, mental anguish, ophthalmia, serious illness, fear from kings or rulers and enemies, diseases, cheated by others.";
                                    return returnVal;
                                }
                            default:
                                {
                                    throw new Exception($"Planet not accounted for! : {minorPlanet}");
                                }
                        }
                    }
                case Library.PlanetName.PlanetNameEnum.Moon:
                    {
                        switch (minorPlanet.Name)
                        {
                            case Library.PlanetName.PlanetNameEnum.Sun:
                                {
                                    returnVal.eventNature = EventNature.Neutral;
                                    returnVal.desciption = @"Feverish complaints, pains in the eyes, success or failure according to position of the Sun and the Moon, legal power, free from diseases, decadence of enemies, happiness and prosperity, jaundice, dropsy, fever, loss of money, travels, danger to father and mother, piles, weakness, loss of children and friends.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Moon:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Devoted attention to learning, love or music, good clothing, company of refined society, sound health, good reputation, journey to holy places, acquisition of abandoned wealth, power, vehicles and lands; marriage, relatives, fortunate deeds, inclination to public life, change of residence, birth of a child, increase of wealth, prosperity to relatives.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mars:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Quarrels and litigation among friends and relatives, headlong enterprises, danger of disputes between husband and wife, between lovers or in regard to marital affairs; disease, petulence, loss of money, waste of wealth, trouble from brothers and friends, danger from fever and fire, injury from instruments or stones, loss of blood and disease to household animals.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mercury:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Acquisition of wealth from maternal relatives, new clothes and ornaments, settlement of disputes, pleasure through children or lover, increase of wealth, accomplishment of desires, intellectual achievements, new education, honour from rulers, general happiness, enjoyment with females, addiction to betting and drinks.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Jupiter:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Increase of property, plenty of food and comforts, prosperous, benefits from superiors such as masters or governors, birth. of a child, vehicles, abundance of clothes and ornaments and success in undertakings, patronage or rulers, gain or property, respect, learned.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Venus:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Sudden gain from wife, enjoys comforts of agriculture, water products and clothing, suffers from diseases inherited by mother, sickness, pain, loss of property, enmity, gain of houses, good works and good meals, birth of children, expenses due to marriage or other auspicious acts.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Saturn:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Wife's death or separation, much mental anguish, loss of property, loss of friends, ill health, mental trouble due to mother, wind and bilious affections, harsh words, and discussion with unfriendly people, disease due to indigestion, no peace of mind, quarrels with relatives.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Rahu:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Distress of risks from falls and dangerous diseases, waste of wealth and loss of relatives and no ease to body, loss of money, danger of stirring up enemies, sickness, anxiety, enmity of superiors and elders, anxiety and troubles through wife, scandals, change of residence, diseases of skin, danger from thieves and poison, ill-health to father and mother, suffering from hunger.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Ketu:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Illness to wife, loss of relatives, suffering from stomach ache, loss or property, sickness of a feverish nature, danger from fire, subject to swellings or eruptions, eye troubles, mind filled with cares, public criticism or displeasure, dishonor, danger to father, mother and children, scandals among equals, eating of prohibited food, bad acts, bad company, loss of money and memory.";
                                    return returnVal;
                                }
                            default:
                                throw new Exception($"Planet not accounted for! : {minorPlanet}");
                        }
                    }
                case Library.PlanetName.PlanetNameEnum.Mars:
                    {
                        switch (minorPlanet.Name)
                        {
                            case Library.PlanetName.PlanetNameEnum.Sun:
                                {
                                    returnVal.eventNature = EventNature.Neutral;
                                    returnVal.desciption = @"Gain of money in bad ways, destruction of enemies, good reputation, long journey to foreign lands and peace of mind, blame, odium of ciders, quarrels with them, sufferings by diseases, heartache occasioned by one's own relatives, fever or other inflammatory affection, danger of fire, troubles through persons in position, many enemies.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Moon:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Profit, acquisition of wealth, house renovated or some improvements effected in it, comforts of wealth, heavy sleep, ardent passion, enjoyment by the help or women.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mars:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Great heat, dislike of friends, annoyance from brothers and sisters, danger from rulers, failure of all undertakings, danger of hurts according to the sign held by Kuja, trouble with superiors and some anxiety through strangers, foreigners or people abroad and through warlike clan. Danger of open violence, quarrel with relations, loss of money, skin disease, consumption, loss of blood, fistula, and fissures in anus, loss of females and brothers, evil doings and boils.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mercury:
                                {
                                    returnVal.eventNature = EventNature.Neutral;
                                    returnVal.desciption = @"Marriage or inclination to marriage, knowledge and fruits of knowledge, wealth, bodily evils disappear, slander, fear of insects, poisoned by animals and insects, gain of wealth by trade, abundance of houses, trouble from enemies and mental worries, service rendered to friends and relatives, new knowledge, success in litigation.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Jupiter:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Loss of wealth, enemies, end the unfortunate period, favour from superiors and persons in position, gain of money, birth of children, auspicious celebrations, acquisition or wealth through holy people, freedom from illness, public reputation, ascendancy and happiness.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Venus:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Acquisition of property, gain of money, domestic happiness, successful love affairs, inclination towards religious observances and festivities, favourable associations, influenced by priests, skin eruptions, boils, pleasure from travelling, jewels to wife, clothing, money from relatives and brothers, odium.of females and their society, increase of intelligence, enjoyment of females and gain of money.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Saturn:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Loss of money, diseases, loss of relatives and danger from arms or operation, illness leading to misery, evil threatened by enemies and robbers, disputes with rulers, loss of wealth, quarrel, disputes, litigation, loss of property, cutaneous effects. loss of office or position and much anxiety.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Rahu:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Danger from rulers and robbers, loss of wealth, success in evil pursuits, suffering from poisonous complaints, loss of relatives, danger from skin diseases, change of residence, some severe kind of cutaneous disease, journey to a foreign country, scandals, loss of cows and buffaloes, illness to wife, loss of memory, fear from insects and thieves, falls into well, fear from ghosts, affection of gonorrhea, fretting and";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Ketu:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Enmity and quarrels with low people, loss of money due to evil works, commission of signs, great sufferings due to troubles from relatives and brothers and opposition of bad people, family disputes, troubles with one's own kindred diseases, poisonous complaints, trouble through women, many enemies.";
                                    return returnVal;
                                }
                            default:
                                throw new Exception($"Planet not accounted for! : {minorPlanet}");
                        }
                    }
                case Library.PlanetName.PlanetNameEnum.Mercury:
                    {
                        switch (minorPlanet.Name)
                        {
                            case Library.PlanetName.PlanetNameEnum.Sun:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Pains in bead and stomach, enmity of people, loss of respect, danger of fire, anxieties, sickness to wire, troubles from enemies, many obstacles, troubles through superiors, acquisition of wealth.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Moon:
                                {
                                    returnVal.eventNature = EventNature.Neutral;
                                    returnVal.desciption = @"Loss of health, some swellings or hurts in the limbs, quarrels and troubles through women, many difficulties, gain of money through ladies and agriculture and trade, success, happiness, diseases, ill-will of enemies, miscarriage of every concern, risk from quadrupeds.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mars:
                                {
                                    returnVal.eventNature = EventNature.Neutral;
                                    returnVal.desciption = @"Disappearance of all dangers diseases or enemies, fame derived from acts of charity and beneficence, royal favour, danger from jaundice or bilious fever, affections of the blood, neuralgic pains and headaches, troubles through neighbors, sickness, wounds or hurts, quarrels, addiction to drinks, betting and prostitutes, boils and hurts of arms, travels in forests and villages, sorrows, royal disfavour, imprisonment.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mercury:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Acquisition of beautiful house and apparel, money through relatives, success in every undertaking, the birth of a brother or sister, increase in family, gain in business, good mind charitable acts, learning of mathematics and arts.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Jupiter:
                                {
                                    returnVal.eventNature = EventNature.Neutral;
                                    returnVal.desciption = @"Hatred of friends, relatives and elders, wealth, liable to diseases, acquisition of land and wealth, gain by trade, reputation, good happiness, good credit, benefits from superiors, birth of a child or marriage.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Venus:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Observance of duty, conformable to religion and morality, acquisition of wealth, clothes and jewels, birth or good children, happiness in married state, relatives prosper, trade increases, knowledge gained, return from a long journey, if not married, betrothal in this period, health, ornaments, vehicles, house, money gained.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Saturn:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Bad luck. stranger to success and happiness, severe reversal, enmity, pain in the part governed by Saturn, downfall or disgrace to relatives, mind full of evil forebodings and distress. rear from diseases, loans, loss of children, destruction of family, scandals, troubles from foreigners, earnings through evil ways, acts of charity and beneficence, acquisition of wealth, material comforts through petty chiefs, failure in agricultural operations.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Rahu:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Intercourse with servants and prostitutes, skin diseases, sufferings from hot diseases, bad company and dirty meals. change or present position, fear and danger through foreigners, disputes concerning property, failure in litigation, evil dreams. headaches, sickness and loss of appetite. wealth from friends and relatives, happiness, new earnings.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Ketu:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Sorrow, disease, loss of work and Dharma, bilious sickness, aimless wandering, loss of property, misfortune to relatives, troubles through doctors, mental anxiety, trouble from relatives, mental agony, loss of comfort, dread of enemy, failure in business.";
                                    return returnVal;
                                }
                            default:
                                throw new Exception($"Planet not accounted for! : {minorPlanet}");
                        }
                    }
                case Library.PlanetName.PlanetNameEnum.Jupiter:
                    {
                        switch (minorPlanet.Name)
                        {
                            case Library.PlanetName.PlanetNameEnum.Sun:
                                {
                                    returnVal.eventNature = EventNature.Neutral;
                                    returnVal.desciption = @"Enemies, victory, ease, great diligence, coming in of wealth, royal favour and sound health, gain,good actions or fruits of good action, loss of bodily strength.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Moon:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Increase of prosperity, gain of fame and fortune, acquisition or property, benefits through children, sexual intercourse with beautiful women, good meals and clothing, success and birth or a female child or marriage to some male member in the family, gain of money.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mars:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Disappointments and troubles of various kinds, loss by thefts, loss of near and dear relatives, inflammatory disease, transfer or leave, failure in hopes and business, wandering, high fever, great risks, loss of wealth and depression of mind, pilgrimage to temples, acquisition of wealth and fame, adventures.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mercury:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Increase of wealth, good and auspicious works in the house, communion with relatives, happy, increase of knowledge, acquisition of wealth through trade, favour from rulers, material comforts, perfect practice of hospitality, gain through knowledge in fine arts, birth of a well-favoured child, advantages from superiors.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Jupiter:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Increase of property, domestic happiness, benefit from employment or occupation, birth of children, reputation, good meals, good deeds, health. royal favour, great diligence, success in all attempts, travels, dips in sacred rivers, pilgrimage, honour at stake if afflicted.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Venus:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Appointment, wealth, reputation, gain of money, savings, development of sons and grandsons, jewels, good and delicious meals, marital happiness, auspicious works, reunion of the family, good success in profession or business, gain of land in the month of Taurus or Libra, much enjoyment, relatives, friends, peace or mind, acquisition or valuables, troubles from females and odium of public.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Saturn:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"A feeling of aversion, mental anguish, waste of wealth through sons, failure of business, increase of wealth and prosperity, pain in the body, rheumatic pains in limbs, trouble through wife or partners, failure in profit and credit, sorrows, fears, enmity of friends and relatives, adultery, unrighteous, a witness in court, quarrels in family, mental depression, funeral ceremonies for others,";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Rahu:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Income through low-caste people, apprehension of diseases, possibility of every possible calamity, deprivation of wealth.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Ketu:
                                {
                                    returnVal.eventNature = EventNature.Neutral;
                                    returnVal.desciption = @"Pilgrimages to holy shrines, increase of wealth, suffering for the sake of several seniors and rulers, death of partner if in business, change of residence, separation from relatives and friends, may forsake business, poisonous effects, loss of wealth, destruction of work, illness, boils.";
                                    return returnVal;
                                }
                            default:
                                throw new Exception($"Planet not accounted for! : {minorPlanet}");
                        }
                        break;
                    }
                case Library.PlanetName.PlanetNameEnum.Venus:
                    {
                        switch (minorPlanet.Name)
                        {
                            case Library.PlanetName.PlanetNameEnum.Sun:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Anxious about everything, prosperity collapses, troubles with wife, children, land, family, disputes and quarrels, diseases affecting head, belly and eyes, damage in respect of agriculture.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Moon:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Gains of females, education, knowledge, money, children and vehicles, worship of God, accomplishments of desires, troubles through wife, domestic happiness afterwards, pain and disease due to inflammation of nervous tissue and from lust and other passions of human nature.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mars:
                                {
                                    returnVal.eventNature = EventNature.Neutral;
                                    returnVal.desciption = @"Flow of bile, disease of the eyes, great exertion, much income, acquisition or wealth, marriage, acquisition of lands, venereal diseases, danger from arms, exile in foreign places, atheistic tendencies, increase of property through the influence of females, negligence of duty, bent on pleasure and passion, temporary affection of eyes.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mercury:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Association with prostitutes, enjoyments, knowledge, mathematical learning, success in litigations, inclination to learn music, piles and other hot ailments, pleasure through wife and children, increase of wealth, gain of knowledge in aru and sciences, wealth, royal favour, prosperity on a large scale and sound health.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Jupiter:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Means of livelihood settled, gains from profession, benefits through superiors or employers or persons ruled by Jupiter fame, anxiety, quarrels with saints and religious men, gain of knowledge, end of dependence, worship of certain inferior natural forces, happiness and health, marriages, sexual intercourse, increase of family reputation and good deeds, wealth. ultimate happiness, wife and children suffer in the end.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Venus:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Success, good servants and good many pleasures, money plentiful. disappearance of enemies, attainment of fame and birth of children.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Saturn:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Sexual intercourse with females advanced in age, accession to lands and wealth, disappearance of enemies, affection of excretory system, piles, etc., rheumatic pains in legs and bands, danger to eye sight, distaste for food, loss of appetite, physical condition poor, loss of money, wanderings, servitude, bolting and gambling, addiction to liquor, bad company, etc., ill-health, loss of memory, impotence.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Rahu:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Meditation, seclusion, quarrels among relatives and his people, entire change of surroundings, schemes of deception. miserliness, acquisition of lost property, dislike of relatives. evil from friends and injury by fire.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Ketu:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Discordance, death of relatives, injury inflicted by enemies, misgivings in heart, deprivation of wealth, troubles through wife, danger from quadrupeds, illness to partner or a member of the family, accidental blood poisoning, delirious fits, weakness in body and mind, gradual loss of wealth, loss of relatives, bad company, abode in seclusion, manifold sorrows, but happiness in the end.";
                                    return returnVal;
                                }
                            default:
                                throw new Exception($"Planet not accounted for! : {minorPlanet}");
                        }
                    }
                case Library.PlanetName.PlanetNameEnum.Saturn:
                    {
                        switch (minorPlanet.Name)
                        {
                            case Library.PlanetName.PlanetNameEnum.Sun:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Loss of wife and children, trouble from rulers or robbers. sinking of heart, danger of blood poisoning, haemorrhage of the generative system, chronic poisoning, intestinal swellings, affliction of the eyes, sickness even to healthy children and wife, body full of pain and disorders, danger of death. fear of death to father-in-law.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Moon:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Increase in cattle, enmity of friends and relatives, cold affections, troubles and sickness, family disputes, loss of money and property, reduction to great need, mortgage of property and its recovery after a lapse of time, death of a near relative, sorrow, dislike of relatives, coming in of money, windy diseases.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mars:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Some disgrace, serious enmity, strife, much blame, wanderings from place to place, unsettled life, many enemies, loss of money by fraud or theft, change of residence, serious illness, distress to brothers and friends, hot diseases.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mercury:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Charitable works, gain of wealth, birth of children, increase of knowledge in some branch, prosperity to children, success to relatives, general prosperity, favours and approbation from superiors, increase of happiness, wealth and fame, benefits occurring from acts of piety and customary religious observances, agriculture and commerce.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Jupiter:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Worship of gods and holy people, happiness to family, increase in bodily comforts, accomplishment of intentions by the help of superiors, increase of family circles, attainment of rank.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Venus:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Auspicious, general happiness, attentions and favours from others, gifts, profits in business, increase of family members, victory over enemies, success in life, goodwill of relatives, accession to wife's property and wealth.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Saturn:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Brings on diseases, troubles and torments, much mental anguish, capacity of kings and free-hooters, loss of wealth, fear or poisonous effect to cattle, much sufferings to family, fever, wind or phlegm, bodily ailments and colic, body languishes, loss of money and children, serious enmities, dispute and troubles from relatives, blood and bilious complaints, quarrels in family, loss of money, mental derangement.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Rahu:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Disease in every limb, loss of wealth by rulers, robbers and foes, danger of physical hurts, various physical troubles, fevers, enemies, increase of troubles.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Ketu:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Rheumatism or sickness, danger of poison, danger from sons, loss of money, contentions and quarrels with vile and wicked people, dread of evil dreams, quarrels in family.";
                                    return returnVal;
                                }
                            default:
                                throw new Exception($"Planet not accounted for! : {minorPlanet}");
                        }
                    }
                case Library.PlanetName.PlanetNameEnum.Rahu:
                    {
                        switch (minorPlanet.Name)
                        {
                            case Library.PlanetName.PlanetNameEnum.Sun:
                                {
                                    returnVal.eventNature = EventNature.Neutral;
                                    returnVal.desciption = @"Hot fevers, giddiness, fear and enmity of people, quarrels in family, benefits from persons in good position, fear and suspicion in connection with wife, children and relatives, change of position or residence, love of charitable acts, contentment, cessation of all violence and outrage of contagious diseases, success in examinations, private life happy, much reputation and fame, but mental unrest.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Moon:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Abundance of enjoyments, good crops, coming in of money and communion with kith and kin, loss of relatives, loss of money through wife, pains in the limbs, change of position or residence, danger of personal hurts, unstability of health, sea voyages, gain of lands and money, loss or danger to wife and children.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mars:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Danger from rulers, fires or thieves and by arms, defeat in litigation, loss of money due to cousins, difficulties, sorrows, danger to the person due to malice of enemies, tendency to ease or dissolute habits, disputes and mental anxiety, combination of all possible calamities, bewilderment in every work and culpable failure of memory.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mercury:
                                {
                                    returnVal.eventNature = EventNature.Neutral;
                                    returnVal.desciption = @"Many friends and relatives, wife and children, accession to wealth or royal favour. In the first 18 months of this period very busy, seriously inclined to marry. In the latter 12 months, enemies increase through his own action, happiness, birth of children, acquisition of vehicles, happiness to relatives and family, enjoyments with prostitutes, showy, gains through trade, fraudulent schemes.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Jupiter:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Total disappearance of enemies and sickness, royal favour, acquisition of wealth, birth of children, increase of pleasure, gain through nobles or persons in power, benefits and comforts from superiors, success in all efforts, marriage in the house, increase of enemies, litigations and dips in sacred rivers.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Venus:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Accession to vehicles and things of foreign land, troubles from foes, relatives and diseases, acquisition of wealth and other advantages, friendly alliances, wife a source of fortune and happiness, benefits from superiors or beads above in office, liable to deception, false friends, gain in land, birth of a child or marriage.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Saturn:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Scandal, danger due to fall of a tree, bad associations, divorce of wife or husband, incessant disputes and contests, rheumatism, biliousness, etc., throughout: disease due to wind and bile, distress of relatives, friends and well-wishers, residence in a remote foreign land.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Rahu:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Disturbance in mind, anxieties, quarrels among relatives, death of partner, master or the head of the family, mental anxiety, danger of poisoning, transfer, all sorts of scandals and quarrels, fever, bites of insects or wounds by arms, death of relatives, going to court as witness, quarrels with parents, diseases, illness to wife, failure of intellect, loss of wealth, wandering in far-off countries and distress there.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Ketu:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Danger, disease in the anus, no good and timely meals, epidemic diseases, danger of physical hurts and poison, ill-health to children, some swellings in the body, troubles through wife, danger from superiors, loss of wealth and honour, loss of children, death of cattle and misfortunes of all kinds.";
                                    return returnVal;
                                }
                            default:
                                throw new Exception($"Planet not accounted for! : {minorPlanet}");
                        }
                    }
                case Library.PlanetName.PlanetNameEnum.Ketu:
                    {
                        switch (minorPlanet.Name)
                        {
                            case Library.PlanetName.PlanetNameEnum.Sun:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Disappointment, physical pain, exile in foreign country, peril and obstruction in every business, increase of knowledge, sickness in family, long journey and return, anxiety about wife's health.";
                                    break;
                                }
                            case Library.PlanetName.PlanetNameEnum.Moon:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Disputes about fair sex, trouble through children, gains and financial success, diseases of biliousness and cold, loss of relatives and money, destruction of wealth and distress of mind.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mars:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Odium of sons, wife and younger brothers, loss of relatives, trouble from diseases, foes and bad rulers, path of progress obstructed, fear and anxiety, disputes and contests of different kinds, enemies arise, danger of disputes and destruction through females, sufferings from fever, fear of robbers, death, imprisonment, urinary diseases, loss and difficulties and surgical operations.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mercury:
                                {
                                    returnVal.eventNature = EventNature.Neutral;
                                    returnVal.desciption = @"Society of relatives, friends and the like, material gains from knowledge, danger from relatives, anxiety on account of children, failure in plans, deception, jealousy, falsehood, and knowledge.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Jupiter:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Exemption from ailments, acquisition of lands and birth of children, profitable transactions, association with people of good position, danger of poison, wife an object of pleasure, if unmarried marriage takes place.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Venus:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Wealth and happiness, birth of a child, efforts crowned with success, in the end sickness, wife ill, illness to children, quarrels, loss of relatives and friends, fever and dysentery.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Saturn:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Loss of wife, danger from enemies, imprisonment, loss of wealth, indigestion, property in danger or ruin, heavy loss in different ways, change of residence, some cutaneous diseases. anxiety owing to sickness of partner misgivings in the heart, mental anguish, difference of opinion with relations, exile in foreign countries.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Rahu:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Loss of lands, imprisonment, quarrel with friends, danger of blood poisoning, danger of ruin, loss of property, fame and honour, rear of kings and robbers, sorrow, ruin of all business, adultery with mean women.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Ketu:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Fear of death of wife or children, loss of wealth and happiness, mental troubles, separation from relatives, subject to some estrangement, restraint or detention, danger of poison.";
                                    return returnVal;
                                }
                            default:
                                throw new Exception($"Planet not accounted for! : {minorPlanet}");
                        }
                    }
                    break;
                default:
                    throw new Exception($"Planet not accounted for! : {majorPlanet}");
            }

            //return the specialized nature & description packaged together
            return returnVal;
        }

        /// <summary>
        /// Gets dasa counted from birth dasa
        /// </summary>
        public static int CurrentDasaCountFromBirth(Time birthTime, Time currentTime)
        {
            //get dasa planet at birth (birth time = current time)
            var birthDasaPlanet = CurrentDasa8Levels(birthTime, birthTime).PD1;

            var currentDasaPlanet = CurrentDasa8Levels(birthTime, currentTime).PD1;

            //count from birth dasa planet to current dasa planet
            var dasaCount = 1; //minimum 1

            //start with birth dasa planet,
            //incase current & birth dasa planet is same
            PlanetName nextPlanet = birthDasaPlanet;

        TryAgain:
            //planet found, stop counting
            if (nextPlanet == currentDasaPlanet)
            {
                return dasaCount;
            }
            //else planet not found,
            else
            {
                //change to next planet 
                nextPlanet = NextDasaPlanet(nextPlanet);
                //increase count
                dasaCount++;
                //try checking again if it is same planet
                goto TryAgain;
            }

        }

        /// <summary>
        /// The main method that starts all Dasa Calculations
        /// Gets the occuring Planet Dasas (PD1, PD2,...) for a person at the given time
        /// </summary>
        public static Dasas CurrentDasa8Levels(Time birthTime, Time currentTime)
        {
            //todo strength determined by constellation rules not shad or bhava bala
            //lagna lord or moon constellation used based on which is stronger
            //var isLagnaLordStronger = Calculate.LagnaLordVsMoonStrength(birthTime);
            //var finalConstellation = isLagnaLordStronger
            //    ? GetPlanetConstellation(birthTime, Calculate.GetLordOfHouse(HouseName.House1, birthTime))
            //    : GetMoonConstellation(birthTime);

            //get dasa planet at birth
            var moonConstellation = Calculate.PlanetConstellation(PlanetName.Moon, birthTime);
            //var risingConstellation = GetHouseConstellation(1, birthTime);

            var birthDasaPlanetMoon = ConstellationDasaPlanet(moonConstellation.GetConstellationName());
            //var birthDasaPlanet = GetConstellationDasaPlanet(risingConstellation.GetConstellationName());

            //get time traversed in birth dasa 
            var timeTraversedInDasa = YearsTraversedInBirthDasa(moonConstellation, birthTime);

            //NOTE: variable number of days per year here dynamically set for BV. RAMAN ayanamsa only
           // if (Calculate.Ayanamsa == (int)Ayanamsa.RAMAN) { Calculate.SolarYearTimeSpan = 360; }
            if (Calculate.Ayanamsa == (int)Ayanamsa.KRISHNAMURTI) 
            { 
                Calculate.SolarYearTimeSpan = 365.2564; 
            } //CPJ 

            //get time from birth to current time 
            var timeBetween = currentTime.Subtract(birthTime).TotalDays / Calculate.SolarYearTimeSpan;

            //combine years traversed at birth and years to current time
            //this is done to easily calculate to current dasa, bhukti & antaram
            var combinedYears = timeTraversedInDasa + timeBetween;
            var wholeDasa = DasaCountedFromInputDasa(birthDasaPlanetMoon, combinedYears);

            return wholeDasa;
        }

        /// <summary>
        /// Counts from inputed dasa by years to dasa, bhukti & antaram
        /// Inputed planet is taken as birth dasa ("starting dasa" to count from)
        /// Note: It is easier to calculate from start of Dasa,
        ///       so years already traversed at birth must be added into inputed years
        /// Exp: Get dasa, bhukti & antaram planet 3.5 years from start of Sun dasa
        /// </summary>
        public static Dasas DasaCountedFromInputDasa(PlanetName startDasaPlanet, double years)
        {
            double pd1Years = years;
            double pd2Years; //will be filled when getting dasa
            double pd3Years; //will be filled when getting bhukti
            double pd4Years; //will be filled when getting antaram
            double pd5Years; //will be filled when getting prana
            double pd6Years; //will be filled when getting prana
            double pd7Years; //will be filled when getting prana
            double pd8Years; //will be filled when getting prana

            //NOTE: Get Dasa prepares values for Get Bhukti and so on.

            //first get the major dasa planet
            //then based on the pd2 get pd3 and so on in same pattern
            var pd1Planet = GetPD1();
            var pd2Planet = GetPD2();
            var pd3Planet = GetPD3();
            var pd4Planet = GetPD4();
            var pd5Planet = GetPD5();
            var pd6Planet = GetPD6();
            var pd7Planet = GetPD7();
            var pd8Planet = GetPD8();


            return new Dasas()
            {
                PD1 = pd1Planet,
                PD2 = pd2Planet,
                PD3 = pd3Planet,
                PD4 = pd4Planet,
                PD5 = pd5Planet,
                PD6 = pd6Planet,
                PD7 = pd7Planet,
                PD8 = pd8Planet
            };


            //--------------------------------LOCAL FUNCTIONS

            PlanetName GetPD8()
            {
                //first possible PD8 planet is the PD7 planet
                var possiblePD8Planet = pd7Planet;

            //minus the possible PD8 planet's full years
            MinusPD8Years:
                var pd8PlanetFullYears = PD8PlanetFullYears(pd1Planet, pd2Planet, pd3Planet, pd4Planet, pd5Planet, pd6Planet, pd7Planet, possiblePD8Planet);
                pd8Years -= pd8PlanetFullYears;

                //if remaining pd8 time is negative,
                //than current possible PD7 planet is correct
                if (pd8Years <= 0)
                {
                    //return possible planet as correct
                    return possiblePD8Planet;
                }
                //else possible pd8 planet not correct, go to next one 
                else
                {
                    //change to next pd8 planet in order
                    possiblePD8Planet = NextDasaPlanet(possiblePD8Planet);
                    //go back to minus this planet's years
                    goto MinusPD8Years;
                }

            }

            PlanetName GetPD7()
            {
                //first possible PD7 planet is the PD6 planet
                var possiblePD7Planet = pd6Planet;

            //minus the possible PD7 planet's full years
            MinusPD7Years:
                var pd7PlanetFullYears = PD7PlanetFullYears(pd1Planet, pd2Planet, pd3Planet, pd4Planet, pd5Planet, pd6Planet, possiblePD7Planet);
                pd7Years -= pd7PlanetFullYears;

                //if remaining pd7 time is negative,
                //than current possible PD6 planet is correct
                if (pd7Years <= 0)
                {
                    //get back the PD7 time before it becomes negative
                    //this is the time inside the current PD7, aka PD8 time
                    //save it for late use
                    pd8Years = pd7Years + pd7PlanetFullYears;

                    //return possible planet as correct
                    return possiblePD7Planet;
                }
                //else possible pd7 planet not correct, go to next one 
                else
                {
                    //change to next pd7 planet in order
                    possiblePD7Planet = NextDasaPlanet(possiblePD7Planet);
                    //go back to minus this planet's years
                    goto MinusPD7Years;
                }

            }

            PlanetName GetPD6()
            {
                //first possible PD6 planet is the PD4 planet
                var possiblePD6Planet = pd5Planet;

            //minus the possible PD6 planet's full years
            MinusPD6Years:
                var pd6PlanetFullYears = PD6PlanetFullYears(pd1Planet, pd2Planet, pd3Planet, pd4Planet, pd5Planet, possiblePD6Planet);
                pd6Years -= pd6PlanetFullYears;

                //if remaining pd6 time is negative,
                //than current possible PD5 planet is correct
                if (pd6Years <= 0)
                {
                    //get back the PD6 time before it becomes negative
                    //this is the time inside the current PD6, aka PD7 time
                    //save it for late use
                    pd7Years = pd6Years + pd6PlanetFullYears;

                    //return possible planet as correct
                    return possiblePD6Planet;
                }
                //else possible pd6 planet not correct, go to next one 
                else
                {
                    //change to next pd6 planet in order
                    possiblePD6Planet = NextDasaPlanet(possiblePD6Planet);
                    //go back to minus this planet's years
                    goto MinusPD6Years;
                }

            }

            PlanetName GetPD5()
            {
                //first possible PD5 planet is the PD4 planet
                var possiblePD5Planet = pd4Planet;

            //minus the possible PD5 planet's full years
            MinusPD5Years:
                var pd5PlanetFullYears = PD5PlanetFullYears(pd1Planet, pd2Planet, pd3Planet, pd4Planet, possiblePD5Planet);
                pd5Years -= pd5PlanetFullYears;

                //if remaining pd5 time is negative,
                //than current possible PD4 planet is correct
                if (pd5Years <= 0)
                {
                    //get back the PD5 time before it becomes negative
                    //this is the time inside the current PD5, aka PD6 time
                    //save it for late use
                    pd6Years = pd5Years + pd5PlanetFullYears;

                    //return possible planet as correct
                    return possiblePD5Planet;
                }
                //else possible pd5 planet not correct, go to next one 
                else
                {
                    //change to next pd5 planet in order
                    possiblePD5Planet = NextDasaPlanet(possiblePD5Planet);
                    //go back to minus this planet's years
                    goto MinusPD5Years;
                }

            }

            PlanetName GetPD4()
            {
                //first possible pd4 planet is the antaram planet
                var possiblePD4Planet = pd3Planet;

            //minus the possible pd4 planet's full years
            MinusPD4Years:
                var pd4PlanetFullYears = PD4PlanetFullYears(pd1Planet, pd2Planet, pd3Planet, possiblePD4Planet);
                pd4Years -= pd4PlanetFullYears;

                //if remaining pd4 years is negative,
                //than current possible pd4 planet is correct
                if (pd4Years <= 0)
                {
                    //get back the PD4 time before it becomes negative
                    //this is the time inside the current PD4, aka PD5 time
                    //save it for late use
                    pd5Years = pd4Years + pd4PlanetFullYears;

                    //return possible planet as correct
                    return possiblePD4Planet;
                }
                //else possible pd4 planet not correct, go to next one 
                else
                {
                    //change to next pd4 planet in order
                    possiblePD4Planet = NextDasaPlanet(possiblePD4Planet);
                    //go back to minus this planet's years
                    goto MinusPD4Years;
                }

            }

            PlanetName GetPD3()
            {
                //first possible antaram planet is the bhukti planet
                var possibleAntaramPlanet = pd2Planet;

            //minus the possible antaram planet's full years
            MinusPD3Years:
                var antaramPlanetFullYears = PD3PlanetFullYears(pd1Planet, pd2Planet, possibleAntaramPlanet);
                pd3Years -= antaramPlanetFullYears;

                //if remaining antaram years is negative,
                //than current possible antaram planet is correct
                if (pd3Years <= 0)
                {
                    //get back the antaram time before it became negative
                    //this is the time inside the current antaram, aka Sukshma time
                    //save it for late use
                    pd4Years = pd3Years + antaramPlanetFullYears;

                    //return possible planet as correct
                    return possibleAntaramPlanet;
                }
                //else possible antaram planet not correct, go to next one 
                else
                {
                    //change to next antaram planet in order
                    possibleAntaramPlanet = NextDasaPlanet(possibleAntaramPlanet);
                    //go back to minus this planet's years
                    goto MinusPD3Years;
                }


            }

            PlanetName GetPD2()
            {
                //first possible pd2 planet is the major Dasa planet
                var possiblePD2Planet = pd1Planet;

            //minus the possible pd2 planet's full years
            MinusPD2Years:
                var pd2PlanetFullYears = PD2PlanetFullYears(pd1Planet, possiblePD2Planet);
                pd2Years -= pd2PlanetFullYears;

                //if remaining pd2 years is negative,
                //than current possible pd2 planet is correct
                if (pd2Years <= 0)
                {
                    //get back the pd2 years before it became negative
                    //this is the years inside the current pd2, aka antaram years
                    //save it for late use
                    pd3Years = pd2Years + pd2PlanetFullYears;

                    //return possible planet as correct
                    return possiblePD2Planet;
                }
                //else possible pd2 planet not correct, go to next one 
                else
                {
                    //change to next pd2 planet in order
                    possiblePD2Planet = NextDasaPlanet(possiblePD2Planet);
                    //go back to minus this planet's years
                    goto MinusPD2Years;
                }

            }

            PlanetName GetPD1()
            {
                //possible planet starts with the inputed one
                var possibleDasaPlanet = startDasaPlanet;

            //minus planet years
            MinusPD1Years:
                var dasaPlanetFullYears = PD1PlanetFullYears(possibleDasaPlanet);
                pd1Years -= dasaPlanetFullYears;

                //if remaining dasa years is negative,
                //than possible dasa planet is correct
                if (pd1Years <= 0)
                {
                    //get back the dasa years before it became negative
                    //this is the years inside the current dasa, aka bhukti years
                    //save it for late use
                    pd2Years = pd1Years + dasaPlanetFullYears;

                    //return possible planet as correct
                    return possibleDasaPlanet;
                }
                //else possible dasa planet not correct, go to next one 
                else
                {
                    //change to next dasa planet in order
                    possibleDasaPlanet = NextDasaPlanet(possibleDasaPlanet);
                    //go back to minus this planet's years
                    goto MinusPD1Years;
                }
            }

        }

        /// <summary>
        /// Gets next planet in Dasa order
        /// This order is also used for Bhukti & Antaram
        /// Ref:Hindu Predictive Astrology pg. 54
        /// </summary>
        public static PlanetName NextDasaPlanet(PlanetName planet)
        {
            if (planet == Library.PlanetName.Sun) { return Library.PlanetName.Moon; }
            if (planet == Library.PlanetName.Moon) { return Library.PlanetName.Mars; }
            if (planet == Library.PlanetName.Mars) { return Library.PlanetName.Rahu; }
            if (planet == Library.PlanetName.Rahu) { return Library.PlanetName.Jupiter; }
            if (planet == Library.PlanetName.Jupiter) { return Library.PlanetName.Saturn; }
            if (planet == Library.PlanetName.Saturn) { return Library.PlanetName.Mercury; }
            if (planet == Library.PlanetName.Mercury) { return Library.PlanetName.Ketu; }
            if (planet == Library.PlanetName.Ketu) { return Library.PlanetName.Venus; }
            if (planet == Library.PlanetName.Venus) { return Library.PlanetName.Sun; }

            //if no plant found something wrong
            throw new Exception("Planet not found!");

        }

        /// <summary>
        ///  Gets years left in birth dasa at birth
        ///  Note : Returned years can only be 0 or above
        ///  Start constellation can be of moon or Lagna
        /// </summary>
        public static double TimeLeftInBirthDasa(Constellation startConstellation, Time birthTime)
        {
            //get years already passed in birth dasa
            var yearsTraversed = YearsTraversedInBirthDasa(startConstellation, birthTime);

            //get full years of birth dasa planet
            var birthDasaPlanet = ConstellationDasaPlanet(startConstellation.GetConstellationName());
            var fullYears = PD1PlanetFullYears(birthDasaPlanet);

            //calculate the years left in birth dasa at birth
            var yearsLeft = fullYears - yearsTraversed;

            //raise error if years traversed is more than full years
            if (yearsLeft < 0) { throw new Exception("Dasa years traversed is more than full years!"); }

            return yearsLeft;
        }

        /// <summary>
        /// Gets the time in years traversed in Dasa at birth
        /// Start constellation can of Moon's or Lagna lord
        /// </summary>
        public static double YearsTraversedInBirthDasa(Constellation startConstellation, Time birthTime)
        {
            //get longitude minutes the Moon/Lagna already traveled in the constellation 
            var minutesTraversed = startConstellation.GetDegreesInConstellation().TotalMinutes;

            //get the time period each minute represents
            var timePerMinute = DasaTimePerMinute(startConstellation.GetConstellationName());

            //calculate the years already traversed
            var traversedYears = minutesTraversed * timePerMinute;

            return traversedYears;
        }

        /// <summary>
        /// Gets the Dasa time period each longitude minute in a constellation represents,
        /// based on the planet which is related (lord) to it.
        /// Note: Returns the time in years, exp 0.5 = half year
        /// </summary>
        public static double DasaTimePerMinute(ConstellationName constellationName)
        {
            //maximum longitude minutes of a constellation
            const double maxMinutes = 800.0;

            //get the related (lord) planet for the constellation
            var relatedPlanet = ConstellationDasaPlanet(constellationName);

            //get the full Dasa years for the related planet
            var fullYears = PD1PlanetFullYears(relatedPlanet);

            //calculate the time in years each longitude minute represents
            var timePerMinute = fullYears / maxMinutes;

            return timePerMinute;
        }

        /// <summary>
        /// Gets the related (lord) Dasa planet for a given constellation
        /// Used to find the ruling Dasa Planet
        /// Ref:Hindu Predictive Astrology pg. 54
        /// </summary>
        public static PlanetName ConstellationDasaPlanet(ConstellationName constellationName)
        {
            switch (constellationName)
            {
                case ConstellationName.Krithika:
                case ConstellationName.Uttara:
                case ConstellationName.Uttarashada:
                    return Library.PlanetName.Sun;

                case ConstellationName.Rohini:
                case ConstellationName.Hasta:
                case ConstellationName.Sravana:
                    return Library.PlanetName.Moon;

                case ConstellationName.Mrigasira:
                case ConstellationName.Chitta:
                case ConstellationName.Dhanishta:
                    return Library.PlanetName.Mars;

                case ConstellationName.Aridra:
                case ConstellationName.Swathi:
                case ConstellationName.Satabhisha:
                    return Library.PlanetName.Rahu;

                case ConstellationName.Punarvasu:
                case ConstellationName.Vishhaka:
                case ConstellationName.Poorvabhadra:
                    return Library.PlanetName.Jupiter;

                case ConstellationName.Pushyami:
                case ConstellationName.Anuradha:
                case ConstellationName.Uttarabhadra:
                    return Library.PlanetName.Saturn;

                case ConstellationName.Aslesha:
                case ConstellationName.Jyesta:
                case ConstellationName.Revathi:
                    return Library.PlanetName.Mercury;

                case ConstellationName.Makha:
                case ConstellationName.Moola:
                case ConstellationName.Aswini:
                    return Library.PlanetName.Ketu;

                case ConstellationName.Pubba:
                case ConstellationName.Poorvashada:
                case ConstellationName.Bharani:
                    return Library.PlanetName.Venus;
            }

            //if it reaches here something wrong
            throw new Exception("Dasa planet for constellation not found!");
        }

        /// <summary>
        /// Gets the full Dasa years for a given planet
        /// Note: Returns "double" so that division down the road is accurate
        /// Ref:Hindu Predictive Astrology pg. 54
        /// </summary>
        public static double PD1PlanetFullYears(PlanetName planet)
        {

            if (planet == Library.PlanetName.Sun) { return 6.0; }
            if (planet == Library.PlanetName.Moon) { return 10.0; }
            if (planet == Library.PlanetName.Mars) { return 7.0; }
            if (planet == Library.PlanetName.Rahu) { return 18.0; }
            if (planet == Library.PlanetName.Jupiter) { return 16.0; }
            if (planet == Library.PlanetName.Saturn) { return 19.0; }
            if (planet == Library.PlanetName.Mercury) { return 17.0; }
            if (planet == Library.PlanetName.Ketu) { return 7.0; }
            if (planet == Library.PlanetName.Venus) { return 20.0; }

            //if no plant found something wrong
            throw new Exception("Planet not found!");

        }

        /// <summary>
        /// Gets the full years of a bhukti planet in a dasa
        /// </summary>
        public static double PD2PlanetFullYears(PlanetName pd1Planet, PlanetName pd2Planet)
        {
            //120 years is the total of all the dasa planet's years
            const double fullHumanLifeYears = 120.0;

            //the time a bhukti planet consumes in a dasa is
            //a fixed percentage it consumes in a person's full life
            var bhuktiPlanetPercentage = PD1PlanetFullYears(pd2Planet) / fullHumanLifeYears;

            //bhukti planet's years in a dasa is percentage of the dasa planet's full years
            var bhuktiPlanetFullYears = bhuktiPlanetPercentage * PD1PlanetFullYears(pd1Planet);

            //return the calculated value
            return bhuktiPlanetFullYears;

        }

        /// <summary>
        /// Gets the full years of an antaram planet in a bhukti of a dasa
        /// </summary>
        public static double PD3PlanetFullYears(PlanetName pd1Planet, PlanetName pd2Planet, PlanetName pd3Planet)
        {
            //120 years is the total of all the dasa planet's years
            const double fullHumanLifeYears = 120.0;

            //the time an antaram planet consumes in a bhukti is
            //a fixed percentage it consumes in a person's full life
            var antaramPlanetPercentage = PD1PlanetFullYears(pd3Planet) / fullHumanLifeYears;

            //Antaram planet's full years is a percentage of the Bhukti planet's full years
            var antaramPlanetFullYears = antaramPlanetPercentage * PD2PlanetFullYears(pd1Planet, pd2Planet);

            //return the calculated value
            return antaramPlanetFullYears;

        }

        /// <summary>
        /// Gets the full time of an Sukshma planet 
        /// Sukshma is a Sanskrit word meaning "subtle" or "dormant." The presence of sukshma is felt, but not seen.
        /// </summary>
        public static double PD4PlanetFullYears(PlanetName pd1Planet, PlanetName pd2Planet, PlanetName pd3Planet, PlanetName pd4Planet)
        {
            //120 years is the total of all the dasa planet's years
            const double fullHumanLifeYears = 120.0;

            //the time an sukshma planet consumes in a antaram is
            //a fixed percentage it consumes in a person's full life
            var sukshmaPlanetPercentage = PD1PlanetFullYears(pd4Planet) / fullHumanLifeYears;

            //sukshma planet's full years is a percentage of the Antaram planet's full years
            var sukshmaPlanetFullYears = sukshmaPlanetPercentage * PD3PlanetFullYears(pd1Planet, pd2Planet, pd3Planet);

            //return the calculated value
            return sukshmaPlanetFullYears;

        }

        /// <summary>
        /// Gets the full time of an Prana planet 
        /// </summary>
        public static double PD5PlanetFullYears(PlanetName pd1Planet, PlanetName pd2Planet, PlanetName pd3Planet, PlanetName pd4Planet, PlanetName pd5Planet)
        {
            //120 years is the total of all the dasa planet's years
            const double fullHumanLifeYears = 120.0;

            //the time an PD5 planet consumes in a PD4 is
            //a fixed percentage it consumes in a person's full life
            var pd5PlanetPercentage = PD1PlanetFullYears(pd5Planet) / fullHumanLifeYears;

            //Prana planet's full time is a percentage of the Sukshma planet's full time
            var pd5PlanetFullTime = pd5PlanetPercentage * PD4PlanetFullYears(pd1Planet, pd2Planet, pd3Planet, pd4Planet);

            //return the calculated value
            return pd5PlanetFullTime;

        }

        public static double PD6PlanetFullYears(PlanetName pd1Planet, PlanetName pd2Planet, PlanetName pd3Planet, PlanetName pd4Planet, PlanetName pd5Planet, PlanetName pd6Planet)
        {
            //120 years is the total of all the dasa planet's years
            const double fullHumanLifeYears = 120.0;

            //the time an PD6 planet consumes in a PD5 is
            //a fixed percentage it consumes in a person's full life
            var pd6PlanetPercentage = PD1PlanetFullYears(pd6Planet) / fullHumanLifeYears;

            //Prana planet's full time is a percentage of the Sukshma planet's full time
            var pd6PlanetFullTime = pd6PlanetPercentage * PD5PlanetFullYears(pd1Planet, pd2Planet, pd3Planet, pd4Planet, pd5Planet);

            //return the calculated value
            return pd6PlanetFullTime;

        }

        public static double PD7PlanetFullYears(PlanetName pd1Planet, PlanetName pd2Planet, PlanetName pd3Planet, PlanetName pd4Planet, PlanetName pd5Planet, PlanetName pd6Planet, PlanetName pd7Planet)
        {
            //120 years is the total of all the dasa planet's years
            const double fullHumanLifeYears = 120.0;

            //the time an PD7 planet consumes in a PD6 is
            //a fixed percentage it consumes in a person's full life
            var pd7PlanetPercentage = PD1PlanetFullYears(pd7Planet) / fullHumanLifeYears;

            //Prana planet's full time is a percentage of the Sukshma planet's full time
            var pd7PlanetFullTime = pd7PlanetPercentage * PD6PlanetFullYears(pd1Planet, pd2Planet, pd3Planet, pd4Planet, pd5Planet, pd6Planet);

            //return the calculated value
            return pd7PlanetFullTime;

        }

        public static double PD8PlanetFullYears(PlanetName pd1Planet, PlanetName pd2Planet, PlanetName pd3Planet, PlanetName pd4Planet, PlanetName pd5Planet, PlanetName pd6Planet, PlanetName pd7Planet, PlanetName pd8Planet)
        {
            //120 years is the total of all the dasa planet's years
            const double fullHumanLifeYears = 120.0;

            //the time an PD8 planet consumes in a PD7 is
            //a fixed percentage it consumes in a person's full life
            var pd8PlanetPercentage = PD1PlanetFullYears(pd8Planet) / fullHumanLifeYears;

            //PD8 planet's full time is a percentage of the Sukshma planet's full time
            var pd8PlanetFullTime = pd8PlanetPercentage * PD7PlanetFullYears(pd1Planet, pd2Planet, pd3Planet, pd4Planet, pd5Planet, pd6Planet, pd7Planet);

            //return the calculated value
            return pd8PlanetFullTime;

        }

    }
}
