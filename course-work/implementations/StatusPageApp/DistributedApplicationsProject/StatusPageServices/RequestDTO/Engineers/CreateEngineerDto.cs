using System;

namespace StatusPageServices.RequestDTO.Engineers
{
    public record CreateEngineerDto(
        string Name,
        string Email,
        DateTime? HiredDate,
        bool OnCall,
        double HourlyRate
    );
}
