using Covid19Api.Presentation.Response;
using Covid19Api.UseCases.Abstractions.Queries.Countries;
using MediatR;

namespace Covid19Api.UseCases.Queries.Countries;

public class LoadCountryFlagQueryHandler : IRequestHandler<LoadCountryFlagQuery, ImageDto>
{
    public async Task<ImageDto> Handle(LoadCountryFlagQuery request, CancellationToken cancellationToken)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "CountryFlags",
            $"{request.CountryCode.ToLowerInvariant()}.svg");

        var image = await File.ReadAllBytesAsync(path, cancellationToken);

        return new ImageDto(image, "image/svg+xml");
    }
}