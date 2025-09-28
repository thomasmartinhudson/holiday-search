# Holiday Search

A .NET application for searching holiday packages with flights and hotels.

## Requirements

- .NET 9.0 SDK
- Docker (for running tests in a containerized environment)
- Make (for running build and test commands)

## Installation

1. Clone the repository
2. Ensure you have .NET 9.0 SDK installed
3. Install Docker
4. Install Make

## Running Tests

### Using Docker (Recommended)

Build and run tests in a Docker container:

```bash
docker build -t holiday-search-tests .
docker run --rm holiday-search-tests
```

This will:
1. Build a Docker image with the application and test dependencies
2. Run all tests in the containerized environment

### Using .NET directly

Run tests locally:

```bash
# Run all tests
make test

# Run unit tests with verbose output
make test-unit-verbose

# Run e2e tests with verbose output
make test-e2e-verbose
```

## Application Overview

The Holiday Search application provides a comprehensive holiday package search system that combines flights and hotels to find the best deals for travelers.

### Core Features

- **Flight Search**: Search flights by departure airport, destination, and departure date
- **Hotel Search**: Find hotels by destination, arrival date, and duration
- **Airport Code Resolution**: Automatically resolve city names to airport codes (e.g., "Manchester" → "MAN")
- **Price Optimization**: Results are automatically sorted by total price (flight + hotel) from cheapest to most expensive
- **Flexible Search Options**: Support for "Any Airport" searches and multiple airport options (e.g., "London" includes LHR, LGW, LTN, STN)

### Architecture

The application follows a clean architecture pattern with:
- **Controllers**: Handle data retrieval and business logic
- **Models**: Define data structures and validation
- **Views**: Orchestrate the search process and coordinate between controllers

The main `HolidaySearch` view class orchestrates the entire search process, from parsing search criteria to returning ranked holiday results.

## Development

### Code Quality

```bash
# Check code formatting
make format-check

# Apply code formatting
make format

# Check for linting issues
make lint
```