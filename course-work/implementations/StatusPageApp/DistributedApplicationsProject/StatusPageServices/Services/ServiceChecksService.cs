using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using StatusPageData.Entities;
using StatusPageRepo;
using StatusPageServices.Interfaces;
using StatusPageServices.RequestDTO.ServiceChecks;
using StatusPageServices.ResponseDTO.ServiceChecks;

namespace StatusPageServices.Services
{
    public class ServiceChecksService : BaseService<ServiceCheckEntity>, IServiceChecksService
    {
        private readonly IRepo<StatusPageData.Entities.ServiceEntity> _servicesRepo;

        public ServiceChecksService(IRepo<ServiceCheckEntity> checksRepo, IRepo<StatusPageData.Entities.ServiceEntity> servicesRepo) : base(checksRepo)
        {
            _servicesRepo = servicesRepo;
        }

        public async Task<IEnumerable<ServiceCheckDto>> GetAllAsync()
        {
            var entities = await GetAllEntityAsync();
            return entities.Select(ToDto);
        }

        public async Task<ServiceCheckDto?> GetByIdAsync(int id)
        {
            var entity = await GetEntityByIdAsync(id);
            return entity is null ? null : ToDto(entity);
        }

        public async Task<ServiceCheckDto> CreateAsync(CreateServiceCheckDto dto)
        {
            var entity = new ServiceCheckEntity
            {
                ServiceId = dto.ServiceId,
                CheckedAt = dto.CheckedAt,
                IsHealthy = dto.IsHealthy,
                ResponseTimeMs = dto.ResponseTimeMs,
                StatusCode = dto.StatusCode,
                ErrorMessage = dto.ErrorMessage
            };

            await AddEntityAsync(entity);
            return ToDto(entity);
        }

        public async Task<IEnumerable<ServiceCheckDto>> ExecuteSweepAsync()
        {
            //get all services
            var servicesToPing = await _servicesRepo.GetAllAsync();

            //ping them in parallel
            var resultsList = await Task.WhenAll(servicesToPing.Select(ProbeServiceAsync));


            //save results and return
            foreach (var check in resultsList)
            {
                await AddEntityAsync(check);
            }
            return resultsList.Select(ToDto);
        }

        private static async Task<ServiceCheckEntity> ProbeServiceAsync(StatusPageData.Entities.ServiceEntity service)
        {
            var target = TryGetPingTarget(service.TargetUrl);
            var check = new ServiceCheckEntity
            {
                ServiceId = service.Id,
                CheckedAt = DateTime.UtcNow
            };

            if (target is null)
            {
                check.IsHealthy = false;
                check.ErrorMessage = "Invalid service URL.";
                return check;
            }

            using var ping = new Ping();
            try
            {
                var reply = await ping.SendPingAsync(target, 5000);
       check.IsHealthy = reply.Status == IPStatus.Success;
                check.ResponseTimeMs = (int)reply.RoundtripTime;
                check.StatusCode = null;
                check.ErrorMessage = check.IsHealthy ? null : reply.Status.ToString();
            }
            catch (Exception ex)
            {
                check.IsHealthy = false;
                check.ErrorMessage = ex.Message;
            }

            return check;
        }

        private static string? TryGetPingTarget(string targetUrl)
        {
            if (Uri.TryCreate(targetUrl, UriKind.Absolute, out var uri) && !string.IsNullOrWhiteSpace(uri.Host))
            {
                return uri.Host;
            }

            return string.IsNullOrWhiteSpace(targetUrl) ? null : targetUrl;
        }

        private static ServiceCheckDto ToDto(ServiceCheckEntity entity)
        {
            return new ServiceCheckDto(
                entity.Id,
                entity.ServiceId,
                entity.CheckedAt,
                entity.IsHealthy,
                entity.ResponseTimeMs,
                entity.StatusCode,
                entity.ErrorMessage
            );
        }

        private static ServiceCheckEntity ToEntity(CreateServiceCheckDto dto)
        {
            return new ServiceCheckEntity
            {
                ServiceId = dto.ServiceId,
                CheckedAt = dto.CheckedAt,
                IsHealthy = dto.IsHealthy,
                ResponseTimeMs = dto.ResponseTimeMs,
                StatusCode = dto.StatusCode,
                ErrorMessage = dto.ErrorMessage
            };
        }
    }
}
