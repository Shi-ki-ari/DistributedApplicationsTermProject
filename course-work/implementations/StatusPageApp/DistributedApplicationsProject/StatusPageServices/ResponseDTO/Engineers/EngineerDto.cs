using System;

namespace StatusPageServices.ResponseDTO.Engineers
{
    public record EngineerDto(
        int Id,
        string Name,
        string Email,
        DateTime HiredDate,
        bool OnCall,
        double HourlyRate
    );
}
