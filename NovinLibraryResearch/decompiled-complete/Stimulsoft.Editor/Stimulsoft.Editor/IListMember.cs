using System;

namespace Stimulsoft.Editor;

public interface IListMember : IComparable
{
	string Name { get; set; }

	string DisplayText { get; set; }

	string DataType { get; set; }

	string Description { get; set; }

	string Qualifier { get; set; }

	int Overloads { get; set; }

	object CustomData { get; set; }

	string ParamText { get; set; }

	IParameterMember[] Parameters { get; set; }

	int CurrentParamIndex { get; set; }

	bool OwnDescription { get; set; }

	string OwnDescriptionText { get; set; }

	int ImageIndex { get; set; }

	IListMembers Owner { get; set; }

	string GetOwnDescription();

	void UpdateParamText();

	string GetParamText(bool useFormatting);
}
