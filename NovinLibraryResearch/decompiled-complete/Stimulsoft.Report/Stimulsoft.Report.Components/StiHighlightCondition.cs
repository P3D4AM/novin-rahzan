using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Report.Components;

[RefreshProperties(RefreshProperties.All)]
public class StiHighlightCondition : ICloneable
{
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[Description("The brush of the component, which is used to display text.")]
	public StiBrush TextBrush { get; set; } = (StiBrush)new StiSolidBrush(Color.Red);


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[Description("The brush, which is used to draw background.")]
	public StiBrush Brush { get; set; } = (StiBrush)new StiSolidBrush(Color.Transparent);


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[Description("Gets or sets the expression for calculation of the condition.")]
	public StiExpression Condition { get; set; } = new StiExpression();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[Description("Gets or sets font of component.")]
	public Font Font { get; set; } = new Font("Arial", 8f);


	public object Clone()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		StiHighlightCondition stiHighlightCondition = (StiHighlightCondition)MemberwiseClone();
		stiHighlightCondition.TextBrush = (StiBrush)TextBrush.Clone();
		stiHighlightCondition.Brush = (StiBrush)Brush.Clone();
		stiHighlightCondition.Condition = (StiExpression)Condition.Clone();
		return stiHighlightCondition;
	}
}
