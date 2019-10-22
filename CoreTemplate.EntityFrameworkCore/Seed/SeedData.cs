using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTemplate.EntityFrameworkCore.Seed
{
    public class SeedData
    {
        public static void SeedDb()
        {
            new UserRoleCreator().Create();
        }
    }
}
