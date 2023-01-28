using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwissEphNet;
using System.Reflection;

namespace Jothidam
{
    public class DateSpan
    {
        public DateTimeOffset start;
        public DateTimeOffset end;

        public override string ToString()
        {


            return string.Format("{0}/{1}/{2} : {3} to {4}", this.start.Day, this.start.Month, this.start.Year, this.start.ToString("hh:mm:ss tt"), this.end.ToString("hh:mm:ss tt"));
        }
    }
    public enum FilterKind
    {
        Find = 1,
        Remove
    }
    
    public enum Hemisphere
    {
        Zero = '\0',
        West = 'W',
        East = 'E',
        North = 'N',
        South = 'S'
    }

    public enum PlanetName
    {
        Sun = 1,
        Moon,
        Mars,
        Mercury,
        Jupiter,
        Venus,
        Saturn,
        Rahu,
        Ketu
    }

    public enum PlanetMotion
    {
        Direct = 1,
        Retrograde = 2
    }

    public enum DateSearchKind
    {
        Range,
        List
    }
    
    public enum Constellation
    {
        Aswini = 1,
        Bharani = 2,
        Krithika = 3,
        Rohini = 4,
        Mrigasira = 5,
        Aridra = 6,
        Punarvasu = 7,
        Pushyami = 8,
        Aslesha = 9,
        Makha = 10,
        Pubba = 11,
        Uttara = 12,
        Hasta = 13,
        Chitta = 14,
        Swathi = 15,
        Vishhaka = 16,
        Anuradha = 17,
        Jyesta = 18,
        Moola = 19,
        Poorvashada = 20,
        Uttarashada = 21,
        Sravana = 22,
        Dhanishta = 23,
        Satabhisha = 24,
        Poorvabhadra = 25,
        Uttarabhadra = 26,
        Revathi = 27
    }
    
    public enum Zodiac //Rasi  
    {
        Aries = 1,
        Taurus = 2,
        Gemini = 3,
        Cancer = 4,
        Leo = 5,
        Virgo = 6,
        Libra = 7,
        Scorpio = 8,
        Sagittarius = 9,
        Capricornus = 10,
        Aquarius = 11,
        Pisces = 12
    }

    public enum StandardTimeKind
    {
        EET = 1,
        Malaysia,
        India
    }
   
    [Flags]
    public enum TravelDirection
    {
        None = 0,
        East = 1,
        West = 2,
        North = 4,
        South = 8
    }

    [Flags]
    public enum TravelCriteria
    {
        None = 0,
        LunarDay01 = 1,
        Constelation01 = 2,
        TravelWest = 4,
        TravelEast = 8,
        TravelNorth = 16,
        TravelSouth = 32
    }

    public class Angle
    {
        //FIELDS
        public const long SecondsPerMinute = 60;
        private const double MinutesPerSecond = 1.0 / SecondsPerMinute; //0.01666666

        public const long SecondsPerDegree = SecondsPerMinute * 60; //3600
        private const double DegreesPerSecond = 1.0 / SecondsPerDegree; //0.000277777777

        private const double DegreesPerRadian = 180.0 / Math.PI; //57.2957795 multiply for Rad to Deg

        public static readonly Angle Zero = new Angle(0,0,0);

        private long _seconds;

        //CTOR
        public Angle(double degrees, double minutes, long seconds)
        {
            this._seconds = (long)(degrees * SecondsPerDegree) + (long)(minutes * SecondsPerMinute) + seconds;
        }

        //PROPERTIES
 
        public long Degrees
        {
            get { return this._seconds / SecondsPerDegree; }
        }
        public long Minutes
        {
            get { return (this._seconds % SecondsPerDegree) / SecondsPerMinute; }
        }
        public long Seconds
        {
            get { return (this._seconds % SecondsPerDegree) % SecondsPerMinute; }
        }
        public double TotalMinutes
        {
            get { return this._seconds * MinutesPerSecond; }
        }
        public double TotalDegrees
        {
            get { return this._seconds * DegreesPerSecond; }
        }
        public double TotalRadians
        {
            get { return TotalDegrees / DegreesPerRadian; }
        }
        public long ZodiacDegrees
        {
            get { return (long)((Degrees % 360.0) % 30.0); }
        }

        //METHODS
        
        public static Angle FromDegrees(double value)
        {
            return new Angle(value,0,0);
        }
        public static Angle FromMinutes(double value)
        {
            return new Angle(0, value, 0);
        }
        public static Angle FromSeconds(long value)
        {
            return new Angle(0,0,value);
        }
        public static Angle FromRadians(double value)
        {
            return new Angle(value * DegreesPerRadian, 0, 0);
        }
        public TimeSpan ToAngleTime()
        {
            return TimeSpan.FromHours(this.TotalDegrees/15);
        }
        

        //OPERATOR OVERLOADS
        public static Angle operator +(Angle a1, Angle a2)
        {
            return Angle.FromSeconds(a1._seconds + a2._seconds);
        }
        public static Angle operator -(Angle a1, Angle a2)
        {
            return Angle.FromSeconds(a1._seconds - a2._seconds);
        }


        
        //METHOD OVERRIDES
        public override string ToString()
        {  
           return String.Format("{0}°{1}'{2}\"", this.Degrees, Math.Abs(this.Minutes), Math.Abs(this.Seconds)); //Only degrees is in negative
        }
        public string ToString(int flag = 0)
        {   
            return String.Format("{0}°{1}'{2}\"", this.ZodiacDegrees, this.Minutes, this.Seconds);//Prints in zodiac degrees
        }

      }

    public class Charakhanda
    {
        //FIELDS
        public readonly Angle Latitude;
        public readonly Asu Group1;
        public readonly Asu Group2;
        public readonly Asu Group3;

        //CTOR
        public Charakhanda(Angle latitude)
        {
            this.Latitude = latitude;

            switch ((long)Math.Round(this.Latitude.TotalDegrees))
            {
                case 0: Group1 = (Asu)new GhatisTime(vighatis: 0); Group2 = (Asu)new GhatisTime(vighatis: 0); Group3 = (Asu)new GhatisTime(vighatis: 0); break;
                case 1: Group1 = (Asu)new GhatisTime(vighatis: 2.1); Group2 = (Asu)new GhatisTime(vighatis: 1.68); Group3 = (Asu)new GhatisTime(vighatis: 0.73); break;
                case 2: Group1 = (Asu)new GhatisTime(vighatis: 4.2); Group2 = (Asu)new GhatisTime(vighatis: 3.36); Group3 = (Asu)new GhatisTime(vighatis: 1.4); break;
                case 3: Group1 = (Asu)new GhatisTime(vighatis: 6.3); Group2 = (Asu)new GhatisTime(vighatis: 5.04); Group3 = (Asu)new GhatisTime(vighatis: 2.1); break;
                case 4: Group1 = (Asu)new GhatisTime(vighatis: 8.4); Group2 = (Asu)new GhatisTime(vighatis: 6.72); Group3 = (Asu)new GhatisTime(vighatis: 2.8); break;
                case 5: Group1 = (Asu)new GhatisTime(vighatis: 10.5); Group2 = (Asu)new GhatisTime(vighatis: 8.4); Group3 = (Asu)new GhatisTime(vighatis: 3.5); break;
                case 6: Group1 = (Asu)new GhatisTime(vighatis: 12.6); Group2 = (Asu)new GhatisTime(vighatis: 10.08); Group3 = (Asu)new GhatisTime(vighatis: 4.2); break;
                case 7: Group1 = (Asu)new GhatisTime(vighatis: 14.7); Group2 = (Asu)new GhatisTime(vighatis: 11.76); Group3 = (Asu)new GhatisTime(vighatis: 4.9); break;
                case 8: Group1 = (Asu)new GhatisTime(vighatis: 16.9); Group2 = (Asu)new GhatisTime(vighatis: 13.52); Group3 = (Asu)new GhatisTime(vighatis: 5.63); break;
                case 9: Group1 = (Asu)new GhatisTime(vighatis: 19.0); Group2 = (Asu)new GhatisTime(vighatis: 15.2); Group3 = (Asu)new GhatisTime(vighatis: 6.33); break;
                case 10: Group1 = (Asu)new GhatisTime(vighatis: 21.2); Group2 = (Asu)new GhatisTime(vighatis: 16.96); Group3 = (Asu)new GhatisTime(vighatis: 7.06); break;
                case 11: Group1 = (Asu)new GhatisTime(vighatis: 23.3); Group2 = (Asu)new GhatisTime(vighatis: 18.64); Group3 = (Asu)new GhatisTime(vighatis: 7.76); break;
                case 12: Group1 = (Asu)new GhatisTime(vighatis: 25.5); Group2 = (Asu)new GhatisTime(vighatis: 20.4); Group3 = (Asu)new GhatisTime(vighatis: 8.5); break;
                case 13: Group1 = (Asu)new GhatisTime(vighatis: 27.0); Group2 = (Asu)new GhatisTime(vighatis: 21.7); Group3 = (Asu)new GhatisTime(vighatis: 8.8); break;
                case 14: Group1 = (Asu)new GhatisTime(vighatis: 29.9); Group2 = (Asu)new GhatisTime(vighatis: 23.92); Group3 = (Asu)new GhatisTime(vighatis: 9.96); break;
                case 15: Group1 = (Asu)new GhatisTime(vighatis: 32.1); Group2 = (Asu)new GhatisTime(vighatis: 25.68); Group3 = (Asu)new GhatisTime(vighatis: 10.7); break;
                case 16: Group1 = (Asu)new GhatisTime(vighatis: 34.4); Group2 = (Asu)new GhatisTime(vighatis: 27.52); Group3 = (Asu)new GhatisTime(vighatis: 11.46); break;
                case 17: Group1 = (Asu)new GhatisTime(vighatis: 36.6); Group2 = (Asu)new GhatisTime(vighatis: 29.28); Group3 = (Asu)new GhatisTime(vighatis: 12.2); break;
                case 18: Group1 = (Asu)new GhatisTime(vighatis: 39.0); Group2 = (Asu)new GhatisTime(vighatis: 31.2); Group3 = (Asu)new GhatisTime(vighatis: 13.0); break;
                case 19: Group1 = (Asu)new GhatisTime(vighatis: 41.3); Group2 = (Asu)new GhatisTime(vighatis: 33.04); Group3 = (Asu)new GhatisTime(vighatis: 13.76); break;
                case 20: Group1 = (Asu)new GhatisTime(vighatis: 43.7); Group2 = (Asu)new GhatisTime(vighatis: 34.96); Group3 = (Asu)new GhatisTime(vighatis: 14.56); break;
                case 21: Group1 = (Asu)new GhatisTime(vighatis: 46.0); Group2 = (Asu)new GhatisTime(vighatis: 36.8); Group3 = (Asu)new GhatisTime(vighatis: 15.33); break;
                case 22: Group1 = (Asu)new GhatisTime(vighatis: 48.5); Group2 = (Asu)new GhatisTime(vighatis: 38.8); Group3 = (Asu)new GhatisTime(vighatis: 16.16); break;
                case 23: Group1 = (Asu)new GhatisTime(vighatis: 50.9); Group2 = (Asu)new GhatisTime(vighatis: 40.72); Group3 = (Asu)new GhatisTime(vighatis: 16.96); break;
                case 24: Group1 = (Asu)new GhatisTime(vighatis: 53.4); Group2 = (Asu)new GhatisTime(vighatis: 42.72); Group3 = (Asu)new GhatisTime(vighatis: 17.8); break;
                case 25: Group1 = (Asu)new GhatisTime(vighatis: 55.9); Group2 = (Asu)new GhatisTime(vighatis: 44.72); Group3 = (Asu)new GhatisTime(vighatis: 18.63); break;
                case 26: Group1 = (Asu)new GhatisTime(vighatis: 58.5); Group2 = (Asu)new GhatisTime(vighatis: 46.8); Group3 = (Asu)new GhatisTime(vighatis: 19.5); break;
                case 27: Group1 = (Asu)new GhatisTime(vighatis: 61.1); Group2 = (Asu)new GhatisTime(vighatis: 48.88); Group3 = (Asu)new GhatisTime(vighatis: 20.36); break;
                case 28: Group1 = (Asu)new GhatisTime(vighatis: 63.8); Group2 = (Asu)new GhatisTime(vighatis: 51.04); Group3 = (Asu)new GhatisTime(vighatis: 21.26); break;
                case 29: Group1 = (Asu)new GhatisTime(vighatis: 66.5); Group2 = (Asu)new GhatisTime(vighatis: 53.2); Group3 = (Asu)new GhatisTime(vighatis: 22.16); break;
                case 30: Group1 = (Asu)new GhatisTime(vighatis: 69.3); Group2 = (Asu)new GhatisTime(vighatis: 55.44); Group3 = (Asu)new GhatisTime(vighatis: 23.1); break;
                case 31: Group1 = (Asu)new GhatisTime(vighatis: 72.1); Group2 = (Asu)new GhatisTime(vighatis: 57.68); Group3 = (Asu)new GhatisTime(vighatis: 24.33); break;
                case 32: Group1 = (Asu)new GhatisTime(vighatis: 75.0); Group2 = (Asu)new GhatisTime(vighatis: 60.0); Group3 = (Asu)new GhatisTime(vighatis: 25.0); break;
                case 33: Group1 = (Asu)new GhatisTime(vighatis: 77.9); Group2 = (Asu)new GhatisTime(vighatis: 62.32); Group3 = (Asu)new GhatisTime(vighatis: 25.96); break;
                case 34: Group1 = (Asu)new GhatisTime(vighatis: 80.9); Group2 = (Asu)new GhatisTime(vighatis: 64.72); Group3 = (Asu)new GhatisTime(vighatis: 26.96); break;
                case 35: Group1 = (Asu)new GhatisTime(vighatis: 84.0); Group2 = (Asu)new GhatisTime(vighatis: 67.20); Group3 = (Asu)new GhatisTime(vighatis: 28.00); break;
                case 36: Group1 = (Asu)new GhatisTime(vighatis: 87.1); Group2 = (Asu)new GhatisTime(vighatis: 69.68); Group3 = (Asu)new GhatisTime(vighatis: 29.03); break;
                case 37: Group1 = (Asu)new GhatisTime(vighatis: 90.4); Group2 = (Asu)new GhatisTime(vighatis: 72.32); Group3 = (Asu)new GhatisTime(vighatis: 30.13); break;
                case 38: Group1 = (Asu)new GhatisTime(vighatis: 93.7); Group2 = (Asu)new GhatisTime(vighatis: 74.96); Group3 = (Asu)new GhatisTime(vighatis: 31.23); break;
                case 39: Group1 = (Asu)new GhatisTime(vighatis: 97.2); Group2 = (Asu)new GhatisTime(vighatis: 77.76); Group3 = (Asu)new GhatisTime(vighatis: 32.4); break;
                case 40: Group1 = (Asu)new GhatisTime(vighatis: 100.6); Group2 = (Asu)new GhatisTime(vighatis: 80.48); Group3 = (Asu)new GhatisTime(vighatis: 33.53); break;
                case 41: Group1 = (Asu)new GhatisTime(vighatis: 104.3); Group2 = (Asu)new GhatisTime(vighatis: 83.44); Group3 = (Asu)new GhatisTime(vighatis: 34.73); break;
                case 42: Group1 = (Asu)new GhatisTime(vighatis: 108.0); Group2 = (Asu)new GhatisTime(vighatis: 86.4); Group3 = (Asu)new GhatisTime(vighatis: 36.0); break;
                case 43: Group1 = (Asu)new GhatisTime(vighatis: 111.9); Group2 = (Asu)new GhatisTime(vighatis: 89.52); Group3 = (Asu)new GhatisTime(vighatis: 37.3); break;
                case 44: Group1 = (Asu)new GhatisTime(vighatis: 115.8); Group2 = (Asu)new GhatisTime(vighatis: 92.64); Group3 = (Asu)new GhatisTime(vighatis: 38.6); break;
                case 45: Group1 = (Asu)new GhatisTime(vighatis: 120.0); Group2 = (Asu)new GhatisTime(vighatis: 96.0); Group3 = (Asu)new GhatisTime(vighatis: 40.0); break;
                case 46: Group1 = (Asu)new GhatisTime(vighatis: 124.2); Group2 = (Asu)new GhatisTime(vighatis: 99.36); Group3 = (Asu)new GhatisTime(vighatis: 41.4); break;
                case 47: Group1 = (Asu)new GhatisTime(vighatis: 128.7); Group2 = (Asu)new GhatisTime(vighatis: 102.96); Group3 = (Asu)new GhatisTime(vighatis: 42.9); break;
                case 48: Group1 = (Asu)new GhatisTime(vighatis: 133.3); Group2 = (Asu)new GhatisTime(vighatis: 106.64); Group3 = (Asu)new GhatisTime(vighatis: 44.43); break;
                case 49: Group1 = (Asu)new GhatisTime(vighatis: 138.0); Group2 = (Asu)new GhatisTime(vighatis: 110.4); Group3 = (Asu)new GhatisTime(vighatis: 46.0); break;
                case 50: Group1 = (Asu)new GhatisTime(vighatis: 143.0); Group2 = (Asu)new GhatisTime(vighatis: 114.4); Group3 = (Asu)new GhatisTime(vighatis: 47.66); break;
                case 51: Group1 = (Asu)new GhatisTime(vighatis: 148.2); Group2 = (Asu)new GhatisTime(vighatis: 118.56); Group3 = (Asu)new GhatisTime(vighatis: 49.4); break;
                case 52: Group1 = (Asu)new GhatisTime(vighatis: 153.5); Group2 = (Asu)new GhatisTime(vighatis: 122.83); Group3 = (Asu)new GhatisTime(vighatis: 51.17); break;
                case 53: Group1 = (Asu)new GhatisTime(vighatis: 159.2); Group2 = (Asu)new GhatisTime(vighatis: 127.36); Group3 = (Asu)new GhatisTime(vighatis: 53.06); break;
                case 54: Group1 = (Asu)new GhatisTime(vighatis: 165.2); Group2 = (Asu)new GhatisTime(vighatis: 132.16); Group3 = (Asu)new GhatisTime(vighatis: 55.06); break;
                case 55: Group1 = (Asu)new GhatisTime(vighatis: 171.3); Group2 = (Asu)new GhatisTime(vighatis: 137.04); Group3 = (Asu)new GhatisTime(vighatis: 57.1); break;
                case 56: Group1 = (Asu)new GhatisTime(vighatis: 177.9); Group2 = (Asu)new GhatisTime(vighatis: 142.32); Group3 = (Asu)new GhatisTime(vighatis: 59.3); break;
                case 57: Group1 = (Asu)new GhatisTime(vighatis: 184.6); Group2 = (Asu)new GhatisTime(vighatis: 147.84); Group3 = (Asu)new GhatisTime(vighatis: 61.6); break;
                case 58: Group1 = (Asu)new GhatisTime(vighatis: 192.0); Group2 = (Asu)new GhatisTime(vighatis: 153.6); Group3 = (Asu)new GhatisTime(vighatis: 64.0); break;
                case 59: Group1 = (Asu)new GhatisTime(vighatis: 199.7); Group2 = (Asu)new GhatisTime(vighatis: 159.76); Group3 = (Asu)new GhatisTime(vighatis: 66.56); break;
                case 60: Group1 = (Asu)new GhatisTime(vighatis: 207.8); Group2 = (Asu)new GhatisTime(vighatis: 166.24); Group3 = (Asu)new GhatisTime(vighatis: 69.26); break;
                
            }
        }
    }

    public class Rasimana
    {
        public readonly Angle Latitude;
        public readonly string Hemisphere;
        public readonly Asu Aries = 1674, Virgo = 1674, Libra = 1674, Pisces = 1674;             //GROUP1
        public readonly Asu Taurus = 1795, Leo = 1795, Scorpio = 1795, Aquarius = 1795;          //GROUP2     //Could be 1725 as on pg.63
        public readonly Asu Gemini = 1931, Cancer = 1931, Sagittarius = 1931, Capricorn = 1931;  //GROUP3
        private readonly Charakhanda _charakhanda;

        public Rasimana(Angle latitude, string hemisphere)
        {
            this.Latitude = latitude;
            this.Hemisphere = hemisphere;
            this._charakhanda = new Charakhanda(latitude);

            switch (this.Hemisphere)
            {
                case "N":
                    Aries -= _charakhanda.Group1;
                    Taurus -= _charakhanda.Group2;
                    Gemini -= _charakhanda.Group3;
                    Cancer += _charakhanda.Group3;
                    Leo += _charakhanda.Group2;
                    Virgo += _charakhanda.Group1;
                    Libra += _charakhanda.Group1;
                    Scorpio += _charakhanda.Group2;
                    Sagittarius += _charakhanda.Group3;
                    Capricorn -= _charakhanda.Group3;
                    Aquarius -= _charakhanda.Group2;
                    Pisces -= _charakhanda.Group1;
                    break;
                case "S":
                    Aries += _charakhanda.Group1;
                    Taurus += _charakhanda.Group2;
                    Gemini += _charakhanda.Group3;
                    Cancer -= _charakhanda.Group3;
                    Leo -= _charakhanda.Group2;
                    Virgo -= _charakhanda.Group1;
                    Libra -= _charakhanda.Group1;
                    Scorpio -= _charakhanda.Group2;
                    Sagittarius -= _charakhanda.Group3;
                    Capricorn += _charakhanda.Group3;
                    Aquarius += _charakhanda.Group2;
                    Pisces += _charakhanda.Group1;
                    break;
                
            }

        }
    }

    public struct GhatisTime //Note : Data type optimization needed
    {
        //FIELDS
        public const long TatparasPerPara = 60;
        private const double ParasPerTatpara = 1.0 / TatparasPerPara;

        public const long TatparasPerViliptha = TatparasPerPara * 60;
        private const double VilipthasPerTatpara = 1.0 / TatparasPerViliptha;

        public const long TatparasPerLiptha = TatparasPerViliptha * 60;
        private const double LipthasPerTatpara = 1.0 / TatparasPerViliptha;

        public const long TatparasPerVighati = TatparasPerLiptha * 60;
        private const double VighatisPerTatpara = 1.0 / TatparasPerVighati;

        public const long TatparasPerGhati = TatparasPerVighati * 60;
        private const double GhatisPerTatpara = 1.0 / TatparasPerGhati;

        public const long TatparasPerDay = TatparasPerGhati * 60;
        private const double DaysPerTatpara = 1.0 / TatparasPerDay;

        private double _tatparas;

        //CTOR
        public GhatisTime(double days = 0, double ghatis = 0, double vighatis = 0, double lipthas = 0, double vilipthas = 0, double paras = 0, double tatparas = 0)
        {
            //convert all to tatparas
            this._tatparas = (days*TatparasPerDay)+(ghatis*TatparasPerGhati)+(vighatis*TatparasPerVighati)+(lipthas*TatparasPerLiptha)+(vilipthas*TatparasPerViliptha)+(paras*TatparasPerPara)+tatparas;
        }

        //PROPERTIES
        public double TotalTatparas
        {
            get { return this._tatparas; }
        }
        public double TotalVighatis
        {
            get { return this._tatparas * VighatisPerTatpara; }
        }

        //METHODS
        public string ToGhatisVighatis()
        {
            long ghatis; // "Long" cause no decimal places here
            double vighatis;
            double remaining;

            ghatis = (long)(this._tatparas / TatparasPerGhati);
            remaining = this._tatparas % TatparasPerGhati;

            vighatis = remaining / TatparasPerVighati;
            remaining = remaining % TatparasPerVighati;
            //Console.WriteLine(remaining);
            return string.Format("{0}Gh. {1}Vig.", ghatis, vighatis);
        }

        //METHOD OVERRIDES
        public override string ToString()
        {
            return String.Format("{0} Tatparas",this._tatparas);
        }

        //OPERATOR OVERLOADS
        public static explicit operator GhatisTime(Asu a1)
        {
            return new GhatisTime(vighatis: a1 / 6.0);
        }
        
    }

    public struct Asu
    {
        private double _asu;
        
        //CTOR
        public Asu(double a1)
        {
            this._asu = a1;
        }

        //METHODS 

        //METHOD OVERRIDES
        public override string ToString()
        {
            return String.Format("{0}",this._asu);
        }

        //OVERLOADED OPERATORS
        public static Asu operator +(Asu a1, Asu a2)
        {
            return new Asu(a1._asu + a2._asu);
        }
        public static double operator +(Asu a1, double d1)
        {
            return a1._asu + d1;
        }
        public static double operator +(double d1, Asu a1)
        {
            return d1 + a1._asu;
        }
        public static Asu operator -(Asu a1, Asu a2)
        {
            return new Asu(a1._asu - a2._asu);
        }
        public static double operator -(Asu a1, double d1)
        {
            return a1._asu - d1;
        }
        public static double operator -(double d1, Asu a1)
        {
            return d1 - a1._asu;
        }
        public static Asu operator /(Asu a1, Asu a2)
        {
            return new Asu(a1._asu / a2._asu);
        }
        public static double operator /(Asu a1, double d1)
        {
            return a1._asu / d1;
        }
        public static double operator /(double d1, Asu a1)
        {
            return d1 / a1._asu;
        }
        public static implicit operator Asu(double i)
        {
            return new Asu(i);
        }
        public static explicit operator Asu(GhatisTime g1)
        {
            return new Asu(g1.TotalVighatis * 6);
        }

    }

    public class GeoCoordinate 
    {
        //FILEDS
        private double _longitude;
        private double _latitude;

        public GeoCoordinate(double longitude = 0, double latitude = 0)
        {
            this._longitude = longitude;
            this._latitude = latitude;
        }

        public GeoCoordinate(Angle latitude = null, Hemisphere latitudeHemisphere = Hemisphere.Zero, Angle longitude = null, Hemisphere longitudeHemisphere = Hemisphere.Zero)
        {
            if (latitude == null)
                this.ChangeLatitude(Angle.Zero, Hemisphere.Zero);
            else
                this.ChangeLatitude(latitude, latitudeHemisphere);

            if (longitude == null)
                this.ChangeLongitude(Angle.Zero, Hemisphere.Zero);
            else
                this.ChangeLongitude(longitude, longitudeHemisphere);
        }


        //PROPERTIES
        public Angle Longitude
        {
            get { return Angle.FromDegrees(Math.Abs(this._longitude)); }
        }
        public Angle Latitude
        {
            get { return Angle.FromDegrees(Math.Abs(this._latitude)); }
        }

        public Hemisphere LongitudeHemisphere
        {
            get { return (this._longitude == 0) ? Hemisphere.Zero : (this._longitude < 0) ? Hemisphere.West : Hemisphere.East; }
        }
        public Hemisphere LatitudeHemisphere
        {
            get { return (this._latitude == 0) ? Hemisphere.Zero : (this._latitude < 0) ? Hemisphere.South : Hemisphere.North; }
        }

        public TimeSpan LMToffset
        {
            
            get 
            {
                TimeSpan offset = TimeSpan.Zero;
                
                switch (LongitudeHemisphere)
                {
                    case Hemisphere.Zero:
                        offset = TimeSpan.FromHours(this.Longitude.TotalDegrees / 15.0);
                        break;
                    case Hemisphere.East:
                        offset = TimeSpan.FromHours(this.Longitude.TotalDegrees / 15.0);
                        break;
                    case Hemisphere.West:
                        offset = TimeSpan.FromHours(-(this.Longitude.TotalDegrees / 15.0));
                        break;
                }

                offset = TimeSpan.FromMinutes(Math.Round(offset.TotalMinutes)); //Rounding off to full minutes

                return offset;
            }
        }

        //METHODS
        private void ChangeLongitude(Angle longitude, Hemisphere hemisphere)
        {
            //Gets absolute angle value
            Angle absLongitude = Angle.FromDegrees(Math.Abs(longitude.TotalDegrees));

            this._longitude = (hemisphere == Hemisphere.Zero) ? 0 : (hemisphere == Hemisphere.West) ? -absLongitude.TotalDegrees : absLongitude.TotalDegrees;
        }

        private void ChangeLatitude(Angle latitude, Hemisphere hemisphere)
        {
            //Gets absolute angle value
            Angle absLatitude = Angle.FromDegrees(Math.Abs(latitude.TotalDegrees));
           
            this._latitude = (hemisphere == Hemisphere.Zero) ? 0 : (hemisphere == Hemisphere.South) ? -absLatitude.TotalDegrees : absLatitude.TotalDegrees;
        }
       

        //METHOD OVERRIDES
        public override string ToString()
        {
                return string.Format("{0}{1} {2}{3}",Latitude,(char)LatitudeHemisphere,Longitude,(char)LongitudeHemisphere);
        }

    }

    public class Planet
    {
        public PlanetName Name;
        public Angle Longitude;
        public double Speed;//remove

        public Planet(PlanetName name, Angle longitude, double speed)
        {
            this.Name = name;
            this.Longitude = longitude;
            this.Speed = speed;
       
        }

        //PROP
        public PlanetMotion Motion
        {
            get { return (Speed < 0) ? PlanetMotion.Retrograde : PlanetMotion.Direct; }
        }

        public Zodiac Zodiac
        {
            get { return Tools.GetZodiac(Longitude); }
        }

        //
        public static Planet FromName(PlanetName value)
        {
            return new Planet(value,Angle.Zero,0);
        }

        public Planet ChangeLongitude(Angle value)
        {
            return new Planet(this.Name,value,this.Speed);
        }

        public Planet ChangeSpeed(double value)
        {
            return new Planet(this.Name, this.Longitude, value);
        }


        //MODULE OVERRIDES

        public override string ToString()
        {
            return string.Format("{0} {1}",this.Name,this.Longitude);
        }

    }

    public class House 
    {
        //FIELDS
        public int Number;
        public double Longitude;
        public double Arambha;
        public double Anthya;

        public double Poorva
        {
            get { return (this.Longitude < this.Arambha) ? (this.Longitude + 360) - this.Arambha : this.Longitude - this.Arambha; }
        }

        public double Uttara
        {

            get { return (this.Anthya < this.Longitude) ? (this.Anthya + 360) - this.Longitude : this.Anthya - this.Longitude; }
        }

        public double Length
        {
            get { return this.Poorva + this.Uttara; }
        }
    }

    public struct ZodiacNum
    {
       //Number that loops from 1 to 12
        private const int maxValue = 12;
        private const int minValue = 1;
        private int _value;

        public ZodiacNum(int value)
        {
            this._value = (value > maxValue) ? this._value = minValue : (value < minValue) ? this._value = minValue : this._value = value;
        }

        public static implicit operator ZodiacNum(int value)
        {
            return new ZodiacNum(value);
        }

        public static implicit operator int(ZodiacNum value)
        {
            return value._value;
        }
    }

    public class RulingConstellation
    {
        private readonly double _bodyPosition;
        private const int quarterMax = 4;
        private const int quarterMin = 1;

        public RulingConstellation(double bodyPosition)
        {
            this._bodyPosition = bodyPosition;
        }

        public RulingConstellation(Constellation name, int quarter)
        {
            //Replace with proper argument exception
            if (quarter < quarterMin || quarter > quarterMax)
                Console.WriteLine("Quarter exceeds limit");

            double constellationPositionTemp;

            constellationPositionTemp = ((int)name) - 1;

            switch (quarter)
            {   
                case 1 :
                    constellationPositionTemp += 0.125;
                    break;
                case 2 :
                    constellationPositionTemp += 0.385;
                    break;
                case 3:
                    constellationPositionTemp += 0.635;
                    break;
                case 4:
                    constellationPositionTemp += 0.885;
                    break;
            }

            this._bodyPosition = constellationPositionTemp;

        }
        
        public Constellation Name
        {
            get { return (Constellation)(Math.Ceiling(this._bodyPosition)); }
        }

        public int Quarter
        {
            get
            {
                double remainder = this._bodyPosition - Math.Floor(this._bodyPosition);

                if (remainder >= 0 && remainder <= 0.25)
                    return 1;
                else if (remainder > 0.25 && remainder <= 0.5)
                    return 2;
                else if (remainder > 0.5 && remainder <= 0.75)
                    return 3;
                else if (remainder > 0.75 && remainder <= 1)
                    return 4;

                return 0;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Name, Quarter);
        }
    }

    public class Horoscope
    {
        //FIELDS
        public Planet[] Planets;
        public House[] Houses; 
        public DateTimeOffset BirthDate;
        public GeoCoordinate BirthLocation;

       
        //CTOR
        public Horoscope(DateTimeOffset birthDate, GeoCoordinate birthLocation)
        {
            this.BirthDate = birthDate;
            this.BirthLocation = birthLocation;

            Houses = Tools.GetHouses(BirthDate, birthLocation);
            InitializePlanets();
        }
        public Horoscope(DateTime birthDateLMT, GeoCoordinate birthLocation) 
            : this(Tools.DOBFormatter(birthDateLMT, birthLocation), birthLocation)
        {
        }
       
        //PROPERTIES
        public Zodiac JanmaRasi
        {
            get { return Tools.GetRulingZodiac(BirthDate); }
        }

        public Zodiac JanmaLagna
        {
            get { return Tools.GetRisingZodiac(BirthDate,BirthLocation); }
        }

        public RulingConstellation JanmaNakshatra
        {
            get { return Tools.GetRulingConstellation(BirthDate); }
        }

        //METHODS
        private Planet GetPlanet(PlanetName planetName)
        {
            for (int i = 0; i < 9; i++)
            {
                if (planetName == Planets[i].Name)
                    return Planets[i];
            }

            return null; //No planet found
        }

        public House GetHouseByPlanet(PlanetName planetName)
        {
            double plnLongtitude = GetPlanet(planetName).Longitude.TotalDegrees;

            int midHouse=0;

            for (int i = 1; i < 13; i++)
            {
                midHouse = (Houses[i].Anthya > Houses[i].Arambha) ? i : 0;
            }

            for (int i = 1; i < 13; i++)
            {
                if (plnLongtitude > Houses[i].Arambha && plnLongtitude < Houses[i].Anthya)
                {
                    return Houses[i];
                }
            }

            

            return Houses[midHouse];
        }

        //public void FillHouseRasis()
        //{

           
        //    for (int x = 1; x < 13; x++)
        //    {
        //        int extentUp;
        //        int extentDown;

        //        for (int i = 1; i < 13; i++)
        //        {
        //            //Generates house extents
        //            extentDown = i * 30;
        //            extentUp = extentDown - 30;
        //            extentDown = (extentDown == 360) ? extentDown : --extentDown;

        //            if (Houses[x].Longitude >= extentUp && Houses[x].Longitude <= extentDown)
        //                Houses[x].Rasis = (ZodiacSign)i;
        //        }
        //    }

        //}

        public void PrintStuff()
        {
            double poorSum, UttarSum, Lengthsum;

            poorSum = UttarSum = Lengthsum = 0;


            Console.WriteLine("Bhava\tArambha\tMadhya\tAnthya\n");
            for (int i = 1; i < 13; i++)
            {
                Console.WriteLine("{0}\t{1}\t{2}\t{3}", i, Houses[i].Arambha, Houses[i].Longitude, Houses[i].Anthya);
            }

            Console.WriteLine("Bhava\tPoorva\tUttara\tLength\n");
            for (int i = 1; i < 13; i++)
            {
                Console.WriteLine("{0}\t{1}\t{2}\t{3}", i, Houses[i].Poorva, Houses[i].Uttara, Houses[i].Length);
                poorSum += Houses[i].Poorva;
                UttarSum += Houses[i].Uttara;
                Lengthsum += Houses[i].Length;
            }
            Console.WriteLine(poorSum);
            Console.WriteLine(UttarSum);
            Console.WriteLine(Lengthsum);
        }
                        
        private void InitializePlanets()
        {
            Planets = new Planet[9] 
            { 
                Planet.FromName(PlanetName.Sun),
                Planet.FromName(PlanetName.Moon),
                Planet.FromName(PlanetName.Mars),
                Planet.FromName(PlanetName.Mercury),
                Planet.FromName(PlanetName.Jupiter),
                Planet.FromName(PlanetName.Venus),
                Planet.FromName(PlanetName.Saturn),
                Planet.FromName(PlanetName.Rahu),
                Planet.FromName(PlanetName.Ketu)
            };


            for (int i = 0; i < 9; i++)
            {
                Planets[i].Longitude = Tools.GetPlanetNirayanaLongitude(BirthDate, Planets[i].Name);
            }


        }

    }

    public class SearchInput
    {
        public Horoscope Horoscope;
        public GeoCoordinate Location;
        public TravelDirection TravelDirection;

        public SearchInput() 
        {
        }

        public SearchInput(Horoscope horoscope, GeoCoordinate location, TravelDirection travelDirection)
        {
            this.Horoscope = horoscope;
            this.Location = location;
            this.TravelDirection = travelDirection;
        }
    }

    public delegate List<DateTimeOffset> FilterMethodDelegate(List<DateTimeOffset> dateList, Criteria criteria, SearchInput searchInput);

    public class DateFilter
    {
        private readonly FilterKind filterKind;
        private readonly FilterMethodDelegate filterMethodDelegate;
        private readonly Criteria criteria;
        private readonly SearchInput searchInput;
        private readonly int id;

        public DateFilter(int id, Criteria criteria, SearchInput searchInput, FilterKind filterKind, FilterMethodDelegate filterMethodDelegate)
        {
            this.id = id;
            this.filterKind = filterKind;
            this.filterMethodDelegate = filterMethodDelegate;
            this.criteria = criteria;
            this.searchInput = searchInput;

        }

        public List<DateTimeOffset> Process(List<DateTimeOffset> dateList)
        {
            List<DateTimeOffset> returnDates = new List<DateTimeOffset> { };

            switch (this.filterKind)
	        {
		        case FilterKind.Find:
                   
                    returnDates = filterMethodDelegate(dateList, criteria, searchInput);
                    break;
                
                case FilterKind.Remove:
                    
                    List<DateTimeOffset> filteredDates = filterMethodDelegate(dateList, criteria, searchInput);
                    returnDates = new List<DateTimeOffset>(dateList);

                    foreach (DateTimeOffset day in dateList)
                    {
                        foreach (DateTimeOffset x in filteredDates)
                        {
                            if (day == x)
                                returnDates.Remove(day);
                        } 
                    }
                    break;
	        }


            return returnDates;
        }

        public static List<DateFilter> CreateList(SearchInput searchInput, int[,] selectedFilter)
        {
            List<DateFilter> dateFilterList = new List<DateFilter> { };

            DatabaseDataSetTableAdapters.date_filtersTableAdapter dateFilterTblAdapter = new DatabaseDataSetTableAdapters.date_filtersTableAdapter();
            DatabaseDataSet.date_filtersDataTable dateFilterDataTbl = dateFilterTblAdapter.GetData();            
            IEnumerable<DatabaseDataSet.date_filtersRow> dateFilterRowList;
           

            for (int i = 0; i < selectedFilter.GetLength(0); i++)
			{
			    int input_id = selectedFilter[i,0];
                FilterKind filterKind = (FilterKind)selectedFilter[i,1];
                
                //Get method
                dateFilterRowList = dateFilterDataTbl.Where(e => e.id == input_id).OrderBy(e => e.id);
                string methodName = dateFilterRowList.ElementAt(0).method_name;
                MethodInfo methodInfo = typeof(DateFilterMethod).GetMethod(methodName);
                FilterMethodDelegate methodDelegate = (FilterMethodDelegate)FilterMethodDelegate.CreateDelegate(typeof(FilterMethodDelegate), methodInfo);


                DateFilter dateFilter = new DateFilter(input_id, new Criteria(input_id), searchInput, filterKind, methodDelegate);

                dateFilterList.Add(dateFilter);
			}

            return dateFilterList;
        }

    }

    public class Criteria
    {
        public List<int> lunarDayList;
        public List<RulingConstellation> rulingConstellationList;
        public List<Zodiac> zodiacList;
        public List<DayOfWeek> dayOfWeekList;

        public Criteria(int dateFilterId)
        {
            int input_id = dateFilterId;

            //Lunar day
            DatabaseDataSetTableAdapters.lunar_day_criteriasTableAdapter lunarTblAdapter = new DatabaseDataSetTableAdapters.lunar_day_criteriasTableAdapter();
            DatabaseDataSet.lunar_day_criteriasDataTable lunarDataTbl = lunarTblAdapter.GetData();
            IEnumerable<DatabaseDataSet.lunar_day_criteriasRow> lunarRowList = lunarDataTbl.Where(e => e.date_filter_id == input_id).OrderBy(e => e.id);

            if (lunarRowList.Count() > 0)
            {
                lunarDayList = new List<int> { };

                foreach (var i in lunarRowList)
                {
                    lunarDayList.Add(i.lunar_day);
                }
            }


            //Constellation
            DatabaseDataSetTableAdapters.constellation_criteriasTableAdapter constTblAdapter = new DatabaseDataSetTableAdapters.constellation_criteriasTableAdapter();
            DatabaseDataSet.constellation_criteriasDataTable constDataTbl = constTblAdapter.GetData();
            IEnumerable<DatabaseDataSet.constellation_criteriasRow> constRowList = constDataTbl.Where(e => e.date_filter_id == input_id).OrderBy(e => e.id);

            if (constRowList.Count() > 0)
            {
                rulingConstellationList = new List<RulingConstellation> { };

                foreach (var i in constRowList)
                {
                    Constellation constellation_name = new Constellation().Parse(i.constellation_name);

                    rulingConstellationList.Add(new RulingConstellation(constellation_name, i.quarter));
                }
            }


            //Zodiac
            DatabaseDataSetTableAdapters.zodiac_criteriasTableAdapter zodTblAdapter = new DatabaseDataSetTableAdapters.zodiac_criteriasTableAdapter();
            DatabaseDataSet.zodiac_criteriasDataTable zodDataTbl = zodTblAdapter.GetData();
            IEnumerable<DatabaseDataSet.zodiac_criteriasRow> zodRowList = zodDataTbl.Where(e => e.date_filter_id == input_id).OrderBy(e => e.id);

            if (zodRowList.Count() > 0)
            {
                zodiacList = new List<Zodiac> { };

                foreach (var i in zodRowList)
                {
                    zodiacList.Add(new Zodiac().Parse(i.zodiac));
                }
            }

            //Week Day
            DatabaseDataSetTableAdapters.day_of_week_criteriasTableAdapter dayTblAdapter = new DatabaseDataSetTableAdapters.day_of_week_criteriasTableAdapter();
            DatabaseDataSet.day_of_week_criteriasDataTable dayDataTbl = dayTblAdapter.GetData();
            IEnumerable<DatabaseDataSet.day_of_week_criteriasRow> dayRowList = dayDataTbl.Where(e => e.date_filter_id == input_id).OrderBy(e => e.id);

            
            if (dayRowList.Count() > 0)
            {
                dayOfWeekList = new List<DayOfWeek> { };

                foreach (var i in dayRowList)
                {
                    dayOfWeekList.Add(new DayOfWeek().Parse(i.week_day));
                }
            }
        }

    }

}
