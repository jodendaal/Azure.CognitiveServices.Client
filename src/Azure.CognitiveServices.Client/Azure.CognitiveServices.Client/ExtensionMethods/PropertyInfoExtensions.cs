using System.Reflection;
using System.Text.Json.Serialization;

namespace Azure.CognitiveServices.Client
{
    public static class PropertyInfoExtensions
    {
        public static string GetPropertyName(this PropertyInfo prop)
        {
            var attribute = prop.GetCustomAttribute<JsonPropertyNameAttribute>();
            if (attribute != null)
            {
                return attribute.Name;
            }

            return prop.Name.ToLowerInvariant();
        }

    }
}
