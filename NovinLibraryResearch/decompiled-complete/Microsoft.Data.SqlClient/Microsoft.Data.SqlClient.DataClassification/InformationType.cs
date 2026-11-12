namespace Microsoft.Data.SqlClient.DataClassification;

public sealed class InformationType
{
	public string Name { get; private set; }

	public string Id { get; private set; }

	public InformationType(string name, string id)
	{
		Name = name;
		Id = id;
	}
}
