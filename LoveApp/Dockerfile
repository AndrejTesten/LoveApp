# -----------------------------
# Step 1: Build stage
# -----------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY *.sln ./
COPY LoveApp/*.csproj ./LoveApp/

# Restore NuGet packages
RUN dotnet restore

# Copy the rest of the backend code
COPY LoveApp/. ./LoveApp/
WORKDIR /src/LoveApp

# Build and publish the app into /app/publish
RUN dotnet publish -c Release -o /app/publish

# -----------------------------
# Step 2: Runtime stage
# -----------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/publish .

# Expose port for Render (Render sets PORT environment variable)
EXPOSE 10000

# Run the app
ENTRYPOINT ["dotnet", "LoveApp.dll"]
