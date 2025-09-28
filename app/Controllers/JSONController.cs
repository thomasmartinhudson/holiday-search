using System.Text.Json;

namespace HolidaySearch.Controllers
{
    public class JSONController
    {
        private readonly string _dataPath;

        public JSONController(string dataPath)
        {
            _dataPath = dataPath;
        }

        public List<T> ReadAll<T>()
        {
            if (!File.Exists(_dataPath))
            {
                throw new FileNotFoundException($"Data file not found: {_dataPath}");
            }

            try
            {
                var jsonContent = File.ReadAllText(_dataPath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                };
                
                var items = JsonSerializer.Deserialize<List<T>>(jsonContent, options);

                return items ?? new List<T>();
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
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                };
                
                var item = JsonSerializer.Deserialize<T>(jsonContent, options);

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
