using EmailScheduler.Model;

namespace EmailScheduler.Service.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAll();
        Task Insert(User user);
    }
}
