using SQLMVC99.Models;

public class UserViewModel
{
    public IEnumerable<User> Users { get; set; }  // استخدام User بدلاً من Usernew
    public User NewUser { get; set; }  // هذا النموذج سيتم استخدامه في الـ Modal
}
