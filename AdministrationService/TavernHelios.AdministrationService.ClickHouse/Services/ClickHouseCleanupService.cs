using ClickHouse.Client.ADO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TavernHelios.ClickHouse.Settings;

namespace TavernHelios.AdministrationService.ClickHouse.Services
{
    public class ClickHouseCleanupService
    {
        private readonly ILogger<ClickHouseCleanupService> _logger;
        private readonly string _connectionString;

        public ClickHouseCleanupService(ILogger<ClickHouseCleanupService> logger, IOptions<ClickHouseSettings> settings)
        {
            _logger = logger;
            _connectionString = GetConnectionString(settings.Value);
        }
        private string GetConnectionString(ClickHouseSettings settings)
        {
            return $"Host={settings.Host};Port={settings.Port};Database={settings.Database};User={settings.User};Password={settings.Password};";
        }

        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Проверка необходимости очистки данных...");

                try
                {
                    // Проверка количества записей
                    var countQuery = "SELECT COUNT(*) FROM logs";
                    using var connection = new ClickHouseConnection(_connectionString);
                    connection.Open();
                    using var command = connection.CreateCommand();
                    command.CommandText = countQuery;
                    var count = (ulong)await command.ExecuteScalarAsync(stoppingToken);

                    if (count > 2000)
                    {
                        _logger.LogInformation($"Найдено {count} записей. Удаление последних 1000 записей...");

                        // Удаление последних 1000 записей
                        var deleteQuery = @"
                            ALTER TABLE logs
                            DELETE WHERE timestamp IN (
                                SELECT timestamp
                                FROM logs
                                ORDER BY timestamp DESC
                                LIMIT 1000
                            )";
                        command.CommandText = deleteQuery;
                        await command.ExecuteNonQueryAsync(stoppingToken);

                        _logger.LogInformation("Удаление завершено.");
                    }
                    else
                    {
                        _logger.LogInformation("Количество записей не превышает 2000. Очистка не требуется.");
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при выполнении очистки данных.");
                }

                // Ожидание следующего часа
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}