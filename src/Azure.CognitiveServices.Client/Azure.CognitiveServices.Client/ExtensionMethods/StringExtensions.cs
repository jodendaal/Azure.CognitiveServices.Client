﻿namespace Azure.CognitiveServices.Client
{
    public static class StringExtensions
    {
        public static IList<string> ToList(this string value)
        {
            return new List<string> { value };
        }
    }
}
