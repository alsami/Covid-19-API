FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS publish
WORKDIR /app
COPY . .
RUN dotnet publish ./src/Covid19Api/Covid19Api.csproj -c Release -o publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim
EXPOSE 80
EXPOSE 443
EXPOSE 8080
WORKDIR /app
COPY --from=publish /app/publish .
RUN ls .
ENTRYPOINT ["./Covid19Api"]
