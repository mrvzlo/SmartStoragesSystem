using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace SmartKitchen.DomainService
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

    }
}
