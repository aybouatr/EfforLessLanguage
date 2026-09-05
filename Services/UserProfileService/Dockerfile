# =========================
# BUILD
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copy solution
COPY UserProfileService.sln .

# Copy project files first
COPY UserProfileService.Api/UserProfileService.Api.csproj UserProfileService.Api/
COPY UserProfileService.Business/UserProfileService.Business.csproj UserProfileService.Business/
COPY UserProfileService.Data/UserProfileService.Data.csproj UserProfileService.Data/

# Restore dependencies
RUN dotnet restore

# Copy the rest of the source code
COPY . .

# Build and publish API
RUN dotnet publish UserProfileService.Api/UserProfileService.Api.csproj \
    -c Release \
    -o /app/publish \
    --no-restore


# =========================
# RUNTIME
# =========================

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "UserProfileService.Api.dll"]