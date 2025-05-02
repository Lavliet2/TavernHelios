using AutoFixture;
using AutoFixture.AutoMoq;
using DishServiceServer.Controllers;
using Grpc.Core;
using GrpcContract;
using GrpcContract.MenuService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TavernHelios.MenuService.Common.DTOValues.Menu;

namespace DishServiceServer.Tests.Controllers
{
    [TestFixture]
    public class DishControllerTests
    {
        private Fixture _fixture;
        private Mock<MenuService.MenuServiceClient> _grpcClientMock;
        private Mock<ILogger<DishController>> _loggerMock;
        private DishController _controller;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization());

            _grpcClientMock = new Mock<MenuService.MenuServiceClient>();
            _loggerMock = new Mock<ILogger<DishController>>();

            _controller = new DishController(_loggerMock.Object, _grpcClientMock.Object);
        }

        [Test]
        public async Task GetByConditionAsync_ReturnsOkWithDishes_WhenGrpcCallSucceeds()
        {
            var queryRequest = _fixture.Create<DishQueryRequestValue>();
            var grpcDishes = _fixture.CreateMany<Dish>().ToList();
            var grpcReply = new DishesReply
            {
                State = ReplyState.Ok,
                Dishes = { grpcDishes }
            };

            _grpcClientMock.Setup(x => x.GetDishesAsync(
                It.IsAny<DishQueryRequest>(), null, null, default))
                .Returns(new AsyncUnaryCall<DishesReply>(
                    Task.FromResult(grpcReply),
                    null, null, null, null));

            var result = await _controller.GetByConditionAsync(queryRequest);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            var returnedDishes = okResult.Value as IEnumerable<DishValue>;
            Assert.AreEqual(grpcDishes.Count, returnedDishes.Count());
        }

        [Test]
        public async Task GetDishByIdAsync_ReturnsOkWithDish_WhenDishExists()
        {
            var dishId = _fixture.Create<string>();
            var grpcDish = _fixture.Create<Dish>();
            var grpcReply = new DishesReply
            {
                State = ReplyState.Ok,
                Dishes = { grpcDish }
            };

            _grpcClientMock.Setup(x => x.GetDishesAsync(
                It.IsAny<DishQueryRequest>(), null, null, default))
                .Returns(new AsyncUnaryCall<DishesReply>(
                    Task.FromResult(grpcReply),
                    null, null, null, null));

            var result = await _controller.GetDishByIdAsync(dishId);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult.Value as DishValue);
        }

        [Test]
        public async Task GetDishByIdAsync_ReturnsNotFound_WhenDishDoesNotExist()
        {
            var dishId = _fixture.Create<string>();
            var grpcReply = new DishesReply
            {
                State = ReplyState.Error,
                Messages = { "Not found" }
            };

            _grpcClientMock.Setup(x => x.GetDishesAsync(
                It.IsAny<DishQueryRequest>(), null, null, default))
                .Returns(new AsyncUnaryCall<DishesReply>(
                    Task.FromResult(grpcReply),
                    null, null, null, null));

            var result = await _controller.GetDishByIdAsync(dishId);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task CreateDishAsync_ReturnsCreated_WhenDishIsCreated()
        {
            var dishValue = _fixture.Create<DishValue>();
            var grpcDish = _fixture.Create<Dish>();
            var grpcReply = new DishesReply
            {
                State = ReplyState.Ok,
                Dishes = { grpcDish }
            };

            _grpcClientMock.Setup(x => x.AddDishAsync(
                It.IsAny<Dish>(), null, null, default))
                .Returns(new AsyncUnaryCall<DishesReply>(
                    Task.FromResult(grpcReply),
                    null, null, null, null));

            var result = await _controller.CreateDishAsync(dishValue);

            Assert.IsInstanceOf<CreatedResult>(result);
        }

        [Test]
        public async Task CreateDishAsync_ReturnsBadRequest_WhenCreationFails()
        {
            var dishValue = _fixture.Create<DishValue>();
            var grpcReply = new DishesReply
            {
                State = ReplyState.Error,
                Messages = { "Error occurred" }
            };

            _grpcClientMock.Setup(x => x.AddDishAsync(
                It.IsAny<Dish>(), null, null, default))
                .Returns(new AsyncUnaryCall<DishesReply>(
                    Task.FromResult(grpcReply),
                    null, null, null, null));

            var result = await _controller.CreateDishAsync(dishValue);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task UpdateDishAsync_ReturnsOk_WhenUpdateSucceeds()
        {
            var dishValue = _fixture.Create<DishValue>();
            var grpcDish = _fixture.Create<Dish>();
            var grpcReply = new DishesReply
            {
                State = ReplyState.Ok,
                Dishes = { grpcDish }
            };

            _grpcClientMock.Setup(x => x.UpdateDishAsync(
                It.IsAny<Dish>(), null, null, default))
                .Returns(new AsyncUnaryCall<DishesReply>(
                    Task.FromResult(grpcReply),
                    null, null, null, null));

            var result = await _controller.UpdateDishAsync(dishValue);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task UpdateDishAsync_ReturnsBadRequest_WhenUpdateFails()
        {
            var dishValue = _fixture.Create<DishValue>();
            var grpcReply = new DishesReply
            {
                State = ReplyState.Error,
                Messages = { "Error occurred" }
            };

            _grpcClientMock.Setup(x => x.UpdateDishAsync(
                It.IsAny<Dish>(), null, null, default))
                .Returns(new AsyncUnaryCall<DishesReply>(
                    Task.FromResult(grpcReply),
                    null, null, null, null));

            var result = await _controller.UpdateDishAsync(dishValue);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task DeleteDishAsync_ReturnsNoContent_WhenDeletionSucceeds()
        {
            var dishId = _fixture.Create<string>();
            var grpcReply = new IdReply
            {
                State = ReplyState.Ok
            };

            _grpcClientMock.Setup(x => x.DeleteDishAsync(
                It.IsAny<IdRequest>(), null, null, default))
                .Returns(new AsyncUnaryCall<IdReply>(
                    Task.FromResult(grpcReply),
                    null, null, null, null));

            var result = await _controller.DeleteDishAsync(dishId);

            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeleteDishAsync_ReturnsBadRequest_WhenDeletionFails()
        {
            var dishId = _fixture.Create<string>();
            var grpcReply = new IdReply
            {
                State = ReplyState.Error,
                Messages = { "Error occurred" }
            };

            _grpcClientMock.Setup(x => x.DeleteDishAsync(
                It.IsAny<IdRequest>(), null, null, default))
                .Returns(new AsyncUnaryCall<IdReply>(
                    Task.FromResult(grpcReply),
                    null, null, null, null));

            var result = await _controller.DeleteDishAsync(dishId);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
    }
}