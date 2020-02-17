﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Routine.Api.Helpers
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<ExpandoObject> ShapeData<TSource>(this IEnumerable<TSource> sources, string fields)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            var expandoObjectList = new List<ExpandoObject>(sources.Count());

            var propertyInfoList = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                var propertyInfos = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                propertyInfoList.AddRange(propertyInfos);
            }
            else
            {
                var fieldsAfterSplit = fields.Split(",");
                foreach (var field in fieldsAfterSplit)
                {
                    var propertyName = field.Trim();
                    var propertyInfo = typeof(TSource).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo == null)
                    {
                        throw new Exception($"Property:{propertyName}没找到：{typeof(TSource)}");
                    }


                    propertyInfoList.Add(propertyInfo);
                }
            }

            foreach (TSource obj in sources)
            {
                var shapeObj = new ExpandoObject();

                foreach (var propertyInfo in propertyInfoList)
                {
                    var propertyValue = propertyInfo.GetValue(obj);
                    ((IDictionary<string, object>)shapeObj).Add(propertyInfo.Name, propertyValue);
                }

                expandoObjectList.Add(shapeObj);
            }

            return expandoObjectList;


        }
    }
}
