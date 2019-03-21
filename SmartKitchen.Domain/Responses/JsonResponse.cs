namespace SmartKitchen.Domain.Responses
{
    public class JsonResponse : ServiceResponse
    {
        public JsonResponse() { }

        public JsonResponse(ServiceResponse response)
        {
            Errors = response.Errors;
        }
    }
}
