# Use the official .NET 9.0 SDK image
FROM mcr.microsoft.com/dotnet/sdk:9.0

# Set the working directory
WORKDIR /app

# Copy the project files
COPY app/ ./app/
COPY tests/ ./tests/
COPY data/ ./data/
COPY Makefile ./Makefile

# Restore dependencies
RUN dotnet restore tests/tests.csproj

# Run the tests
CMD ["dotnet", "test", "tests/tests.csproj"]
