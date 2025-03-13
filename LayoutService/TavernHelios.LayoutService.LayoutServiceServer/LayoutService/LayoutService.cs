using TavernHelios.LayoutService.Common.Interfaces;
using Grpc.Core;
using GrpcContract;
using Microsoft.Extensions.Options;
using TavernHelios.LayoutService.Common.Entities;
using TavernHelios.GrpcCommon.Settings;
using static GrpcContract.LayoutService.LayoutService;
using GrpcContract.LayoutService;
using TavernHelios.LayoutService.APICore.Entities.Layout;
using System.Linq.Expressions;
using TavernHelios.LayoutService.APICore.Extensions;

namespace LayoutServiceServer.LayoutService
{
    /// <summary>
    /// Логика GRPC сервера, общение с репозиториями
    /// </summary>
    public class LayoutServiceApi : LayoutServiceBase
    {
        private readonly ILogger<LayoutServiceApi> _logger;
        private readonly IRepository<LayoutEntity> _layoutRepository;
        private readonly GrpcLayoutServiceSettings _settings;

        public LayoutServiceApi(
            IOptions<GrpcLayoutServiceSettings> settings,
            ILogger<LayoutServiceApi> logger,
            IRepository<LayoutEntity> LayoutRepository
            )
        {
            _settings = settings.Value;
            _logger = logger;
            _layoutRepository = LayoutRepository;
        }

        
        public override async Task<LayoutsReply> AddLayout(AddLayoutRequest request, ServerCallContext context)
        {
            try
            {
                var addResult = await _layoutRepository.CreateAsync(request.ToEntity());

                if (addResult == null)
                {
                    return CreateErrorLayoutsReply("Ошибка при добавлении схемы");
                }

                var result = new LayoutsReply() { State = ReplyState.Ok };
                result.Layouts.Add(addResult.ToGrpc());
                return result;
            }
            catch (Exception ex)
            {
                return CreateErrorLayoutsReply(ex.Message);
            }
        }

        

        public override async Task<IdReply> DeleteLayout(IdRequest request, ServerCallContext context)
        {
            try
            {
                var deleteResult = await _layoutRepository.DeleteAsync(request.Id);

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

        

        public override async Task<LayoutsReply> GetLayouts(LayoutQueryRequest request, ServerCallContext context)
        {
            try
            {
                var condition = request.ToQuery().Compile();
                var getResult = await _layoutRepository.GetByConditionAsync(condition);
                var result = new LayoutsReply() { State= ReplyState.Ok };
                result.Layouts.AddRange(getResult.Select(x => x.ToGrpc()));
                return result;
            }
            catch (Exception ex)
            {
                return CreateErrorLayoutsReply(ex.Message);
            }
        }

        
        public override async Task<LayoutsReply> UpdateLayout(Layout request, ServerCallContext context)
        {
            try
            {
                var addResult = await _layoutRepository.UpdateAsync(request.ToEntity());

                if (addResult == null)
                {
                    return CreateErrorLayoutsReply("Ошибка при редактировании схемы");
                }

                var result = new LayoutsReply() { State = ReplyState.Ok };
                result.Layouts.Add(addResult.ToGrpc());
                return result;
            }
            catch (Exception ex)
            {
                return CreateErrorLayoutsReply(ex.Message);
            }
        }

        private LayoutsReply CreateErrorLayoutsReply(string message)
        {
            var reply = new LayoutsReply()
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
