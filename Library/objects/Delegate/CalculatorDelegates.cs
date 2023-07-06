namespace VedAstro.Library
{
    /// <summary>
    /// Mask-method for Muhurtha event calculator methods
    /// Event calculators must match this methods signature else won't be detected
    /// </summary>
    public delegate CalculatorResult EventCalculatorDelegate(Time eventTime, Person person);

    /// <summary>
    /// Mask-method for Horoscope calculator methods
    /// Horoscope calculators must match this methods signature else won't be detected
    /// </summary>
    public delegate CalculatorResult HoroscopeCalculatorDelegate(Time birthTime);

}