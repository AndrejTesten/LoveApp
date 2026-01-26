# -----------------------------
# Step 1: Build stage
# -----------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution file
COPY LoveApp.sln ./

# Copy the backend project folder
COPY LoveApp/ ./LoveApp/

# Restore NuGet packages
WORKDIR /src/LoveApp
RUN dotnet restore

# Copy everything else (optional if already copied above)
COPY LoveApp/. ./

# Build and publish the app
RUN dotnet publish -c Release -o /app/publish

# -----------------------------
# Step 2: Runtime stage
# -----------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy published app from build stage
COPY --from=build /app/publish .

# Expose port (Render will set PORT env)
EXPOSE 10000

# Run the app
ENTRYPOINT ["dotnet", "LoveApp.dll"]
