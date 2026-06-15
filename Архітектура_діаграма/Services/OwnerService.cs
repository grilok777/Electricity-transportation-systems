using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class OwnerService
    {
        private readonly IGenerationUnitOfWork _uow;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;

        public OwnerService(IGenerationUnitOfWork uow, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
        {
            _uow = uow;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        public async Task RegisterOwnerAsync(string username, string password)
        {
            if (!password.Any(char.IsDigit) || !password.Any(char.IsUpper))
                throw new ArgumentException("Пароль повинен містити хоча б одну цифру та одну велику літеру.");

            var existingUser = await _uow.Owners.GetByUsernameAsync(username);
            if (existingUser != null)
                throw new Exception("Користувач з таким ім'ям вже існує.");

            string hash = _passwordHasher.HashPassword(password);

            var newOwner = new User
            {
                Username = username,
                Password = hash,
                DateRegistration = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            await _uow.Owners.AddAsync(newOwner);
            await _uow.SaveChangesAsync();
        }

        public async Task<(string Token, int OwnerId)> AuthenticateAsync(string username, string password)
        {
            var owner = await _uow.Owners.GetByUsernameAsync(username);
            if (owner == null)
                throw new UnauthorizedAccessException("Неправильний логін або пароль.");

            bool isValid = _passwordHasher.VerifyPassword(password, owner.Password);

            if (!isValid)
                throw new UnauthorizedAccessException("Неправильний логін або пароль.");

            string token = _jwtProvider.GenerateToken(owner);

            return (token, owner.Id);
        }
        public async Task ChangePasswordAsync(int ownerId, string oldPassword, string newPassword)
        {
            if (oldPassword == newPassword)
                throw new ArgumentException("Новий пароль не може співпадати зі старим.");

            var owner = await _uow.Owners.GetByIdAsync(ownerId);
            if (owner == null) throw new Exception("Користувача не знайдено.");

            bool isOldValid = _passwordHasher.VerifyPassword(oldPassword, owner.Password);
            if (!isOldValid) throw new UnauthorizedAccessException("Старий пароль неправильний.");

            owner.Password = _passwordHasher.HashPassword(newPassword);

            _uow.Owners.Update(owner);
            await _uow.SaveChangesAsync();
        }

        public async Task<UserProfileDto> GetOwnerProfileAsync(int ownerId)
        {
            var owner = await _uow.Owners.GetByIdAsync(ownerId);
            if (owner == null) throw new Exception("Користувача не знайдено.");

            return new UserProfileDto
            {
                Id = owner.Id,
                Username = owner.Username,
                DateRegistration = owner.DateRegistration
            };
        }

        public async Task UpdateOwnerContactInfoAsync(int ownerId, int dealId, ContactInfoDto dto)
        {
            // Шукаємо власника разом з усіма його угодами
            var owners = await _uow.Owners.FindAsync(o => o.Id == ownerId, o => o.Deals);
            var owner = owners.FirstOrDefault();

            if (owner == null) throw new Exception("Користувача не знайдено.");

            var targetDeal = owner.Deals.FirstOrDefault(d => d.Id == dealId);

            if (targetDeal == null)
                throw new Exception($"Угоду з Id {dealId} для цього користувача не знайдено.");

            targetDeal.NumberPhone = dto.NumberPhone;
            targetDeal.PlaceLocation = dto.PlaceLocation;

            _uow.Owners.Update(owner);
            await _uow.SaveChangesAsync();
        }
    }
}
