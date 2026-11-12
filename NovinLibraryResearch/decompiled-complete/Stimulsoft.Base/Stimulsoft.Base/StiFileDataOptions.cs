namespace Stimulsoft.Base;

public abstract class StiFileDataOptions
{
	public byte[] Content { get; }

	protected StiFileDataOptions(byte[] content)
	{
		Content = content;
	}
}
