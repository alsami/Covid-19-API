using System;
using System.Collections.Generic;

namespace Covid19Api.Presentation.Response
{
    public record CountryVaryStatisticContainerDto(DateTime Time, IEnumerable<CountryVaryStatisticDto> Vary);
}