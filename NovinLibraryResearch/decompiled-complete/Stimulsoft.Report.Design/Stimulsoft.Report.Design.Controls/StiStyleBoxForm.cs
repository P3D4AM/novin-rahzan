using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;

namespace Stimulsoft.Report.Design.Controls;

public class StiStyleBoxForm : StiPopupForm
{
	internal IStiDesignerBase designer;

	internal StiStyleBox styleBox;

	private bool showEditStyles;

	private bool showReportStyles = true;

	private bool showCrossTabStyles = true;

	private bool showChartStyles = true;

	private List<StiBaseStyle> styles = new List<StiBaseStyle>();

	private IContainer components;

	private ListBox lbStyles;

	public StiStyleBoxForm(Control parentControl, IStiDesignerBase designer, bool showEditStyles, bool showReportStyles, bool showCrossTabStyles, bool showChartStyles)
		: base(parentControl)
	{
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		this.showEditStyles = showEditStyles;
		this.showReportStyles = showReportStyles;
		this.showCrossTabStyles = showCrossTabStyles;
		this.showChartStyles = showChartStyles;
		this.designer = designer;
		InitStyles();
		foreach (StiStyle style in StiOptions.Designer.Styles)
		{
			switch (style.Name)
			{
			case "Normal":
				style.Description = StiLocalization.Get("Report", "StyleNormal");
				break;
			case "Bad":
				style.Description = StiLocalization.Get("Report", "StyleBad");
				break;
			case "Good":
				style.Description = StiLocalization.Get("Report", "StyleGood");
				break;
			case "Neutral":
				style.Description = StiLocalization.Get("Report", "StyleNeutral");
				break;
			case "Warning":
				style.Description = StiLocalization.Get("Report", "StyleWarning");
				break;
			case "Note":
				style.Description = StiLocalization.Get("Report", "StyleNote");
				break;
			}
		}
		((Form)this).ShowIcon = false;
	}

	private void InitStyles()
	{
		styleBox = base.ParentControl as StiStyleBox;
		lbStyles.Items.Clear();
		if (styleBox.IsCrossTabStyle)
		{
			Color[] styleColors = StiOptions.Designer.CrossTab.StyleColors;
			Color[] array = styleColors;
			foreach (Color color in array)
			{
				lbStyles.Items.Add((object)StiLocalization.Get("PropertyColor", color.Name));
			}
			return;
		}
		lbStyles.Items.Add((object)string.Empty);
		if (showEditStyles)
		{
			lbStyles.Items.Add((object)"Edit Styles");
		}
		styles.Clear();
		foreach (StiBaseStyle style in designer.Report.Styles)
		{
			if (style is StiStyle && showReportStyles)
			{
				lbStyles.Items.Add((object)style);
				styles.Add(style);
			}
			else if (style is StiCrossTabStyle && showCrossTabStyles)
			{
				lbStyles.Items.Add((object)style);
				styles.Add(style);
			}
			else if (style is StiChartStyle && showChartStyles)
			{
				lbStyles.Items.Add((object)style);
				styles.Add(style);
			}
			if (style == styleBox.SelectedStyle)
			{
				lbStyles.SelectedItem = style;
			}
		}
		if (!((StiStyleBox)(object)base.ParentControl).ShowPredefinedStyles)
		{
			return;
		}
		foreach (StiStyle style2 in StiOptions.Designer.Styles)
		{
			lbStyles.Items.Add((object)style2);
			if (style2 == styleBox.SelectedStyle)
			{
				lbStyles.SelectedItem = style2;
			}
		}
	}

	private StiBaseStyle GetStyle(int index)
	{
		StiBaseStyle stiBaseStyle = null;
		if (styleBox.IsCrossTabStyle)
		{
			return stiBaseStyle;
		}
		index -= ((!showEditStyles) ? 1 : 2);
		if (index >= 0 && index < styles.Count)
		{
			stiBaseStyle = styles[index];
		}
		if (stiBaseStyle != null)
		{
			return stiBaseStyle;
		}
		index -= styles.Count;
		if (index >= 0 && index <= StiOptions.Designer.Styles.Count)
		{
			stiBaseStyle = StiOptions.Designer.Styles[index];
		}
		return stiBaseStyle;
	}

	private void ListBoxStyles_DrawItem(object sender, DrawItemEventArgs e)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Invalid comparison between Unknown and I4
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		Rectangle bounds = e.Bounds;
		bounds.Width--;
		if (styleBox.IsCrossTabStyle)
		{
			Color[] styleColors = StiOptions.Designer.CrossTab.StyleColors;
			StiControlPaint.DrawItemBackground(graphics, bounds, e.State);
			bounds.X++;
			bounds.Y++;
			bounds.Height -= 2;
			bounds.Width -= 2;
			Brush val = (Brush)new SolidBrush(StiColorUtils.Light(styleColors[e.Index], 50));
			try
			{
				graphics.FillRectangle(val, bounds);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			Rectangle rectangle = bounds;
			rectangle.Height /= 2;
			SolidBrush val2 = new SolidBrush(styleColors[e.Index]);
			try
			{
				graphics.FillRectangle((Brush)(object)val2, rectangle);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			bounds.Height--;
			bounds.Width--;
			Pen val3 = new Pen(Color.FromArgb(150, Color.Gray));
			try
			{
				val3.DashStyle = (DashStyle)1;
				graphics.DrawRectangle(val3, bounds);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			StringFormat val4 = new StringFormat();
			try
			{
				val4.Alignment = (StringAlignment)1;
				val4.LineAlignment = (StringAlignment)1;
				val4.FormatFlags = (StringFormatFlags)4096;
				val4.Trimming = (StringTrimming)3;
				graphics.DrawString(lbStyles.Items[e.Index].ToString(), ((Control)lbStyles).Font, SystemBrushes.ControlText, (RectangleF)bounds, val4);
				return;
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
		}
		StiBaseStyle style = GetStyle(e.Index);
		if (style != null)
		{
			StiBaseStyle.DrawStyle(graphics, bounds, style, e.State, fillBackground: true, paintImage: true);
		}
		else
		{
			if (!(lbStyles.Items[e.Index] is string text))
			{
				return;
			}
			string text2 = null;
			if (string.IsNullOrEmpty(text))
			{
				text2 = StiLocalization.Get("Report", "No");
			}
			else if (text == "Edit Styles")
			{
				text2 = StiLocalization.Get("Report", "EditStyles");
			}
			StiControlPaint.DrawItemBackground(graphics, bounds, e.State);
			StringFormat val5 = new StringFormat();
			try
			{
				val5.Alignment = (StringAlignment)1;
				val5.LineAlignment = (StringAlignment)1;
				val5.FormatFlags = (StringFormatFlags)4096;
				val5.Trimming = (StringTrimming)3;
				if ((int)base.ParentControl.RightToLeft == 1)
				{
					val5.FormatFlags = (StringFormatFlags)(val5.FormatFlags | 1);
				}
				graphics.DrawString(text2, ((Control)lbStyles).Font, SystemBrushes.ControlText, (RectangleF)bounds, val5);
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
		}
	}

	private void ListBoxStyles_MeasureItem(object sender, MeasureItemEventArgs e)
	{
		if (styleBox.IsCrossTabStyle)
		{
			e.ItemHeight = StiStyleBox.OneStyleHeight;
		}
		else if (e.Index >= 1)
		{
			e.ItemHeight = StiStyleBox.OneStyleHeight;
		}
		else if (((StiStyleBox)(object)base.ParentControl).ShowNone)
		{
			e.ItemHeight = StiStyleBox.OneStyleHeight;
		}
		else
		{
			e.ItemHeight = 0;
		}
	}

	private void ListBoxStyles_MouseUp(object sender, MouseEventArgs e)
	{
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		styleBox.SelectedIndex = ((ListControl)lbStyles).SelectedIndex;
		StiBaseStyle style = GetStyle(((ListControl)lbStyles).SelectedIndex);
		if (style != null)
		{
			styleBox.SelectedStyle = GetStyle(((ListControl)lbStyles).SelectedIndex) as StiStyle;
			ClosePopupForm();
			return;
		}
		string text = lbStyles.Items[((ListControl)lbStyles).SelectedIndex] as string;
		if (string.IsNullOrEmpty(text))
		{
			styleBox.SelectedStyle = null;
			ClosePopupForm();
		}
		else if (text == "Edit Styles")
		{
			IStiDesignerBase stiDesignerBase = designer;
			ClosePopupForm();
			StiStyleDesigner stiStyleDesigner = new StiStyleDesigner(stiDesignerBase.DesignerControl as StiDesignerControl, stiDesignerBase.Report);
			try
			{
				designer?.AddOwnedForm((Form)(object)stiStyleDesigner);
				((Form)stiStyleDesigner).ShowDialog();
				styleBox.InvokeEditStylesInvoked(styleBox, EventArgs.Empty);
				((Control)styleBox).Invalidate();
			}
			finally
			{
				((IDisposable)stiStyleDesigner)?.Dispose();
			}
			if (stiDesignerBase != null)
			{
				stiDesignerBase.Refresh();
				stiDesignerBase.Report.ApplyStyles();
			}
		}
	}

	private void ListBoxStyles_MouseMove(object sender, MouseEventArgs e)
	{
		Point pt = ((Control)lbStyles).PointToClient(Cursor.Position);
		for (int i = 0; i < lbStyles.Items.Count; i++)
		{
			if (lbStyles.GetItemRectangle(i).Contains(pt))
			{
				((ListControl)lbStyles).SelectedIndex = i;
				break;
			}
		}
	}

	private void This_Paint(object sender, PaintEventArgs e)
	{
		e.Graphics.DrawRectangle(SystemPens.ControlDark, 0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Expected O, but got Unknown
		lbStyles = new ListBox();
		((Control)this).SuspendLayout();
		((Control)lbStyles).BackColor = SystemColors.Window;
		lbStyles.BorderStyle = (BorderStyle)0;
		((Control)lbStyles).Dock = (DockStyle)5;
		lbStyles.DrawMode = (DrawMode)2;
		lbStyles.IntegralHeight = false;
		((Control)lbStyles).Location = new Point(1, 1);
		((Control)lbStyles).Name = "lbStyles";
		((Control)lbStyles).Size = new Size(238, 230);
		((Control)lbStyles).TabIndex = 0;
		lbStyles.MeasureItem += new MeasureItemEventHandler(ListBoxStyles_MeasureItem);
		((Control)lbStyles).MouseUp += new MouseEventHandler(ListBoxStyles_MouseUp);
		((Control)lbStyles).MouseMove += new MouseEventHandler(ListBoxStyles_MouseMove);
		lbStyles.DrawItem += new DrawItemEventHandler(ListBoxStyles_DrawItem);
		((Form)this).AutoScaleBaseSize = new Size(5, 13);
		((Form)this).ClientSize = new Size(240, 232);
		((Control)this).Controls.AddRange((Control[])(object)new Control[1] { (Control)lbStyles });
		((ScrollableControl)this).DockPadding.All = 1;
		((Control)this).Name = "StiStyleBoxForm";
		((Control)this).Text = "PenStyleButtonForm";
		((Control)this).Paint += new PaintEventHandler(This_Paint);
		((Control)this).ResumeLayout(false);
	}
}
