using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Design.Components;

namespace Stimulsoft.Base.Drawing.Design;

public class StiColorEditor : UITypeEditor
{
	private IWindowsFormsEditorService edSvc;

	private Color color;

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if (context == null || context.Instance == null)
		{
			return ((UITypeEditor)this).GetEditStyle(context);
		}
		return (UITypeEditorEditStyle)3;
	}

	public override void PaintValue(PaintValueEventArgs e)
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		if (IsExpressionEditor())
		{
			string name = e.Context.PropertyDescriptor.Name;
			object instance = e.Context.Instance;
			if (StiAppExpressionHelper.IsExpressionSpecified(instance, name))
			{
				StiExpressionEditor.PaintExpressionMarker(e);
				return;
			}
		}
		if (e.Value != null)
		{
			SolidBrush val = new SolidBrush((Color)e.Value);
			try
			{
				e.Graphics.FillRectangle((Brush)(object)val, e.Bounds);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	public override bool GetPaintValueSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		color = ((value == null) ? Color.Transparent : ((Color)value));
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		Panel val = new Panel
		{
			Width = StiScale.XXI(166.0),
			AutoScroll = true
		};
		StiColorSelectionControl stiColorSelectionControl = new StiColorSelectionControl(color, IsExpressionEditor(), context);
		stiColorSelectionControl.IsFromStyle = this is StiStyleColorEditor;
		((Control)stiColorSelectionControl).Location = new Point(StiScale.I1, StiScale.I1);
		((Control)stiColorSelectionControl).Width = ((Control)val).Width - StiScale.I2;
		((Control)stiColorSelectionControl).Anchor = (AnchorStyles)13;
		StiColorSelectionControl stiColorSelectionControl2 = stiColorSelectionControl;
		stiColorSelectionControl2.ColorChanged += ColorSelectionControl_ValueChanged;
		stiColorSelectionControl2.ExpressionChanged += ColorSelectionControl_ValueChanged;
		((Control)val).Height = stiColorSelectionControl2.GetHeight() + StiScale.I4;
		((Control)val).Controls.Add((Control)(object)stiColorSelectionControl2);
		edSvc.DropDownControl((Control)(object)val);
		if (!string.IsNullOrEmpty(stiColorSelectionControl2.Expression))
		{
			return color;
		}
		return stiColorSelectionControl2.SelectedColor;
	}

	private bool IsExpressionEditor()
	{
		if (!(this is StiExpressionStyleColorEditor))
		{
			return this is StiExpressionColorEditor;
		}
		return true;
	}

	private void ColorSelectionControl_ValueChanged(object sender, EventArgs e)
	{
		edSvc.CloseDropDown();
	}
}
