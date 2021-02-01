namespace Covid19Api.Presentation.Response
{
    public record CountryVaryStatisticDto(string ValueType, double? Vary, int? ValueYesterday, int ValueToday);
}