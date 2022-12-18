namespace Genso.Astrology.Library
{
    /// <summary>
    /// Mask-method for Muhurtha event calculator methods
    /// </summary>
    public delegate CalculatorResult EventCalculatorDelegate(Time time, Person person);

    /// <summary>
    /// Mask-method for Horoscope calculator methods
    /// </summary>
    public delegate CalculatorResult HoroscopeCalculatorDelegate(Time time, Person person);

}