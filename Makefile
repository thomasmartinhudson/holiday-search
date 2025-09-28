test:
	dotnet test tests/tests.csproj

test-e2e-verbose:
	dotnet test tests/tests.csproj --filter "FullyQualifiedName~HolidaySearch.Tests.E2E" --logger "console;verbosity=detailed"

test-unit-verbose:
	dotnet test tests/tests.csproj --filter "FullyQualifiedName~HolidaySearch.Tests.Unit" --logger "console;verbosity=detailed"

docker-test:
	docker build -t holiday-search-tests .
	docker run --rm holiday-search-tests
