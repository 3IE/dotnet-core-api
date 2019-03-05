using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Helpers;
using TodoApi.Models;
using TodoApi.DataAccess;

namespace TodoApi.BusinessManagement
{
    public interface IUserServices
    {
        Task<Users> Authenticate(string username, string password);
        Task<IEnumerable<Users>> GetAllUsers();
        Task<Users> GetById(int id);
        Task<IEnumerable<TodoItems>> GetUserTodos(string name);
        Task<Users> Create(Users user, string password);
        Task UpdateUser(Users userParam, string password = null);
        Task DeleteUser(int id);
    }

    public class UserServices : IUserServices
    {
        private IRepositoryUser _userRepository;

        public UserServices(IRepositoryUser userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Users> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            // Returns a single, specific element of a sequence,
            // or a default value if that element is not found.
            var user = await _userRepository.GetAnyUsername(username);

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }

        public async Task<IEnumerable<Users>> GetAllUsers()
        {
            return await _userRepository.GetAll();
        }

        public async Task<Users> GetById(int id)
        {
            return await _userRepository.GetElem(id);
        }

        public async Task<IEnumerable<TodoItems>> GetUserTodos(string name)
        {
            return await _userRepository.GetUserTodos(name);
        }

        public async Task<Users> Create(Users user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            var selectedUser = await _userRepository.GetAnyUsername(user.Username);

            // Check user does not already exists
            if (selectedUser != null)
                throw new AppException("Username \"" + user.Username + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _userRepository.AddElem(user);

            return user;
        }

        public async Task UpdateUser(Users userParam, string password = null)
        {
            var user = await _userRepository.GetElem(userParam.Id);

            if (user == null)
                throw new AppException("User not found");

            if (userParam.Username != user.Username)
            {
                var selectedUser = await _userRepository.GetAnyUsername(userParam.Username);

                // username has changed so check if the new username is already taken
                if (selectedUser != null)
                    throw new AppException("Username " + userParam.Username + " is already taken");
            }

            // update user properties
            user.Username = userParam.Username;

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            await _userRepository.AddElem(user);
        }

        public async Task DeleteUser(int id)
        {
            var user = await _userRepository.GetElem(id);

            if (user != null)
                await _userRepository.DeleteElem(user);
            else
                throw new AppException("User not found");
        }

        // private helper methods

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}