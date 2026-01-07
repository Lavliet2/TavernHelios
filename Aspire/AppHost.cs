var builder = DistributedApplication.CreateBuilder(args);

// Параметры для PostgreSQL (как в примере пользователя)
var postgresUser = builder.AddParameter("postgres-user", "postgres");
var postgresPassword = builder.AddParameter("postgres-password", "helios");

// Базы данных и инфраструктура
var mongoMenu = builder.AddMongoDB("mongoMenu")
    .WithDataVolume("mongoMenu_data")
    .WithEnvironment("MONGO_INITDB_DATABASE", "mongoMenu");

var mongoLayout = builder.AddMongoDB("mongoLayout")
    .WithDataVolume("mongoLayout_data")
    .WithEnvironment("MONGO_INITDB_DATABASE", "mongoLayout");

var postgresReservation = builder.AddPostgres("postgresReservation", userName: postgresUser, password: postgresPassword)
    .WithDataVolume("postgresReservation_data")
    .WithEnvironment("POSTGRES_DB", "reservationdb");
var reservationDb = postgresReservation.AddDatabase("reservationdb");

var postgresAuth = builder.AddPostgres("postgresAuth", userName: postgresUser, password: postgresPassword)
    .WithDataVolume("postgresAuth_data")
    .WithEnvironment("POSTGRES_DB", "authdb");
var authDb = postgresAuth.AddDatabase("authdb");

var redis = builder.AddRedis("redis")
    .WithDataVolume("redis_weather_data");

var rabbitmq = builder.AddRabbitMQ("rabbitmq-monitoring")
    .WithEnvironment("RABBITMQ_DEFAULT_USER", "admin")
    .WithEnvironment("RABBITMQ_DEFAULT_PASS", "admin");

// ClickHouse, Prometheus, Grafana - добавляем как контейнеры
var clickhouse = builder.AddContainer("clickhouse-monitoring", "clickhouse/clickhouse-server", "23.8.4.69-alpine")
    .WithContainerName("clickhouse-monitoring")
    .WithHttpEndpoint(8123, name: "clickhouse-http", targetPort: 8123)
    .WithEndpoint(9000, name: "clickhouse-native", targetPort: 9000)
    .WithEnvironment("CLICKHOUSE_DB", "default")
    .WithEnvironment("CLICKHOUSE_USER", "default")
    .WithEnvironment("CLICKHOUSE_PASSWORD", "");

var prometheus = builder.AddContainer("prometheus-monitoring", "prom/prometheus")
    .WithHttpEndpoint(9090, name: "prometheus", targetPort: 9090)
    .WithBindMount("../prometheus.yml", "/etc/prometheus/prometheus.yml");

var grafana = builder.AddContainer("grafana-monitoring", "grafana/grafana")
    .WithHttpEndpoint(3000, name: "grafana", targetPort: 3000)
    .WithEnvironment("GF_SECURITY_ADMIN_USER", "admin")
    .WithEnvironment("GF_SECURITY_ADMIN_PASSWORD", "admin");

// Сервисы приложения
var menuService = builder.AddProject<Projects.TavernHelios_MenuService_MenuServiceServer>("menuService")
    .WaitFor(mongoMenu)
    .WithReference(mongoMenu)
    .WithHttpEndpoint(5064, name: "menu-grpc")
    .WithEnvironment("MenuMongoConnectionSettings:DbName", "mongoMenu")
    .WithEnvironment("GrpcMenuServiceSettings:Ip", "0.0.0.0")
    .WithEnvironment("GrpcMenuServiceSettings:Port", "5064");

var layoutService = builder.AddProject<Projects.TavernHelios_LayoutService_LayoutServiceServer>("layoutService")
    .WaitFor(mongoLayout)
    .WithReference(mongoLayout)
    .WithHttpEndpoint(5066, name: "layout-grpc")
    .WithEnvironment("LayoutMongoConnectionSettings:DbName", "mongoLayout")
    .WithEnvironment("GrpcLayoutServiceSettings:Ip", "0.0.0.0")
    .WithEnvironment("GrpcLayoutServiceSettings:Port", "5066");

var reservationService = builder.AddProject<Projects.TavernHelios_ReservationServiceServer>("reservationService")
    .WaitFor(reservationDb)
    .WithReference(reservationDb)
    .WithHttpEndpoint(5065, name: "reservation-grpc")
    .WithEnvironment("GrpcReservationServiceSettings:Ip", "0.0.0.0")
    .WithEnvironment("GrpcReservationServiceSettings:Port", "5065");

var authService = builder.AddProject<Projects.TavernHelios_Auth>("authService")
    .WaitFor(authDb)
    .WithReference(authDb)
    .WithHttpEndpoint(5067, name: "auth-http");

var weatherService = builder.AddProject<Projects.TavernHelios_WeatherServiceServer>("weatherService")
    .WaitFor(redis)
    .WithReference(redis)
    .WithHttpEndpoint(5069, name: "weather-grpc")
    .WithEnvironment("GrpcWeatherServiceSettings:Ip", "0.0.0.0")
    .WithEnvironment("GrpcWeatherServiceSettings:Port", "5069");

var server = builder.AddProject<Projects.TavernHelios_Server>("server")
    .WaitFor(rabbitmq)
    .WaitFor(menuService)
    .WaitFor(layoutService)
    .WaitFor(reservationService)
    .WaitFor(weatherService)
    .WaitFor(authService)
    .WithReference(rabbitmq)
    .WithHttpEndpoint(5555, name: "server-https")
    .WithEnvironment("RABBITMQ_HOST", rabbitmq.Resource.Name)
    .WithEnvironment("RABBITMQ_USER", "admin")
    .WithEnvironment("RABBITMQ_PASSWORD", "admin")
    .WithEnvironment("RABBITMQ_PORT", "5672")
    .WithEnvironment("RabbitMQ:Host", rabbitmq.Resource.Name)
    .WithEnvironment("RabbitMQ:User", "admin")
    .WithEnvironment("RabbitMQ:Password", "admin")
    .WithEnvironment("RabbitMQ:Port", "5672")
    .WithEnvironment("GrpcMenuServiceSettings:Ip", menuService.Resource.Name)
    .WithEnvironment("GrpcMenuServiceSettings:Port", "5064")
    .WithEnvironment("GrpcLayoutServiceSettings:Ip", layoutService.Resource.Name)
    .WithEnvironment("GrpcLayoutServiceSettings:Port", "5066")
    .WithEnvironment("GrpcReservationServiceSettings:Ip", reservationService.Resource.Name)
    .WithEnvironment("GrpcReservationServiceSettings:Port", "5065")
    .WithEnvironment("GrpcWeatherServiceSettings:Ip", weatherService.Resource.Name)
    .WithEnvironment("GrpcWeatherServiceSettings:Port", "5069")
    .WithEnvironment("AuthServiceUrl", $"http://{authService.Resource.Name}:5067");

var adminService = builder.AddProject<Projects.TavernHelios_AdministrationService_AdministrationServiceServer>("adminService")
    .WaitFor(clickhouse)
    .WaitFor(rabbitmq)
    .WithReference(rabbitmq)
    .WithHttpEndpoint(5068, name: "admin-http")
    .WithEnvironment("ClickHouse:Host", "clickhouse-monitoring")
    .WithEnvironment("ClickHouse:Port", "8123")
    .WithEnvironment("ClickHouse:Database", "default")
    .WithEnvironment("ClickHouse:User", "default")
    .WithEnvironment("ClickHouse:Password", "")
    .WithEnvironment("RabbitMQ:Host", rabbitmq.Resource.Name)
    .WithEnvironment("RabbitMQ:Port", "5672")
    .WithEnvironment("RabbitMQ:User", "admin")
    .WithEnvironment("RabbitMQ:Password", "admin")
    .WithEnvironment("RABBITMQ_HOST", rabbitmq.Resource.Name)
    .WithEnvironment("RABBITMQ_USER", "admin")
    .WithEnvironment("RABBITMQ_PASSWORD", "admin")
    .WithEnvironment("RABBITMQ_PORT", "5672");

builder.Build().Run();
