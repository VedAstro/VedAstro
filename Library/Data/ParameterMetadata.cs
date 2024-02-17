using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VedAstro.Library
{
    public class ParameterMetadata
    {
        public ParameterMetadata(ParameterInfo param, string description)
        {
            Name = param.Name;
            Description = description;
            DefaultValue = param.DefaultValue?.ToString() ?? "";
            ParamType = param.ParameterType;
            IsOptional = param.IsOptional;
        }
        public ParameterMetadata(string name, string description, dynamic defaultValue, Type parameterType, bool isOptional)
        {
            Name = name;
            Description = description;
            DefaultValue = defaultValue?.ToString() ?? "";
            ParamType = parameterType;
            IsOptional = isOptional;
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DefaultValue { get; set; }
        public bool IsOptional { get; set; }
        public Type ParamType { get; set; }
    }
}
