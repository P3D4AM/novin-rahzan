using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Gauge.Design;
using Stimulsoft.Report.Components.Gauge;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Design.Chart.Editor;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Design.Gauges.Editor;

[ToolboxItem(false)]
public class StiCloudGaugeStylesControl : UserControl
{
	public class StiStyleInfo
	{
		public Bitmap Bitmap;

		public StiGaugeStyleXF Style;

		public int X;

		public int Y;

		public int Width = StiScale.I(86);

		public int Height = StiScale.I(102);

		public string Text;

		public StiStyleInfo(Bitmap bitmap, StiGaugeStyleXF style, int x, int y)
		{
			Bitmap = bitmap;
			Style = style;
			X = x;
			Y = y;
			Text = style.ToString();
		}
	}

	private StiGauge gauge;

	private StiGaugePanel gaugePanel;

	private StiPropertyGrid propertyGrid;

	private List<StiStyleInfo> listStylesImages;

	private int stylesImagesSelectedIndex;

	private int stylesImagesMouseOverIndex;

	private List<StiGaugeStyleXF> gaugeStyle2013;

	private IStiGaugeStyle lastChartStyle;

	private Timer styleTimer;

	private IContainer components;

	public StiToolBar toolBar;

	private StiToolButton buttonAddGaugeStyle;

	private Panel pageStylesLeftPanel;

	private StiMouseScrollableControl panelStyles;

	public StiCloudGaugeStylesControl(StiCloudGaugeForm gaugeDesigner)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			gauge = gaugeDesigner.Gauge;
			gaugePanel = gaugeDesigner.GaugePanel;
			Localize();
			gaugeStyle2013 = new List<StiGaugeStyleXF>();
			FillDefaultStyles();
			styleTimer = new Timer
			{
				Interval = 70
			};
			styleTimer.Tick += styleTimerTick;
			BuildStyles();
			propertyGrid = gaugeDesigner.PropertyGrid;
			propertyGrid.SetStandardPropertyTabs();
			((PropertyGrid)propertyGrid).HelpVisible = false;
		}
		DoScaling();
	}

	private void styleTimerTick(object sender, EventArgs e)
	{
		styleTimer.Stop();
		if (lastChartStyle != null)
		{
			StiGaugeStyleXF stiGaugeStyleXF = (StiGaugeStyleXF)lastChartStyle;
			lastChartStyle = null;
			gauge.Style = stiGaugeStyleXF;
			if (stiGaugeStyleXF is StiCustomGaugeStyle stiCustomGaugeStyle)
			{
				gauge.CustomStyleName = stiCustomGaugeStyle.CustomCore.ReportStyleName;
			}
			else
			{
				gauge.CustomStyleName = null;
			}
			gauge.ApplyStyle(gauge.Style);
			gaugePanel.Gauge = gauge;
			((Control)gaugePanel).Invalidate();
		}
	}

	private void buttonStyleMouseLeave()
	{
		styleTimer.Stop();
		styleTimer.Start();
	}

	private void buttonStyleMouseEnter(StiStyleInfo info)
	{
		styleTimer.Stop();
		if (lastChartStyle == null)
		{
			lastChartStyle = gauge.Style;
		}
		StiGaugeStyleXF stiGaugeStyleXF = (StiGaugeStyleXF)info.Style.Clone();
		gauge.Style = stiGaugeStyleXF;
		if (stiGaugeStyleXF is StiCustomGaugeStyle stiCustomGaugeStyle)
		{
			gauge.CustomStyleName = stiCustomGaugeStyle.CustomCore.ReportGaugeStyle.Name;
		}
		else
		{
			gauge.CustomStyleName = null;
		}
		gauge.ApplyStyle(gauge.Style);
		gaugePanel.Gauge = gauge;
		((Control)gaugePanel).Invalidate();
	}

	private void Localize()
	{
		((Control)buttonAddGaugeStyle).Text = StiLocalization.Get("FormStyleDesigner", "title");
	}

	private void DoScaling()
	{
		((ButtonBase)buttonAddGaugeStyle).Image = StiImageResizer.GetImage(((ButtonBase)buttonAddGaugeStyle).Image);
	}

	private void FillDefaultStyles()
	{
		gaugeStyle2013.Clear();
		foreach (StiBaseStyle style in gauge.Report.Styles)
		{
			if (style is StiGaugeStyle)
			{
				StiCustomGaugeStyle item = new StiCustomGaugeStyle((StiGaugeStyle)style);
				gaugeStyle2013.Add(item);
			}
		}
		gaugeStyle2013.AddRange(StiOptions.Services.GaugeStyles);
	}

	internal void BuildStyles()
	{
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Expected O, but got Unknown
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Expected O, but got Unknown
		stylesImagesMouseOverIndex = 0;
		if (listStylesImages == null)
		{
			listStylesImages = new List<StiStyleInfo>();
		}
		if (listStylesImages.Count > 0)
		{
			return;
		}
		foreach (StiStyleInfo listStylesImage in listStylesImages)
		{
			((Image)listStylesImage.Bitmap).Dispose();
		}
		listStylesImages.Clear();
		StiGauge stiGauge = (StiGauge)gauge.Clone();
		if (stiGauge.Scales.Count > 0)
		{
			foreach (StiScaleBase scale in stiGauge.Scales)
			{
				if (scale is StiRadialScale)
				{
					((StiRadialScale)scale).Radius = 0.5f;
				}
				else if (scale is StiLinearScale)
				{
					((StiLinearScale)scale).RelativeHeight = 0.8f;
				}
				foreach (object item in scale.Items)
				{
					if (item is StiTickLabelBase)
					{
						((StiTickLabelBase)item).Font = new Font("Arial", 7f);
					}
				}
			}
		}
		stiGauge.ClientRectangle = new RectangleD(0.0, 0.0, 80.0, 80.0);
		int num = 0;
		int num2 = StiScale.I2;
		int num3 = StiScale.I(80);
		int num4 = StiScale.I(80);
		bool flag = false;
		int num5 = 0;
		List<StiGaugeStyleXF> list = gaugeStyle2013;
		int num6 = 0;
		foreach (StiGaugeStyleXF item2 in list)
		{
			StiGaugeStyleXF stiGaugeStyleXF = (StiGaugeStyleXF)(stiGauge.Style = (StiGaugeStyleXF)item2.Clone());
			if (stiGaugeStyleXF is StiCustomGaugeStyle stiCustomGaugeStyle)
			{
				stiGauge.CustomStyleName = stiCustomGaugeStyle.CustomCore.ReportGaugeStyle.Name;
			}
			else
			{
				stiGauge.CustomStyleName = null;
			}
			stiGauge.ApplyStyle(stiGaugeStyleXF);
			RectangleF rectangleF = new RectangleF(0f, 0f, num3, num4);
			Bitmap val = new Bitmap(num3, num4);
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				StiGaugeGdiPainter stiGaugeGdiPainter = new StiGaugeGdiPainter();
				stiGaugeGdiPainter.DrawGauge(val2, stiGauge, new RectangleF(0f, 0f, rectangleF.Width - (float)StiScale.I10, rectangleF.Height - (float)StiScale.I10), 0.5f);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			listStylesImages.Add(new StiStyleInfo(val, item2, num, num2));
			if (!flag && stiGaugeStyleXF != null && stiGaugeStyleXF.CompareGaugeStyle(item2))
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
		panelStyles.ScrollHeight = num2;
	}

	internal void CheckPropertyGridValue()
	{
		((PropertyGrid)propertyGrid).SelectedObject = null;
	}

	private void buttonAddGaugeStyle_Click(object sender, EventArgs e)
	{
		StiDesignerControl designer = (StiDesignerControl)gauge.Report.Designer.DesignerControl;
		StiActions.Style.InvokeStyleDesigner(designer);
		FillDefaultStyles();
		BuildStyles();
	}

	private void panelStyles_Paint(object sender, PaintEventArgs e)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		if (listStylesImages == null || listStylesImages.Count == 0)
		{
			return;
		}
		Graphics graphics = e.Graphics;
		int num = 0;
		Font val = new Font("Arial", 8f);
		try
		{
			StringFormat val2 = new StringFormat();
			try
			{
				val2.Alignment = (StringAlignment)1;
				foreach (StiStyleInfo listStylesImage in listStylesImages)
				{
					Rectangle rectangle = new Rectangle(listStylesImage.X, listStylesImage.Y - ((ScrollBar)panelStyles.VScrollBar).Value, listStylesImage.Width, listStylesImage.Height);
					if (num == stylesImagesSelectedIndex)
					{
						SolidBrush val3 = new SolidBrush(Color.FromArgb(212, 224, 241));
						try
						{
							graphics.FillRectangle((Brush)(object)val3, rectangle);
						}
						finally
						{
							((IDisposable)val3)?.Dispose();
						}
						Pen val4 = new Pen(Color.FromArgb(43, 87, 154));
						try
						{
							graphics.DrawRectangle(val4, rectangle);
						}
						finally
						{
							((IDisposable)val4)?.Dispose();
						}
					}
					else if (num == stylesImagesMouseOverIndex)
					{
						SolidBrush val5 = new SolidBrush(Color.FromArgb(212, 224, 241));
						try
						{
							graphics.FillRectangle((Brush)(object)val5, rectangle);
						}
						finally
						{
							((IDisposable)val5)?.Dispose();
						}
					}
					Rectangle rectangle2 = new Rectangle(rectangle.X + StiScale.I3, rectangle.Y + StiScale.I3, StiScale.I(80), StiScale.I(80));
					graphics.DrawImage((Image)(object)listStylesImage.Bitmap, rectangle2);
					RectangleF rectangleF = new RectangleF(listStylesImage.X + StiScale.I3, rectangle2.Bottom + StiScale.I3, StiScale.I(80), StiScale.I14);
					graphics.DrawString(listStylesImage.Text, val, Brushes.Black, rectangleF, val2);
					num++;
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
	}

	private void panelStyles_MouseDown(object sender, MouseEventArgs e)
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
				if (stylesImagesSelectedIndex != num)
				{
					styleTimer.Stop();
					lastChartStyle = null;
					stylesImagesSelectedIndex = num;
					((Control)panelStyles).Invalidate();
					StiGaugeStyleXF style = (StiGaugeStyleXF)listStylesImage.Style.Clone();
					gauge.Style = style;
					if (gauge.Style is StiCustomGaugeStyle stiCustomGaugeStyle)
					{
						gauge.CustomStyleName = stiCustomGaugeStyle.CustomCore.ReportGaugeStyle.Name;
					}
					else
					{
						gauge.CustomStyleName = null;
					}
					gauge.ApplyStyle(gauge.Style);
					gaugePanel.Gauge = gauge;
					((Control)gaugePanel).Invalidate();
				}
				break;
			}
			num++;
		}
	}

	private void panelStyles_MouseMove(object sender, MouseEventArgs e)
	{
		int num = 0;
		foreach (StiStyleInfo listStylesImage in listStylesImages)
		{
			if (new Rectangle(listStylesImage.X, listStylesImage.Y - ((ScrollBar)panelStyles.VScrollBar).Value, listStylesImage.Width, listStylesImage.Height).Contains(e.X, e.Y))
			{
				if (stylesImagesMouseOverIndex != num)
				{
					stylesImagesMouseOverIndex = num;
					buttonStyleMouseEnter(listStylesImage);
					((Control)panelStyles).Invalidate();
				}
				return;
			}
			num++;
		}
		if (stylesImagesMouseOverIndex != -1)
		{
			buttonStyleMouseLeave();
			stylesImagesMouseOverIndex = -1;
			((Control)panelStyles).Invalidate();
		}
	}

	private void panelStyles_MouseLeave(object sender, EventArgs e)
	{
		if (stylesImagesMouseOverIndex != -1)
		{
			buttonStyleMouseLeave();
			stylesImagesMouseOverIndex = -1;
			((Control)panelStyles).Invalidate();
		}
	}

	private void pageStylesLeftPanel_Paint(object sender, PaintEventArgs e)
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
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Expected O, but got Unknown
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Expected O, but got Unknown
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Expected O, but got Unknown
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Expected O, but got Unknown
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiCloudGaugeStylesControl));
		toolBar = new StiToolBar();
		buttonAddGaugeStyle = new StiToolButton();
		pageStylesLeftPanel = new Panel();
		panelStyles = new StiMouseScrollableControl();
		((Control)toolBar).SuspendLayout();
		((Control)pageStylesLeftPanel).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)toolBar).Controls.Add((Control)(object)buttonAddGaugeStyle);
		toolBar.ControlStyle = StiControlStyle.Office2013Blue;
		((Control)toolBar).Dock = (DockStyle)1;
		toolBar.LineStyle = StiToolBar.StiToolBarLineStyle.Bottom;
		((Control)toolBar).Location = new Point(0, 0);
		((Control)toolBar).Name = "toolBar";
		((Control)toolBar).Padding = new Padding(12, 3, 3, 3);
		((Control)toolBar).Size = new Size(500, 28);
		((Control)toolBar).TabIndex = 6;
		((Control)toolBar).Text = "stiToolBar1";
		((Control)buttonAddGaugeStyle).AutoSize = true;
		((Control)buttonAddGaugeStyle).Dock = (DockStyle)3;
		buttonAddGaugeStyle.Draw3DButton = false;
		buttonAddGaugeStyle.DropDownMenu = null;
		buttonAddGaugeStyle.HotDragDrop = false;
		((ButtonBase)buttonAddGaugeStyle).Image = (Image)componentResourceManager.GetObject("buttonAddGaugeStyle.Image");
		((Control)buttonAddGaugeStyle).Location = new Point(12, 3);
		((Control)buttonAddGaugeStyle).Name = "buttonAddGaugeStyle";
		((Control)buttonAddGaugeStyle).Size = new Size(103, 22);
		((Control)buttonAddGaugeStyle).TabIndex = 1;
		((Control)buttonAddGaugeStyle).Text = "Style Designer";
		((ButtonBase)buttonAddGaugeStyle).UseVisualStyleBackColor = false;
		((Control)buttonAddGaugeStyle).Click += buttonAddGaugeStyle_Click;
		((Control)pageStylesLeftPanel).Anchor = (AnchorStyles)7;
		((Control)pageStylesLeftPanel).BackColor = Color.White;
		((Control)pageStylesLeftPanel).Controls.Add((Control)(object)panelStyles);
		((Control)pageStylesLeftPanel).Location = new Point(0, 30);
		((Control)pageStylesLeftPanel).Name = "pageStylesLeftPanel";
		((Control)pageStylesLeftPanel).Size = new Size(205, 470);
		((Control)pageStylesLeftPanel).TabIndex = 7;
		((Control)pageStylesLeftPanel).Paint += new PaintEventHandler(pageStylesLeftPanel_Paint);
		((Control)panelStyles).Anchor = (AnchorStyles)15;
		((ScrollableControl)panelStyles).AutoScroll = true;
		((Control)panelStyles).BackColor = Color.White;
		((Control)panelStyles).Font = new Font("Arial", 8f);
		((Control)panelStyles).ForeColor = Color.Black;
		((Control)panelStyles).Location = new Point(0, 0);
		((Control)panelStyles).Name = "panelStyles";
		panelStyles.ScrollHeight = 0;
		panelStyles.ScrollWidth = 0;
		((Control)panelStyles).Size = new Size(200, 470);
		((Control)panelStyles).TabIndex = 0;
		((Control)panelStyles).Paint += new PaintEventHandler(panelStyles_Paint);
		((Control)panelStyles).MouseDown += new MouseEventHandler(panelStyles_MouseDown);
		((Control)panelStyles).MouseLeave += panelStyles_MouseLeave;
		((Control)panelStyles).MouseMove += new MouseEventHandler(panelStyles_MouseMove);
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)pageStylesLeftPanel);
		((Control)this).Controls.Add((Control)(object)toolBar);
		((Control)this).Name = "StiCloudGaugeStylesControl";
		((Control)this).Size = new Size(500, 500);
		((Control)toolBar).ResumeLayout(false);
		((Control)toolBar).PerformLayout();
		((Control)pageStylesLeftPanel).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
