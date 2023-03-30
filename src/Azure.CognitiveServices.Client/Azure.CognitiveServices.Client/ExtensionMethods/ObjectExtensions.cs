using Azure.CognitiveServices.Client.OpenAI.Models.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Azure.CognitiveServices.Client
{
    public static class ObjectExtensions
    {
        public static void Validate(this object @object)
        {
            ICollection<ValidationResult> validationErrors = new List<ValidationResult>();
            Validator.TryValidateObject(@object, new ValidationContext(@object), validationErrors);
            if (validationErrors.Count > 0)
            {
                throw new OpenAIValidationException(string.Join(Environment.NewLine, validationErrors));
            }
        }

        public static HttpContent ToHttpContent(this object value)
        {
            if (value is byte[])
            {
                return new ByteArrayContent((byte[])value);
            }

            return new StringContent(value?.ToString() ?? "");
        }

        public static MultipartFormDataContent ToMultipartFormDataContent(this object @object)
        {
            MultipartFormDataContent formData = new();

            foreach (var prop in @object.GetType().GetProperties())
            {
                formData.AddField(prop, @object);
            }

            return formData;
        }
    }
}
