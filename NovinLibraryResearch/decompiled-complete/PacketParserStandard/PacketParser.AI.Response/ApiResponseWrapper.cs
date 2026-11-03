namespace PacketParser.AI.Response;

public class ApiResponseWrapper
{
	public string Response { get; set; }

	public int TokensUsed { get; set; }

	public string Error { get; set; }
}
