using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusPageServices.ResponseDTO.User
{
    public record UserDto
    (
        int Id,
        string Username
    );
}
