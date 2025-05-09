using System.Text.Json;
using TavernHelios.MenuService.Common.DTOValues.Menu;
using TavernHelios.MenuService.Common.Enums;
using TavernHelios.Utils.Reports;
using TavernHelios.ReservationService.APICore.DTOValues;


namespace TavernHelios.Server.Services
{
    public class ReservationExportService
    {
        private readonly HttpClient _httpClient;
        private readonly string _menuServiceBaseUrl;
        private readonly string _reservationServiceBaseUrl;

        public ReservationExportService(HttpClient httpClient, IConfiguration configuration)
        {
            var httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            _httpClient = new HttpClient(httpClientHandler);

            _menuServiceBaseUrl = configuration["ApiSettings:MenuServiceBaseUrl"] ?? "https://localhost:5555";
            _reservationServiceBaseUrl = configuration["ApiSettings:ReservationServiceBaseUrl"] ?? "https://localhost:5555";
        }

        public async Task<Stream> ExportReservationsAsync(DateTime date, string format)
        {
            string isoBegin = date.ToString("yyyy-MM-ddT00:00:00Z");
            string isoEnd = date.ToString("yyyy-MM-ddT23:59:59Z");
            var response = await _httpClient.GetAsync($"{_reservationServiceBaseUrl}/api/Reservation?IsDeleted=false&BeginDate={isoBegin}&EndDate={isoEnd}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Ошибка при получении бронирований.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var reservations = JsonSerializer.Deserialize<List<ReservationValue>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (reservations == null || !reservations.Any())
            {
                throw new Exception("Нет бронирований на эту дату.");
            }

            var dishData = await FetchDishesForReservations(reservations);

            var reservationsAt12 = reservations.Where(r => r.Date.Hour == 12).ToList();
            var reservationsAt13 = reservations.Where(r => r.Date.Hour == 13).ToList();

            var headers = new List<string> { "Сотрудник","Стол", "Суп", "Горячее", "Салаты", "Напитки" };

            List<List<string>> tableData12 = reservationsAt12.Select(res =>
                new List<string>
                {
                    res.PersonId,
                    res.TableName ?? "—",
                    GetDishByType(dishData, res.DishIds, DishType.Soup),
                    GetDishByType(dishData, res.DishIds, DishType.HotDish),
                    GetDishByType(dishData, res.DishIds, DishType.Salad),
                    GetDishByType(dishData, res.DishIds, DishType.Drink)
                }).ToList();

            List<List<string>> tableData13 = reservationsAt13.Select(res =>
                new List<string>
                {
                    res.PersonId,
                    res.TableName ?? "—",
                    GetDishByType(dishData, res.DishIds, DishType.Soup),
                    GetDishByType(dishData, res.DishIds, DishType.HotDish),
                    GetDishByType(dishData, res.DishIds, DishType.Salad),
                    GetDishByType(dishData, res.DishIds, DishType.Drink)
                }).ToList();

            var reportGenerator = ReportFactory.CreateReportGenerator(format, $"Брони на {date:yyyy-MM-dd}", headers);

            var tables = new List<(string, List<List<string>>)>();

            if (tableData12.Any())
                tables.Add(("Брони на 12:00", tableData12));
            if (tableData13.Any())
                tables.Add(("Брони на 13:00", tableData13));

            return reportGenerator.GenerateMultiTableReport(tables);
        }


        private async Task<Dictionary<string, DishValue>> FetchDishesForReservations(IEnumerable<ReservationValue> reservations)
        {
            var dishDict = new Dictionary<string, DishValue>();
            var dishIds = reservations.SelectMany(r => r.DishIds).Distinct().ToList();

            foreach (var dishId in dishIds)
            {
                var response = await _httpClient.GetAsync($"{_menuServiceBaseUrl}/api/dish/{dishId}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var dish = JsonSerializer.Deserialize<DishValue>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (dish != null)
                        dishDict[dish.Id] = dish;
                }
            }

            return dishDict;
        }

        private string GetDishByType(Dictionary<string, DishValue> dishes, List<string> dishIds, DishType type)
        {
            var selectedDishes = dishIds
                .Where(dishId => dishes.ContainsKey(dishId) && dishes[dishId].DishType == type)
                .Select(dishId => dishes[dishId].Name)
                .ToList();

            return selectedDishes.Any() ? string.Join(", ", selectedDishes) : "—";
        }
    }
}
