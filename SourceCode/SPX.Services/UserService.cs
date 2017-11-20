using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPX.DataProvider;

namespace SPX.Services
{
   public class UserService : IUserService
    {
       private readonly UnitOfWork _unitOfWork;


       public UserService()
        {
            _unitOfWork = new UnitOfWork();
        }

       
        public int Authenticate(string userName, string password)
        {
            var user = _unitOfWork.UserRepository.Get().FirstOrDefault(u => u.UserName == userName && u.Password == password);
            if (user != null && user.UserId > 0)
            {
                return user.UserId;
            }
            return 0;
        }
    }
}
