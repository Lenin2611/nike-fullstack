using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class AuthorizationHelper
    {
        public enum Roles
        {
            Admin,
            Manager,
            Employee,
            Person
        }

        public const Roles rol_default = Roles.Person;
    }
}