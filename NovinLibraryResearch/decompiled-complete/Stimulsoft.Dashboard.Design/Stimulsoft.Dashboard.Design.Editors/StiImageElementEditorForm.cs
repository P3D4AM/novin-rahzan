using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.Image;
using Stimulsoft.Report;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary.Design;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Helper;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Dashboard.Design.Editors;

public class StiImageElementEditorForm : StiElementEditorForm
{
	private StiPopup iconsPopup;

	private Panel panelIcons;

	private StiHintTextBox textBoxSearch;

	private IContainer components;

	private ToolTip toolTip;

	private StiPanelUX panelImage;

	private StiOperationButton buttonOpen;

	private StiOperationButton buttonRemove;

	private ImageList imageList1;

	private StiLabelUX labelHyperlink;

	private StiPanelBox panelHyperlink;

	private HintTextBox textBoxHyperlink;

	private StiOperationButton buttonEditHyperlink;

	private Timer timerRefreshHyperlink;

	private StiPanelUX panelOr;

	private StiLabelUX labelOr;

	private StiGroupPanelUX panelLine;

	private StiPanelUX panel1;

	private StiLabelUX labelIcon;

	private StiPanelUX panel2;

	private StiLabelUX labelOr1;

	private StiGroupPanelUX panelLine2;

	private StiColorBoxXF colorBoxIconColor;

	private StiPanelUX panelBoxIcon;

	private StiOperationButton buttonIconRemove;

	private StiOperationButton buttonIcon;

	internal StiImageElement ImageElement => base.Element as StiImageElement;

	public override bool AllowResize => false;

	public StiImageElementEditorForm()
		: this(null)
	{
	}

	public StiImageElementEditorForm(StiElement element)
		: base(element)
	{
		InitializeComponent();
		if (StiOptions.Appearance.ShowFormsHelp)
		{
			((Form)this).HelpButton = true;
		}
		textBoxHyperlink.Hint = "http://www.site.com/image.png";
		LocalizeInternal();
		DoScaling();
	}

	public override void Localize()
	{
		LocalizeInternal();
		((Control)panelImage).Invalidate();
	}

	private void LocalizeInternal()
	{
		((Control)this).Text = Loc.Get("Components", "StiImage");
		((Control)labelHyperlink).Text = Loc.Get("PropertyMain", "Hyperlink");
		StiLabelUX stiLabelUX = labelOr;
		string text2 = (((Control)labelOr1).Text = StiLocalization.Get("PropertyEnum", "StiFilterModeOr").ToLowerInvariant());
		((Control)stiLabelUX).Text = text2;
		((Control)labelIcon).Text = Loc.Get("PropertyMain", "Icon");
		toolTip.SetToolTip((Control)(object)buttonOpen, Loc.Get("Buttons", "Open"));
		toolTip.SetToolTip((Control)(object)buttonRemove, Loc.Get("Buttons", "Remove"));
	}

	private void DoScaling()
	{
		((ButtonBase)buttonEditHyperlink).Image = StiReportDesignImages.Actions.Edit();
		((ButtonBase)buttonOpen).Image = StiReportDesignImages.Actions.OpenFile();
		((ButtonBase)buttonRemove).Image = StiReportDesignImages.Actions.Remove();
		((ButtonBase)buttonIconRemove).Image = StiReportDesignImages.Actions.Remove();
		StiButtonsLocator.ArrangeButtons((Control)(object)panelHyperlink);
		StiButtonsLocator.ArrangeButtons((Control)(object)panelBoxIcon);
	}

	public override void ReadPropertiesFromElement()
	{
		base.LockRefresh = true;
		((Control)textBoxHyperlink).Text = ImageElement.ImageHyperlink;
		base.LockRefresh = false;
		CheckStatus();
		RefreshUiPanel();
	}

	private void CheckStatus()
	{
		((Control)buttonRemove).Enabled = ImageElement.Image != null;
	}

	private void RefreshImageAndStatus()
	{
		((Control)panelImage).Invalidate();
		RefreshElementView();
		CheckStatus();
	}

	private void BuildIconControls()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		iconsPopup = new StiPopup((Control)(object)GetIconPanel());
		((Control)panelBoxIcon).Paint += new PaintEventHandler(PanelBoxIcon_Paint);
		colorBoxIconColor.SelectedColor = ImageElement.IconColor;
		((Control)buttonIconRemove).Visible = ImageElement.Icon.HasValue;
	}

	private void DrawBoxIcon()
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		FontFamily fontFamilyIcons = StiFontIconsHelper.GetFontFamilyIcons();
		Graphics val = ((Control)panelBoxIcon).CreateGraphics();
		val.TextRenderingHint = (TextRenderingHint)3;
		StiDrawing.FillRectangle(val, StiUX.Background, new Rectangle(StiScale.I2, StiScale.I4, ((Control)panelBoxIcon).Width - StiScale.I4, ((Control)panelBoxIcon).Height - StiScale.I6));
		if (ImageElement.Icon.HasValue)
		{
			SolidBrush val2 = new SolidBrush(ImageElement.IconColor);
			try
			{
				Font val3 = new Font(fontFamilyIcons, 13f);
				try
				{
					StringFormat val4 = new StringFormat();
					try
					{
						val.DrawString(StiFontIconsHelper.GetContent(ImageElement.Icon.GetValueOrDefault()), val3, (Brush)(object)val2, (RectangleF)new Rectangle(StiScale.I2, StiScale.I4, ((Control)panelBoxIcon).Width, ((Control)panelBoxIcon).Height), val4);
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
		val.DrawRectangle(SystemPens.ControlDark, 0, 0, ((Control)panelBoxIcon).Width - 1, ((Control)panelBoxIcon).Height - 1);
	}

	private Panel GetIconPanel()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Expected O, but got Unknown
		Panel val = new Panel
		{
			Width = StiScale.XXI(274.0),
			Height = StiScale.YYI(342.0),
			BackColor = StiUX.Background,
			Padding = new Padding(StiScale.I1)
		};
		((Control)val).Paint += new PaintEventHandler(StiIconControlHelper.PanelRootPainter);
		Panel val2 = new Panel
		{
			Dock = (DockStyle)1,
			Size = new Size(StiScale.XXI(150.0), StiScale.YYI(28.0)),
			Padding = new Padding(StiScale.I10, StiScale.I6, StiScale.I10, StiScale.I6),
			BackColor = StiUX.Background
		};
		StiHintTextBox stiHintTextBox = new StiHintTextBox();
		((Control)stiHintTextBox).Dock = (DockStyle)5;
		stiHintTextBox.AllowDrawBorder = false;
		stiHintTextBox.Hint = StiLocalization.Get("FormViewer", "Find");
		((Control)stiHintTextBox).Font = new Font("Arial", 10f);
		textBoxSearch = stiHintTextBox;
		((Control)textBoxSearch).TextChanged += TextBoxSearch_TextChanged;
		((Control)val2).Controls.Add((Control)(object)textBoxSearch);
		panelIcons = new Panel
		{
			AutoScroll = true,
			Dock = (DockStyle)5
		};
		((Control)val).Controls.Add((Control)(object)panelIcons);
		((Control)val).Controls.Add((Control)(object)val2);
		BuildIconPanel();
		return val;
	}

	private void BuildIconPanel(string filterName = null)
	{
		((Control)panelIcons).Controls.Clear();
		StiFontIconSelectionControl stiFontIconSelectionControl = new StiFontIconSelectionControl(ImageElement.Icon, ImageElement.IconColor);
		((Control)stiFontIconSelectionControl).Location = new Point(StiScale.I2, StiScale.I2);
		((Control)stiFontIconSelectionControl).Width = ((Control)panelIcons).Width;
		((Control)stiFontIconSelectionControl).Anchor = (AnchorStyles)13;
		stiFontIconSelectionControl.TextFilter = filterName;
		StiFontIconSelectionControl stiFontIconSelectionControl2 = stiFontIconSelectionControl;
		stiFontIconSelectionControl2.ValueChanged += FontIconSelectionControl_ValueChanged;
		((Control)panelIcons).Controls.Add((Control)(object)stiFontIconSelectionControl2);
	}

	private void CheckColorIcons()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		foreach (object item in (ArrangedElementCollection)((Control)(Panel)iconsPopup.Content).Controls)
		{
			if (item is StiFontIconButton stiFontIconButton)
			{
				((Control)stiFontIconButton).ForeColor = ImageElement.IconColor;
			}
		}
	}

	protected override void OnGlobalElementChanged(object sender, EventArgs e)
	{
		if (sender == base.Element)
		{
			ReadPropertiesFromElement();
		}
	}

	private void ButtonRemove_Click(object sender, EventArgs e)
	{
		ImageElement.Image = null;
		RefreshImageAndStatus();
	}

	private void ButtonOpen_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Invalid comparison between Unknown and I4
		OpenFileDialog val = new OpenFileDialog();
		try
		{
			((FileDialog)val).Filter = StiFileDialogHelper.GetImageFilter();
			((FileDialog)val).RestoreDirectory = true;
			if ((int)((CommonDialog)val).ShowDialog() != 1)
			{
				return;
			}
			try
			{
				if (StiMaximumResourceSizeMessageBox.Check((Form)(object)this, ((FileDialog)val).FileName))
				{
					ImageElement.Image = StiImageHelper.TryBytesFromFile(((FileDialog)val).FileName);
					RefreshImageAndStatus();
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void PanelImage_Paint(object sender, PaintEventArgs e)
	{
		StiImageDialogPainter.Paint((Control)(object)panelImage, e.Graphics, ImageElement.Image, drawDragDrop: true, drawBorder: true, isImage: true);
	}

	private void PanelImage_DoubleClick(object sender, EventArgs e)
	{
		ButtonOpen_Click(sender, e);
	}

	private void TextBoxHyperlink_TextChanged(object sender, EventArgs e)
	{
		ImageElement.ImageHyperlink = ((Control)textBoxHyperlink).Text;
		timerRefreshHyperlink.Stop();
		timerRefreshHyperlink.Start();
	}

	private void ButtonEditHyperlink_Click(object sender, EventArgs e)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Invalid comparison between Unknown and I4
		string text = ((Control)textBoxHyperlink).Text;
		text = StiExpressionPacker.PackExpression(text, base.Element.Report.Designer, useBraces: false);
		DialogResult val = StiTextEditor.EditText(base.Element.Report, ref text, StiCodeEditorMode.EditorDashboardExpression);
		if ((int)val == 1)
		{
			((Control)textBoxHyperlink).Text = StiExpressionPacker.UnPackExpression(text, base.Element.Report.Designer, useBraces: false);
			ImageElement.ImageHyperlink = ((Control)textBoxHyperlink).Text;
			timerRefreshHyperlink.Stop();
			timerRefreshHyperlink.Start();
		}
		((Control)textBoxHyperlink).Focus();
		((TextBoxBase)textBoxHyperlink).Select(0, 0);
	}

	private void TimerRefreshHyperlink_Tick(object sender, EventArgs e)
	{
		timerRefreshHyperlink.Stop();
		RefreshElementView();
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		((Control)this).OnSizeChanged(e);
		if (panelImage != null)
		{
			((Control)panelImage).Invalidate();
		}
	}

	protected override void OnClosed(EventArgs e)
	{
		((Form)this).OnClosed(e);
		timerRefreshHyperlink.Stop();
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		base.OnLoad(e);
		ReadPropertiesFromElement();
		if (!string.IsNullOrWhiteSpace(ImageElement.ImageHyperlink))
		{
			Timer timer = new Timer
			{
				Enabled = true,
				Interval = 300
			};
			timer.Tick += delegate
			{
				timer.Stop();
				((Component)(object)timer).Dispose();
				((Control)textBoxHyperlink).Focus();
				((TextBoxBase)textBoxHyperlink).Select(0, 0);
			};
		}
		BuildIconControls();
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/dashboards_image.htm");
	}

	private void FontIconSelectionControl_ValueChanged(object sender, EventArgs e)
	{
		ImageElement.Icon = ((StiFontIconSelectionControl)sender).SelectedIcon;
		DrawBoxIcon();
		((Control)iconsPopup).Hide();
		((Control)buttonIconRemove).Visible = ImageElement.Icon.HasValue;
		RefreshImageAndStatus();
	}

	private void TextBoxSearch_TextChanged(object sender, EventArgs e)
	{
		StiHintTextBox stiHintTextBox = sender as StiHintTextBox;
		string filterName = ((Control)stiHintTextBox).Text.ToLower();
		BuildIconPanel(filterName);
	}

	private void PanelBoxIcon_Paint(object sender, PaintEventArgs e)
	{
		DrawBoxIcon();
	}

	private void ButtonIcon_Click(object sender, EventArgs e)
	{
		CheckColorIcons();
		((ToolStripDropDown)iconsPopup).Show(new Point(((Control)panelBoxIcon).RectangleToScreen(Rectangle.Empty).Left, ((Control)buttonIcon).RectangleToScreen(Rectangle.Empty).Bottom + ((Control)buttonIcon).Height + StiScale.I2));
		((Control)textBoxSearch).Focus();
	}

	private void ColorBoxIconColor_SelectedColorChanged(object sender, EventArgs e)
	{
		ImageElement.IconColor = colorBoxIconColor.SelectedColor;
		BuildIconPanel();
		DrawBoxIcon();
		RefreshImageAndStatus();
	}

	private void ButtonIconRemove_Click(object sender, EventArgs e)
	{
		ImageElement.Icon = null;
		((Control)buttonIconRemove).Visible = false;
		RefreshImageAndStatus();
	}

	private void PanelImage_DragDrop(object sender, DragEventArgs e)
	{
		object data = e.Data.GetData(typeof(StiDictionaryControl));
		if (data != null && StiDragDropColumnHelper.IsImageElementDragged(Designer))
		{
			((Control)textBoxHyperlink).Text = StiDragDropColumnHelper.GetDraggedImageElement(Designer).ImageHyperlink;
			RefreshImageAndStatus();
			return;
		}
		string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
		if (array == null)
		{
			return;
		}
		foreach (string item in array.Where((string f) => f != null))
		{
			string ext = Path.GetExtension(item).ToLowerInvariant();
			if (StiResourceTypeHelper.IsImageType(ext))
			{
				if (StiMaximumResourceSizeMessageBox.Check((Form)(object)this, item))
				{
					ImageElement.Image = StiImageHelper.TryBytesFromFile(item);
					RefreshImageAndStatus();
				}
				break;
			}
		}
	}

	private void PanelImage_DragOver(object sender, DragEventArgs e)
	{
		object data = e.Data.GetData(typeof(StiDictionaryControl));
		if (data != null && StiDragDropColumnHelper.IsImageElementDragged(Designer))
		{
			e.Effect = (DragDropEffects)2;
		}
		else
		{
			if (!e.Data.GetDataPresent(DataFormats.FileDrop, false))
			{
				return;
			}
			string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
			if (array != null)
			{
				foreach (string item in array.Where((string f) => f != null))
				{
					string ext = Path.GetExtension(item).ToLowerInvariant();
					if (StiResourceTypeHelper.IsImageType(ext))
					{
						e.Effect = (DragDropEffects)(-2147483645);
						return;
					}
				}
			}
			e.Effect = (DragDropEffects)0;
		}
	}

	private void TextBoxHyperlink_DragOver(object sender, DragEventArgs e)
	{
		object data = e.Data.GetData(typeof(StiDictionaryControl));
		if (data != null && StiDragDropColumnHelper.IsImageElementDragged(Designer))
		{
			e.Effect = (DragDropEffects)2;
		}
		else
		{
			e.Effect = (DragDropEffects)0;
		}
	}

	private void TextBoxHyperlink_DragDrop(object sender, DragEventArgs e)
	{
		object data = e.Data.GetData(typeof(StiDictionaryControl));
		if (data != null && StiDragDropColumnHelper.IsImageElementDragged(Designer))
		{
			((Control)textBoxHyperlink).Text = StiDragDropColumnHelper.GetDraggedImageElement(Designer).ImageHyperlink;
			RefreshImageAndStatus();
		}
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
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Expected O, but got Unknown
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Expected O, but got Unknown
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Expected O, but got Unknown
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Expected O, but got Unknown
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Expected O, but got Unknown
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Expected O, but got Unknown
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0530: Unknown result type (might be due to invalid IL or missing references)
		//IL_0591: Unknown result type (might be due to invalid IL or missing references)
		//IL_059b: Expected O, but got Unknown
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b2: Expected O, but got Unknown
		//IL_05d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05da: Expected O, but got Unknown
		//IL_05fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0786: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0865: Unknown result type (might be due to invalid IL or missing references)
		//IL_0927: Unknown result type (might be due to invalid IL or missing references)
		//IL_0996: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab6: Expected O, but got Unknown
		//IL_0ad3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b69: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e7f: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiImageElementEditorForm));
		toolTip = new ToolTip(components);
		imageList1 = new ImageList(components);
		panelImage = new StiPanelUX();
		buttonOpen = new StiOperationButton();
		buttonRemove = new StiOperationButton();
		panelHyperlink = new StiPanelBox();
		textBoxHyperlink = new HintTextBox();
		buttonEditHyperlink = new StiOperationButton();
		labelHyperlink = new StiLabelUX();
		timerRefreshHyperlink = new Timer(components);
		panelOr = new StiPanelUX();
		labelOr = new StiLabelUX();
		panelLine = new StiGroupPanelUX();
		panel1 = new StiPanelUX();
		colorBoxIconColor = new StiColorBoxXF();
		panelBoxIcon = new StiPanelUX();
		buttonIconRemove = new StiOperationButton();
		buttonIcon = new StiOperationButton();
		labelIcon = new StiLabelUX();
		panel2 = new StiPanelUX();
		labelOr1 = new StiLabelUX();
		panelLine2 = new StiGroupPanelUX();
		((Control)panelImage).SuspendLayout();
		((Control)panelHyperlink).SuspendLayout();
		((Control)panelOr).SuspendLayout();
		((Control)panel1).SuspendLayout();
		((Control)panelBoxIcon).SuspendLayout();
		((Control)panel2).SuspendLayout();
		((Control)this).SuspendLayout();
		imageList1.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageList1.ImageStream");
		imageList1.TransparentColor = Color.Transparent;
		imageList1.Images.SetKeyName(0, "File");
		imageList1.Images.SetKeyName(1, "Hyperlink");
		imageList1.Images.SetKeyName(2, "DataColumn");
		imageList1.Images.SetKeyName(3, "Expression");
		((Control)panelImage).AllowDrop = true;
		((Control)panelImage).Controls.Add((Control)(object)buttonOpen);
		((Control)panelImage).Controls.Add((Control)(object)buttonRemove);
		((Control)panelImage).Dock = (DockStyle)1;
		((Control)panelImage).Location = new Point(24, 23);
		((Control)panelImage).Margin = new Padding(4, 4, 4, 4);
		((Control)panelImage).Name = "panelImage";
		((Control)panelImage).Size = new Size(416, 387);
		((Control)panelImage).TabIndex = 0;
		((Control)panelImage).DragDrop += new DragEventHandler(PanelImage_DragDrop);
		((Control)panelImage).DragOver += new DragEventHandler(PanelImage_DragOver);
		((Control)panelImage).Paint += new PaintEventHandler(PanelImage_Paint);
		((Control)panelImage).DoubleClick += PanelImage_DoubleClick;
		((Control)buttonOpen).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonOpen).Image = (Image)componentResourceManager.GetObject("buttonOpen.Image");
		((Control)buttonOpen).Location = new Point(356, 10);
		((Control)buttonOpen).Margin = new Padding(4, 4, 4, 4);
		((Control)buttonOpen).Name = "buttonOpen";
		((Control)buttonOpen).Size = new Size(48, 44);
		((Control)buttonOpen).TabIndex = 0;
		((Control)buttonOpen).TabStop = false;
		((ButtonBase)buttonOpen).UseVisualStyleBackColor = true;
		((Control)buttonOpen).Click += ButtonOpen_Click;
		((Control)buttonRemove).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonRemove).Image = (Image)componentResourceManager.GetObject("buttonRemove.Image");
		((Control)buttonRemove).Location = new Point(356, 58);
		((Control)buttonRemove).Margin = new Padding(4, 4, 4, 4);
		((Control)buttonRemove).Name = "buttonRemove";
		((Control)buttonRemove).Size = new Size(48, 44);
		((Control)buttonRemove).TabIndex = 1;
		((Control)buttonRemove).TabStop = false;
		((ButtonBase)buttonRemove).UseVisualStyleBackColor = true;
		((Control)buttonRemove).Click += ButtonRemove_Click;
		((Control)panelHyperlink).Controls.Add((Control)(object)textBoxHyperlink);
		((Control)panelHyperlink).Controls.Add((Control)(object)buttonEditHyperlink);
		((Control)panelHyperlink).Dock = (DockStyle)2;
		((Control)panelHyperlink).Location = new Point(24, 662);
		((Control)panelHyperlink).Margin = new Padding(4, 4, 4, 4);
		((Control)panelHyperlink).Name = "panelHyperlink";
		((Control)panelHyperlink).Size = new Size(416, 48);
		((Control)panelHyperlink).TabIndex = 2;
		((Control)textBoxHyperlink).AllowDrop = true;
		((Control)textBoxHyperlink).Anchor = (AnchorStyles)13;
		((TextBoxBase)textBoxHyperlink).BorderStyle = (BorderStyle)0;
		textBoxHyperlink.Hint = null;
		((Control)textBoxHyperlink).Location = new Point(8, 12);
		((Control)textBoxHyperlink).Margin = new Padding(4, 4, 4, 4);
		((Control)textBoxHyperlink).Name = "textBoxHyperlink";
		((Control)textBoxHyperlink).Size = new Size(354, 24);
		((Control)textBoxHyperlink).TabIndex = 0;
		((Control)textBoxHyperlink).TextChanged += TextBoxHyperlink_TextChanged;
		((Control)textBoxHyperlink).DragDrop += new DragEventHandler(TextBoxHyperlink_DragDrop);
		((Control)textBoxHyperlink).DragOver += new DragEventHandler(TextBoxHyperlink_DragOver);
		((Control)buttonEditHyperlink).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonEditHyperlink).Image = (Image)componentResourceManager.GetObject("buttonEditHyperlink.Image");
		((Control)buttonEditHyperlink).Location = new Point(368, 2);
		((Control)buttonEditHyperlink).Margin = new Padding(4, 4, 4, 4);
		((Control)buttonEditHyperlink).Name = "buttonEditHyperlink";
		((Control)buttonEditHyperlink).Size = new Size(46, 44);
		((Control)buttonEditHyperlink).TabIndex = 1;
		((Control)buttonEditHyperlink).TabStop = false;
		((ButtonBase)buttonEditHyperlink).UseVisualStyleBackColor = true;
		((Control)buttonEditHyperlink).Click += ButtonEditHyperlink_Click;
		((Control)labelHyperlink).BackColor = Color.Transparent;
		((Control)labelHyperlink).Dock = (DockStyle)2;
		((Control)labelHyperlink).Location = new Point(24, 614);
		((Control)labelHyperlink).Margin = new Padding(4, 0, 4, 0);
		((Control)labelHyperlink).Name = "labelHyperlink";
		((Control)labelHyperlink).Size = new Size(416, 48);
		((Control)labelHyperlink).TabIndex = 1;
		((Control)labelHyperlink).Text = "Hyperlink";
		((Label)labelHyperlink).TextAlign = (ContentAlignment)16;
		timerRefreshHyperlink.Enabled = true;
		timerRefreshHyperlink.Interval = 2000;
		timerRefreshHyperlink.Tick += TimerRefreshHyperlink_Tick;
		((Control)panelOr).Controls.Add((Control)(object)labelOr);
		((Control)panelOr).Controls.Add((Control)(object)panelLine);
		((Control)panelOr).Dock = (DockStyle)1;
		((Control)panelOr).Location = new Point(24, 410);
		((Control)panelOr).Margin = new Padding(6, 6, 6, 6);
		((Control)panelOr).Name = "panelOr";
		((Control)panelOr).Size = new Size(416, 54);
		((Control)panelOr).TabIndex = 18;
		((Control)labelOr).AutoSize = true;
		((Control)labelOr).Location = new Point(192, 29);
		((Control)labelOr).Margin = new Padding(6, 0, 6, 0);
		((Control)labelOr).Name = "labelOr";
		((Control)labelOr).Size = new Size(31, 25);
		((Control)labelOr).TabIndex = 14;
		((Control)labelOr).Text = "or";
		((Control)panelLine).Anchor = (AnchorStyles)1;
		((Control)panelLine).Location = new Point(0, 42);
		((Control)panelLine).Margin = new Padding(6, 6, 6, 6);
		((Control)panelLine).Name = "panelLine";
		((Control)panelLine).Size = new Size(420, 2);
		((Control)panelLine).TabIndex = 13;
		((Control)panel1).Controls.Add((Control)(object)colorBoxIconColor);
		((Control)panel1).Controls.Add((Control)(object)panelBoxIcon);
		((Control)panel1).Controls.Add((Control)(object)labelIcon);
		((Control)panel1).Controls.Add((Control)(object)panel2);
		((Control)panel1).Dock = (DockStyle)1;
		((Control)panel1).Location = new Point(24, 464);
		((Control)panel1).Margin = new Padding(6, 6, 6, 6);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(416, 150);
		((Control)panel1).TabIndex = 19;
		((Control)colorBoxIconColor).AccessibleRole = (AccessibleRole)43;
		((Control)colorBoxIconColor).Location = new Point(220, 48);
		((Control)colorBoxIconColor).Margin = new Padding(4, 4, 4, 4);
		((Control)colorBoxIconColor).Name = "colorBoxIconColor";
		colorBoxIconColor.SelectedColor = Color.Transparent;
		colorBoxIconColor.ShowColorName = false;
		((Control)colorBoxIconColor).Size = new Size(196, 48);
		((Control)colorBoxIconColor).TabIndex = 22;
		colorBoxIconColor.SelectedColorChanged += ColorBoxIconColor_SelectedColorChanged;
		((Control)panelBoxIcon).Controls.Add((Control)(object)buttonIconRemove);
		((Control)panelBoxIcon).Controls.Add((Control)(object)buttonIcon);
		((Control)panelBoxIcon).Location = new Point(0, 48);
		((Control)panelBoxIcon).Margin = new Padding(2, 2, 2, 2);
		((Control)panelBoxIcon).Name = "panelBoxIcon";
		((Control)panelBoxIcon).Size = new Size(196, 48);
		((Control)panelBoxIcon).TabIndex = 21;
		((Control)buttonIconRemove).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonIconRemove).Image = (Image)componentResourceManager.GetObject("buttonIconRemove.Image");
		((Control)buttonIconRemove).Location = new Point(102, 2);
		((Control)buttonIconRemove).Margin = new Padding(2, 2, 2, 2);
		((Control)buttonIconRemove).Name = "buttonIconRemove";
		((Control)buttonIconRemove).Size = new Size(48, 44);
		((Control)buttonIconRemove).TabIndex = 2;
		((Control)buttonIconRemove).TabStop = false;
		((ButtonBase)buttonIconRemove).UseVisualStyleBackColor = true;
		((Control)buttonIconRemove).Click += ButtonIconRemove_Click;
		((Control)buttonIcon).Anchor = (AnchorStyles)9;
		((Control)buttonIcon).Location = new Point(148, 2);
		((Control)buttonIcon).Margin = new Padding(2, 2, 2, 2);
		((Control)buttonIcon).Name = "buttonIcon";
		((Control)buttonIcon).Size = new Size(46, 44);
		((Control)buttonIcon).TabIndex = 1;
		((Control)buttonIcon).TabStop = false;
		((ButtonBase)buttonIcon).UseVisualStyleBackColor = true;
		((Control)buttonIcon).Click += ButtonIcon_Click;
		((Control)labelIcon).BackColor = Color.Transparent;
		((Control)labelIcon).Dock = (DockStyle)1;
		((Control)labelIcon).Location = new Point(0, 0);
		((Control)labelIcon).Margin = new Padding(4, 0, 4, 0);
		((Control)labelIcon).Name = "labelIcon";
		((Control)labelIcon).Size = new Size(416, 48);
		((Control)labelIcon).TabIndex = 20;
		((Control)labelIcon).Text = "Icon";
		((Label)labelIcon).TextAlign = (ContentAlignment)16;
		((Control)panel2).Controls.Add((Control)(object)labelOr1);
		((Control)panel2).Controls.Add((Control)(object)panelLine2);
		((Control)panel2).Dock = (DockStyle)2;
		((Control)panel2).Location = new Point(0, 96);
		((Control)panel2).Margin = new Padding(6, 6, 6, 6);
		((Control)panel2).Name = "panel2";
		((Control)panel2).Size = new Size(416, 54);
		((Control)panel2).TabIndex = 19;
		((Control)labelOr1).AutoSize = true;
		((Control)labelOr1).Location = new Point(192, 29);
		((Control)labelOr1).Margin = new Padding(6, 0, 6, 0);
		((Control)labelOr1).Name = "labelOr1";
		((Control)labelOr1).Size = new Size(31, 25);
		((Control)labelOr1).TabIndex = 14;
		((Control)labelOr1).Text = "or";
		((Control)panelLine2).Anchor = (AnchorStyles)1;
		((Control)panelLine2).Location = new Point(0, 42);
		((Control)panelLine2).Margin = new Padding(6, 6, 6, 6);
		((Control)panelLine2).Name = "panelLine2";
		((Control)panelLine2).Size = new Size(420, 2);
		((Control)panelLine2).TabIndex = 13;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(464, 733);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Control)this).Controls.Add((Control)(object)panelOr);
		((Control)this).Controls.Add((Control)(object)labelHyperlink);
		((Control)this).Controls.Add((Control)(object)panelHyperlink);
		((Control)this).Controls.Add((Control)(object)panelImage);
		((Form)this).Margin = new Padding(6, 6, 6, 6);
		((Control)this).Name = "StiImageElementEditorForm";
		((Control)this).Padding = new Padding(24, 23, 24, 23);
		((Control)this).Text = "Image";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Control)panelImage).ResumeLayout(false);
		((Control)panelHyperlink).ResumeLayout(false);
		((Control)panelHyperlink).PerformLayout();
		((Control)panelOr).ResumeLayout(false);
		((Control)panelOr).PerformLayout();
		((Control)panel1).ResumeLayout(false);
		((Control)panelBoxIcon).ResumeLayout(false);
		((Control)panel2).ResumeLayout(false);
		((Control)panel2).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
