using MealMate.BLL.Dtos.Delivery;
using MealMate.BLL.IServices.Delivery;
using Newtonsoft.Json;
using System.Text.Json;

namespace MealMate.BLL.Services.Delivery
{
    internal class RouteService : IRouteService
    {
        private readonly string GeoCodingApiKey = Environment.GetEnvironmentVariable("GEOCODING_API_KEY");
        private readonly string DistanceMatrixApiKey = Environment.GetEnvironmentVariable("DISTANCE_MATRIX_API_KEY");


        public async Task<RouteResult> GetOptimalRouteAsync(string shopAddress, List<string> deliveryAddresses)
        {
            // Step 1: Geocode all addresses
            var allAddresses = new List<string> { shopAddress }.Concat(deliveryAddresses).ToList();
            var coordinates = await GetCoordinatesAsync(allAddresses);

            // Step 2: Get Distance Matrix
            var distanceMatrix = await GetDistanceMatrixAsync(coordinates);

            // Step 3: Solve TSP
            var optimalRouteIndices = SolveTsp(distanceMatrix);

            // Step 4: Map back to addresses
            var optimalRoute = optimalRouteIndices.Select(index => allAddresses[index]).ToList();
            var totalDistance = CalculateTotalDistance(distanceMatrix, optimalRouteIndices);

            return new RouteResult
            {
                OptimalRoute = optimalRoute,
                TotalDistance = totalDistance
            };
        }

        private async Task<List<(double lat, double lng)>> GetCoordinatesAsync(List<string> addresses)
        {
            var coordinates = new List<(double lat, double lng)>();
            using var client = new HttpClient();

            foreach (var address in addresses)
            {
                var response = await client.GetStringAsync($"https://api.distancematrix.ai/maps/api/geocode/json?address={Uri.EscapeDataString(address)}&key={GeoCodingApiKey}");
                JsonDocument doc = JsonDocument.Parse(response);
                var root = doc.RootElement;

                // Get status
                string status = root.GetProperty("status").GetString() ?? throw new Exception("Invalid response from the geocoding API.");

                if (status != "OK")
                {
                    switch (status)
                    {
                        case "ZERO_RESULTS":
                            throw new Exception($"No results found for address: {address}");
                        case "OVER_DAILY_LIMIT":
                            throw new Exception("Daily limit exceeded or invalid API key.");
                        case "OVER_QUERY_LIMIT":
                            throw new Exception("Query limit exceeded.");
                        case "REQUEST_DENIED":
                            throw new Exception("Request denied. Check API key and permissions.");
                        case "INVALID_REQUEST":
                            throw new Exception("Invalid request. Address parameter is likely missing.");
                        case "UNKNOWN_ERROR":
                            throw new Exception("Unknown error occurred. Try again later.");
                        default:
                            throw new Exception($"Unexpected status: {status}");
                    }
                }

                // Get latitude and longitude
                if (status == "OK" && root.GetProperty("result").GetArrayLength() > 0)
                {
                    var location = root.GetProperty("result")[0]
                                      .GetProperty("geometry")
                                      .GetProperty("location");

                    double lat = location.GetProperty("lat").GetDouble();
                    double lng = location.GetProperty("lng").GetDouble();
                    coordinates.Add((lat, lng));
                }
                /*var geocodeResponse = JsonConvert.DeserializeObject<GeocodeResponse>(response) ?? throw new Exception("Invalid response from the geocoding API.");*/

                // Check the status field
                /*string status = geocodeResponse.Status;
                if (status != "OK")
                {
                    switch (status)
                    {
                        case "ZERO_RESULTS":
                            throw new Exception($"No results found for address: {address}");
                        case "OVER_DAILY_LIMIT":
                            throw new Exception("Daily limit exceeded or invalid API key.");
                        case "OVER_QUERY_LIMIT":
                            throw new Exception("Query limit exceeded.");
                        case "REQUEST_DENIED":
                            throw new Exception("Request denied. Check API key and permissions.");
                        case "INVALID_REQUEST":
                            throw new Exception("Invalid request. Address parameter is likely missing.");
                        case "UNKNOWN_ERROR":
                            throw new Exception("Unknown error occurred. Try again later.");
                        default:
                            throw new Exception($"Unexpected status: {status}");
                    }
                }

                // Extract latitude and longitude
                var location = geocodeResponse.Results[0].Geometry.Location;
                double lat = location.Lat;
                double lng = location.Lng;*/

            }

            return coordinates;
        }

        private async Task<double[,]> GetDistanceMatrixAsync(List<(double lat, double lng)> coordinates)
        {
            var locations = string.Join("|", coordinates.Select(c => $"{c.lat},{c.lng}"));
            using var client = new HttpClient();

            var response = await client.GetStringAsync(
                $"https://api.distancematrix.ai/maps/api/distancematrix/json?origins={locations}&destinations={locations}&key={DistanceMatrixApiKey}");
            dynamic data = JsonConvert.DeserializeObject(response);

            int n = coordinates.Count;
            var matrix = new double[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] = data.rows[i].elements[j].distance.value; // Distance in meters
                }
            }

            return matrix;
        }

        private List<int> SolveTsp(double[,] distanceMatrix)
        {
            int n = distanceMatrix.GetLength(0);
            var visited = new bool[n];
            var route = new List<int> { 0 }; // Start from the shop (index 0)
            visited[0] = true;

            for (int step = 1; step < n; step++)
            {
                int last = route.Last();
                double minDistance = double.MaxValue;
                int next = -1;

                for (int i = 0; i < n; i++)
                {
                    if (!visited[i] && distanceMatrix[last, i] < minDistance)
                    {
                        minDistance = distanceMatrix[last, i];
                        next = i;
                    }
                }

                visited[next] = true;
                route.Add(next);
            }

            return route;
        }

        private double CalculateTotalDistance(double[,] distanceMatrix, List<int> route)
        {
            double totalDistance = 0;
            for (int i = 0; i < route.Count - 1; i++)
            {
                totalDistance += distanceMatrix[route[i], route[i + 1]];
            }

            return totalDistance;
        }


    }

    public class AddressComponent
    {
        public string LongName { get; set; }
        public string ShortName { get; set; }
        public List<string> Types { get; set; }
    }

    public class Location
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class Geometry
    {
        public Location Location { get; set; }
        public string LocationType { get; set; }
    }

    public class Result
    {
        public List<AddressComponent> AddressComponents { get; set; }
        public string FormattedAddress { get; set; }
        public Geometry Geometry { get; set; }
        public string PlaceId { get; set; }
        public List<string> Types { get; set; }
    }

    public class GeocodeResponse
    {
        public List<Result> Result { get; set; }
        public string Status { get; set; }
    }
}
