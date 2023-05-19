using CalculadoraMVC.Models.ViewModels;
using CalculadoraMVC.Models;
using CalculadoraMVC.Repositories;

public class UserService
{
   private readonly IUnitOfWork _unitOfWork;

   public UserService(IUnitOfWork unitOfWork)
   {
      _unitOfWork = unitOfWork;
   }

   public void UpdateUser(ApplicationUser user, EditUserViewModel data)
   {
      UpdateUserProperties(user, data);
      _unitOfWork.User.UpdateUser(user);
   }

   private void UpdateUserProperties(ApplicationUser user, EditUserViewModel data)
   {
      user.FirstName = data.User.FirstName;
      user.LastName = data.User.LastName;
      user.Email = data.User.Email;
   }
}