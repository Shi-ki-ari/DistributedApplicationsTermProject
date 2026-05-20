using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Net.Http;
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
        private readonly IRepo<IncidentEntity> _incidentsRepo;

        public ServiceChecksService(IRepo<ServiceCheckEntity> checksRepo, IRepo<StatusPageData.Entities.ServiceEntity> servicesRepo, IRepo<IncidentEntity> incidentsRepo) : base(checksRepo)
        {
            _servicesRepo = servicesRepo;
            _incidentsRepo = incidentsRepo;
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

        public async Task DeleteAsync(int id)
        {
            await DeleteEntityAsync(id);
        }

        public async Task<IEnumerable<ServiceCheckDto>> ExecuteSweepAsync()
        {
            //get all services
            var servicesToPing = await _servicesRepo.GetAllAsync();
            var servicesById = servicesToPing.ToDictionary(service => service.Id);

            //ping them in parallel
            var resultsList = await Task.WhenAll(servicesToPing.Select(ProbeServiceAsync));


            //save results and return
            foreach (var check in resultsList)
            {
                await AddEntityAsync(check);

                if (!check.IsHealthy)
                {
                    servicesById.TryGetValue(check.ServiceId, out var service);
                    var title = service is null
                        ? "Service check failed"
                        : $"Service check failed: {service.Name}";
                    var description = string.IsNullOrWhiteSpace(check.ErrorMessage)
                        ? "Service health check failed."
                        : check.ErrorMessage;

                    var incident = new IncidentEntity
                    {
                        Title = title,
                        Description = description,
                        StartTime = check.CheckedAt,
                        EndTime = null,
                        IsScheduled = false,
                        IsSystemGenerated = true,
                        ServiceId = check.ServiceId,
                        AssignedEngineerId = null
                    };

                    await _incidentsRepo.AddAsync(incident);
                }
            }
            return resultsList.Select(ToDto);
        }

        private static readonly HttpClient HttpClient = new HttpClient();

        private static async Task<ServiceCheckEntity> ProbeServiceAsync(ServiceEntity service)
        {
            var check = new ServiceCheckEntity
            {
                ServiceId = service.Id,
                CheckedAt = DateTime.UtcNow
            };

            if (!Uri.TryCreate(service.TargetUrl, UriKind.Absolute, out var target))
            {
                check.IsHealthy = false;
                check.ErrorMessage = "Invalid service URL.";
                return check;
            }

            var stopwatch = Stopwatch.StartNew();
            try
            {
                using var response = await HttpClient.GetAsync(target);
                stopwatch.Stop();
                check.IsHealthy = response.IsSuccessStatusCode;
                check.ResponseTimeMs = (int)stopwatch.ElapsedMilliseconds;
                check.StatusCode = (int)response.StatusCode;
                check.ErrorMessage = response.IsSuccessStatusCode ? null : response.ReasonPhrase;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                check.IsHealthy = false;
                check.ResponseTimeMs = (int)stopwatch.ElapsedMilliseconds;
                check.ErrorMessage = ex.Message;
            }

            return check;
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
