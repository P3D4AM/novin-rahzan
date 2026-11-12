namespace Microsoft.Data.SqlClient.DataClassification;

public sealed class Label
{
	public string Name { get; private set; }

	public string Id { get; private set; }

	public Label(string name, string id)
	{
		Name = name;
		Id = id;
	}
}
