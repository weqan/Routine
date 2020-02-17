using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Models
{
    public class CompanyAddDto
    {
        [Required(ErrorMessage ="这个字段是必须的")]
        [Display(Name ="公司名")]
        [MaxLength(5,ErrorMessage ="{0}的最大长度不可超过{1}")]
        public string Name { get; set; }

        [Display(Name = "简介")]
        [StringLength(500,MinimumLength =10, ErrorMessage = "{0}的最大长度范围从{2}到{1}")]
        public string Introduction { get; set; }
        public ICollection<EmployeeAddDto> Employees { get; set; } = new List<EmployeeAddDto>();


    }
}
