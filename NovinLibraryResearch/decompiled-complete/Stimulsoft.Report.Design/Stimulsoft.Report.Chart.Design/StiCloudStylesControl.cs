using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Controls;

namespace Stimulsoft.Report.Chart.Design;

[ToolboxItem(false)]
public class StiCloudStylesControl : UserControl
{
	private class StiStyleInfo
	{
		public Bitmap Bitmap { get; set; }

		public StiChartStyle Style { get; set; }

		public StiCloudStylesControl StylesControl { get; set; }

		public int X { get; set; }

		public int Y { get; set; }

		public int Width { get; set; } = StiScale.I(86);


		public int Height { get; set; } = StiScale.I(102);


		public string Text { get; set; }

		public virtual void Draw(Graphics g, Font font, int id)
		{
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Expected O, but got Unknown
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Expected O, but got Unknown
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Expected O, but got Unknown
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Expected O, but got Unknown
			Rectangle rectangle = new Rectangle(X, Y - ((ScrollBar)StylesControl.panelStyles.VScrollBar).Value, Width, Height);
			if (id == StylesControl.stylesImagesSelectedIndex)
			{
				SolidBrush val = new SolidBrush(Color.FromArgb(212, 224, 241));
				try
				{
					g.FillRectangle((Brush)(object)val, rectangle);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
				Pen val2 = new Pen(Color.FromArgb(43, 87, 154));
				try
				{
					g.DrawRectangle(val2, rectangle);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			else if (id == StylesControl.stylesImagesMouseOverIndex)
			{
				SolidBrush val3 = new SolidBrush(Color.FromArgb(212, 224, 241));
				try
				{
					g.FillRectangle((Brush)(object)val3, rectangle);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			Rectangle rectangle2 = new Rectangle(rectangle.X + StiScale.I3, rectangle.Y + StiScale.I3, StiScale.I(80), StiScale.I(80));
			g.DrawImage((Image)(object)Bitmap, rectangle2);
			RectangleF rectangleF = new RectangleF(X + StiScale.I3, rectangle2.Bottom + StiScale.I3, StiScale.I(80), StiScale.I(14));
			StringFormat val4 = new StringFormat();
			try
			{
				val4.Alignment = (StringAlignment)1;
				g.DrawString(Text, font, Brushes.Black, rectangleF, val4);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
		}

		public StiStyleInfo(StiCloudStylesControl stylesControl, Bitmap bitmap, StiChartStyle style, int x, int y)
		{
			StylesControl = stylesControl;
			Bitmap = bitmap;
			Style = style;
			X = x;
			Y = y;
			if (style != null)
			{
				Text = ((style is StiCustomStyle) ? ((StiCustomStyleCoreXF)((StiCustomStyle)style).Core).ReportStyleName : style.ServiceName);
			}
		}
	}

	private class StiShowMoreButton : StiStyleInfo
	{
		public override void Draw(Graphics g, Font font, int id)
		{
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Expected O, but got Unknown
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Expected O, but got Unknown
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Expected O, but got Unknown
			Rectangle rectangle = new Rectangle(base.X, base.Y - ((ScrollBar)base.StylesControl.panelStyles.VScrollBar).Value, base.Width, base.Height);
			if (id == base.StylesControl.stylesImagesMouseOverIndex)
			{
				SolidBrush val = new SolidBrush(Color.FromArgb(212, 224, 241));
				try
				{
					g.FillRectangle((Brush)(object)val, rectangle);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			Rectangle rectangle2 = new Rectangle(rectangle.X + StiScale.I5, rectangle.Y + StiScale.I5, rectangle.Width - StiScale.I10, rectangle.Height - StiScale.I10);
			Pen val2 = new Pen(Color.FromArgb(255, 190, 190, 190));
			try
			{
				g.DrawRectangle(val2, rectangle2);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			StringFormat val3 = new StringFormat();
			try
			{
				val3.Alignment = (StringAlignment)1;
				val3.LineAlignment = (StringAlignment)1;
				g.DrawString(base.Text, font, Brushes.Black, (RectangleF)rectangle2, val3);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}

		public StiShowMoreButton(StiCloudStylesControl stylesControl, string text, int x, int y, int width)
			: base(stylesControl, null, null, x, y)
		{
			base.Text = text;
			base.Width = width;
			base.Height = StiScale.I(40);
		}
	}

	private IStiChartStyle lastChartStyle;

	private Timer styleTimer;

	private List<StiChartStyle> chartStyles = new List<StiChartStyle>();

	private List<StiChartStyle> defaultStyles = new List<StiChartStyle>();

	private StiCloudChartForm chartWindow;

	private Type lastAreaType;

	private List<StiStyleInfo> listStylesImages = new List<StiStyleInfo>();

	private int stylesImagesSelectedIndex;

	private int stylesImagesMouseOverIndex;

	private bool showMoreButton = true;

	private IContainer components;

	public StiToolBar toolBar;

	private StiToolButton buttonAddChartStyle;

	private Panel pageStylesLeftPanel;

	private StiMouseScrollableControl panelStyles;

	public StiCloudStylesControl(StiCloudChartForm chartWindow)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		this.chartWindow = chartWindow;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			FillDefaultStyles();
			FillChartStyles();
			styleTimer = new Timer
			{
				Interval = 70
			};
			styleTimer.Tick += StyleTimer_Tick;
			Localize();
			BuildStyles();
			DoScaling();
		}
	}

	private void Localize()
	{
		((Control)buttonAddChartStyle).Text = StiLocalization.Get("FormStyleDesigner", "title");
	}

	private void DoScaling()
	{
		((ButtonBase)buttonAddChartStyle).Image = StiImageResizer.GetImage(((ButtonBase)buttonAddChartStyle).Image);
	}

	private void FillDefaultStyles()
	{
		defaultStyles.Clear();
		foreach (StiBaseStyle style in chartWindow.Chart.Report.Styles)
		{
			if (style is Stimulsoft.Report.StiChartStyle)
			{
				StiCustomStyle stiCustomStyle = new StiCustomStyle(style.Name);
				stiCustomStyle.CustomCore.ReportChartStyle = style as Stimulsoft.Report.StiChartStyle;
				defaultStyles.Add(stiCustomStyle);
			}
		}
		if (defaultStyles.Count > 0)
		{
			defaultStyles = defaultStyles.OrderBy((StiChartStyle x) => x.ServiceName).ToList();
		}
		defaultStyles.Add(new StiStyle35());
		defaultStyles.Add(new StiStyle34());
		defaultStyles.Add(new StiStyle33());
		defaultStyles.Add(new StiStyle32());
		defaultStyles.Add(new StiStyle31());
		defaultStyles.Add(new StiStyle30());
		defaultStyles.Add(new StiStyle29());
		defaultStyles.Add(new StiStyle27());
		defaultStyles.Add(new StiStyle26());
		defaultStyles.Add(new StiStyle25());
		defaultStyles.Add(new StiStyle24());
		defaultStyles.Add(new StiStyle21());
	}

	private void FillChartStyles()
	{
		if (chartStyles.Count != 0)
		{
			return;
		}
		bool flag = chartWindow.Chart.Style == null;
		foreach (StiChartStyle item in from s in StiOptions.Services.ChartStyles
			where s.ServiceEnabled
			select s into x
			orderby x.ServiceName descending
			select x)
		{
			if (!(item is StiStyle21) && !(item is StiStyle24) && !(item is StiStyle25) && !(item is StiStyle26) && !(item is StiStyle27) && !(item is StiStyle29) && !(item is StiStyle30) && !(item is StiStyle31) && !(item is StiStyle32) && !(item is StiStyle33) && !(item is StiStyle34) && !(item is StiStyle35))
			{
				if (!flag && ((StiChartStyle)chartWindow.Chart.Style).CompareChartStyle(item))
				{
					defaultStyles.Add(item);
					flag = true;
				}
				else
				{
					chartStyles.Add(item);
				}
			}
		}
	}

	internal void BuildStyles()
	{
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Expected O, but got Unknown
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Expected O, but got Unknown
		stylesImagesMouseOverIndex = 0;
		if (listStylesImages.Count > 0 && lastAreaType == chartWindow.Chart.Area.GetType())
		{
			return;
		}
		if (listStylesImages.Count > 0)
		{
			foreach (StiStyleInfo listStylesImage in listStylesImages)
			{
				if (listStylesImage.Bitmap != null)
				{
					((Image)listStylesImage.Bitmap).Dispose();
					listStylesImage.Bitmap = null;
				}
			}
			listStylesImages.Clear();
		}
		lastAreaType = chartWindow.Chart.Area.GetType();
		StiChart simpleChart = StiSimpleChartHelper.GetSimpleChart(chartWindow.Chart);
		int num = 0;
		int num2 = StiScale.I2;
		int num3 = StiScale.I(80);
		int num4 = StiScale.I(80);
		bool flag = false;
		int num5 = 0;
		int num6 = 0;
		foreach (StiChartStyle defaultStyle in defaultStyles)
		{
			simpleChart.Style = (StiChartStyle)defaultStyle.Clone();
			simpleChart.Core.ApplyStyle(simpleChart.Style);
			RectangleF rect = new RectangleF(0f, 0f, num3, num4);
			Bitmap val = new Bitmap(num3, num4);
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				simpleChart.DrawChart(val2, rect, useMargins: false, useBackground: false);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			listStylesImages.Add(new StiStyleInfo(this, val, defaultStyle, num, num2));
			if (!flag && chartWindow.Chart.Style != null && ((StiChartStyle)chartWindow.Chart.Style).CompareChartStyle(defaultStyle))
			{
				stylesImagesSelectedIndex = num5;
				flag = true;
			}
			if (num6 % 2 == 0)
			{
				num += StiScale.I(91);
			}
			else
			{
				num = 0;
				num2 += StiScale.I(107);
			}
			num6++;
			num5++;
		}
		if (showMoreButton)
		{
			int num7 = num2;
			int width;
			if (num == 0)
			{
				width = StiScale.I(177);
			}
			else
			{
				width = StiScale.I(86);
				num7 += StiScale.I(26);
			}
			StiShowMoreButton item = new StiShowMoreButton(this, StiLocalization.Get("Buttons", "ShowMore"), num, num7, width);
			listStylesImages.Add(item);
		}
		else if (chartStyles != null)
		{
			foreach (StiChartStyle chartStyle in chartStyles)
			{
				simpleChart.Style = (StiChartStyle)chartStyle.Clone();
				simpleChart.Core.ApplyStyle(simpleChart.Style);
				RectangleF rect2 = new RectangleF(0f, 0f, num3, num4);
				Bitmap val3 = new Bitmap(num3, num4);
				Graphics val4 = Graphics.FromImage((Image)(object)val3);
				try
				{
					simpleChart.DrawChart(val4, rect2, useMargins: false, useBackground: false);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
				listStylesImages.Add(new StiStyleInfo(this, val3, chartStyle, num, num2));
				if (!flag && chartWindow.Chart.Style != null && ((StiChartStyle)chartWindow.Chart.Style).CompareChartStyle(chartStyle))
				{
					stylesImagesSelectedIndex = num5;
					flag = true;
				}
				if (num6 % 2 == 0)
				{
					num += StiScale.I(91);
				}
				else
				{
					num = 0;
					num2 += StiScale.I(107);
				}
				num6++;
				num5++;
			}
		}
		int scrollHeight = StiScale.I(121) * (listStylesImages.Count / 2);
		panelStyles.ScrollHeight = scrollHeight;
		((Control)panelStyles).Invalidate();
	}

	internal void CheckPropertyGridValue()
	{
		((PropertyGrid)chartWindow.PropertyGrid).SelectedObject = null;
	}

	internal void Collect()
	{
		if (styleTimer != null)
		{
			styleTimer.Tick -= StyleTimer_Tick;
			((Component)(object)styleTimer).Dispose();
			styleTimer = null;
		}
		if (listStylesImages == null)
		{
			return;
		}
		foreach (StiStyleInfo listStylesImage in listStylesImages)
		{
			if (listStylesImage.Bitmap != null)
			{
				((Image)listStylesImage.Bitmap).Dispose();
				listStylesImage.Bitmap = null;
			}
		}
		listStylesImages.Clear();
		listStylesImages = null;
	}

	private void ButtonStyleMouseLeave()
	{
		if (styleTimer != null)
		{
			styleTimer.Stop();
			styleTimer.Start();
		}
	}

	private void ButtonStyleMouseEnter(StiStyleInfo info)
	{
		styleTimer.Stop();
		if (!(info is StiShowMoreButton))
		{
			if (lastChartStyle == null)
			{
				lastChartStyle = chartWindow.Chart.Style;
			}
			StiChartStyle stiChartStyle = (StiChartStyle)info.Style.Clone();
			chartWindow.Chart.Style = stiChartStyle;
			if (stiChartStyle is StiCustomStyle stiCustomStyle)
			{
				chartWindow.Chart.CustomStyleName = stiCustomStyle.CustomCore.ReportChartStyle.Name;
			}
			chartWindow.Chart.Core.ApplyStyle(chartWindow.Chart.Style);
			chartWindow.UpdateSample();
		}
	}

	private void StyleTimer_Tick(object sender, EventArgs e)
	{
		styleTimer.Stop();
		if (lastChartStyle != null)
		{
			StiChartStyle stiChartStyle = (StiChartStyle)lastChartStyle;
			lastChartStyle = null;
			chartWindow.Chart.Style = stiChartStyle;
			if (stiChartStyle is StiCustomStyle stiCustomStyle)
			{
				chartWindow.Chart.CustomStyleName = ((stiCustomStyle.CustomCore.ReportChartStyle != null) ? stiCustomStyle.CustomCore.ReportChartStyle.Name : null);
			}
			chartWindow.Chart.Core.ApplyStyle(chartWindow.Chart.Style);
			chartWindow.UpdateSample();
		}
	}

	private void ButtonAddChartStyle_Click(object sender, EventArgs e)
	{
		StiDesignerControl designer = (StiDesignerControl)chartWindow.Chart.Report.Designer.DesignerControl;
		StiActions.Style.InvokeStyleDesigner(designer);
		listStylesImages.Clear();
		FillDefaultStyles();
		BuildStyles();
	}

	private void PanelStyles_Paint(object sender, PaintEventArgs e)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		if (listStylesImages == null || listStylesImages.Count == 0)
		{
			return;
		}
		Graphics graphics = e.Graphics;
		int num = 0;
		Font val = new Font("Arial", 8f);
		try
		{
			foreach (StiStyleInfo listStylesImage in listStylesImages)
			{
				listStylesImage.Draw(graphics, val, num);
				num++;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void PanelStyles_MouseDown(object sender, MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button != 1048576)
		{
			return;
		}
		int num = 0;
		foreach (StiStyleInfo listStylesImage in listStylesImages)
		{
			if (new Rectangle(listStylesImage.X, listStylesImage.Y - ((ScrollBar)panelStyles.VScrollBar).Value, listStylesImage.Width, listStylesImage.Height).Contains(e.X, e.Y))
			{
				if (listStylesImage is StiShowMoreButton)
				{
					showMoreButton = false;
					if (listStylesImages.Count > 0)
					{
						foreach (StiStyleInfo listStylesImage2 in listStylesImages)
						{
							if (listStylesImage2.Bitmap != null)
							{
								((Image)listStylesImage2.Bitmap).Dispose();
								listStylesImage2.Bitmap = null;
							}
						}
						listStylesImages.Clear();
					}
					BuildStyles();
				}
				else if (stylesImagesSelectedIndex != num)
				{
					styleTimer.Stop();
					lastChartStyle = null;
					stylesImagesSelectedIndex = num;
					((Control)panelStyles).Invalidate();
					StiChartStyle style = (StiChartStyle)listStylesImage.Style.Clone();
					chartWindow.Chart.Style = style;
					if (chartWindow.Chart.Style is StiCustomStyle stiCustomStyle)
					{
						chartWindow.Chart.CustomStyleName = stiCustomStyle.CustomCore.ReportChartStyle.Name;
					}
					chartWindow.Chart.Core.ApplyStyle(chartWindow.Chart.Style);
					chartWindow.UpdateSample();
				}
				break;
			}
			num++;
		}
	}

	private void PanelStyles_MouseMove(object sender, MouseEventArgs e)
	{
		int num = 0;
		foreach (StiStyleInfo listStylesImage in listStylesImages)
		{
			if (new Rectangle(listStylesImage.X, listStylesImage.Y - ((ScrollBar)panelStyles.VScrollBar).Value, listStylesImage.Width, listStylesImage.Height).Contains(e.X, e.Y))
			{
				if (stylesImagesMouseOverIndex != num)
				{
					stylesImagesMouseOverIndex = num;
					ButtonStyleMouseEnter(listStylesImage);
					((Control)panelStyles).Invalidate();
				}
				return;
			}
			num++;
		}
		if (stylesImagesMouseOverIndex != -1)
		{
			ButtonStyleMouseLeave();
			stylesImagesMouseOverIndex = -1;
			((Control)panelStyles).Invalidate();
		}
	}

	private void PanelStyles_MouseLeave(object sender, EventArgs e)
	{
		if (stylesImagesMouseOverIndex != -1)
		{
			ButtonStyleMouseLeave();
			stylesImagesMouseOverIndex = -1;
			((Control)panelStyles).Invalidate();
		}
	}

	private void PanelStylesLeftPanel_Paint(object sender, PaintEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		Panel val = (Panel)sender;
		Pen val2 = new Pen(Color.FromArgb(170, 170, 170));
		try
		{
			val2.DashPattern = new float[2] { 1f, 2f };
			Point point = new Point(((Control)val).Width - 1, 0);
			Point point2 = new Point(((Control)val).Width - 1, ((Control)val).Height);
			e.Graphics.DrawLine(val2, point, point2);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Expected O, but got Unknown
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Expected O, but got Unknown
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Expected O, but got Unknown
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Expected O, but got Unknown
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiCloudStylesControl));
		toolBar = new StiToolBar();
		pageStylesLeftPanel = new Panel();
		panelStyles = new StiMouseScrollableControl();
		buttonAddChartStyle = new StiToolButton();
		((Control)toolBar).SuspendLayout();
		((Control)pageStylesLeftPanel).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)toolBar).Controls.Add((Control)(object)buttonAddChartStyle);
		toolBar.ControlStyle = StiControlStyle.Office2013Blue;
		((Control)toolBar).Dock = (DockStyle)1;
		toolBar.LineStyle = StiToolBar.StiToolBarLineStyle.Bottom;
		((Control)toolBar).Location = new Point(0, 0);
		((Control)toolBar).Name = "toolBar";
		((Control)toolBar).Padding = new Padding(12, 3, 3, 3);
		((Control)toolBar).Size = new Size(500, 28);
		((Control)toolBar).TabIndex = 5;
		((Control)toolBar).Text = "stiToolBar1";
		((Control)pageStylesLeftPanel).Anchor = (AnchorStyles)7;
		((Control)pageStylesLeftPanel).BackColor = Color.White;
		((Control)pageStylesLeftPanel).Controls.Add((Control)(object)panelStyles);
		((Control)pageStylesLeftPanel).Location = new Point(0, 30);
		((Control)pageStylesLeftPanel).Name = "pageStylesLeftPanel";
		((Control)pageStylesLeftPanel).Size = new Size(205, 470);
		((Control)pageStylesLeftPanel).TabIndex = 6;
		((Control)pageStylesLeftPanel).Paint += new PaintEventHandler(PanelStylesLeftPanel_Paint);
		((Control)panelStyles).Anchor = (AnchorStyles)15;
		((ScrollableControl)panelStyles).AutoScroll = false;
		((Control)panelStyles).BackColor = Color.White;
		((Control)panelStyles).Font = new Font("Arial", 8f);
		((Control)panelStyles).ForeColor = Color.Black;
		((Control)panelStyles).Location = new Point(0, 0);
		((Control)panelStyles).Name = "panelStyles";
		panelStyles.ScrollHeight = 0;
		panelStyles.ScrollWidth = 0;
		((Control)panelStyles).Size = new Size(200, 470);
		((Control)panelStyles).TabIndex = 0;
		((Control)panelStyles).Paint += new PaintEventHandler(PanelStyles_Paint);
		((Control)panelStyles).MouseDown += new MouseEventHandler(PanelStyles_MouseDown);
		((Control)panelStyles).MouseLeave += PanelStyles_MouseLeave;
		((Control)panelStyles).MouseMove += new MouseEventHandler(PanelStyles_MouseMove);
		((Control)buttonAddChartStyle).AutoSize = true;
		((Control)buttonAddChartStyle).Dock = (DockStyle)3;
		buttonAddChartStyle.Draw3DButton = false;
		buttonAddChartStyle.DropDownMenu = null;
		buttonAddChartStyle.HotDragDrop = false;
		((ButtonBase)buttonAddChartStyle).Image = (Image)componentResourceManager.GetObject("buttonAddChartStyle.Image");
		((Control)buttonAddChartStyle).Location = new Point(12, 3);
		((Control)buttonAddChartStyle).Name = "buttonAddChartStyle";
		((Control)buttonAddChartStyle).Size = new Size(103, 22);
		((Control)buttonAddChartStyle).TabIndex = 1;
		((Control)buttonAddChartStyle).Text = "Style Designer";
		((ButtonBase)buttonAddChartStyle).UseVisualStyleBackColor = false;
		((Control)buttonAddChartStyle).Click += ButtonAddChartStyle_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)pageStylesLeftPanel);
		((Control)this).Controls.Add((Control)(object)toolBar);
		((Control)this).Name = "StiCloudStylesControl";
		((Control)this).Size = new Size(500, 500);
		((Control)toolBar).ResumeLayout(false);
		((Control)toolBar).PerformLayout();
		((Control)pageStylesLeftPanel).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
