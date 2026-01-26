# -----------------------------
# Step 1: Build stage
# -----------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY LoveApp.sln ./

# Copy backend project
COPY LoveApp/ ./LoveApp/

# Restore dependencies
WORKDIR /src/LoveApp
RUN dotnet restore

# Build and publish the app
RUN dotnet publish -c Release -o /app/publish

# -----------------------------
# Step 2: Runtime stage
# -----------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy published app from build stage
COPY --from=build /app/publish .

# Expose port (Render uses PORT env)
EXPOSE 10000
ENV ASPNETCORE_URLS=http://+:10000

# Run the app
ENTRYPOINT ["dotnet", "LoveApp.dll"]
