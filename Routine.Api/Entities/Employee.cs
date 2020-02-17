using Routine.Api.ValidationAttributes;
using System;

namespace Routine.Api.Entities
{
    [EmployeeNoMustDifferentFromFirstName(ErrorMessage ="员工编号和员工名不同")]
    public class Employee
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string EmployeeNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Company Company { get; set; }
    }
}