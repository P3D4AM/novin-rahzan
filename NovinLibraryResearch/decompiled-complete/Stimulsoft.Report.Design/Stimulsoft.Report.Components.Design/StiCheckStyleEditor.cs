using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;

namespace Stimulsoft.Report.Components.Design;

public class StiCheckStyleEditor : UITypeEditor
{
	private IWindowsFormsEditorService edSvc;

	private StiCheckBox checkBox;

	private static int sizeOfCheckBox = StiScale.YYI(30.0);

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (context != null && context.Instance != null)
		{
			return (UITypeEditorEditStyle)3;
		}
		return ((UITypeEditor)this).GetEditStyle(context);
	}

	public override bool GetPaintValueSupported(ITypeDescriptorContext context)
	{
		return false;
	}

	protected void ListBox_DrawItem(object sender, DrawItemEventArgs e)
	{
		DrawCheckStyleItem(e, checkBox);
	}

	public static int GetMaxWidth(Graphics g, Font font)
	{
		Array values = Enum.GetValues(typeof(StiCheckStyle));
		int num = 0;
		for (int i = 0; i < values.Length; i++)
		{
			string text = ((StiCheckStyle)values.GetValue(i)).ToString();
			string text2 = StiLocalization.Get("PropertyEnum", "StiCheckStyle" + text);
			if (text2 != null)
			{
				text = text2;
			}
			int val = (int)g.MeasureString(text, font).Width;
			num = Math.Max(num, val);
		}
		return num + sizeOfCheckBox;
	}

	public static void DrawCheckStyleItem(DrawItemEventArgs e, StiCheckBox checkBox)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		Rectangle bounds = e.Bounds;
		bounds.Width--;
		StiControlPaint.DrawItemBackground(graphics, bounds, e.State);
		Array values = Enum.GetValues(typeof(StiCheckStyle));
		StiComponentPainter.DrawCheckStyle(graphics, new RectangleD(bounds.X, bounds.Y, sizeOfCheckBox, sizeOfCheckBox), (StiCheckStyle)values.GetValue(e.Index), checkBox, 1f);
		Font val = new Font("Arial", 8f);
		try
		{
			StringFormat val2 = new StringFormat();
			try
			{
				val2.LineAlignment = (StringAlignment)1;
				string text = ((StiCheckStyle)values.GetValue(e.Index)).ToString();
				string text2 = StiLocalization.Get("PropertyEnum", "StiCheckStyle" + text);
				if (text2 != null)
				{
					text = text2;
				}
				e.Graphics.DrawString(text, val, Brushes.Black, (RectangleF)new Rectangle(sizeOfCheckBox, bounds.Top, bounds.Width - sizeOfCheckBox, sizeOfCheckBox), val2);
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

	protected void ListBox_DoubleClick(object sender, EventArgs e)
	{
		edSvc.CloseDropDown();
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Expected O, but got Unknown
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Expected O, but got Unknown
		if (context == null || context.Instance == null || provider == null)
		{
			return null;
		}
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		if (context.Instance is Array)
		{
			Array array = context.Instance as Array;
			checkBox = array.GetValue(0) as StiCheckBox;
		}
		else
		{
			checkBox = context.Instance as StiCheckBox;
		}
		Array values = Enum.GetValues(typeof(StiCheckStyle));
		StiCheckStyle selectedIndex = StiCheckStyle.None;
		if (value != null)
		{
			selectedIndex = (StiCheckStyle)value;
		}
		StiListBox stiListBox = new StiListBox();
		try
		{
			((ListBox)stiListBox).BorderStyle = (BorderStyle)0;
			((ListBox)stiListBox).DrawMode = (DrawMode)2;
			((ListBox)stiListBox).ItemHeight = sizeOfCheckBox;
			((Control)stiListBox).Height = ((ListBox)stiListBox).ItemHeight * values.Length;
			if (((Control)stiListBox).Height > StiScale.YYI(330.0))
			{
				((Control)stiListBox).Height = StiScale.YYI(330.0);
			}
			Graphics val = Graphics.FromHwnd(((Control)stiListBox).Handle);
			try
			{
				Font val2 = new Font("Arial", 8f);
				try
				{
					((Control)stiListBox).Width = GetMaxWidth(val, val2);
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
			for (int i = 0; i < values.Length; i++)
			{
				((ListBox)stiListBox).Items.Add((object)((StiCheckStyle)values.GetValue(i)).ToString());
			}
			((ListControl)stiListBox).SelectedIndex = (int)selectedIndex;
			((ListBox)stiListBox).DrawItem += new DrawItemEventHandler(ListBox_DrawItem);
			((Control)stiListBox).DoubleClick += ListBox_DoubleClick;
			((ListBox)stiListBox).Click += ListBox_DoubleClick;
			edSvc.DropDownControl((Control)(object)stiListBox);
			return (StiCheckStyle)values.GetValue(((ListControl)stiListBox).SelectedIndex);
		}
		finally
		{
			((IDisposable)stiListBox)?.Dispose();
		}
	}
}
