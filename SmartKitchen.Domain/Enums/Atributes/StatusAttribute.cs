using System.Xml.Serialization;

namespace SmartKitchen.Domain.Enums.Atributes
{
    public class StatusAttribute : XmlEnumAttribute
    {
        public StatusAttribute(StatusType type) => Type = type;
        
        public StatusType Type { get; set; }
    }
}
