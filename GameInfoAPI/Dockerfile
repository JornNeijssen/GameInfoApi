# Use the official .NET SDK image as the build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory in the container
WORKDIR /src

# Copy the project file(s) and restore the dependencies
COPY ["GameInfoAPI/GameInfoAPI.csproj", "GameInfoAPI/"]
RUN dotnet restore "GameInfoAPI/GameInfoAPI.csproj"

# Copy the rest of the application code
COPY . .

# Build the application
RUN dotnet build "GameInfoAPI/GameInfoAPI.csproj" -c Release -o /app/build

# Publish the application
RUN dotnet publish "GameInfoAPI/GameInfoAPI.csproj" -c Release -o /app/publish

# Use the official .NET runtime image for the final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory in the container
WORKDIR /app

# Copy the published output from the build image
COPY --from=build /app/publish .

# Expose the port the app runs on
EXPOSE 80

# Start the application
ENTRYPOINT ["dotnet", "GameInfoAPI.dll"]
