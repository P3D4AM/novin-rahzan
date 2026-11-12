using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Editors;

namespace Stimulsoft.Base.Drawing.Design;

public class StiExpressionBoolEditor : UITypeEditor
{
	private class DoubleBufferedListBox : ListBox
	{
		private StiExpressionBoolEditor editor;

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
			editor.ListBox_DrawItem(this, e);
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
		}

		public DoubleBufferedListBox(StiExpressionBoolEditor editor)
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

	private bool cancel;

	private int colorCellSize = StiScale.XXI(14.0);

	private int colorCellStep = StiScale.I4;

	private int hotTrackingIndex = -1;

	private const int IndexTrueValue = 0;

	private const int IndexFalseValue = 1;

	private const int IndexInsertExpression = 2;

	private const int IndexCurrentExpression = 3;

	private const int IndexEditExpression = 4;

	private string LocTrue
	{
		get
		{
			if (!StiPropertyGridOptions.Localizable)
			{
				return "True";
			}
			return Loc.Get("PropertyEnum", "boolTrue");
		}
	}

	private string LocFalse
	{
		get
		{
			if (!StiPropertyGridOptions.Localizable)
			{
				return "False";
			}
			return Loc.Get("PropertyEnum", "boolFalse");
		}
	}

	private string LocExpression => Loc.GetMain("Expression");

	private string LocEditExpression => Loc.Get("Dashboard", "EditExpression");

	private string LocInsertExpression => Loc.Get("FormRichTextEditor", "Insert");

	private void ListBox_DrawItem(object sender, DrawItemEventArgs e)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Invalid comparison between Unknown and I4
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		Rectangle bounds = e.Bounds;
		bounds.Width--;
		ListBox val = (ListBox)((sender is ListBox) ? sender : null);
		object obj = val.Items[e.Index];
		Graphics graphics = e.Graphics;
		string text = obj as string;
		bool flag = (e.State & 1) > 0;
		bool isHotLight = e.Index == hotTrackingIndex;
		Color color = StiListBoxPainter.GetForegroundColor(flag, isHotLight, isFocused: true);
		Color color2 = StiListBoxPainter.GetBackgroundColor(flag, isHotLight, isFocused: true);
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
		if (text == LocInsertExpression || text == LocEditExpression)
		{
			graphics.DrawLine(Pens.LightGray, bounds.Left + StiScaleUI.I8, bounds.Top, bounds.Right - StiScaleUI.I8, bounds.Top);
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

	private void ListBox_MeasureItem(object sender, MeasureItemEventArgs e)
	{
		if (e.Index != -1)
		{
			e.ItemWidth = GetTotalWidth();
			e.ItemHeight = colorCellSize + colorCellStep * 2;
		}
	}

	protected void ListBox_DoubleClick(object sender, EventArgs e)
	{
		cancel = false;
		edSvc.CloseDropDown();
	}

	private void ListBox_MouseLeave(object sender, EventArgs e)
	{
		ListBox val = (ListBox)((sender is ListBox) ? sender : null);
		hotTrackingIndex = -1;
		((Control)val).Invalidate();
	}

	private void ListBox_MouseMove(object sender, MouseEventArgs e)
	{
		ListBox val = (ListBox)((sender is ListBox) ? sender : null);
		int num = val.IndexFromPoint(e.Location);
		if (hotTrackingIndex != num)
		{
			hotTrackingIndex = num;
			((Control)val).Invalidate();
		}
	}

	public override bool GetPaintValueSupported(ITypeDescriptorContext context)
	{
		return StiAppExpressionHelper.IsExpressionSpecified(context.Instance, context.PropertyDescriptor.Name);
	}

	public override void PaintValue(PaintValueEventArgs e)
	{
		StiExpressionEditor.PaintExpressionMarker(e);
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
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Expected O, but got Unknown
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Expected O, but got Unknown
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Invalid comparison between Unknown and I4
		if (context == null || context.Instance == null || provider == null)
		{
			return null;
		}
		cancel = true;
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		string name = context.PropertyDescriptor.Name;
		object instance = context.Instance;
		DoubleBufferedListBox doubleBufferedListBox = new DoubleBufferedListBox(this);
		((ListBox)doubleBufferedListBox).BorderStyle = (BorderStyle)0;
		((ListBox)doubleBufferedListBox).DrawMode = (DrawMode)2;
		DoubleBufferedListBox doubleBufferedListBox2 = doubleBufferedListBox;
		int selectedIndex = -1;
		((ListBox)doubleBufferedListBox2).Items.Add((object)LocTrue);
		((ListBox)doubleBufferedListBox2).Items.Add((object)LocFalse);
		bool flag = StiAppExpressionHelper.IsExpressionSpecified(instance, name);
		if (!flag)
		{
			if (value is bool && (bool)value)
			{
				selectedIndex = 0;
			}
			if (value is bool && !(bool)value)
			{
				selectedIndex = 1;
			}
		}
		string expressionValue = StiAppExpressionHelper.GetExpressionValue(instance, name);
		if (flag)
		{
			((ListBox)doubleBufferedListBox2).Items.Add((object)LocExpression);
			((ListBox)doubleBufferedListBox2).Items.Add((object)expressionValue);
			selectedIndex = 3;
		}
		((ListBox)doubleBufferedListBox2).Items.Add((object)(flag ? LocEditExpression : LocInsertExpression));
		((Control)doubleBufferedListBox2).Width = GetTotalWidth();
		((Control)doubleBufferedListBox2).Height = GetTotalHeight((ListBox)(object)doubleBufferedListBox2);
		((ListControl)doubleBufferedListBox2).SelectedIndex = selectedIndex;
		((Control)doubleBufferedListBox2).DoubleClick += ListBox_DoubleClick;
		((ListBox)doubleBufferedListBox2).Click += ListBox_DoubleClick;
		((ListBox)doubleBufferedListBox2).MeasureItem += new MeasureItemEventHandler(ListBox_MeasureItem);
		((Control)doubleBufferedListBox2).MouseMove += new MouseEventHandler(ListBox_MouseMove);
		((Control)doubleBufferedListBox2).MouseLeave += ListBox_MouseLeave;
		edSvc.DropDownControl((Control)(object)doubleBufferedListBox2);
		if (cancel)
		{
			return value;
		}
		string text = ((ListBox)doubleBufferedListBox2).SelectedItem?.ToString();
		int num = (flag ? 4 : 2);
		if (((ListControl)doubleBufferedListBox2).SelectedIndex == 0)
		{
			StiAppExpressionHelper.RemoveExpression(instance, name);
			return true;
		}
		if (((ListControl)doubleBufferedListBox2).SelectedIndex == 1)
		{
			StiAppExpressionHelper.RemoveExpression(instance, name);
			return false;
		}
		if (((ListControl)doubleBufferedListBox2).SelectedIndex == 3 && flag)
		{
			return value;
		}
		if (((ListControl)doubleBufferedListBox2).SelectedIndex == num)
		{
			StiReport report = (context.Instance as StiComponent)?.Report;
			string text2 = expressionValue;
			DialogResult val = StiTextEditor.EditText(report, ref text2, StiCodeEditorMode.EditorAppExpression, null, allowPackExpression: true, new string[5] { "True", "false", "MyVariable", "MyData.MyColumn > 15", "MyVariable == 5" });
			if ((int)val == 1)
			{
				StiAppExpressionHelper.SetExpression(instance, name, text2);
			}
		}
		return value;
	}

	private int GetTotalWidth()
	{
		return (colorCellSize + colorCellStep) * 6 + colorCellStep;
	}

	private int GetTotalHeight(ListBox listBox)
	{
		return Math.Min(listBox.Items.Count, 12) * (colorCellSize + colorCellStep * 2);
	}
}
