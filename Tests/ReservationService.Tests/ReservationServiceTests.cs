using Google.Protobuf.WellKnownTypes;
using GrpcContract;
using GrpcContract.ReservationService;
using Microsoft.Extensions.Logging;
using Moq;
using ReservationServiceServer.ReservationService;
using TavernHelios.ReservationService.ApiCore.Interfaces;
using TavernHelios.ReservationService.APICore.Entities;

namespace ReservationService.Tests
{
    [TestFixture]
    public class ReservationServiceApiTests
    {
        private Mock<ILogger<ReservationServiceApi>> _loggerMock;
        private Mock<IRepository<ReservationEntity>> _reservationRepositoryMock;
        private ReservationServiceApi _service;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<ReservationServiceApi>>();
            _reservationRepositoryMock = new Mock<IRepository<ReservationEntity>>();
            _service = new ReservationServiceApi(
                _loggerMock.Object,
                _reservationRepositoryMock.Object);
        }

        #region Test Data Factories

        private static Reservation CreateTestReservation(
            string id = "1",
            string personId = "person1",
            DateTime? date = null,
            string tableName = "Table1",
            int seatNumber = 4,
            string[] dishIds = null)
        {
            return new Reservation
            {
                Id = id,
                PersonId = personId,
                Date = Timestamp.FromDateTime(date ?? DateTime.UtcNow),
                TableName = tableName,
                SeatNumber = seatNumber,
                DishIds = { dishIds ?? Array.Empty<string>() },
                IsDeleted = false
            };
        }

        private static ReservationEntity CreateTestReservationEntity(
            long id = 1,
            string personId = "person1",
            DateTime? date = null,
            string tableName = "Table1",
            int seatNumber = 4,
            List<DishReservationEntity> dishReservations = null)
        {
            return new ReservationEntity
            {
                Id = id,
                PersonId = personId,
                Date = date ?? DateTime.UtcNow,
                TableName = tableName,
                SeatNumber = seatNumber,
                DishReservations = dishReservations ?? new List<DishReservationEntity>(),
                IsDeleted = false
            };
        }

        private static DishReservationEntity CreateTestDishReservation(long id, string dishId, long reservationId)
        {
            return new DishReservationEntity
            {
                Id = id,
                DishId = dishId,
                ReservationId = reservationId
            };
        }

        private static ReservationQueryRequest CreateTestQueryRequest(
            long? reservationId = null,
            string personId = null,
            string dishId = null,
            DateTime? beginDate = null,
            DateTime? endDate = null,
            bool? isDeleted = null)
        {
            var request = new ReservationQueryRequest();
            if (reservationId.HasValue)
                request.ReservationId = reservationId.Value;
            if (personId != null)
                request.PersonId = personId;
            if (dishId != null)
                request.DishId = dishId;
            if (beginDate != null)
                request.BeginDate = Timestamp.FromDateTime(beginDate.Value);
            if (endDate != null)
                request.EndDate = Timestamp.FromDateTime(endDate.Value);
            if (isDeleted.HasValue)
                request.IsDeleted = isDeleted.Value;
            return request;
        }

        #endregion

        #region AddReservation Tests

        [Test]
        public async Task AddReservation_ValidRequest_ReturnsSuccessWithCreatedReservation()
        {
            var testReservation = CreateTestReservation();
            var entity = CreateTestReservationEntity(
                dishReservations: testReservation.DishIds
                    .Select((d, i) => CreateTestDishReservation(i + 1, d, 1))
                    .ToList());

            _reservationRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<ReservationEntity>()))
                .ReturnsAsync(entity);

            var result = await _service.AddReservation(testReservation, null);

            Assert.That(result.State, Is.EqualTo(ReplyState.Ok));
            Assert.That(result.Reservations, Has.Count.EqualTo(1));
            Assert.That(result.Reservations[0].Id, Is.EqualTo(entity.Id.ToString()));
            Assert.That(result.Reservations[0].TableName, Is.EqualTo(entity.TableName));
        }

        [Test]
        public async Task AddReservation_WithDishes_CreatesDishReservations()
        {
            var testReservation = CreateTestReservation(dishIds: new[] { "dish1", "dish2" });
            var entity = CreateTestReservationEntity(
                dishReservations: testReservation.DishIds
                    .Select((d, i) => CreateTestDishReservation(i + 1, d, 1))
                    .ToList());

            _reservationRepositoryMock.Setup(x => x.CreateAsync(It.Is<ReservationEntity>(r =>
                r.DishReservations.Count == testReservation.DishIds.Count)))
                .ReturnsAsync(entity);

            var result = await _service.AddReservation(testReservation, null);

            Assert.That(result.Reservations[0].DishIds, Is.EquivalentTo(testReservation.DishIds));
        }

        #endregion

        #region GetReservations Tests

        [Test]
        public async Task GetReservations_ByDishId_ReturnsReservationsWithDish()
        {
            var testDishId = "dish1";
            var testData = new[]
            {
                CreateTestReservationEntity(
                    dishReservations: new List<DishReservationEntity>
                    {
                        CreateTestDishReservation(1, testDishId, 1)
                    }),
                CreateTestReservationEntity(
                    dishReservations: new List<DishReservationEntity>
                    {
                        CreateTestDishReservation(2, "dish2", 2)
                    })
            };

            var request = CreateTestQueryRequest(dishId: testDishId);

            _reservationRepositoryMock.Setup(x => x.GetByQueryAsync(It.IsAny<Func<ReservationEntity, bool>>()))
                .ReturnsAsync(testData.Where(r =>
                    r.DishReservations.Any(d => d.DishId == testDishId)).ToList());

            var result = await _service.GetReservations(request, null);

            Assert.That(result.Reservations, Has.Count.EqualTo(1));
            Assert.That(result.Reservations[0].DishIds, Contains.Item(testDishId));
        }

        [Test]
        public async Task GetReservations_IncludesDishReservationsInResponse()
        {
            var testData = new[]
            {
                CreateTestReservationEntity(
                    dishReservations: new List<DishReservationEntity>
                    {
                        CreateTestDishReservation(1, "dish1", 1),
                        CreateTestDishReservation(2, "dish2", 1)
                    })
            };

            _reservationRepositoryMock.Setup(x => x.GetByQueryAsync(It.IsAny<Func<ReservationEntity, bool>>()))
                .ReturnsAsync(testData.ToList());

            var result = await _service.GetReservations(new ReservationQueryRequest(), null);

            Assert.That(result.Reservations[0].DishIds, Has.Count.EqualTo(2));
        }

        #endregion

        #region UpdateReservation Tests

        [Test]
        public async Task UpdateReservation_WithDishes_UpdatesDishReservations()
        {
            var testReservation = CreateTestReservation(
                id: "1",
                dishIds: new[] { "newDish1", "newDish2" });

            var existingEntity = CreateTestReservationEntity(
                id: 1,
                dishReservations: new List<DishReservationEntity>
                {
                    CreateTestDishReservation(1, "oldDish1", 1)
                });

            var updatedEntity = CreateTestReservationEntity(
                id: 1,
                dishReservations: testReservation.DishIds
                    .Select((d, i) => CreateTestDishReservation(i + 2, d, 1))
                    .ToList());

            _reservationRepositoryMock.Setup(x => x.UpdateAsync(It.Is<ReservationEntity>(r =>
                r.DishReservations.Count == testReservation.DishIds.Count)))
                .ReturnsAsync(updatedEntity);

            var result = await _service.UpdateReservation(testReservation, null);

            Assert.That(result.Reservations[0].DishIds, Is.EquivalentTo(testReservation.DishIds));
        }

        #endregion

        #region DeleteReservation Tests

        [Test]
        public async Task DeleteReservation_AlsoDeletesDishReservations()
        {
            var request = new IdRequest { Id = "1" };

            _reservationRepositoryMock.Setup(x => x.DeleteAsync(1))
                .ReturnsAsync(1);

            var result = await _service.DeleteReservation(request, null);

            Assert.That(result.State, Is.EqualTo(ReplyState.Ok));
            _reservationRepositoryMock.Verify(x => x.DeleteAsync(1), Times.Once);
        }

        #endregion
    }
}