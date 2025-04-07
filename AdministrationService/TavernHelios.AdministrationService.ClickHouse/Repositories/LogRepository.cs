using ClickHouse.Client.ADO;
using Dapper;
using Microsoft.Extensions.Options;
using TavernHelios.AdministrationService.ClickHouse.Entities;
using TavernHelios.AdministrationService.ClickHouse.Interfaces;
using TavernHelios.ClickHouse.Helpers;
using TavernHelios.ClickHouse.Settings;

namespace TavernHelios.AdministrationService.ClickHouse
{
    public class LogRepository : IRepository<LogEntity>
    {
        private readonly string _connectionString;

        public LogRepository(IOptions<ClickHouseSettings> options)
        {
            _connectionString = ConnectionHelper.GetClickHouseConnectionString(options.Value);
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var connection = new ClickHouseConnection(_connectionString);
            connection.Open();

            var createTableQuery = $@"
            CREATE TABLE IF NOT EXISTS logs (
                {nameof(LogEntity.Timestamp)} DateTime,
                {nameof(LogEntity.Level)} String,
                {nameof(LogEntity.MessageTemplate)} String,
                {nameof(LogEntity.Method)} String,
                {nameof(LogEntity.Path)} String,
                {nameof(LogEntity.RequestBody)} String,
                {nameof(LogEntity.StatusCode)} Int32,
                {nameof(LogEntity.ResponseBody)} String,
                {nameof(LogEntity.Controller)} String,
                {nameof(LogEntity.Application)} String,
                {nameof(LogEntity.SourceContext)} String,
                {nameof(LogEntity.RequestId)} String,
                {nameof(LogEntity.RequestPath)} String,
                {nameof(LogEntity.ConnectionId)} String,
                {nameof(LogEntity.User)} String,
                {nameof(LogEntity.Exception)} String
            ) ENGINE = MergeTree()
            ORDER BY {nameof(LogEntity.Timestamp)}";

            connection.Execute(createTableQuery);
        }

        public async Task<LogEntity?> CreateAsync(LogEntity? log)
        {
            try
            {
                if (log == null)
                {
                    return null;
                }

                using var connection = new ClickHouseConnection(_connectionString);
                await connection.OpenAsync();

                // Генерируем уникальный RequestId, если он не задан
                if (string.IsNullOrEmpty(log.RequestId))
                {
                    log.RequestId = Guid.NewGuid().ToString();
                }

                log.RequestBody = log.RequestBody.Substring(0, log.RequestBody.Length < 200 ? log.RequestBody.Length : 200);
                log.ResponseBody = log.ResponseBody.Substring(0, log.ResponseBody.Length < 200 ? log.ResponseBody.Length : 200);

                var insertQuery = $@"
            INSERT INTO logs (
                {nameof(LogEntity.Timestamp)}, {nameof(LogEntity.Level)}, {nameof(LogEntity.MessageTemplate)},
                {nameof(LogEntity.Method)}, {nameof(LogEntity.Path)}, {nameof(LogEntity.RequestBody)},
                {nameof(LogEntity.StatusCode)}, {nameof(LogEntity.ResponseBody)}, {nameof(LogEntity.Controller)},
                {nameof(LogEntity.Application)}, {nameof(LogEntity.SourceContext)}, {nameof(LogEntity.RequestId)},
                {nameof(LogEntity.RequestPath)}, {nameof(LogEntity.ConnectionId)}, {nameof(LogEntity.User)}" +
                    (log.Exception != null ? $", {nameof(LogEntity.Exception)}" : "") +
                $@") VALUES (
                @{nameof(LogEntity.Timestamp)}, @{nameof(LogEntity.Level)}, @{nameof(LogEntity.MessageTemplate)},
                @{nameof(LogEntity.Method)}, @{nameof(LogEntity.Path)}, @{nameof(LogEntity.RequestBody)},
                @{nameof(LogEntity.StatusCode)}, @{nameof(LogEntity.ResponseBody)}, @{nameof(LogEntity.Controller)},
                @{nameof(LogEntity.Application)}, @{nameof(LogEntity.SourceContext)}, @{nameof(LogEntity.RequestId)},
                @{nameof(LogEntity.RequestPath)}, @{nameof(LogEntity.ConnectionId)}, @{nameof(LogEntity.User)}" +
                    (log.Exception != null ? $", @{nameof(LogEntity.Exception)}" : "") +
                ");";

                await connection.ExecuteAsync(insertQuery, log);

                // Получаем добавленную запись по RequestId
                var selectQuery = $"SELECT * FROM logs WHERE {nameof(LogEntity.RequestId)} = @RequestId LIMIT 1";
                var addedLog = await connection.QueryFirstOrDefaultAsync<LogEntity>(selectQuery, new { log.RequestId });

                return addedLog;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<LogEntity>> GetByQueryAsync(
            DateTime? timestamp = null,
            string? level = null,
            string? messageTemplate = null,
            string? method = null,
            string? path = null,
            string? requestBody = null,
            int? statusCode = null,
            string? responseBody = null,
            string? controller = null,
            string? application = null,
            string? sourceContext = null,
            string? requestId = null,
            string? requestPath = null,
            string? connectionId = null,
            string? user = null,
            string? exception = null)
        {
            using var connection = new ClickHouseConnection(_connectionString);
            await connection.OpenAsync();

            // Базовый запрос
            var query = $"SELECT * FROM logs WHERE 1=1";

            // Динамически добавляем фильтры
            if (timestamp.HasValue)
                query += $" AND {nameof(LogEntity.Timestamp)} = @Timestamp";
            if (!string.IsNullOrEmpty(level))
                query += $" AND {nameof(LogEntity.Level)} = @Level";
            if (!string.IsNullOrEmpty(messageTemplate))
                query += $" AND {nameof(LogEntity.MessageTemplate)} = @MessageTemplate";
            if (!string.IsNullOrEmpty(method))
                query += $" AND {nameof(LogEntity.Method)} = @Method";
            if (!string.IsNullOrEmpty(path))
                query += $" AND {nameof(LogEntity.Path)} = @Path";
            if (!string.IsNullOrEmpty(requestBody))
                query += $" AND {nameof(LogEntity.RequestBody)} = @RequestBody";
            if (statusCode.HasValue)
                query += $" AND {nameof(LogEntity.StatusCode)} = @StatusCode";
            if (!string.IsNullOrEmpty(responseBody))
                query += $" AND {nameof(LogEntity.ResponseBody)} = @ResponseBody";
            if (!string.IsNullOrEmpty(controller))
                query += $" AND {nameof(LogEntity.Controller)} = @Controller";
            if (!string.IsNullOrEmpty(application))
                query += $" AND {nameof(LogEntity.Application)} = @Application";
            if (!string.IsNullOrEmpty(sourceContext))
                query += $" AND {nameof(LogEntity.SourceContext)} = @SourceContext";
            if (!string.IsNullOrEmpty(requestId))
                query += $" AND {nameof(LogEntity.RequestId)} = @RequestId";
            if (!string.IsNullOrEmpty(requestPath))
                query += $" AND {nameof(LogEntity.RequestPath)} = @RequestPath";
            if (!string.IsNullOrEmpty(connectionId))
                query += $" AND {nameof(LogEntity.ConnectionId)} = @ConnectionId";
            if (!string.IsNullOrEmpty(user))
                query += $" AND {nameof(LogEntity.User)} = @User";
            if (!string.IsNullOrEmpty(exception))
                query += $" AND {nameof(LogEntity.Exception)} = @Exception";

            // Выполняем запрос с параметрами
            return await connection.QueryAsync<LogEntity>(query, new
            {
                Timestamp = timestamp,
                Level = level,
                MessageTemplate = messageTemplate,
                Method = method,
                Path = path,
                RequestBody = requestBody,
                StatusCode = statusCode,
                ResponseBody = responseBody,
                Controller = controller,
                Application = application,
                SourceContext = sourceContext,
                RequestId = requestId,
                RequestPath = requestPath,
                ConnectionId = connectionId,
                User = user,
                Exception = exception
            });
        }
    }
}