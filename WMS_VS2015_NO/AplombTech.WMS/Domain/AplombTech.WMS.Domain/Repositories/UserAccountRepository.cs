using AplombTech.WMS.Domain.UserAccounts;
using AplombTech.WMS.Utility;
using NakedObjects;
using NakedObjects.Menu;
using NakedObjects.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.Repositories
{
    [DisplayName("User Accounts")]
    public class UserAccountRepository : AbstractFactoryAndRepository
    {
        public static void Menu(IMenu menu)
        {
            menu.CreateSubMenu("Users")
                .AddAction("AddUser")
                .AddAction("ShowAllUsers");

            menu.CreateSubMenu("Role")
                .AddAction("AddRole")
                .AddAction("ShowAllRoles");
        }

        #region ASP DOT NET MEMBERSHIP
        #region ROLE
        public void AddRole(string name)
        {
            Role role = Container.NewTransientInstance<Role>();

            role.Id = Guid.NewGuid().ToString();
            role.Name = name;

            Container.Persist(ref role);
        }
        [Eagerly(EagerlyAttribute.Do.Rendering)]
        [TableView(true, "Name")]
        public IQueryable<Role> ShowAllRoles()
        {
            return Container.Instances<Role>();
        }
        #endregion

        #region USERS
        public void AddUser([RegEx(Validation = @"^[\-\w\.]+@[\-\w\.]+\.[A-Za-z]+$", Message = "Not a valid email address")]string email, [DataType(DataType.Password)]string password, [DataType(DataType.Password)]string confirmPassword)
        {
            LoginUser user = Container.NewTransientInstance<LoginUser>();

            user.Id = Guid.NewGuid().ToString();
            user.UserName = email;
            user.Email = email;
            user.EmailConfirmed = false;
            user.PasswordHash = PasswordHash.HashPassword(password);
            user.SecurityStamp = Guid.NewGuid().ToString();
            user.PhoneNumberConfirmed = false;
            user.TwoFactorEnabled = false;
            user.LockoutEnabled = false;
            user.AccessFailedCount = 0;

            Container.Persist(ref user);
        }

        public string ValidateAddUser(string email, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                return "Password does not match";
            }
            return null;
        }

        [Eagerly(EagerlyAttribute.Do.Rendering)]
        [TableView(true, "UserName", "Email")]
        public IQueryable<LoginUser> ShowAllUsers()
        {
            return Container.Instances<LoginUser>();
        }
        #endregion 
        #endregion
    }
}
