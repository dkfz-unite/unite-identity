FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS restore
ARG USER
ARG TOKEN
WORKDIR /src
# RUN dotnet nuget add source https://nuget.pkg.github.com/dkfz-unite/index.json -n github -u ${USER} -p ${TOKEN} --store-password-in-clear-text
# COPY ["Unite.Genome.Indices/Unite.Genome.Indices.csproj", "Unite.Genome.Indices/"]
# RUN dotnet restore "Unite.Genome.Indices/Unite.Genome.Indices.csproj"

FROM restore as build
COPY . .
WORKDIR "/src/Unite.Identity.Web"
RUN dotnet build --no-restore "Unite.Identity.Web.csproj" -c Release

FROM build AS publish
RUN dotnet publish --no-build "Unite.Identity.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Unite.Identity.Web.dll"]