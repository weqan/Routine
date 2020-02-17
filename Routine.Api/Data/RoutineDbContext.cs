using Microsoft.EntityFrameworkCore;
using Routine.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Data
{
    public class RoutineDbContext : DbContext
    {
        public RoutineDbContext(DbContextOptions<RoutineDbContext> options) : base(options)
        {

        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Company>().Property(x => x.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Company>().Property(x => x.Introduction).HasMaxLength(500);

            modelBuilder.Entity<Employee>().Property(x => x.EmployeeNo).IsRequired().HasMaxLength(10);
            modelBuilder.Entity<Employee>().Property(x => x.FirstName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Employee>().Property(x => x.LastName).IsRequired().HasMaxLength(50);

            modelBuilder.Entity<Employee>().HasOne(x => x.Company).WithMany(x => x.Employees).HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Company>().HasData(
                new Company
                {
                    Id = Guid.Parse("12fb6e84-59fb-407b-80c9-18ed1bcd2109"),
                    Name = "Microsoft",
                    Introduction = "Great Company"
                },
                new Company
                {
                    Id = Guid.Parse("dca35a92-4642-4899-8483-8fcd8fcd9111"),
                    Name = "Google",
                    Introduction = "Don't be evil"
                },
                new Company
                {
                    Id = Guid.Parse("b00d88e9-b59f-450a-880e-e31e90af5f06"),
                    Name = "Alibaba",
                    Introduction = "Fubao Company"
                }
                );


            //modelBuilder.Entity<Employee>().HasData(
            //                 new Employee
            //                 {
            //                     Id = Guid.NewGuid(),
            //                     //CompanyId = Guid.Parse("12fb6e84-59fb-407b-80c9-18ed1bcd2109"),
            //                     DateOfBirth = new DateTime(1955, 3, 25),
            //                     EmployeeNo = "no2",
            //                     FirstName = "zhang",
            //                     LastName = "san",
            //                     Gender = Gender.男
            //                 },
            //                new Employee
            //                {
            //                    Id = Guid.NewGuid(),
            //                    //CompanyId = Guid.Parse("12fb6e84-59fb-407b-80c9-18ed1bcd2109"),
            //                    DateOfBirth = new DateTime(1999, 2, 23),
            //                    EmployeeNo = "no1",
            //                    FirstName = "li",
            //                    LastName = "siyuan",
            //                    Gender = Gender.男
            //                }

            //    );
        }
    }
}
