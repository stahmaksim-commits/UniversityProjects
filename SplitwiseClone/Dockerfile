FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY SplitwiseClone/SplitwiseClone.Domain/SplitwiseClone.Domain.csproj SplitwiseClone/SplitwiseClone.Domain/
COPY SplitwiseClone/SplitwiseClone.Application/SplitwiseClone.Application.csproj SplitwiseClone/SplitwiseClone.Application/
COPY SplitwiseClone/SplitwiseClone.Infrastructure/SplitwiseClone.Infrastructure.csproj SplitwiseClone/SplitwiseClone.Infrastructure/
COPY SplitwiseClone/SplitwiseClone.Api/SplitwiseClone.Api.csproj SplitwiseClone/SplitwiseClone.Api/

RUN dotnet restore SplitwiseClone/SplitwiseClone.Api/SplitwiseClone.Api.csproj

COPY SplitwiseClone/ SplitwiseClone/
RUN dotnet publish SplitwiseClone/SplitwiseClone.Api/SplitwiseClone.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SplitwiseClone.Api.dll"]
