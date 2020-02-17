﻿using Routine.Api.Entities;
using Routine.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> _employeePropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id",new PropertyMappingValue(new List<string>{"Id" }) },
                {"CompanyId",new PropertyMappingValue(new List<string>{"CompanyId" }) },
                {"EmployeeNo",new PropertyMappingValue(new List<string>{"EmployeeNo" }) },
                {"Name",new PropertyMappingValue(new List<string>{"FirstName","LastName" }) },
                {"GenderDisplay",new PropertyMappingValue(new List<string>{"Gender" }) },
                {"Age",new PropertyMappingValue(new List<string>{"DateOfBirth" },true) }
            };

        private IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<EmployeeDto, Employee>(_employeePropertyMapping));
        }

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            var matchingMapping = _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            var matchingMappings = matchingMapping.ToList();

            if (matchingMappings.Count() == 1)
            {
                return matchingMappings.First().MappingDictionary;
            }

            throw new Exception($"无法找到唯一的映射关系:{typeof(TSource)},{typeof(TDestination)}");
        }

    }
}
