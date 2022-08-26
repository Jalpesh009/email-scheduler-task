using EmailScheduler.Model;
using EmailScheduler.Model.Helper;
using EmailScheduler.Service.Interface;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace EmailScheduler.Service
{
    public class UserService : IUserService
    {
        private readonly IOptions<AppSetting> _config;
        private IWebHostEnvironment _hostEnvironment;
        public UserService(IWebHostEnvironment environment, IOptions<AppSetting> config)
        {
            _config = config;
            _hostEnvironment = environment;
        }
        async Task<IEnumerable<User>> IUserService.GetAll()
        {
            string path = Path.Combine(_hostEnvironment.ContentRootPath, "Data/" + _config.Value.UserFile);
            string userDataJson = System.IO.File.ReadAllText(path);
            List<User> userData = JsonConvert.DeserializeObject<List<User>>(userDataJson);
            return userData;
        }

        async Task IUserService.Insert(User user)
        {
            string path = Path.Combine(_hostEnvironment.ContentRootPath, "Data/" + _config.Value.UserFile);
            string userDataJson = System.IO.File.ReadAllText(path);
            List<User> userData = JsonConvert.DeserializeObject<List<User>>(userDataJson);
            user.UserId = Guid.NewGuid().ToString();

            if (userData == null || userData.Count <= 0)
            {
                userData = new List<User>();
                userData.Add(new User
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    UserName = user.UserName
                });
            }
            else
            {
                userData.Add(new User
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    UserName = user.UserName
                });
            }
            string userDataJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(userData);
            System.IO.File.WriteAllText(path, userDataJsonString);
        }
    }
}
