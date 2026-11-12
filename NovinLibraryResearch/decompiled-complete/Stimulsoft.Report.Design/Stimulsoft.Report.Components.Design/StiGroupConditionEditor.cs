using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Components.Design;

public class StiGroupConditionEditor : UITypeEditor
{
	public override void PaintValue(PaintValueEventArgs e)
	{
		if (e.Value != null)
		{
			Rectangle bounds = e.Bounds;
			Graphics graphics = e.Graphics;
			graphics.FillRectangle(Brushes.WhiteSmoke, bounds);
			int num = StiDataBuilder.Images.ImageSize.Width - StiScale.I4;
			int num2 = StiDataBuilder.Images.ImageSize.Height - StiScale.I4;
			int x = bounds.X + (bounds.Width - num) / 2;
			int y = bounds.Y + (bounds.Height - num2) / 2;
			Rectangle rectangle = new Rectangle(x, y, num, num2);
			graphics.DrawImage(StiDataBuilder.Images.Images["Function"], rectangle);
		}
	}

	public override bool GetPaintValueSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (context != null && context.Instance != null)
		{
			return (UITypeEditorEditStyle)2;
		}
		return ((UITypeEditor)this).GetEditStyle(context);
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		StiComponent stiComponent = context.Instance as StiComponent;
		if ((int)StiConditionEditor.EditCondition((IStiGroup)stiComponent, StiConditionEditorForm.StartPage.Expression) == 1)
		{
			return ((IStiGroup)stiComponent).Condition;
		}
		return value;
	}
}
