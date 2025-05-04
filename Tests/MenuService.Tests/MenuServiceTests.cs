using GrpcContract;
using GrpcContract.MenuService;
using MenuServiceServer.MenuService;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using TavernHelios.GrpcCommon.Settings;
using TavernHelios.MenuService.APICore.Entities.Menu;
using TavernHelios.MenuService.Common.Entities;
using TavernHelios.MenuService.Common.Extensions;
using TavernHelios.MenuService.Common.Interfaces;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;

namespace MenuService.Tests
{
    [TestFixture]
    public class MenuServiceApiTests
    {
        private IFixture _fixture;
        private Mock<ILogger<MenuServiceApi>> _loggerMock;
        private Mock<IRepository<MenuEntity>> _menuRepositoryMock;
        private Mock<IRepository<DishEntity>> _dishRepositoryMock;
        private Mock<IRepository<MenuScheduleEntity>> _scheduleRepositoryMock;
        private MenuServiceApi _service;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _fixture.Customize<Dish>(c => c
                .With(d => d.IsDeleted, false)
                .Without(d => d.ImageBase64));
            _fixture.Customize<Menu>(c => c
                .With(m => m.IsDeleted, false));

            _loggerMock = _fixture.Freeze<Mock<ILogger<MenuServiceApi>>>();
            _menuRepositoryMock = _fixture.Freeze<Mock<IRepository<MenuEntity>>>();
            _dishRepositoryMock = _fixture.Freeze<Mock<IRepository<DishEntity>>>();
            _scheduleRepositoryMock = _fixture.Freeze<Mock<IRepository<MenuScheduleEntity>>>();

            _service = new MenuServiceApi(
                Options.Create(_fixture.Create<GrpcMenuServiceSettings>()),
                _loggerMock.Object,
                _menuRepositoryMock.Object,
                _dishRepositoryMock.Object,
                _scheduleRepositoryMock.Object);
        }

        #region AddDish Tests

        [Test, AutoData]
        public async Task AddDish_ValidDish_ReturnsSuccessReply(Dish testDish)
        {
            var dishEntity = testDish.ToEntity();

            _dishRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<DishEntity>()))
                .ReturnsAsync(dishEntity);

            var result = await _service.AddDish(testDish, null);

            Assert.That(result.State, Is.EqualTo(ReplyState.Ok));
            Assert.That(result.Dishes.Count, Is.EqualTo(1));
            Assert.That(result.Dishes[0].Id, Is.EqualTo(testDish.Id));
        }

        [Test, AutoData]
        public async Task AddDish_RepositoryReturnsNull_ReturnsErrorReply(Dish testDish)
        {
            _dishRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<DishEntity>()))
                .ReturnsAsync((DishEntity)null);

            var result = await _service.AddDish(testDish, null);

            Assert.That(result.State, Is.EqualTo(ReplyState.Error));
            Assert.That(result.Messages.Any(), Is.True);
        }

        [Test, AutoData]
        public async Task AddDish_ThrowsException_ReturnsErrorReply(Dish testDish)
        {
            _dishRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<DishEntity>()))
                .ThrowsAsync(new Exception("Test exception"));

            var result = await _service.AddDish(testDish, null);

            Assert.That(result.State, Is.EqualTo(ReplyState.Error));
            Assert.That(result.Messages.Any(m => m.Contains("Test exception")), Is.True);
        }

        #endregion

        #region AddDishToMenu Tests

        [Test, AutoData]
        public async Task AddDishToMenu_ValidRequest_ReturnsSuccessReply(
            string menuId,
            string dishId,
            MenuEntity menuEntity,
            DishEntity dishEntity)
        {
            var request = new DishMenuMessage { MenuId = menuId, DishId = dishId };
            menuEntity.Id = menuId;
            menuEntity.Dishes = new List<string>();
            dishEntity.Id = dishId;

            _menuRepositoryMock.Setup(x => x.GetByIdAsync(menuId))
                .ReturnsAsync(menuEntity);
            _dishRepositoryMock.Setup(x => x.GetByIdAsync(dishId))
                .ReturnsAsync(dishEntity);
            _menuRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<MenuEntity>()))
                .ReturnsAsync(menuEntity);

            var result = await _service.AddDishToMenu(request, null);

            Assert.That(result.State, Is.EqualTo(ReplyState.Ok));
            Assert.That(result.Menus.Count, Is.EqualTo(1));
            Assert.That(result.Menus[0].Dishes.Contains(dishId), Is.True);
        }

        [Test]
        public async Task AddDishToMenu_MenuNotFound_ReturnsErrorReply()
        {
            var testRequest = new DishMenuMessage { MenuId = "menu-123", DishId = "dish-456" };

            _menuRepositoryMock.Setup(x => x.GetByIdAsync(testRequest.MenuId))
                .ReturnsAsync((MenuEntity)null);

            var result = await _service.AddDishToMenu(testRequest, null);

            Assert.That(result.State, Is.EqualTo(ReplyState.Error));
            Assert.That(result.Messages.Any(m => m.Contains("не найдено")), Is.True);
        }

        //[Test, AutoData]
        //public async Task AddDishToMenu_MenuNotFound_ReturnsErrorReply(DishMenuMessage request)
        //{
        //    _menuRepositoryMock.Setup(x => x.GetByIdAsync(request.MenuId))
        //        .ReturnsAsync((MenuEntity)null);

        //    var result = await _service.AddDishToMenu(request, null);

        //    Assert.That(result.State, Is.EqualTo(ReplyState.Error));
        //    Assert.That(result.Messages.Any(m => m.Contains("не найдено")), Is.True);
        //}

        [Test]
        public async Task AddDishToMenu_DishAlreadyInMenu_ReturnsErrorReply()
        {
            var testRequest = new DishMenuMessage { MenuId = "menu-abc", DishId = "dish-def" };
            var menuEntity = new MenuEntity { Id = testRequest.MenuId, Dishes = new List<string> { testRequest.DishId } };

            _menuRepositoryMock.Setup(x => x.GetByIdAsync(testRequest.MenuId))
                .ReturnsAsync(menuEntity);

            var result = await _service.AddDishToMenu(testRequest, null);

            Assert.That(result.State, Is.EqualTo(ReplyState.Error));
            Assert.That(result.Messages.Any(m => m.Contains("уже содержит")), Is.True);
        }

        //[Test, AutoData]
        //public async Task AddDishToMenu_DishAlreadyInMenu_ReturnsErrorReply(
        //    string menuId,
        //    string dishId)
        //{
        //    var request = new DishMenuMessage { MenuId = menuId, DishId = dishId };
        //    var menuEntity = new MenuEntity { Id = menuId, Dishes = new List<string> { dishId } };

        //    _menuRepositoryMock.Setup(x => x.GetByIdAsync(menuId))
        //        .ReturnsAsync(menuEntity);

        //    var result = await _service.AddDishToMenu(request, null);

        //    Assert.That(result.State, Is.EqualTo(ReplyState.Error));
        //    Assert.That(result.Messages.Any(m => m.Contains("уже содержит")), Is.True);
        //}

        #endregion

        #region GetMenus Tests

        [Test, AutoData]
        public async Task GetMenus_WithQuery_ReturnsFilteredMenus(List<MenuEntity> testMenus)
        {
            var request = new MenuQueryRequest { Name = testMenus[0].Name };

            _menuRepositoryMock.Setup(x => x.GetByConditionAsync(It.IsAny<Func<MenuEntity, bool>>()))
                .ReturnsAsync(testMenus.Where(m => m.Name == testMenus[0].Name).ToList());

            var result = await _service.GetMenus(request, null);

            Assert.That(result.State, Is.EqualTo(ReplyState.Ok));
            Assert.That(result.Menus.Count, Is.EqualTo(1));
            Assert.That(result.Menus[0].Name, Is.EqualTo(testMenus[0].Name));
        }

        [Test]
        public async Task GetMenus_ThrowsException_ReturnsErrorReply()
        {
            var request = new MenuQueryRequest();

            _menuRepositoryMock.Setup(x => x.GetByConditionAsync(It.IsAny<Func<MenuEntity, bool>>()))
                .ThrowsAsync(new Exception("Test exception"));

            var result = await _service.GetMenus(request, null);

            Assert.That(result.State, Is.EqualTo(ReplyState.Error));
            Assert.That(result.Messages.Any(m => m.Contains("Test exception")), Is.True);
        }

        #endregion

        #region AddMenuSchedule Tests

        [Test, AutoData]
        public async Task AddMenuSchedule_ValidRequest_ReturnsSuccessReply(
            MenuScheduleRequest request,
            MenuEntity menuEntity)
        {
            var scheduleEntity = request.ToEntity();
            menuEntity.Id = request.MenuId;

            _scheduleRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<MenuScheduleEntity>()))
                .ReturnsAsync(scheduleEntity);
            _menuRepositoryMock.Setup(x => x.GetByIdAsync(request.MenuId))
                .ReturnsAsync(menuEntity);

            var result = await _service.AddMenuSchedule(request, null);

            Assert.That(result.State, Is.EqualTo(ReplyState.Ok));
            Assert.That(result.MenusSchedules.Count, Is.EqualTo(1));
            Assert.That(result.MenusSchedules[0].Id, Is.EqualTo(request.Id));
        }

        [Test]
        public async Task AddMenuSchedule_RepositoryReturnsNull_ReturnsErrorReply()
        {
            var testRequest = new MenuScheduleRequest { Id = "schedule1", MenuId = "menu-1" };

            _scheduleRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<MenuScheduleEntity>()))
                .ReturnsAsync((MenuScheduleEntity)null);

            var result = await _service.AddMenuSchedule(testRequest, null);

            Assert.That(result.State, Is.EqualTo(ReplyState.Error));
            Assert.That(result.Messages.Any(m => m.Contains("Ошибка при добавлении")), Is.True);
        }

        //[Test, AutoData]
        //public async Task AddMenuSchedule_RepositoryReturnsNull_ReturnsErrorReply(MenuScheduleRequest request)
        //{
        //    _scheduleRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<MenuScheduleEntity>()))
        //        .ReturnsAsync((MenuScheduleEntity)null);

        //    var result = await _service.AddMenuSchedule(request, null);

        //    Assert.That(result.State, Is.EqualTo(ReplyState.Error));
        //    Assert.That(result.Messages.Any(m => m.Contains("Ошибка при добавлении")), Is.True);
        //}

        #endregion

        #region DeleteMenu Tests

        [Test, AutoData]
        public async Task DeleteMenu_ValidId_ReturnsSuccessReply(string menuId)
        {
            var request = new IdRequest { Id = menuId };

            _menuRepositoryMock.Setup(x => x.DeleteAsync(menuId))
                .ReturnsAsync(menuId);

            var result = await _service.DeleteMenu(request, null);

            Assert.That(result.State, Is.EqualTo(ReplyState.Ok));
            Assert.That(result.Id, Is.EqualTo(menuId));
        }

        [Test]
        public async Task DeleteMenu_RepositoryReturnsNull_ReturnsErrorReply()
        {
            var testRequest = new IdRequest { Id = "menu-404" };

            _menuRepositoryMock.Setup(x => x.DeleteAsync(testRequest.Id))
                .ReturnsAsync((string)null);

            var result = await _service.DeleteMenu(testRequest, null);

            Assert.That(result.State, Is.EqualTo(ReplyState.Error));
            Assert.That(result.Messages.Any(m => m.Contains("Ошибка при удалении")), Is.True);
        }

        //[Test, AutoData]
        //public async Task DeleteMenu_RepositoryReturnsNull_ReturnsErrorReply(IdRequest request)
        //{
        //    _menuRepositoryMock.Setup(x => x.DeleteAsync(request.Id))
        //        .ReturnsAsync((string)null);

        //    var result = await _service.DeleteMenu(request, null);

        //    Assert.That(result.State, Is.EqualTo(ReplyState.Error));
        //    Assert.That(result.Messages.Any(m => m.Contains("Ошибка при удалении")), Is.True);
        //}

        #endregion
    }

    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute() : base(() =>
            new Fixture().Customize(new AutoMoqCustomization()))
        {
        }
    }
}