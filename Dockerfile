FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /src

COPY ["Orcamentaria.PersonService.API/Orcamentaria.PersonService.API.csproj", "Orcamentaria.PersonService.API/"]
COPY ["Orcamentaria.PersonService.Application/Orcamentaria.PersonService.Application.csproj", "Orcamentaria.PersonService.Application/"]
COPY ["Orcamentaria.PersonService.Domain/Orcamentaria.PersonService.Domain.csproj", "Orcamentaria.PersonService.Domain/"]
COPY ["Orcamentaria.PersonService.Infrastructure/Orcamentaria.PersonService.Infrastructure.csproj", "Orcamentaria.PersonService.Infrastructure/"]

COPY nuget.config ./
COPY local-packages ./local-packages

RUN dotnet restore "Orcamentaria.PersonService.API/Orcamentaria.PersonService.API.csproj"

COPY . .

WORKDIR "/src/Orcamentaria.PersonService.API"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Orcamentaria.PersonService.API.dll"]
# ENV ASPNETCORE_URLS=http://+:5000
# EXPOSE 5000