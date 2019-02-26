using System;
using AutoMapper;

namespace SmartKitchen.DomainService.Services
{
    public class BaseService
    {
        /// <summary>
        /// Auto-injected lazy instance of AutoMapper
        /// </summary>
        public Lazy<IMapper> MapperInstance { protected get; set; }
        
        /// <summary>
        /// The AutoMapper
        /// </summary>
        protected IMapper Mapper => MapperInstance.Value;

        /// <summary>
        /// The AutoMapper Configuration
        /// </summary>
        protected IConfigurationProvider MapperConfig => Mapper.ConfigurationProvider;

        protected static string TitledString(string src) =>
            src.Length < 1 ? "" : src[0].ToString().ToUpper() + (src.Length < 2 ? "" : src.Substring(1).ToLower());
    }
}
