using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Controls.Win.DotNetBar.Controls;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Ribbon.Styles;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Styles.Helpers;

namespace Stimulsoft.Report.Design.Styles;

public class StiCreateStyleBasedForm : Form
{
	private StiStyleBasedHelper.StyleInfo info;

	private StiReport report = new StiReport();

	private string newStyleName;

	private StiBaseStyle seletedStyle;

	private List<StiComponentStyleItem> galleryStylesItems = new List<StiComponentStyleItem>();

	private List<Control> allControls = new List<Control>();

	private StiRibbonStylesPopup.StiStylesPanel panel;

	private int columns;

	private CircularProgress progressOperation;

	private StiPopup popup;

	private IContainer components;

	private StiStyleItem buttonGalleryStyles;

	private ItemPanel panelStyles;

	private StiGroupLine glButtons;

	private Button btCancel;

	private Button btOk;

	private Label labelStyle;

	public StiBaseStyle ResultStyle { get; private set; }

	public StiCreateStyleBasedForm(StiStyleBasedHelper.StyleInfo info)
	{
		InitializeComponent();
		this.info = info;
		Localize();
		DoScaling();
		Build();
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("FormStyleDesigner", "CreateStyleOnBase");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok").Replace("&", string.Empty);
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel").Replace("&", string.Empty);
		((Control)labelStyle).Text = StiLocalization.Get("PropertyMain", "Default");
	}

	private void DoScaling()
	{
		buttonGalleryStyles.FixedSize = new Size(StiScale.XXI(150.0), StiScale.YYI(77.0));
		((Control)panelStyles).Size = new Size(StiScale.XXI(150.0), StiScale.YYI(77.0));
	}

	private void Build()
	{
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Expected O, but got Unknown
		string defaultStyle = info.GetDefaultStyle();
		int height = StiScale.YYI(60.0);
		foreach (StiBaseStyle style in info.Styles)
		{
			StiComponentStyleItem stiComponentStyleItem = new StiComponentStyleItem
			{
				Style = style,
				Width = StiScale.XXI(156.0),
				Height = height,
				MapIdent = "USA",
				Report = report,
				ProgressMode = StiProgressElementMode.Circle,
				IndicatorFontIcons = StiFontIcons.Circle,
				GaugeType = StiGaugeType.FullCircular,
				Text = style.Name,
				SelectedStyle = style
			};
			if (StiStyleBasedHelper.GetStyleName(style) == defaultStyle)
			{
				seletedStyle = style;
				stiComponentStyleItem.IsSelected = true;
				buttonGalleryStyles.SelectedStyle = seletedStyle;
			}
			galleryStylesItems.Add(stiComponentStyleItem);
		}
		if (buttonGalleryStyles.SelectedStyle == null && galleryStylesItems.Count > 0)
		{
			StiComponentStyleItem stiComponentStyleItem2 = galleryStylesItems[0];
			seletedStyle = stiComponentStyleItem2.Style;
			stiComponentStyleItem2.IsSelected = true;
			buttonGalleryStyles.SelectedStyle = seletedStyle;
		}
		columns = ((galleryStylesItems.Count < 3) ? 1 : ((galleryStylesItems.Count < 7) ? 2 : 3));
		int num = StiScale.XXI(2.0);
		int num2 = StiScale.YYI(2.0);
		int num3 = 0;
		int num4 = 0;
		for (int i = 0; i < galleryStylesItems.Count; i++)
		{
			StiComponentStyleItem stiComponentStyleItem3 = galleryStylesItems[i];
			if (num3 == columns)
			{
				num3 = 0;
				num = StiScale.XXI(2.0);
				num2 += num4;
			}
			StiComponentStyleButton stiComponentStyleButton = stiComponentStyleItem3.CreateButton(OnStyle_Click);
			((Control)stiComponentStyleButton).Location = new Point(num, num2);
			if (!stiComponentStyleItem3.IsDashboardAutoStyle && stiComponentStyleItem3.SelectedStyle != null)
			{
				stiComponentStyleItem3.InitImage(((Control)stiComponentStyleButton).ClientRectangle);
			}
			num4 = stiComponentStyleItem3.Height;
			allControls.Add((Control)(object)stiComponentStyleButton);
			num3++;
			num += StiScale.XXI(158.0);
		}
		StiRibbonStylesPopup.StiStylesPanel stiStylesPanel = new StiRibbonStylesPopup.StiStylesPanel();
		((Control)stiStylesPanel).BackColor = Color.White;
		panel = stiStylesPanel;
		((ScrollableControl)panel).Scroll += new ScrollEventHandler(Panel_Scroll);
		((Control)panel).Controls.AddRange(allControls.ToArray());
	}

	private void ShowStyles()
	{
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		Rectangle screenRectangle = buttonGalleryStyles.ScreenRectangle;
		int num = screenRectangle.Bottom + StiScale.I2;
		int num2 = CalculateSize();
		int num3 = Screen.PrimaryScreen.Bounds.Height - num - StiScale.YYI(30.0);
		int num4 = 0;
		if (num2 > num3)
		{
			((ScrollableControl)panel).AutoScroll = true;
			num2 = num3;
			num4 += 24;
		}
		((Control)panel).Width = StiScale.XXI(156 * columns + num4 + 4 + (columns - 1) * 2);
		((Control)panel).Height = num2;
		((Control)panel).Paint += new PaintEventHandler(Panel_Paint);
		if (popup == null)
		{
			popup = new StiPopup((Control)(object)panel);
			if (galleryStylesItems.Count > 15)
			{
				CircularProgress circularProgress = new CircularProgress();
				((Control)circularProgress).BackColor = Color.White;
				((Control)circularProgress).Margin = new Padding(6);
				circularProgress.ProgressBarType = eCircularProgressType.Donut;
				circularProgress.ProgressColor = Color.SteelBlue;
				((Control)circularProgress).Size = new Size(StiScale.XXI(52.0), StiScale.YYI(52.0));
				circularProgress.Style = eDotNetBarStyle.OfficeXP;
				progressOperation = circularProgress;
				progressOperation.BackgroundStyle.CornerType = eCornerType.Square;
				progressOperation.Value = 0;
				progressOperation.IsRunning = true;
				((Control)progressOperation).Location = new Point((((Control)panel).Width - ((Control)progressOperation).Width) / 2, (((Control)panel).Height - ((Control)progressOperation).Height) / 2);
				((Control)panel).Controls.Add((Control)(object)progressOperation);
			}
		}
		((ToolStripDropDown)popup).Show(new Point(screenRectangle.Left + screenRectangle.Width - ((Control)popup).Width, num));
	}

	private int CalculateSize()
	{
		int num = StiScale.YYI(2.0);
		int num2 = 0;
		int num3 = 0;
		foreach (StiComponentStyleItem galleryStylesItem in galleryStylesItems)
		{
			if (num2 == columns)
			{
				num2 = 0;
				num += num3;
			}
			num3 = galleryStylesItem.Height;
			num2++;
		}
		if (num2 > 0)
		{
			num += num3;
		}
		return num + StiScale.YYI(2.0);
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			((Form)this).Close();
		}
	}

	private void BtOk_Click(object sender, EventArgs e)
	{
		if (!string.IsNullOrEmpty(newStyleName))
		{
			info.SetDefaultStyle(newStyleName);
		}
		if (seletedStyle != null)
		{
			ResultStyle = info.GenerateStyle(seletedStyle);
		}
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void BtCancel_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void ButtonGalleryStyles_Click(object sender, EventArgs e)
	{
		ShowStyles();
	}

	private void Panel_Scroll(object sender, ScrollEventArgs e)
	{
		((Control)panel).Invalidate();
	}

	private void Panel_Paint(object sender, PaintEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		Panel val = (Panel)sender;
		Rectangle clientRectangle = ((Control)val).ClientRectangle;
		e.Graphics.FillRectangle(Brushes.White, clientRectangle);
		Pen val2 = new Pen(Color.FromArgb(255, 198, 198, 198));
		try
		{
			e.Graphics.DrawRectangle(val2, 0, 0, ((Control)val).Width - 1, ((Control)val).Height - 1);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private void OnStyle_Click(object sender, EventArgs e)
	{
		((Control)popup).Hide();
		StiComponentStyleButton stiComponentStyleButton = (StiComponentStyleButton)sender;
		seletedStyle = stiComponentStyleButton.SelectedStyle;
		newStyleName = StiStyleBasedHelper.GetStyleName(seletedStyle);
		buttonGalleryStyles.SelectedStyle = seletedStyle;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Expected O, but got Unknown
		glButtons = new StiGroupLine();
		btCancel = new Button();
		btOk = new Button();
		labelStyle = new Label();
		panelStyles = new ItemPanel();
		buttonGalleryStyles = new StiStyleItem();
		((Control)glButtons).SuspendLayout();
		((Control)panelStyles).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)glButtons).BackColor = Color.White;
		((Control)glButtons).Controls.Add((Control)(object)btCancel);
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = StiLinePosition.Top;
		glButtons.LineStyle = StiLineStyle.Dot;
		((Control)glButtons).Location = new Point(0, 101);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(450, 48);
		((Control)glButtons).TabIndex = 2;
		buttonGalleryStyles.FixedSize = new Size(160, 77);
		buttonGalleryStyles.GaugeType = null;
		buttonGalleryStyles.ImagePosition = eImagePosition.Top;
		buttonGalleryStyles.ProgressMode = StiProgressElementMode.Circle;
		buttonGalleryStyles.IndicatorFontIcons = StiFontIcons.Circle;
		buttonGalleryStyles.GaugeType = StiGaugeType.FullCircular;
		buttonGalleryStyles.IsDashboardAutoStyle = false;
		buttonGalleryStyles.IsOnRibbonBar = false;
		buttonGalleryStyles.IsRootItem = false;
		buttonGalleryStyles.IsShowMore = false;
		buttonGalleryStyles.IsOnRibbonBar = true;
		buttonGalleryStyles.ShowArrowIfNoSubItems = true;
		buttonGalleryStyles.MapIdent = "USA";
		buttonGalleryStyles.Name = "galleryStyles";
		buttonGalleryStyles.ProgressMode = StiProgressElementMode.Circle;
		buttonGalleryStyles.SubItemsExpandWidth = 14;
		buttonGalleryStyles.Click += ButtonGalleryStyles_Click;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		btCancel.DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(363, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 25);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btCancel).Click += BtCancel_Click;
		((Control)btOk).Anchor = (AnchorStyles)10;
		btOk.DialogResult = (DialogResult)1;
		((Control)btOk).Location = new Point(281, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 25);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += BtOk_Click;
		((Control)labelStyle).ForeColor = Color.DimGray;
		((Control)labelStyle).Location = new Point(12, 42);
		((Control)labelStyle).Name = "labelStyle";
		((Control)labelStyle).Size = new Size(120, 17);
		((Control)labelStyle).TabIndex = 9;
		((Control)labelStyle).Text = "Style";
		labelStyle.TextAlign = (ContentAlignment)64;
		panelStyles.Items.AddRange(new BaseItem[1] { buttonGalleryStyles });
		((Control)panelStyles).Location = new Point(165, 12);
		((Control)panelStyles).Name = "panelStyles";
		((Control)panelStyles).Size = new Size(160, 77);
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(450, 149);
		((Control)this).Controls.Add((Control)(object)panelStyles);
		((Control)this).Controls.Add((Control)(object)labelStyle);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Form)this).ShowIcon = false;
		((Form)this).KeyPreview = true;
		((Form)this).ShowInTaskbar = false;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)this).Name = "StiCreateStyleBasedForm";
		((Control)this).Text = "StiCreateStyleBasedForm";
		((Control)glButtons).ResumeLayout(false);
		((Control)panelStyles).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
