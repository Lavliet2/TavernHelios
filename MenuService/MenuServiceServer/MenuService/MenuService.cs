using TavernHelios.MenuService.Common.Interfaces;
using Grpc.Core;
using GrpcContract;
using Microsoft.Extensions.Options;
using TavernHelios.MenuService.Common.Entities;
using TavernHelios.GrpcCommon.Settings;
using static GrpcContract.MenuService.MenuService;
using GrpcContract.MenuService;
using TavernHelios.MenuService.Common.Extensions;
using TavernHelios.MenuService.APICore.Entities.Menu;
using System.Linq.Expressions;

namespace MenuServiceServer.MenuService
{
    /// <summary>
    /// Логика GRPC сервера, общение с репозиториями
    /// </summary>
    public class MenuServiceApi : MenuServiceBase
    {
        private readonly ILogger<MenuServiceApi> _logger;
        private readonly IRepository<MenuEntity> _menuRepository;
        private readonly IRepository<DishEntity> _dishRepository;
        private readonly IRepository<MenuScheduleEntity> _scheduleRepository;
        private readonly GrpcMenuServiceSettings _settings;

        public MenuServiceApi(
            IOptions<GrpcMenuServiceSettings> settings,
            ILogger<MenuServiceApi> logger,
            IRepository<MenuEntity> menuRepository,
            IRepository<DishEntity> dishRepository,
            IRepository<MenuScheduleEntity> scheduleRepository
            )
        {
            _settings = settings.Value;
            _logger = logger;
            _menuRepository = menuRepository;
            _dishRepository = dishRepository;
            _scheduleRepository = scheduleRepository;
        }

        public override async Task<DishesReply> AddDish(Dish request, ServerCallContext context)
        {
            try
            {
                var addResult = await _dishRepository.CreateAsync(request.ToEntity());

                if (addResult == null)
                {
                    return CreateErrorDishesReply("Ошибка при добавлении блюда");
                }

                var result = new DishesReply() { State = ReplyState.Ok };
                result.Dishes.Add(addResult.ToGrpc());
                return result;
            }
            catch (Exception ex)
            {
                return CreateErrorDishesReply(ex.Message);
            }
           
        }

        public override async Task<MenusReply> AddDishToMenu(DishMenuMessage request, ServerCallContext context)
        {
            try
            {
                var menuEntity = await _menuRepository.GetByIdAsync(request.MenuId);
                if (menuEntity == null)
                    return CreateErrorMenusReply($"Меню с ID={request.MenuId} не найдено");

                if (menuEntity.Dishes.Contains(request.DishId))
                    return CreateErrorMenusReply($"Меню с ID={request.MenuId} не уже содержит блюдо с ID={request.DishId}");

                var dishEntity = await _dishRepository.GetByIdAsync(request.DishId);
                if (dishEntity == null)
                    return CreateErrorMenusReply($"Блюдо с ID={request.DishId} не найдено");

                menuEntity.Dishes.Add(request.DishId);

                var menuResult = await _menuRepository.UpdateAsync(menuEntity);

                if (menuResult == null)
                    return CreateErrorMenusReply("Ошибка при изменении меню");

                var result = new MenusReply() { State = ReplyState.Ok };
                result.Menus.Add(menuResult.ToGrpc());
                return result;
            }
            catch (Exception ex)
            {
                return CreateErrorMenusReply(ex.Message);
            }
        }

        public override async Task<MenusReply> AddMenu(Menu request, ServerCallContext context)
        {
            try
            {
                var addResult = await _menuRepository.CreateAsync(request.ToEntity());

                if (addResult == null)
                {
                    return CreateErrorMenusReply("Ошибка при добавлении меню");
                }

                var result = new MenusReply() { State = ReplyState.Ok };
                result.Menus.Add(addResult.ToGrpc());
                return result;
            }
            catch (Exception ex)
            {
                return CreateErrorMenusReply(ex.Message);
            }
        }

        public override async Task<IdReply> DeleteDish(IdRequest request, ServerCallContext context)
        {
            try
            {
                var deleteResult = await _dishRepository.DeleteAsync(request.Id);

                if (deleteResult == null)
                {
                    return CreateErrorIdReply("Ошибка при удалении блюда");
                }

                var result = new IdReply() { State = ReplyState.Ok };
                result.Id = deleteResult;
                return result;
            }
            catch (Exception ex)
            {
                return CreateErrorIdReply(ex.Message);
            }
        }

        public override async Task<IdReply> DeleteMenu(IdRequest request, ServerCallContext context)
        {
            try
            {
                var deleteResult = await _menuRepository.DeleteAsync(request.Id);

                if (deleteResult == null)
                {
                    return CreateErrorIdReply("Ошибка при удалении меню");
                }

                var result = new IdReply() { State = ReplyState.Ok };
                result.Id = deleteResult;
                return result;
            }
            catch (Exception ex)
            {
                return CreateErrorIdReply(ex.Message);
            }
        }

        public override async Task<DishesReply> GetAllDishesForMenu(IdRequest request, ServerCallContext context)
        {
            try
            {
                var menu = await _menuRepository.GetByIdAsync(request.Id);

                var dishIds = menu.Dishes.ToList();

                //TODO: Здесь загружаются ВСЕ блюда, а потом фильтруются. Нужно прокинуть условие в репозиторий и применять там
                var addResult = (await _dishRepository.GetAllAsync())
                    .Where(x => dishIds.Contains(x.Id));

                if (addResult == null)
                {
                    return CreateErrorDishesReply("Ошибка при получении блюд из БД");
                }

                var result = new DishesReply() { State = ReplyState.Ok };
                result.Dishes.AddRange(addResult.Select(x => x.ToGrpc()));
                return result;
            }
            catch (Exception ex)
            {
                return CreateErrorDishesReply(ex.Message);
            }
        }

        public override async Task<MenusReply> GetMenus(MenuQueryRequest request, ServerCallContext context)
        {
            try
            {
                var condition = request.ToQuery().Compile();
                var getResult = await _menuRepository.GetByConditionAsync(condition);
                var result = new MenusReply() { State= ReplyState.Ok };
                result.Menus.AddRange(getResult.Select(x => x.ToGrpc()));
                return result;
            }
            catch (Exception ex)
            {
                return CreateErrorMenusReply(ex.Message);
            }
        }

        public override async Task<DishesReply> GetDishes(DishQueryRequest request, ServerCallContext context)
        {
            try
            {
                var condition = request.ToQuery().Compile();
                var getResult = await _dishRepository.GetByConditionAsync(condition);
                var result = new DishesReply() { State = ReplyState.Ok };
                result.Dishes.AddRange(getResult.Select(x => x.ToGrpc()));
                return result;
            }
            catch (Exception ex)
            {
                return CreateErrorDishesReply(ex.Message);
            }
        }

        public override async Task<MenusReply> RemoveDishFromMenu(DishMenuMessage request, ServerCallContext context)
        {
            try
            {
                var menuEntity = await _menuRepository.GetByIdAsync(request.MenuId);
                if (menuEntity == null)
                    return CreateErrorMenusReply($"Меню с ID={request.MenuId} не найдено");

                if (!menuEntity.Dishes.Contains(request.DishId))
                    return CreateErrorMenusReply($"Меню с ID={request.MenuId} не содержит блюдо с ID={request.DishId}");

                var dishEntity = await _dishRepository.GetByIdAsync(request.DishId);
                if (dishEntity == null)
                    return CreateErrorMenusReply($"Блюдо с ID={request.DishId} не найдено");

                menuEntity.Dishes.Remove(menuEntity.Dishes.First(x => x == request.DishId));

                var menuResult = await _menuRepository.UpdateAsync(menuEntity);

                if (menuResult == null)
                    return CreateErrorMenusReply("Ошибка при изменении меню");

                var result = new MenusReply() { State = ReplyState.Ok };
                result.Menus.Add(menuResult.ToGrpc());
                return result;
            }
            catch (Exception ex)
            {
                return CreateErrorMenusReply(ex.Message);
            }
        }

        public override async Task<DishesReply> UpdateDish(Dish request, ServerCallContext context)
        {
            try
            {
                var addResult = await _dishRepository.UpdateAsync(request.ToEntity());

                if (addResult == null)
                {
                    return CreateErrorDishesReply("Ошибка при редактировании блюда");
                }

                var result = new DishesReply() { State = ReplyState.Ok };
                result.Dishes.Add(addResult.ToGrpc());
                return result;
            }
            catch (Exception ex)
            {
                return CreateErrorDishesReply(ex.Message);
            }
        }

        public override async Task<MenusReply> UpdateMenu(Menu request, ServerCallContext context)
        {
            try
            {
                var addResult = await _menuRepository.UpdateAsync(request.ToEntity());

                if (addResult == null)
                {
                    return CreateErrorMenusReply("Ошибка при редактировании меню");
                }

                var result = new MenusReply() { State = ReplyState.Ok };
                result.Menus.Add(addResult.ToGrpc());
                return result;
            }
            catch (Exception ex)
            {
                return CreateErrorMenusReply(ex.Message);
            }
        }

        public override async Task<MenuSchedulesReply> GetMenuSchedules(MenuScheduleQueryRequest request, ServerCallContext context)
        {
            try
            {
                var condition = request.ToQuery().Compile();
                var getResult = await _scheduleRepository.GetByConditionAsync(condition);
                var menuIdList = getResult.Select(x => x.MenuId).Distinct().ToList();
                Expression<Func<MenuEntity, bool>> menuCondition = x => menuIdList.Contains(x.Id);
                var getMenusResult = await _menuRepository.GetByConditionAsync(menuCondition.Compile());
                var result = new MenuSchedulesReply() { State= ReplyState.Ok };
                result.MenusSchedules.AddRange(getResult.Select(x => x.ToGrpc(getMenusResult.FirstOrDefault(m => m.Id == x.MenuId))));
                return result;
            }
            catch (Exception ex)
            {
                return CreateErrorSchedulesReply(ex.Message);
            }
        }

        public override async Task<MenuSchedulesReply> AddMenuSchedule(MenuScheduleRequest request, ServerCallContext context)
        {
            try
            {   
                var addResult = await _scheduleRepository.CreateAsync(request.ToEntity());

                if (addResult == null)
                {
                    return CreateErrorSchedulesReply("Ошибка при добавлении расписания меню");
                }

                var result = new MenuSchedulesReply() { State = ReplyState.Ok };
                var menuId = addResult.MenuId;
                Expression<Func<MenuEntity, bool>> menuCondition = x => menuId == x.Id;
                var getMenuResult = await _menuRepository.GetByIdAsync(menuId);
                result.MenusSchedules.Add(addResult.ToGrpc(getMenuResult));
                return result;
            }
            catch (Exception ex)
            {
                return CreateErrorSchedulesReply(ex.Message);
            }
        }

        public override async Task<MenuSchedulesReply> UpdateMenuSchedule(MenuScheduleRequest request, ServerCallContext context)
        {
            try
            {
                var updateResult = await _scheduleRepository.UpdateAsync(request.ToEntity());


                if (updateResult == null)
                {
                    return CreateErrorSchedulesReply("Ошибка при изменении расписания меню");
                }

                var result = new MenuSchedulesReply() { State = ReplyState.Ok };
                var menuId = updateResult.MenuId;
                Expression<Func<MenuEntity, bool>> menuCondition = x => menuId == x.Id;
                var getMenuResult = await _menuRepository.GetByIdAsync(menuId);
                result.MenusSchedules.Add(updateResult.ToGrpc(getMenuResult));
                return result;
            }
            catch (Exception ex)
            {
                return CreateErrorSchedulesReply(ex.Message);
            }
        }

        public override async Task<IdReply> DeleteMenuSchedule(IdRequest request, ServerCallContext context)
        {
            try
            {
                var deleteResult = await _scheduleRepository.DeleteAsync(request.Id);

                if (deleteResult == null)
                {
                    return CreateErrorIdReply("Ошибка при удалении единицы расписания");
                }

                var result = new IdReply() { State = ReplyState.Ok };
                result.Id = deleteResult;
                return result;
            }
            catch (Exception ex)
            {
                return CreateErrorIdReply(ex.Message);
            }
        }

        private DishesReply CreateErrorDishesReply(string message)
        {
            var reply = new DishesReply()
            {
                State = ReplyState.Error
            };
            reply.Messages.Add(message);
            return reply;
        }

        private MenusReply CreateErrorMenusReply(string message)
        {
            var reply = new MenusReply()
            {
                State = ReplyState.Error
            };
            reply.Messages.Add(message);
            return reply;
        }

        private MenuSchedulesReply CreateErrorSchedulesReply(string message)
        {
            var reply = new MenuSchedulesReply()
            {
                State = ReplyState.Error
            };
            reply.Messages.Add(message);
            return reply;
        }

        private IdReply CreateErrorIdReply(string message)
        {
            var reply = new IdReply()
            {
                State = ReplyState.Error
            };
            reply.Messages.Add(message);
            return reply;
        }

        
    }
}
