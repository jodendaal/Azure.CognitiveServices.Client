using System.Text.Json.Serialization;

public class TextCompletionResponse
{
    public string Id { get; set; }
    public string Object { get; set; }
    public int Created { get; set; }
    public string Model { get; set; }
    public Choice[] Choices { get; set; }

    public override string ToString()
    {
        if(Choices.Length > 0)
        {
            return string.Join("\r\n", Choices.Select(i => i.Text));
        }

        return string.Empty;
    }
}

public class Choice
{
    public string Text { get; set; }
    public int Index { get; set; }
    public object Logprobs { get; set; }
    [JsonPropertyName("finish_reason")]
    public string FinishReason { get; set; }
}