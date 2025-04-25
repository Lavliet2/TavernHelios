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

namespace MenuService.Tests
{
    [TestFixture]
    public class MenuServiceApiTests
    {
        private Mock<ILogger<MenuServiceApi>> _loggerMock;
        private Mock<IRepository<MenuEntity>> _menuRepositoryMock;
        private Mock<IRepository<DishEntity>> _dishRepositoryMock;
        private Mock<IRepository<MenuScheduleEntity>> _scheduleRepositoryMock;
        private MenuServiceApi _service;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<MenuServiceApi>>();
            _menuRepositoryMock = new Mock<IRepository<MenuEntity>>();
            _dishRepositoryMock = new Mock<IRepository<DishEntity>>();
            _scheduleRepositoryMock = new Mock<IRepository<MenuScheduleEntity>>();

            _service = new MenuServiceApi(
                Options.Create(new GrpcMenuServiceSettings()),
                _loggerMock.Object,
                _menuRepositoryMock.Object,
                _dishRepositoryMock.Object,
                _scheduleRepositoryMock.Object);
        }

        #region Test Data Factories

        private static Dish CreateTestDish(string id = "dish1", string name = "Test Dish", string description = "Description", int dishType = 1)
        {
            return new Dish
            {
                Id = id,
                Name = name,
                Description = description,
                DishType = dishType,
                ImageBase64 = "",
                IsDeleted = false
            };
        }

        private static Menu CreateTestMenu(string id = "menu1", string name = "Test Menu", IEnumerable<string> dishIds = null)
        {
            var menu = new Menu
            {
                Id = id,
                Name = name,
                IsDeleted = false
            };

            if (dishIds != null)
            {
                menu.Dishes.AddRange(dishIds);
            }

            return menu;
        }

        private static MenuScheduleRequest CreateTestMenuScheduleRequest(string id = "schedule1", string menuId = "menu1")
        {
            return new MenuScheduleRequest
            {
                Id = id,
                MenuId = menuId,
                DateTime = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow)
            };
        }

        #endregion

        #region AddDish Tests

        [Test]
        public async Task AddDish_ValidDish_ReturnsSuccessReply()
        {
            var testDish = CreateTestDish();
            var dishEntity = testDish.ToEntity();

            _dishRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<DishEntity>()))
                .ReturnsAsync(dishEntity);

            var result = await _service.AddDish(testDish, null);

            Assert.That(result.State, Is.EqualTo(ReplyState.Ok));
            Assert.That(result.Dishes.Count, Is.EqualTo(1));
            Assert.That(result.Dishes[0].Id, Is.EqualTo(testDish.Id));
        }

        [Test]
        public async Task AddDish_RepositoryReturnsNull_ReturnsErrorReply()
        {
            var testDish = CreateTestDish();

            _dishRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<DishEntity>()))
                .ReturnsAsync((DishEntity)null);

            var result = await _service.AddDish(testDish, null);

            Assert.That(result.State, Is.EqualTo(ReplyState.Error));
            Assert.That(result.Messages.Any(), Is.True);
        }

        [Test]
        public async Task AddDish_ThrowsException_ReturnsErrorReply()
        {
            var testDish = CreateTestDish();

            _dishRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<DishEntity>()))
                .ThrowsAsync(new Exception("Test exception"));

            var result = await _service.AddDish(testDish, null);

            Assert.That(result.State, Is.EqualTo(ReplyState.Error));
            Assert.That(result.Messages.Any(m => m.Contains("Test exception")), Is.True);
        }

        #endregion

        #region AddDishToMenu Tests

        [Test]
        public async Task AddDishToMenu_ValidRequest_ReturnsSuccessReply()
        {
            var menuId = "menu1";
            var dishId = "dish1";
            var request = new DishMenuMessage { MenuId = menuId, DishId = dishId };

            var menuEntity = new MenuEntity { Id = menuId, Dishes = new List<string>() };
            var dishEntity = new DishEntity { Id = dishId };

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
            var request = new DishMenuMessage { MenuId = "nonexistent", DishId = "dish1" };

            _menuRepositoryMock.Setup(x => x.GetByIdAsync(request.MenuId))
                .ReturnsAsync((MenuEntity)null);

            var result = await _service.AddDishToMenu(request, null);

            Assert.That(result.State, Is.EqualTo(ReplyState.Error));
            Assert.That(result.Messages.Any(m => m.Contains("не найдено")), Is.True);
        }

        [Test]
        public async Task AddDishToMenu_DishAlreadyInMenu_ReturnsErrorReply()
        {
            var menuId = "menu1";
            var dishId = "dish1";
            var request = new DishMenuMessage { MenuId = menuId, DishId = dishId };

            var menuEntity = new MenuEntity { Id = menuId, Dishes = new List<string> { dishId } };

            _menuRepositoryMock.Setup(x => x.GetByIdAsync(menuId))
                .ReturnsAsync(menuEntity);

            var result = await _service.AddDishToMenu(request, null);

            Assert.That(result.State, Is.EqualTo(ReplyState.Error));
            Assert.That(result.Messages.Any(m => m.Contains("уже содержит")), Is.True);
        }

        #endregion

        #region GetMenus Tests

        [Test]
        public async Task GetMenus_WithQuery_ReturnsFilteredMenus()
        {
            var testMenus = new List<MenuEntity>
            {
                new MenuEntity { Id = "menu1", Name = "Menu 1" },
                new MenuEntity { Id = "menu2", Name = "Menu 2" }
            };

            var request = new MenuQueryRequest { Name = "Menu 1" };

            _menuRepositoryMock.Setup(x => x.GetByConditionAsync(It.IsAny<Func<MenuEntity, bool>>()))
                .ReturnsAsync(testMenus.Where(m => m.Name == "Menu 1").ToList());

            var result = await _service.GetMenus(request, null);

            Assert.That(result.State, Is.EqualTo(ReplyState.Ok));
            Assert.That(result.Menus.Count, Is.EqualTo(1));
            Assert.That(result.Menus[0].Name, Is.EqualTo("Menu 1"));
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

        [Test]
        public async Task AddMenuSchedule_ValidRequest_ReturnsSuccessReply()
        {
            var request = CreateTestMenuScheduleRequest();
            var scheduleEntity = request.ToEntity();
            var menuEntity = new MenuEntity { Id = request.MenuId };

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
            var request = CreateTestMenuScheduleRequest();

            _scheduleRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<MenuScheduleEntity>()))
                .ReturnsAsync((MenuScheduleEntity)null);

            var result = await _service.AddMenuSchedule(request, null);

            Assert.That(result.State, Is.EqualTo(ReplyState.Error));
            Assert.That(result.Messages.Any(m => m.Contains("Ошибка при добавлении")), Is.True);
        }

        #endregion

        #region DeleteMenu Tests

        [Test]
        public async Task DeleteMenu_ValidId_ReturnsSuccessReply()
        {
            var menuId = "menu1";
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
            var request = new IdRequest { Id = "nonexistent" };

            _menuRepositoryMock.Setup(x => x.DeleteAsync(request.Id))
                .ReturnsAsync((string)null);

            var result = await _service.DeleteMenu(request, null);

            Assert.That(result.State, Is.EqualTo(ReplyState.Error));
            Assert.That(result.Messages.Any(m => m.Contains("Ошибка при удалении")), Is.True);
        }

        #endregion
    }
}