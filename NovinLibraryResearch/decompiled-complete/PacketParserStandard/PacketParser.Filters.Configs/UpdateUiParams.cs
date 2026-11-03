using System.Threading;

namespace PacketParser.Filters.Configs;

public class UpdateUiParams
{
	public int Total { get; }

	public int Current { get; }

	public string Message { get; }

	public CancellationToken Token { get; }

	public UpdateUiParams(int total, int current, string message, CancellationToken token)
	{
		Total = total;
		Current = current;
		Token = token;
		Message = message;
	}
}
