using System;

namespace SmartKitchen.Domain.Responses
{
    public class ItemCreationResponse : ServiceResponse
    {
        public int AddedId;
        public int AddedGroupId;

        public new ItemCreationResponse AddError(Enum error, string key = "")
        {
            Errors.Add(new ModelStateError(key, error));
            return this;
        }

        public ItemCreationResponse() { }

        public ItemCreationResponse(ServiceResponse response)
        {
            AddedId = 0;
            AddedGroupId = 0;
            Errors = response.Errors;
        }
    }
}
