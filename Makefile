test:
	dotnet test tests/tests.csproj

docker-test:
	docker build -t holiday-search-tests .
	docker run --rm holiday-search-tests
