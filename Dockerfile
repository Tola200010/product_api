FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5025

ENV ASPNETCORE_URLS=http://+:5025

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["/Product.Api/ProductApi/ProductApi.csproj", "/Product.Api/ProductApi/"]
COPY ["/Product.Api/ProductApi.UnitTests/ProductApi.UnitTests.csproj", "/Product.Api/ProductApi.UnitTests/"]
RUN dotnet restore "/Product.Api/ProductApi/ProductApi.csproj"
COPY . .
WORKDIR "/src/Product.Api/ProductApi"
RUN dotnet build "ProductApi.csproj" -c Release -o /app/build

#run the unit test
FROM build as test
WORKDIR "/src/Product.Api/ProductApi.UnitTests"
RUN dotnet test --logger:trx

FROM build AS publish
RUN dotnet publish "ProductApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductApi.dll"]
