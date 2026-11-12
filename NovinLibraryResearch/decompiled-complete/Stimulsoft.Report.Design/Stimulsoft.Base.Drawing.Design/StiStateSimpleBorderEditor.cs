using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Design.Components;

namespace Stimulsoft.Base.Drawing.Design;

public class StiStateSimpleBorderEditor : UITypeEditor
{
	private class DoubleBufferedListBox : ListBox
	{
		private StiStateSimpleBorderEditor editor;

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

		public DoubleBufferedListBox(StiStateSimpleBorderEditor editor)
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

	private StiSimpleBorder currentBorder;

	private const int IndexFromDefaultState = 0;

	private const int IndexFromCurrentState = 1;

	private string LocFromDefaultState => Loc.GetReport("FromDefaultState");

	private string LocFromCurrentState => Loc.GetReport("FromCurrentState");

	private void ListBox_DrawItem(object sender, DrawItemEventArgs e)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Invalid comparison between Unknown and I4
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Expected O, but got Unknown
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		Rectangle bounds = e.Bounds;
		bounds.Width--;
		ListBox val = (ListBox)((sender is ListBox) ? sender : null);
		object obj = val.Items[e.Index];
		Graphics graphics = e.Graphics;
		string text = obj as string;
		bool flag = (e.State & 1) > 0;
		bool isHotLight = e.Index == hotTrackingIndex;
		Color foregroundColor = StiListBoxPainter.GetForegroundColor(flag, isHotLight, isFocused: true);
		Color backgroundColor = StiListBoxPainter.GetBackgroundColor(flag, isHotLight, isFocused: true);
		StiDrawing.FillRectangle(graphics, backgroundColor, bounds.X, bounds.Y, bounds.Width + 1, bounds.Height);
		if (flag && StiUX.IsDark)
		{
			StiDrawing.FillRectangle(graphics, StiUX.Accent.ChangeAlpha(50), bounds.X, bounds.Y, bounds.Width + 1, bounds.Height);
		}
		if (text != null)
		{
			Font val2 = new Font("Arial", 8f);
			try
			{
				Rectangle rectangle = bounds;
				rectangle.Inflate(-StiScale.I4, 0);
				TextFormatFlags val3 = (TextFormatFlags)4;
				TextRenderer.DrawText((IDeviceContext)(object)graphics, text, val2, rectangle, foregroundColor, val3);
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
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
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
		currentBorder = ((value != null) ? (value as StiSimpleBorder) : new StiSimpleBorder());
		int selectedIndex = ((value is StiSimpleBorder) ? 1 : 0);
		((ListBox)doubleBufferedListBox2).Items.Add((object)LocFromDefaultState);
		((ListBox)doubleBufferedListBox2).Items.Add((object)LocFromCurrentState);
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
		if (((ListControl)doubleBufferedListBox2).SelectedIndex == 0)
		{
			return null;
		}
		if (((ListControl)doubleBufferedListBox2).SelectedIndex == 1)
		{
			if (value is StiSimpleBorder)
			{
				return value;
			}
			return new StiSimpleBorder();
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
