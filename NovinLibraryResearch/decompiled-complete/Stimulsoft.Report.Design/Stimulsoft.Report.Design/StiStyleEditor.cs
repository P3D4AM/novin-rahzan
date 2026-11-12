using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Editors;

namespace Stimulsoft.Report.Design;

public class StiStyleEditor : UITypeEditor
{
	private IWindowsFormsEditorService edSvc;

	private bool cancel;

	private StiComponent component;

	private List<StiBaseStyle> styles;

	private bool isExpressionSpecified;

	private ITypeDescriptorContext context;

	private string currentExpression;

	private const int IndexInsertExpression = 0;

	private const int IndexCurrentExpression = 1;

	private const int IndexEditExpression = 2;

	private static string LocEditStyles => Loc.Get("Report", "EditStyles");

	private static string LocNoStyle => Loc.Get("Report", "No");

	private string LocExpression => Loc.GetMain("Expression");

	private string LocEditExpression => Loc.Get("Dashboard", "EditExpression");

	private string LocInsertExpression => Loc.Get("FormRichTextEditor", "Insert");

	private void ListBoxStyles_DrawItem(object sender, DrawItemEventArgs e)
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		Rectangle bounds = e.Bounds;
		bounds.Width--;
		int num = 1;
		if (bounds.Height > 0 && e.Index >= num)
		{
			StiBaseStyle style = styles[e.Index - num];
			StiBaseStyle.DrawStyle(e.Graphics, bounds, style, e.State, fillBackground: true, paintImage: false);
		}
		else
		{
			if (e.Index >= num)
			{
				return;
			}
			Font val = new Font("Arial", 8f);
			try
			{
				string text = LocEditStyles;
				if (e.Index == 0)
				{
					text = LocNoStyle;
				}
				StiControlPaint.DrawItem(e.Graphics, bounds, e.State, text, null, -1, val, SystemColors.Window, SystemColors.ControlText, 0, (RightToLeft)0, (StringAlignment)1);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	private void ListBoxControl_DrawItem(object sender, DrawItemEventArgs e)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Invalid comparison between Unknown and I4
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Expected O, but got Unknown
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		Rectangle bounds = e.Bounds;
		bounds.Width--;
		ListBox val = (ListBox)((sender is ListBox) ? sender : null);
		object obj = val.Items[e.Index];
		Graphics graphics = e.Graphics;
		string text = obj as string;
		bool flag = (e.State & 1) > 0;
		Color color = StiListBoxPainter.GetForegroundColor(flag, isHotLight: false, isFocused: true);
		Color color2 = StiListBoxPainter.GetBackgroundColor(flag, isHotLight: false, isFocused: true);
		if (text == LocExpression)
		{
			color2 = StiColor.Get("f2f2f2");
			color = Color.DimGray;
		}
		StiDrawing.FillRectangle(graphics, color2, bounds.X, bounds.Y, bounds.Width + 1, bounds.Height);
		if (flag && StiUX.IsDark)
		{
			StiDrawing.FillRectangle(graphics, StiUX.Accent.ChangeAlpha(50), bounds.X, bounds.Y, bounds.Width + 1, bounds.Height);
		}
		if (text == LocEditExpression)
		{
			graphics.DrawLine(Pens.LightGray, bounds.Left + StiScaleUI.I8, bounds.Top, bounds.Right - StiScaleUI.I8, bounds.Top);
		}
		else if (e.Index == 0)
		{
			graphics.DrawLine(Pens.LightGray, bounds.Left, bounds.Top, bounds.Right, bounds.Top);
		}
		if (text != null)
		{
			Font val2 = new Font("Arial", 8f);
			try
			{
				bounds.Inflate(-StiScale.I4, 0);
				TextFormatFlags val3 = (TextFormatFlags)4;
				TextRenderer.DrawText((IDeviceContext)(object)graphics, text, val2, bounds, color, val3);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
	}

	protected void ListBoxStyles_DoubleClick(object sender, EventArgs e)
	{
		string name = context.PropertyDescriptor.Name;
		object instance = context.Instance;
		StiAppExpressionHelper.RemoveExpression(instance, name);
		edSvc.CloseDropDown();
	}

	protected void ListBoxControl_DoubleClick(object sender, EventArgs e)
	{
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Invalid comparison between Unknown and I4
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		ListBox val = (ListBox)((sender is ListBox) ? sender : null);
		string text = val.SelectedItem?.ToString();
		if (text == LocEditStyles && StiOptions.Designer.PropertyGrid.AllowEditStyle)
		{
			if (component.Report != null)
			{
				StiStyleDesigner stiStyleDesigner = new StiStyleDesigner(component.Report.Designer.DesignerControl as StiDesignerControl, component.Report);
				try
				{
					component?.Report?.Designer?.DesignerControl?.AddOwnedForm((Form)(object)stiStyleDesigner);
					((Form)stiStyleDesigner).ShowDialog();
				}
				finally
				{
					((IDisposable)stiStyleDesigner)?.Dispose();
				}
				component?.Report?.Designer?.Refresh();
				component?.Report?.ApplyStyles();
				cancel = true;
				edSvc.CloseDropDown();
			}
			return;
		}
		int num = (isExpressionSpecified ? 2 : 0);
		if (((ListControl)val).SelectedIndex == num)
		{
			StiReport report = (context.Instance as StiComponent)?.Report;
			string text2 = currentExpression;
			DialogResult val2 = StiTextEditor.EditText(report, ref text2, StiCodeEditorMode.EditorAppExpression, null, allowPackExpression: true, new string[4] { "\"RedStyle\"", "\"MyStyleName\"", "MyStringVariable", "MyData.MyColumn >= 0 ? \"red-style\" : \"green-style\"" });
			if ((int)val2 == 1)
			{
				StiAppExpressionHelper.SetExpression(context.Instance, context.PropertyDescriptor.Name, text2);
			}
		}
		edSvc.CloseDropDown();
	}

	public override void PaintValue(PaintValueEventArgs e)
	{
		if (e.Value == null)
		{
			return;
		}
		string name = e.Value as string;
		StiComponent stiComponent = e.Context.Instance as StiComponent;
		if (stiComponent?.Report != null)
		{
			StiBaseStyle stiBaseStyle = stiComponent.Report.Styles[name];
			if (stiBaseStyle != null)
			{
				StiBaseStyle.DrawBox(e.Graphics, e.Bounds, stiBaseStyle, paintValue: true, paintImage: false);
			}
		}
	}

	public override bool GetPaintValueSupported(ITypeDescriptorContext context)
	{
		return !string.IsNullOrWhiteSpace(context.PropertyDescriptor.GetValue(context.Instance) as string);
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
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_036d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Expected O, but got Unknown
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Expected O, but got Unknown
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Expected O, but got Unknown
		if (context == null || context.Instance == null || provider == null)
		{
			return null;
		}
		cancel = false;
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		this.context = context;
		component = context.Instance as StiComponent;
		Array array = context.Instance as Array;
		if (component == null && array != null)
		{
			component = array.GetValue(0) as StiComponent;
		}
		if (component == null)
		{
			return null;
		}
		bool flag = component is StiSparkline;
		styles = new List<StiBaseStyle>();
		foreach (StiBaseStyle style in component.Report.Styles)
		{
			if (flag)
			{
				if (style is StiIndicatorStyle)
				{
					styles.Add((StiIndicatorStyle)style);
				}
			}
			else if (style is StiStyle)
			{
				styles.Add(style as StiStyle);
			}
		}
		ListBox val = new ListBox
		{
			BorderStyle = (BorderStyle)0,
			DrawMode = (DrawMode)2,
			ItemHeight = StiScale.YYI(22.0)
		};
		ListBox val2 = new ListBox
		{
			BorderStyle = (BorderStyle)0,
			DrawMode = (DrawMode)2,
			ItemHeight = StiScale.YYI(22.0)
		};
		Graphics val3 = Graphics.FromHwnd(((Control)val).Handle);
		try
		{
			Font val4 = new Font("Arial", 8f);
			try
			{
				((Control)val).Width = GetMaxWidth(val3, val4);
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
		((ListControl)val).SelectedIndex = -1;
		val.Items.Add((object)string.Empty);
		string name = context.PropertyDescriptor.Name;
		object instance = context.Instance;
		isExpressionSpecified = StiAppExpressionHelper.IsExpressionSpecified(instance, name);
		int selectedIndex = -1;
		currentExpression = StiAppExpressionHelper.GetExpressionValue(instance, name);
		if (isExpressionSpecified)
		{
			val2.Items.Add((object)LocExpression);
			val2.Items.Add((object)currentExpression);
			selectedIndex = 1;
		}
		val2.Items.Add((object)(isExpressionSpecified ? LocEditExpression : LocInsertExpression));
		if (StiOptions.Designer.PropertyGrid.AllowEditStyle)
		{
			val2.Items.Add((object)LocEditStyles);
		}
		foreach (StiBaseStyle style2 in styles)
		{
			if (!string.IsNullOrEmpty(style2.Name))
			{
				val.Items.Add((object)style2.Name);
				if (value != null && style2.Name.ToLowerInvariant() == ((string)value).ToLowerInvariant() && !isExpressionSpecified)
				{
					((ListControl)val).SelectedIndex = val.Items.Count - 1;
				}
			}
		}
		((Control)val).Height = Math.Min(val.ItemHeight * val.Items.Count, StiScale.YYI(300.0));
		((Control)val2).Height = val2.ItemHeight * val2.Items.Count;
		((ListControl)val2).SelectedIndex = selectedIndex;
		((Control)val).DoubleClick += ListBoxStyles_DoubleClick;
		val.Click += ListBoxStyles_DoubleClick;
		val.DrawItem += new DrawItemEventHandler(ListBoxStyles_DrawItem);
		((Control)val2).DoubleClick += ListBoxControl_DoubleClick;
		val2.Click += ListBoxControl_DoubleClick;
		val2.DrawItem += new DrawItemEventHandler(ListBoxControl_DrawItem);
		Panel val5 = new Panel();
		((Control)val5).Height = ((Control)val).Height + ((Control)val2).Height;
		((Control)val5).Controls.Add((Control)(object)val);
		((Control)val5).Controls.Add((Control)(object)val2);
		((Control)val).Dock = (DockStyle)5;
		((Control)val2).Dock = (DockStyle)2;
		edSvc.DropDownControl((Control)(object)val5);
		if (!cancel)
		{
			return val.SelectedItem;
		}
		return value;
	}

	private int GetMaxWidth(Graphics g, Font font)
	{
		int num = 0;
		foreach (StiBaseStyle style in styles)
		{
			int val = (int)g.MeasureString(style.Name, font).Width;
			num = Math.Max(num, val);
		}
		return StiScale.XXI(num + 10);
	}
}
