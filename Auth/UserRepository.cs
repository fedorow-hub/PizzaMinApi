public class UserRepository : IUserRepository
{
    private PizzaDb _context;
    public UserRepository(PizzaDb context)
    {
        _context = context;
    }

    public User GetUser(UserModel userModel) =>
        _context.Users.FirstOrDefault(u =>
            string.Equals(u.Login, userModel.Login) &&
            string.Equals(u.Password, userModel.Password)) ??
            throw new Exception();
}