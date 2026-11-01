using System;
using SmartAssembly.Attributes;

namespace TimeAttendanceDAL.Dal.FLRGharardad;

[Serializable]
[DoNotPruneType]
public class FormulaClass
{
	public string Name { get; set; }

	public string FormulaName { get; set; }

	public decimal Value { get; set; } = -1m;


	public int Privilege { get; set; }

	public string Formula { get; set; }

	public FormulaClass(string formula, int privilege, string name)
	{
		Name = name;
		Privilege = privilege;
		Formula = formula;
		FormulaName = Name.Replace(" ", "");
	}
}
