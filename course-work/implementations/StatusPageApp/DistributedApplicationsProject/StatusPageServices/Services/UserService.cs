using StatusPageData.Entities.StatusPageData.Entities;
using StatusPageRepo;
using StatusPageServices.Interfaces;
using StatusPageServices.RequestDTO.Users.StatusPageServices.RequestDTO.Users;
using StatusPageServices.ResponseDTO.Services;
using StatusPageServices.ResponseDTO.User;

namespace StatusPageServices.Services
{
    public class UserService : BaseService<UserEntity>, IUserService
    {


        public UserService(IRepo<UserEntity> repo) : base(repo)
        {

        }

        public async Task<UserEntity?> FindByUsernameAsync(string username)
        {
            return (await _repo.GetAllAsync()).FirstOrDefault(u => u.Username == username);
        }

        public bool VerifyPassword(string inputPassword, string storedPassword)
        {
            return inputPassword == storedPassword;
        }

        public async Task<UserDto> CreateAsync(CreateUserDto dto)
        {
            var e = ToEntity(dto);
            await AddEntityAsync(e);
            return ToDto(e);
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteEntityAsync(id);
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var e = await GetEntityByIdAsync(id);
            if (e is null) return null;
            return ToDto(e);
        }

        private static UserEntity ToEntity(CreateUserDto dto)
        {
            return new UserEntity
            {
                Username = dto.Username,
                Password = dto.Password,
            };
        }

        private static UserDto ToDto(UserEntity entity)
        {
            return new UserDto(
                entity.Id,
                entity.Username
            );
        }


    }
}
