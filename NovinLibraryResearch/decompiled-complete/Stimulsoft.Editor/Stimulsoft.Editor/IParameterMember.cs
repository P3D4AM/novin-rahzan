namespace Stimulsoft.Editor;

public interface IParameterMember
{
	string Name { get; set; }

	string DataType { get; set; }

	string Description { get; set; }

	string Qualifier { get; set; }

	string Text { get; set; }

	ParameterModifer Modifiers { get; set; }
}
