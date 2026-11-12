namespace Stimulsoft.Base;

public class StiXmlOptions : StiFileDataOptions
{
	public byte[] Schema { get; }

	public bool IsAdoNet { get; }

	public StiXmlOptions(byte[] content)
		: this(null, content, isAdoNet: false)
	{
	}

	public StiXmlOptions(byte[] schema, byte[] content, bool isAdoNet = true)
		: base(content)
	{
		Schema = schema;
		IsAdoNet = isAdoNet;
	}
}
