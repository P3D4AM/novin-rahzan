namespace Stimulsoft.Blockly.Model;

public class Mutation
{
	public string Domain { get; set; }

	public string Name { get; set; }

	public string Value { get; set; }

	public Mutation(string domain, string name, string value)
	{
		Domain = domain;
		Name = name;
		Value = value;
	}
}
