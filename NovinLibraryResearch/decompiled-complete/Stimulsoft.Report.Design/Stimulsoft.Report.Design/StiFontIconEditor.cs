using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Design;

public class StiFontIconEditor : UITypeEditor
{
	private IWindowsFormsEditorService edSvc;

	public override void PaintValue(PaintValueEventArgs e)
	{
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		if (e.Value == null)
		{
			return;
		}
		string icon = GetIcon(e.Context.Instance, e.Value);
		if (string.IsNullOrEmpty(icon))
		{
			return;
		}
		FontFamily fontFamilyIcons = StiFontIconsHelper.GetFontFamilyIcons();
		float num = 1f;
		SizeF iconSize = GetIconSize(e.Graphics, icon, num, fontFamilyIcons);
		while (Math.Round(iconSize.Width) < (double)(e.Bounds.Width - 1) && Math.Round(iconSize.Height) < (double)(e.Bounds.Height - 1))
		{
			num += 1f;
			iconSize = GetIconSize(e.Graphics, icon, num, fontFamilyIcons);
		}
		Font val = new Font(fontFamilyIcons, num - 1f);
		iconSize = e.Graphics.MeasureString(icon, val);
		SolidBrush val2 = new SolidBrush(Color.FromArgb(68, 114, 196));
		try
		{
			e.Graphics.DrawString(icon, val, (Brush)(object)val2, (RectangleF)new Rectangle(e.Bounds.X + (int)Math.Round(((float)e.Bounds.Width - iconSize.Width) / 2f), e.Bounds.Y + 1 + (int)Math.Round(((float)e.Bounds.Height - iconSize.Height) / 2f), e.Bounds.Width, e.Bounds.Height));
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private string GetIcon(object instance, object value)
	{
		if (instance is StiPictorialSeries stiPictorialSeries)
		{
			return StiFontIconsHelper.GetContent(stiPictorialSeries.Icon);
		}
		if (instance is IStiFontIconsSeries { Icon: var icon })
		{
			return StiFontIconsHelper.GetContent(icon.GetValueOrDefault());
		}
		if (instance is StiImage { Icon: var icon2 })
		{
			return StiFontIconsHelper.GetContent(icon2.GetValueOrDefault());
		}
		if (value is StiFontIcons value2)
		{
			return StiFontIconsHelper.GetContent(value2);
		}
		return string.Empty;
	}

	public override bool GetPaintValueSupported(ITypeDescriptorContext context)
	{
		return context.PropertyDescriptor.GetValue(context.Instance) != null;
	}

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (context != null && context.Instance != null)
		{
			return (UITypeEditorEditStyle)3;
		}
		return ((UITypeEditor)this).GetEditStyle(context);
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		if (context == null || context.Instance == null || provider == null)
		{
			return null;
		}
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		Panel val = new Panel
		{
			Width = StiScale.XXI(246.0),
			Height = StiScale.YYI(230.0),
			AutoScroll = true
		};
		StiFontIconSelectionControl stiFontIconSelectionControl = new StiFontIconSelectionControl((StiFontIcons?)value, Color.FromArgb(68, 114, 196), showCustomIcon: false, showEmptyIcon: true);
		((Control)stiFontIconSelectionControl).Location = new Point(StiScale.I2, StiScale.I2);
		((Control)stiFontIconSelectionControl).Width = ((Control)val).Width;
		((Control)stiFontIconSelectionControl).Anchor = (AnchorStyles)13;
		StiFontIconSelectionControl stiFontIconSelectionControl2 = stiFontIconSelectionControl;
		stiFontIconSelectionControl2.ValueChanged += FontIconSelectionControl_ValueChanged;
		((Control)val).Controls.Add((Control)(object)stiFontIconSelectionControl2);
		edSvc.DropDownControl((Control)(object)val);
		return stiFontIconSelectionControl2.SelectedIcon;
	}

	private SizeF GetIconSize(Graphics g, string icon, float fontSize, FontFamily fontFamilyIcons)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Expected O, but got Unknown
		Font val = new Font(fontFamilyIcons, fontSize);
		return g.MeasureString(icon, val);
	}

	private void FontIconSelectionControl_ValueChanged(object sender, EventArgs e)
	{
		edSvc.CloseDropDown();
	}
}
