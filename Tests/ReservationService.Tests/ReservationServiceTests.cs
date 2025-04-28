using AutoFixture;
using AutoFixture.AutoMoq;
using Google.Protobuf.Collections;
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

            _fixture.Customize<Reservation>(c => c
                .With(r => r.IsDeleted, false)
                .With(r => r.Date, Timestamp.FromDateTime(DateTime.UtcNow)));

            _fixture.Customize<ReservationEntity>(c => c
                .With(r => r.IsDeleted, false)
                .With(r => r.Date, DateTime.UtcNow));

            _loggerMock = _fixture.Freeze<Mock<ILogger<ReservationServiceApi>>>();
            _reservationRepositoryMock = _fixture.Freeze<Mock<IRepository<ReservationEntity>>>();

            _service = _fixture.Create<ReservationServiceApi>();
        }

        [Test]
        public async Task AddReservation_ValidRequest_ReturnsSuccessWithCreatedReservation()
        {
            var testReservation = _fixture.Create<Reservation>();
            var entity = _fixture.Build<ReservationEntity>()
                .With(e => e.DishReservations, testReservation.DishIds
                    .Select((d, i) => new DishReservationEntity
                    {
                        Id = i + 1,
                        DishId = d,
                        ReservationId = 1
                    })
                    .ToList())
                .Create();

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
            var dishIds = _fixture.CreateMany<string>(2).ToList();
            var repeatedField = new RepeatedField<string>();
            repeatedField.AddRange(dishIds);

            var testReservation = _fixture.Build<Reservation>()
                .With(r => r.DishIds, repeatedField)
                .With(r => r.Id, "1")
                .Create();

            var entity = _fixture.Build<ReservationEntity>()
                .With(e => e.DishReservations, testReservation.DishIds
                    .Select((d, i) => new DishReservationEntity
                    {
                        Id = i + 1,
                        DishId = d,
                        ReservationId = 1
                    })
                    .ToList())
                .Create();

            _reservationRepositoryMock.Setup(x => x.CreateAsync(It.Is<ReservationEntity>(r =>
                r.DishReservations.Count == testReservation.DishIds.Count)))
                .ReturnsAsync(entity);

            var result = await _service.AddReservation(testReservation, null);

            Assert.That(result.Reservations[0].DishIds, Is.EquivalentTo(testReservation.DishIds));
        }

        [Test]
        public async Task GetReservations_ByDishId_ReturnsReservationsWithDish()
        {
            var testDishId = _fixture.Create<string>();
            var testData = _fixture.Build<ReservationEntity>()
                .With(r => r.DishReservations, new List<DishReservationEntity>
                {
                    new DishReservationEntity { DishId = testDishId, ReservationId = 1 }
                })
                .CreateMany(1)
                .Concat(_fixture.Build<ReservationEntity>()
                    .With(r => r.DishReservations, new List<DishReservationEntity>
                    {
                        new DishReservationEntity { DishId = _fixture.Create<string>(), ReservationId = 2 }
                    })
                    .CreateMany(1))
                .ToArray();

            var request = _fixture.Build<ReservationQueryRequest>()
                .With(r => r.DishId, testDishId)
                .Create();

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
            var dishIds = _fixture.CreateMany<string>(2).ToList();
            var testData = _fixture.Build<ReservationEntity>()
                .With(r => r.DishReservations, dishIds
                    .Select((d, i) => new DishReservationEntity
                    {
                        Id = i + 1,
                        DishId = d,
                        ReservationId = 1
                    })
                    .ToList())
                .CreateMany(1)
                .ToArray();

            _reservationRepositoryMock.Setup(x => x.GetByQueryAsync(It.IsAny<Func<ReservationEntity, bool>>()))
                .ReturnsAsync(testData.ToList());

            var result = await _service.GetReservations(new ReservationQueryRequest(), null);

            Assert.That(result.Reservations[0].DishIds, Has.Count.EqualTo(2));
        }

        [Test]
        public async Task UpdateReservation_WithDishes_UpdatesDishReservations()
        {
            var newDishIds = _fixture.CreateMany<string>(2).ToList();
            var dishIdsRepeatedField = new RepeatedField<string>();
            dishIdsRepeatedField.AddRange(newDishIds);

            var testReservation = _fixture.Build<Reservation>()
                .With(r => r.DishIds, dishIdsRepeatedField)
                .With(r => r.Id, "1")
                .Create();

            var oldDishId = _fixture.Create<string>();
            var existingEntity = _fixture.Build<ReservationEntity>()
                .With(r => r.Id, 1)
                .With(r => r.DishReservations, new List<DishReservationEntity>
                {
                    new DishReservationEntity
                    {
                        Id = 1,
                        DishId = oldDishId,
                        ReservationId = 1
                    }
                })
                .Create();

            var updatedEntity = _fixture.Build<ReservationEntity>()
                .With(r => r.Id, 1)
                .With(r => r.DishReservations, newDishIds
                    .Select((d, i) => new DishReservationEntity
                    {
                        Id = i + 2,
                        DishId = d,
                        ReservationId = 1
                    })
                    .ToList())
                .Create();

            _reservationRepositoryMock.Setup(x => x.UpdateAsync(It.Is<ReservationEntity>(r =>
                r.DishReservations.Count == newDishIds.Count &&
                r.DishReservations.All(dr => newDishIds.Contains(dr.DishId)))))
                .ReturnsAsync(updatedEntity);

            var result = await _service.UpdateReservation(testReservation, null);

            Assert.Multiple(() =>
            {
                Assert.That(result.State, Is.EqualTo(ReplyState.Ok));
                Assert.That(result.Reservations, Has.Count.EqualTo(1));
                Assert.That(result.Reservations[0].DishIds, Is.EquivalentTo(newDishIds));
            });
        }

        [Test]
        public async Task DeleteReservation_AlsoDeletesDishReservations()
        {
            var request = _fixture.Build<IdRequest>()
                .With(r => r.Id, "1")
                .Create();

            _reservationRepositoryMock.Setup(x => x.DeleteAsync(1))
                .ReturnsAsync(1);

            var result = await _service.DeleteReservation(request, null);

            Assert.That(result.State, Is.EqualTo(ReplyState.Ok));
            _reservationRepositoryMock.Verify(x => x.DeleteAsync(1), Times.Once);
        }
    }
}