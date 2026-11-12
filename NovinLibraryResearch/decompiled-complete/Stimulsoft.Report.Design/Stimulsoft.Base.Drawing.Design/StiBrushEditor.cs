using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Editors;

namespace Stimulsoft.Base.Drawing.Design;

public class StiBrushEditor : UITypeEditor
{
	private class DoubleBufferedListBox : ListBox
	{
		private StiBrushEditor editor;

		protected override void OnPaint(PaintEventArgs e)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Expected O, but got Unknown
			for (int i = 0; i < ((ListBox)this).Items.Count; i++)
			{
				Rectangle itemRectangle = ((ListBox)this).GetItemRectangle(i);
				DrawItemState val = (DrawItemState)((i == ((ListControl)this).SelectedIndex) ? 1 : 32);
				((ListBox)this).OnDrawItem(new DrawItemEventArgs(e.Graphics, ((Control)this).Font, itemRectangle, i, val));
			}
		}

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			editor.DrawBrushItem((ListBox)(object)this, (StiBrush)editor.brush, e);
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
		}

		public DoubleBufferedListBox(StiBrushEditor editor)
		{
			this.editor = editor;
			((Control)this).SetStyle((ControlStyles)131072, true);
			((Control)this).SetStyle((ControlStyles)8192, true);
			((Control)this).SetStyle((ControlStyles)16, true);
			((Control)this).SetStyle((ControlStyles)2, true);
			((Control)this).SetStyle((ControlStyles)2048, true);
			((Control)this).DoubleBuffered = true;
		}
	}

	private IWindowsFormsEditorService edSvc;

	private object brush;

	private int hotTrackingIndex = -1;

	private bool isExpressionSpecified;

	private int BaseIndex = -2;

	private int IndexDefaultBrush => BaseIndex;

	private int IndexStyleBrush => BaseIndex + 1;

	private int IndexEmptyBrush => BaseIndex + 2;

	private int IndexSolidBrush => BaseIndex + 3;

	private int IndexHatchBrush => BaseIndex + 4;

	private int IndexGradientBrush => BaseIndex + 5;

	private int IndexGlareBrush => BaseIndex + 6;

	private int IndexGlassBrush => BaseIndex + 7;

	private int IndexInsertExpression => BaseIndex + 8;

	private int IndexCurrentExpression => BaseIndex + 9;

	private int IndexEditExpression => BaseIndex + 10;

	private string LocExpression => Loc.GetMain("Expression");

	private string LocEditExpression => Loc.Get("Dashboard", "EditExpression");

	private string LocInsertExpression => Loc.Get("FormRichTextEditor", "Insert");

	private void ListBox_DoubleClick(object sender, EventArgs e)
	{
		edSvc.CloseDropDown();
	}

	private void ListBox_MouseLeave(object sender, EventArgs e)
	{
		ListBox val = (ListBox)((sender is ListBox) ? sender : null);
		int num = hotTrackingIndex;
		hotTrackingIndex = -1;
		if (num != -1)
		{
			((Control)val).Invalidate(GetItemRect(val, num));
		}
	}

	private void ListBox_MouseMove(object sender, MouseEventArgs e)
	{
		ListBox val = (ListBox)((sender is ListBox) ? sender : null);
		int num = val.IndexFromPoint(e.Location);
		if (hotTrackingIndex != num)
		{
			int num2 = hotTrackingIndex;
			hotTrackingIndex = num;
			if (num2 != -1)
			{
				((Control)val).Invalidate(GetItemRect(val, num2));
			}
			if (hotTrackingIndex != -1)
			{
				((Control)val).Invalidate(GetItemRect(val, hotTrackingIndex));
			}
		}
	}

	public void DrawBrushItem(ListBox listBox, StiBrush brush, DrawItemEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Invalid comparison between Unknown and I4
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Expected O, but got Unknown
		//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		Rectangle bounds = e.Bounds;
		if ((e.State & 1) > 0)
		{
			bounds.Width--;
		}
		bool flag = (e.State & 1) > 0;
		bool isHotLight = e.Index == hotTrackingIndex;
		Graphics graphics = e.Graphics;
		Color color = StiListBoxPainter.GetForegroundColor(flag, isHotLight, isFocused: true);
		Color color2 = StiListBoxPainter.GetBackgroundColor(flag, isHotLight, isFocused: true);
		string text = listBox.Items[e.Index].ToString();
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
		Rectangle rectangle = new Rectangle(bounds.X + StiScaleUI.I2, bounds.Y + StiScaleUI.I2, StiScaleUI.I19, bounds.Height - StiScaleUI.I5);
		if (e.Index == IndexDefaultBrush)
		{
			text = Loc.GetReport("FromDefaultState");
		}
		else if (e.Index == IndexStyleBrush)
		{
			text = Loc.Get("FormStyleDesigner", "FromStyle");
		}
		else if (e.Index == IndexEmptyBrush)
		{
			StiDrawing.FillRectangle(graphics, Color.White, rectangle);
			text = StiLocalization.Get("Report", "StiEmptyBrush") ?? "EmptyBrush";
		}
		else if (e.Index == IndexSolidBrush)
		{
			StiDrawing.FillRectangle(graphics, GetSolidBrush(), rectangle);
			text = StiLocalization.Get("Report", "StiSolidBrush") ?? "SolidBrush";
		}
		else if (e.Index == IndexHatchBrush)
		{
			StiDrawing.FillRectangle(graphics, GetHatchBrush(), rectangle);
			text = StiLocalization.Get("Report", "StiHatchBrush") ?? "HatchBrush";
		}
		else if (e.Index == IndexGradientBrush)
		{
			StiDrawing.FillRectangle(graphics, GetGradientBrush(), rectangle);
			text = StiLocalization.Get("Report", "StiGradientBrush") ?? "GradientBrush";
		}
		else if (e.Index == IndexGlareBrush)
		{
			StiDrawing.FillRectangle(graphics, GetGlareBrush(), rectangle);
			text = StiLocalization.Get("Report", "StiGlareBrush") ?? "GlareBrush";
		}
		else if (e.Index == IndexGlassBrush)
		{
			StiDrawing.FillRectangle(graphics, GetGlassBrush(), rectangle);
			text = StiLocalization.Get("Report", "StiGlassBrush") ?? "GlassBrush";
		}
		else
		{
			text = listBox.Items[e.Index].ToString();
		}
		if (e.Index <= IndexGlassBrush && e.Index > IndexStyleBrush)
		{
			Color color3 = StiColorUtils.Dark(GetForeColor(brush), 100);
			StiDrawing.DrawRectangle(graphics, color3, rectangle);
		}
		if (text == LocInsertExpression || text == LocEditExpression || e.Index == IndexEmptyBrush + 1 || (e.Index == IndexStyleBrush + 1 && IndexStyleBrush >= 0))
		{
			graphics.DrawLine(Pens.LightGray, bounds.Left + StiScaleUI.I8, bounds.Top, bounds.Right - StiScaleUI.I8, bounds.Top);
		}
		Font val = new Font("Arial", 8f);
		try
		{
			TextFormatFlags val2 = (TextFormatFlags)4;
			Rectangle rectangle2 = new Rectangle(StiScaleUI.I(25), bounds.Top, bounds.Width - StiScaleUI.I(25), bounds.Height);
			TextRenderer.DrawText((IDeviceContext)(object)graphics, text, val, rectangle2, color, val2);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private Color GetForeColor(StiBrush brush)
	{
		if (brush is StiSolidBrush stiSolidBrush)
		{
			if (!StiColorUtils.IsItTooLight(stiSolidBrush.Color))
			{
				return StiColorUtils.Light(stiSolidBrush.Color, 50);
			}
			return StiColorUtils.Dark(stiSolidBrush.Color, 100);
		}
		if (brush is StiHatchBrush stiHatchBrush)
		{
			return stiHatchBrush.ForeColor;
		}
		if (brush is StiGradientBrush stiGradientBrush)
		{
			return stiGradientBrush.StartColor;
		}
		if (brush is StiGlareBrush stiGlareBrush)
		{
			return stiGlareBrush.StartColor;
		}
		if (brush is StiGlassBrush stiGlassBrush)
		{
			if (!StiColorUtils.IsItTooLight(stiGlassBrush.Color))
			{
				return StiColorUtils.Light(stiGlassBrush.Color, 50);
			}
			return StiColorUtils.Dark(stiGlassBrush.Color, 100);
		}
		return Color.Gray;
	}

	private Color GetBackColor(StiBrush brush)
	{
		if (brush is StiSolidBrush stiSolidBrush)
		{
			if (!StiColorUtils.IsItTooDark(stiSolidBrush.Color))
			{
				return StiColorUtils.Dark(stiSolidBrush.Color, 50);
			}
			return StiColorUtils.Light(stiSolidBrush.Color, 100);
		}
		if (brush is StiHatchBrush stiHatchBrush)
		{
			return stiHatchBrush.BackColor;
		}
		if (brush is StiGradientBrush stiGradientBrush)
		{
			return stiGradientBrush.EndColor;
		}
		if (brush is StiGlareBrush stiGlareBrush)
		{
			return stiGlareBrush.EndColor;
		}
		if (brush is StiGlassBrush stiGlassBrush)
		{
			if (!StiColorUtils.IsItTooDark(stiGlassBrush.Color))
			{
				return StiColorUtils.Dark(stiGlassBrush.Color, 50);
			}
			return StiColorUtils.Light(stiGlassBrush.Color, 100);
		}
		return Color.White;
	}

	private Rectangle GetItemRect(ListBox listBox, int index)
	{
		return new Rectangle(0, index * listBox.ItemHeight, ((Control)listBox).Width, (index + 1) * listBox.ItemHeight);
	}

	private StiBrush GetDefaultBrush()
	{
		return new StiDefaultBrush();
	}

	private StiBrush GetStyleBrush()
	{
		return new StiStyleBrush();
	}

	private StiBrush GetEmptyBrush()
	{
		if (brush is StiEmptyBrush result)
		{
			return result;
		}
		return new StiEmptyBrush();
	}

	private StiBrush GetSolidBrush()
	{
		if (brush is StiSolidBrush result)
		{
			return result;
		}
		if (brush is StiGlassBrush stiGlassBrush)
		{
			return new StiSolidBrush(stiGlassBrush.Color);
		}
		return new StiSolidBrush(StiColorUtils.MixingColors(GetForeColor(brush as StiBrush), GetBackColor(brush as StiBrush)));
	}

	private StiBrush GetHatchBrush()
	{
		if (brush is StiHatchBrush result)
		{
			return result;
		}
		return new StiHatchBrush((HatchStyle)3, GetForeColor(brush as StiBrush), GetBackColor(brush as StiBrush));
	}

	private StiBrush GetGradientBrush()
	{
		if (brush is StiGradientBrush result)
		{
			return result;
		}
		double angle = ((brush is StiGlareBrush stiGlareBrush) ? stiGlareBrush.Angle : 0.0);
		return new StiGradientBrush(GetForeColor(brush as StiBrush), GetBackColor(brush as StiBrush), angle);
	}

	private StiBrush GetGlareBrush()
	{
		if (brush is StiGlareBrush result)
		{
			return result;
		}
		double angle = ((brush is StiGradientBrush stiGradientBrush) ? stiGradientBrush.Angle : 0.0);
		return new StiGlareBrush(GetForeColor(brush as StiBrush), GetBackColor(brush as StiBrush), angle, 0.5f, 1f);
	}

	private StiBrush GetGlassBrush()
	{
		if (brush is StiGlassBrush result)
		{
			return result;
		}
		if (brush is StiSolidBrush stiSolidBrush)
		{
			return new StiGlassBrush(stiSolidBrush.Color, drawHatch: true, 0.2f);
		}
		return new StiGlassBrush(GetForeColor(brush as StiBrush), drawHatch: true, 0.2f);
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

	public override void PaintValue(PaintValueEventArgs e)
	{
		if (this is StiExpressionBrushEditor)
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
			Brush val = StiBrush.GetBrush(e.Value as StiBrush, e.Bounds);
			try
			{
				e.Graphics.FillRectangle(val, e.Bounds);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	public override bool GetPaintValueSupported(ITypeDescriptorContext context)
	{
		object value = context.PropertyDescriptor.GetValue(context.Instance);
		if (!(value is StiDefaultBrush))
		{
			return !(value is StiStyleBrush);
		}
		return false;
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Expected O, but got Unknown
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Invalid comparison between Unknown and I4
		bool isDefaultAndStyleBrushesAllowed = GetIsDefaultAndStyleBrushesAllowed(context);
		bool isStyleBrushAllowed = GetIsStyleBrushAllowed(context);
		BaseIndex = -2;
		int num = 0;
		if (isDefaultAndStyleBrushesAllowed)
		{
			BaseIndex += 2;
			num += 2;
		}
		else if (isStyleBrushAllowed)
		{
			BaseIndex++;
			num++;
		}
		brush = value;
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		string name = context.PropertyDescriptor.Name;
		object instance = context.Instance;
		DoubleBufferedListBox doubleBufferedListBox = new DoubleBufferedListBox(this);
		((ListBox)doubleBufferedListBox).ItemHeight = StiScaleUI.I(22);
		((Control)doubleBufferedListBox).Width = StiScaleUI.I(100);
		((ListBox)doubleBufferedListBox).BorderStyle = (BorderStyle)0;
		DoubleBufferedListBox doubleBufferedListBox2 = doubleBufferedListBox;
		((ListBox)doubleBufferedListBox2).DrawMode = (DrawMode)2;
		if (isDefaultAndStyleBrushesAllowed)
		{
			((ListBox)doubleBufferedListBox2).Items.Add((object)"DefaultBrush");
		}
		if (isStyleBrushAllowed || isDefaultAndStyleBrushesAllowed)
		{
			((ListBox)doubleBufferedListBox2).Items.Add((object)"StyleBrush");
		}
		((ListBox)doubleBufferedListBox2).Items.Add((object)"EmptyBrush");
		((ListBox)doubleBufferedListBox2).Items.Add((object)"SolidBrush");
		((ListBox)doubleBufferedListBox2).Items.Add((object)"HatchBrush");
		((ListBox)doubleBufferedListBox2).Items.Add((object)"GradientBrush");
		((ListBox)doubleBufferedListBox2).Items.Add((object)"GlareBrush");
		((ListBox)doubleBufferedListBox2).Items.Add((object)"GlassBrush");
		isExpressionSpecified = StiAppExpressionHelper.IsExpressionSpecified(instance, name);
		if (!isExpressionSpecified)
		{
			if (value is StiDefaultBrush)
			{
				((ListControl)doubleBufferedListBox2).SelectedIndex = IndexDefaultBrush;
			}
			else if (value is StiStyleBrush)
			{
				((ListControl)doubleBufferedListBox2).SelectedIndex = IndexStyleBrush;
			}
			else if (value is StiEmptyBrush)
			{
				((ListControl)doubleBufferedListBox2).SelectedIndex = IndexEmptyBrush;
			}
			else if (value is StiSolidBrush)
			{
				((ListControl)doubleBufferedListBox2).SelectedIndex = IndexSolidBrush;
			}
			else if (value is StiHatchBrush)
			{
				((ListControl)doubleBufferedListBox2).SelectedIndex = IndexHatchBrush;
			}
			else if (value is StiGradientBrush)
			{
				((ListControl)doubleBufferedListBox2).SelectedIndex = IndexGradientBrush;
			}
			else if (value is StiGlareBrush)
			{
				((ListControl)doubleBufferedListBox2).SelectedIndex = IndexGlareBrush;
			}
			else if (value is StiGlassBrush)
			{
				((ListControl)doubleBufferedListBox2).SelectedIndex = IndexGlassBrush;
			}
		}
		string text = "";
		if (this is StiExpressionBrushEditor)
		{
			text = StiAppExpressionHelper.GetExpressionValue(instance, name);
			if (isExpressionSpecified)
			{
				((ListBox)doubleBufferedListBox2).Items.Add((object)LocExpression);
				((ListBox)doubleBufferedListBox2).Items.Add((object)text);
				((ListControl)doubleBufferedListBox2).SelectedIndex = IndexCurrentExpression;
			}
			((ListBox)doubleBufferedListBox2).Items.Add((object)(isExpressionSpecified ? LocEditExpression : LocInsertExpression));
		}
		if (this is StiExpressionBrushEditor)
		{
			((Control)doubleBufferedListBox2).Height = ((ListBox)doubleBufferedListBox2).ItemHeight * ((isExpressionSpecified ? 9 : 7) + num);
		}
		else
		{
			((Control)doubleBufferedListBox2).Height = ((ListBox)doubleBufferedListBox2).ItemHeight * (6 + num);
		}
		((Control)doubleBufferedListBox2).DoubleClick += ListBox_DoubleClick;
		((ListBox)doubleBufferedListBox2).Click += ListBox_DoubleClick;
		((Control)doubleBufferedListBox2).MouseMove += new MouseEventHandler(ListBox_MouseMove);
		((Control)doubleBufferedListBox2).MouseLeave += ListBox_MouseLeave;
		edSvc.DropDownControl((Control)(object)doubleBufferedListBox2);
		string text2 = ((ListBox)doubleBufferedListBox2).SelectedItem?.ToString();
		int num2 = (isExpressionSpecified ? IndexEditExpression : IndexInsertExpression);
		if (((ListControl)doubleBufferedListBox2).SelectedIndex == IndexDefaultBrush)
		{
			StiAppExpressionHelper.RemoveExpression(instance, name);
			return GetDefaultBrush();
		}
		if (((ListControl)doubleBufferedListBox2).SelectedIndex == IndexStyleBrush)
		{
			StiAppExpressionHelper.RemoveExpression(instance, name);
			return GetStyleBrush();
		}
		if (((ListControl)doubleBufferedListBox2).SelectedIndex == IndexEmptyBrush)
		{
			StiAppExpressionHelper.RemoveExpression(instance, name);
			return GetEmptyBrush();
		}
		if (((ListControl)doubleBufferedListBox2).SelectedIndex == IndexSolidBrush)
		{
			StiAppExpressionHelper.RemoveExpression(instance, name);
			return GetSolidBrush();
		}
		if (((ListControl)doubleBufferedListBox2).SelectedIndex == IndexHatchBrush)
		{
			StiAppExpressionHelper.RemoveExpression(instance, name);
			return GetHatchBrush();
		}
		if (((ListControl)doubleBufferedListBox2).SelectedIndex == IndexGradientBrush)
		{
			StiAppExpressionHelper.RemoveExpression(instance, name);
			return GetGradientBrush();
		}
		if (((ListControl)doubleBufferedListBox2).SelectedIndex == IndexGlareBrush)
		{
			StiAppExpressionHelper.RemoveExpression(instance, name);
			return GetGlareBrush();
		}
		if (((ListControl)doubleBufferedListBox2).SelectedIndex == IndexGlassBrush)
		{
			StiAppExpressionHelper.RemoveExpression(instance, name);
			return GetGlassBrush();
		}
		if (((ListControl)doubleBufferedListBox2).SelectedIndex == IndexCurrentExpression && isExpressionSpecified)
		{
			return value;
		}
		if (((ListControl)doubleBufferedListBox2).SelectedIndex == num2)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				text = GetExpressionFromBrush(value, text);
			}
			StiReport report = (context.Instance as StiComponent)?.Report;
			string text3 = text;
			DialogResult val = StiTextEditor.EditText(report, ref text3, StiCodeEditorMode.EditorAppExpression, null, allowPackExpression: true, new string[4] { "\"Red\"", "\"33ff95\"", "MyStringVariable", "MyData.MyColumn > 0 ? \"Red\" : \"Green\"" });
			if ((int)val == 1)
			{
				StiAppExpressionHelper.SetExpression(instance, name, text3);
			}
		}
		return value;
	}

	private static bool GetIsDefaultAndStyleBrushesAllowed(ITypeDescriptorContext context)
	{
		StiVisualStatePermissionKind stiVisualStatePermissionKind = ((context.PropertyDescriptor.Attributes.Cast<Attribute>().FirstOrDefault((Attribute a) => a is StiVisualStatePermissionAttribute) is StiVisualStatePermissionAttribute stiVisualStatePermissionAttribute) ? stiVisualStatePermissionAttribute.Kinds : StiVisualStatePermissionKind.Deny);
		return stiVisualStatePermissionKind == StiVisualStatePermissionKind.AllowDefaultAndStyle;
	}

	private static bool GetIsStyleBrushAllowed(ITypeDescriptorContext context)
	{
		StiVisualStatePermissionKind stiVisualStatePermissionKind = ((context.PropertyDescriptor.Attributes.Cast<Attribute>().FirstOrDefault((Attribute a) => a is StiVisualStatePermissionAttribute) is StiVisualStatePermissionAttribute stiVisualStatePermissionAttribute) ? stiVisualStatePermissionAttribute.Kinds : StiVisualStatePermissionKind.Deny);
		return stiVisualStatePermissionKind == StiVisualStatePermissionKind.AllowStyle;
	}

	private static string GetExpressionFromBrush(object value, string expression)
	{
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			if (!(value is StiSolidBrush stiSolidBrush))
			{
				if (!(value is StiHatchBrush stiHatchBrush))
				{
					if (!(value is StiGradientBrush stiGradientBrush))
					{
						if (!(value is StiGlassBrush stiGlassBrush))
						{
							if (value is StiGlareBrush stiGlareBrush)
							{
								return $"GlareBrushValue(\"{stiGlareBrush.StartColor.ToHex()}\", \"{stiGlareBrush.EndColor.ToHex()}\", {stiGlareBrush.Angle}, {stiGlareBrush.Focus}, {stiGlareBrush.Scale})";
							}
							return expression;
						}
						return $"GlassBrushValue(\"{stiGlassBrush.Color.ToHex()}\", {stiGlassBrush.DrawHatch.ToString().ToLowerInvariant()}, {stiGlassBrush.Blend})";
					}
					return $"GradientBrushValue(\"{stiGradientBrush.StartColor.ToHex()}\", \"{stiGradientBrush.EndColor.ToHex()}\", {stiGradientBrush.Angle})";
				}
				return $"HatchBrushValue(HatchStyle.{stiHatchBrush.Style}, \"{stiHatchBrush.ForeColor.ToHex()}\", \"{stiHatchBrush.BackColor.ToHex()}\")";
			}
			return "SolidBrushValue(\"" + stiSolidBrush.Color.ToHex() + "\")";
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}
}
