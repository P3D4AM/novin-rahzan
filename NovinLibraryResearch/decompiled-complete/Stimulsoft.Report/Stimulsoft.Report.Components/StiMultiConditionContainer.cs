using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Report.Components;

public class StiMultiConditionContainer
{
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public StiFiltersCollection Filters { get; set; } = new StiFiltersCollection();


	[DefaultValue(StiFilterMode.And)]
	[StiSerializable]
	[Browsable(false)]
	public StiFilterMode FilterMode { get; set; }

	[StiSerializable]
	[DefaultValue(true)]
	public bool Enabled { get; set; } = true;


	[StiSerializable]
	public Color TextColor { get; set; } = Color.Red;


	[StiSerializable]
	public Color BackColor { get; set; } = Color.Transparent;


	[StiSerializable]
	public Font Font { get; set; } = new Font("Arial", 8f);


	[StiSerializable]
	[DefaultValue(false)]
	public bool CanAssignExpression { get; set; }

	[StiSerializable]
	[DefaultValue("")]
	public string AssignExpression { get; set; } = string.Empty;


	[StiSerializable]
	[DefaultValue("")]
	public string Style { get; set; } = string.Empty;


	[StiSerializable]
	[DefaultValue(StiConditionBorderSides.NotAssigned)]
	public StiConditionBorderSides BorderSides { get; set; } = StiConditionBorderSides.NotAssigned;


	[StiSerializable]
	[DefaultValue(StiConditionPermissions.All)]
	public StiConditionPermissions Permissions { get; set; } = StiConditionPermissions.All;


	[StiSerializable]
	[DefaultValue(false)]
	public bool BreakIfTrue { get; set; }
}
