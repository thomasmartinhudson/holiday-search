using System.Text.Json;

namespace HolidaySearch.Controllers
{
    public class JSONController(string dataPath)
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };

        private readonly string _dataPath = dataPath;

        public T[] ReadAll<T>()
        {
            if (!File.Exists(_dataPath))
            {
                throw new FileNotFoundException($"Data file not found: {_dataPath}");
            }

            try
            {
                var jsonContent = File.ReadAllText(_dataPath);
                var items = JsonSerializer.Deserialize<T[]>(jsonContent, JsonOptions);

                return items ?? [];
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException($"Failed to parse JSON data: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error reading data file: {ex.Message}", ex);
            }
        }

        public T ReadSingle<T>()
        {
            if (!File.Exists(_dataPath))
            {
                throw new FileNotFoundException($"Data file not found: {_dataPath}");
            }

            try
            {
                var jsonContent = File.ReadAllText(_dataPath);
                var item = JsonSerializer.Deserialize<T>(jsonContent, JsonOptions);

                if (item == null)
                {
                    throw new InvalidOperationException($"Failed to deserialize data from {_dataPath}");
                }

                return item;
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException($"Failed to parse JSON data: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error reading data file: {ex.Message}", ex);
            }
        }
    }
}
