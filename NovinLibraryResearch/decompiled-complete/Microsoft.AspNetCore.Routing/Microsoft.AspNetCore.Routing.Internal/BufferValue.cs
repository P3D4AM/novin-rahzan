namespace Microsoft.AspNetCore.Routing.Internal;

public struct BufferValue
{
	public bool RequiresEncoding { get; }

	public string Value { get; }

	public BufferValue(string value, bool requiresEncoding)
	{
		Value = value;
		RequiresEncoding = requiresEncoding;
	}
}
