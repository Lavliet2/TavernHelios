using AutoFixture;
using AutoFixture.AutoMoq;
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
        private IFixture _fixture;
        private Mock<ILogger<ReservationServiceApi>> _loggerMock;
        private Mock<IRepository<ReservationEntity>> _reservationRepositoryMock;
        private ReservationServiceApi _service;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _loggerMock = _fixture.Freeze<Mock<ILogger<ReservationServiceApi>>>();
            _reservationRepositoryMock = _fixture.Freeze<Mock<IRepository<ReservationEntity>>>();

            _service = new ReservationServiceApi(
                _loggerMock.Object,
                _reservationRepositoryMock.Object);
        }

        private Reservation CreateReservation(string id, params string[] dishIds)
        {
            var reservation = new Reservation
            {
                Id = id,
                PersonId = "person1",
                Date = Timestamp.FromDateTime(DateTime.UtcNow),
                TableName = "Table1",
                SeatNumber = 4,
                IsDeleted = false
            };

            reservation.DishIds.AddRange(dishIds);
            return reservation;
        }

        private ReservationEntity CreateReservationEntity(long id, params string[] dishIds)
        {
            var entity = new ReservationEntity
            {
                Id = id,
                PersonId = "person1",
                Date = DateTime.UtcNow,
                TableName = "Table1",
                SeatNumber = 4,
                IsDeleted = false
            };

            entity.DishReservations.AddRange(
                dishIds.Select((d, i) => new DishReservationEntity
                {
                    Id = i + 1,
                    DishId = d,
                    ReservationId = id
                }));

            return entity;
        }

        [Test]
        public async Task AddReservation_ValidRequest_ReturnsSuccess()
        {
            var testReservation = CreateReservation("1", "dish1", "dish2");
            var expectedEntity = CreateReservationEntity(1, "dish1", "dish2");

            _reservationRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<ReservationEntity>()))
                .ReturnsAsync(expectedEntity);

            var result = await _service.AddReservation(testReservation, null);

            Assert.That(result.State, Is.EqualTo(ReplyState.Ok));
            Assert.That(result.Reservations[0].Id, Is.EqualTo("1"));
        }

        [Test]
        public async Task UpdateReservation_WithDishes_UpdatesCorrectly()
        {
            var testReservation = CreateReservation("1", "newDish1", "newDish2");
            var existingEntity = CreateReservationEntity(1, "oldDish1");
            var updatedEntity = CreateReservationEntity(1, "newDish1", "newDish2");

            _reservationRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<ReservationEntity>()))
                .ReturnsAsync(updatedEntity);

            var result = await _service.UpdateReservation(testReservation, null);

            Assert.That(result.Reservations[0].DishIds, Is.EquivalentTo(new[] { "newDish1", "newDish2" }));
        }

        [Test]
        public async Task GetReservations_ByDishId_ReturnsCorrectReservations()
        {
            var testData = new[]
            {
                CreateReservationEntity(1, "dish1"),
                CreateReservationEntity(2, "dish2")
            };

            var request = new ReservationQueryRequest { DishId = "dish1" };

            _reservationRepositoryMock.Setup(x => x.GetByQueryAsync(It.IsAny<Func<ReservationEntity, bool>>()))
                .ReturnsAsync(testData.Where(x => x.DishReservations.Any(d => d.DishId == "dish1")).ToList());

            var result = await _service.GetReservations(request, null);

            Assert.That(result.Reservations, Has.Count.EqualTo(1));
            Assert.That(result.Reservations[0].Id, Is.EqualTo("1"));
        }

        [Test]
        public async Task DeleteReservation_ValidId_ReturnsSuccess()
        {
            _reservationRepositoryMock.Setup(x => x.DeleteAsync(1))
                .ReturnsAsync(1);

            var result = await _service.DeleteReservation(new IdRequest { Id = "1" }, null);

            Assert.That(result.State, Is.EqualTo(ReplyState.Ok));
        }
    }
}