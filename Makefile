test:
	dotnet test tests/tests.csproj

test-e2e-verbose:
	dotnet test tests/tests.csproj --filter "FullyQualifiedName~HolidaySearch.Tests.E2E" --logger "console;verbosity=detailed"

test-unit-verbose:
	dotnet test tests/tests.csproj --filter "FullyQualifiedName~HolidaySearch.Tests.Unit" --logger "console;verbosity=detailed"

format-check:
	dotnet format app/app.csproj --verify-no-changes
	dotnet format tests/tests.csproj --verify-no-changes

format:
	dotnet format app/app.csproj
	dotnet format tests/tests.csproj

docker-test:
	docker build -t holiday-search-tests .
	docker run --rm holiday-search-tests
