using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTemplate.EntityFrameworkCore.Seed
{
    public class SeedData
    {
        public static void SeedDb(TempDbContext dbContext)
        {
            new UserRoleCreator().Create(dbContext);
        }
    }
}
