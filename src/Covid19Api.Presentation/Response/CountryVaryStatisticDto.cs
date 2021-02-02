namespace Covid19Api.Presentation.Response
{
    public record CountryVaryStatisticDto(string VaryType, double? Vary, int? ValueYesterday, int ValueToday);
}