using System;

namespace StatusPageServices.RequestDTO.Engineers
{
    public record UpdateEngineerDto(
                int Id,
        string Name,
        string Email,
        DateTime HiredDate,
        bool OnCall,
        double HourlyRate
    );
}
