using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Dashboard.Components.Button;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Design.Components;

public class StiButtonTypeEditor : UITypeEditor
{
	private IWindowsFormsEditorService edSvc;

	private bool cancel;

	private StiButtonElement component;

	private string LocButton => Loc.Get("Dialogs", "StiButtonControl");

	private string LocRadioButton => Loc.Get("Dialogs", "StiRadioButtonControl");

	private string LocCheckBox => Loc.Get("Dialogs", "StiCheckBoxControl");

	private void ListBox_DrawItem(object sender, DrawItemEventArgs e)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Invalid comparison between Unknown and I4
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Expected O, but got Unknown
		Rectangle bounds = e.Bounds;
		bounds.Width--;
		ListBox val = (ListBox)((sender is ListBox) ? sender : null);
		string text = val.Items[e.Index].ToString();
		Graphics graphics = e.Graphics;
		bool flag = (e.State & 1) > 0;
		Color color = (flag ? SystemColors.MenuHighlight : Color.White);
		Color color2 = (flag ? SystemColors.HighlightText : SystemColors.ControlText);
		StiDrawing.FillRectangle(graphics, color, bounds.X, bounds.Y, bounds.Width + 1, bounds.Height);
		if (text == null)
		{
			return;
		}
		Font val2 = new Font("Arial", 8f);
		try
		{
			StringFormat val3 = new StringFormat();
			try
			{
				SolidBrush val4 = new SolidBrush(color2);
				try
				{
					val3.Alignment = (StringAlignment)0;
					val3.LineAlignment = (StringAlignment)1;
					val3.FormatFlags = (StringFormatFlags)4096;
					val3.Trimming = (StringTrimming)3;
					graphics.DrawString(text, val2, (Brush)(object)val4, (RectangleF)bounds, val3);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private void ListBox_MeasureItem(object sender, MeasureItemEventArgs e)
	{
		if (e.Index != -1)
		{
			ListBox val = (ListBox)((sender is ListBox) ? sender : null);
			object item = val.Items[e.Index];
			e.ItemHeight = MeasureItem(val, item).Height;
		}
	}

	protected void ListBox_DoubleClick(object sender, EventArgs e)
	{
		edSvc.CloseDropDown();
	}

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if (context == null || context.Instance == null)
		{
			return ((UITypeEditor)this).GetEditStyle(context);
		}
		return (UITypeEditorEditStyle)3;
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Expected O, but got Unknown
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Expected O, but got Unknown
		if (context == null || context.Instance == null || provider == null)
		{
			return null;
		}
		cancel = false;
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		component = context.Instance as StiButtonElement;
		Array array = context.Instance as Array;
		if (component == null && array != null)
		{
			component = array.GetValue(0) as StiButtonElement;
		}
		if (component == null)
		{
			return null;
		}
		ListBox val = new ListBox
		{
			BorderStyle = (BorderStyle)0,
			DrawMode = (DrawMode)2,
			ItemHeight = 19
		};
		if (StiPropertyGridOptions.Localizable)
		{
			val.Items.Add((object)LocButton);
			val.Items.Add((object)LocRadioButton);
			val.Items.Add((object)LocCheckBox);
		}
		else
		{
			val.Items.Add((object)"Button");
			val.Items.Add((object)"Radio Button");
			val.Items.Add((object)"Check Box");
		}
		((Control)val).Size = MeasureAllItems(val);
		((Control)val).DoubleClick += ListBox_DoubleClick;
		val.Click += ListBox_DoubleClick;
		val.DrawItem += new DrawItemEventHandler(ListBox_DrawItem);
		val.MeasureItem += new MeasureItemEventHandler(ListBox_MeasureItem);
		switch ((StiButtonType)value)
		{
		case StiButtonType.Button:
			((ListControl)val).SelectedIndex = 0;
			break;
		case StiButtonType.RadioButton:
			((ListControl)val).SelectedIndex = 1;
			break;
		case StiButtonType.CheckBox:
			((ListControl)val).SelectedIndex = 2;
			break;
		}
		edSvc.DropDownControl((Control)(object)val);
		if (cancel)
		{
			return value;
		}
		string text = val.SelectedItem as string;
		if (text == LocButton || text == "Button")
		{
			return StiButtonType.Button;
		}
		if (text == LocRadioButton || text == "Radio Button")
		{
			return StiButtonType.RadioButton;
		}
		return StiButtonType.CheckBox;
	}

	private Size MeasureAllItems(ListBox listBox)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		int num = 0;
		int num2 = 0;
		Graphics val = Graphics.FromHwnd(((Control)listBox).Handle);
		try
		{
			Font val2 = new Font("Arial", 8f);
			try
			{
				foreach (object item in listBox.Items)
				{
					Size size = MeasureItem(item, val, val2);
					num += size.Height;
					num2 = Math.Max(num2, size.Width);
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		return new Size(num2, num);
	}

	private static Size MeasureItem(ListBox listBox, object item)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		Graphics val = Graphics.FromHwnd(((Control)listBox).Handle);
		try
		{
			Font val2 = new Font("Arial", 8f);
			try
			{
				return MeasureItem(item, val, val2);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private static Size MeasureItem(object item, Graphics g, Font font)
	{
		string text = item as string;
		Size result = g.MeasureString(text, font).ToSize();
		result.Height += StiScale.XXI(12.0);
		return result;
	}
}
