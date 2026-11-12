using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Components.Design;

public class StiExpressionEditor : UITypeEditor
{
	public static void PaintExpressionMarker(PaintValueEventArgs e)
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

	public override void PaintValue(PaintValueEventArgs e)
	{
		PaintExpressionMarker(e);
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
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Invalid comparison between Unknown and I4
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Invalid comparison between Unknown and I4
		StiComponent stiComponent = context.Instance as StiComponent;
		if (context.Instance is IStiSeries)
		{
			stiComponent = ((IStiSeriesParent)context.Instance).Parent;
		}
		if (context.Instance is StiSeriesInteraction stiSeriesInteraction)
		{
			stiComponent = stiSeriesInteraction.ParentComponent;
		}
		StiReport stiReport = stiComponent?.Report;
		stiReport?.Designer.UndoRedoSave(StiLocalization.Get("PropertyMain", "Expression"), onlyCurrentPage: true);
		bool flag = false;
		if (context.Instance is StiShape)
		{
			flag = true;
		}
		if (context.Instance is IStiExpressionParentComponent stiExpressionParentComponent)
		{
			stiReport = stiExpressionParentComponent.ParentComponent?.Report;
			flag = true;
		}
		if (context.Instance is StiDataParameter stiDataParameter)
		{
			stiReport = stiDataParameter.DataSource.Dictionary.Report;
			flag = true;
		}
		if (context.Instance is StiCalcDataColumn stiCalcDataColumn)
		{
			stiReport = ((stiCalcDataColumn.DataSource == null) ? stiCalcDataColumn.BusinessObject.Dictionary.Report : stiCalcDataColumn.DataSource.Dictionary.Report);
			flag = true;
		}
		if (context.Instance is StiInteraction stiInteraction)
		{
			stiReport = stiInteraction.ParentComponent.Report;
		}
		if (value != null)
		{
			if (flag)
			{
				string text = value as string;
				if ((int)StiTextEditor.EditText(stiReport, ref text, StiCodeEditorMode.EditorCode) == 1)
				{
					if (!(value is StiExpression))
					{
						return text;
					}
					((StiExpression)value).Value = text;
				}
			}
			else
			{
				string text2;
				StiCodeEditorMode mode;
				if (value is StiExpression)
				{
					text2 = ((StiExpression)value).Value;
					mode = StiCodeEditorMode.EditorExpression;
				}
				else
				{
					text2 = value as string;
					mode = StiCodeEditorMode.EditorText;
				}
				if (context.Instance is StiDataBand && context.PropertyDescriptor.Name == "LimitRows")
				{
					mode = StiCodeEditorMode.EditorExpression;
				}
				if ((int)StiTextEditor.EditText(stiReport, ref text2, mode) == 1)
				{
					if (!(value is StiExpression))
					{
						return text2;
					}
					((StiExpression)value).Value = text2;
				}
			}
		}
		return value;
	}
}
