namespace PacketParser.AI.Prompt;

public abstract class BasePrompt
{
	public abstract string Prompt { get; }

	public int MaxTokens { get; set; } = 1000;


	public double Temperature { get; set; } = 0.7;


	protected BasePrompt(int maxTokens, double temperature)
	{
		MaxTokens = maxTokens;
		Temperature = temperature;
	}

	public override string ToString()
	{
		return $"{{\"prompt\":\"{Prompt}\",\"maxTokens\":{MaxTokens},\"temperature\":{Temperature}}}";
	}
}
