using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Design.Controls;

namespace Stimulsoft.Report.Components.Design;

[ToolboxItem(false)]
public class StiIconSetConditionZoneControl : StiZoneControl
{
	private ButtonX lastButton;

	private IContainer components;

	public StiTreeViewBox cbColumn;

	private Label labelColumn;

	private Label labelIconSet;

	public StiComboBox comboBoxType1;

	private Label labelType;

	public StiTextBox textBoxValue1;

	private Button buttonReverse;

	public StiComboBox comboBoxOperation1;

	private Label labelOperation;

	public ButtonX buttonIcon1;

	private Label labelIcon;

	private Label labelItem1;

	public Panel panelItems;

	public Panel panelItem1;

	public Panel panelItem5;

	public ButtonX buttonIcon5;

	private Label labelItem5;

	public StiTextBox textBoxValue5;

	public StiComboBox comboBoxOperation5;

	public StiComboBox comboBoxType5;

	public Panel panelItem4;

	public ButtonX buttonIcon4;

	private Label labelItem4;

	public StiTextBox textBoxValue4;

	public StiComboBox comboBoxOperation4;

	public StiComboBox comboBoxType4;

	public Panel panelItem3;

	public ButtonX buttonIcon3;

	private Label labelItem3;

	public StiTextBox textBoxValue3;

	public StiComboBox comboBoxOperation3;

	public StiComboBox comboBoxType3;

	public Panel panelItem2;

	public ButtonX buttonIcon2;

	private Label labelItem2;

	public StiTextBox textBoxValue2;

	public StiComboBox comboBoxOperation2;

	public StiComboBox comboBoxType2;

	private Label labelAlignment;

	public StiComboBox comboBoxAlignment;

	public ButtonX buttonIconSet;

	private Label labelValue;

	public StiIconSetConditionZoneControl()
	{
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			Localize();
			buttonIcon1.SubItems.Add(new ButtonItem());
			buttonIcon2.SubItems.Add(new ButtonItem());
			buttonIcon3.SubItems.Add(new ButtonItem());
			buttonIcon4.SubItems.Add(new ButtonItem());
			buttonIcon5.SubItems.Add(new ButtonItem());
			buttonIconSet.SubItems.Add(new ButtonItem());
			if (!VisualStyleInformation.IsEnabledByUser)
			{
				buttonIcon1.ThemeAware = false;
				buttonIcon1.Style = eDotNetBarStyle.OfficeXP;
				buttonIcon2.ThemeAware = false;
				buttonIcon2.Style = eDotNetBarStyle.OfficeXP;
				buttonIcon3.ThemeAware = false;
				buttonIcon3.Style = eDotNetBarStyle.OfficeXP;
				buttonIcon4.ThemeAware = false;
				buttonIcon4.Style = eDotNetBarStyle.OfficeXP;
				buttonIcon5.ThemeAware = false;
				buttonIcon5.Style = eDotNetBarStyle.OfficeXP;
			}
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("FormConditions", "title");
		((Control)labelColumn).Text = StiLocalization.Get("PropertyMain", "Column");
		((Control)labelAlignment).Text = StiLocalization.Get("PropertyMain", "Alignment");
		((Control)labelIconSet).Text = StiLocalization.Get("PropertyMain", "IconSet");
		((Control)buttonReverse).Text = StiLocalization.Get("Buttons", "Reverse");
		((Control)labelIcon).Text = StiLocalization.Get("PropertyMain", "Icon");
		((Control)labelOperation).Text = StiLocalization.Get("PropertyMain", "Operation");
		((Control)labelType).Text = StiLocalization.Get("PropertyMain", "Type");
		((Control)labelValue).Text = StiLocalization.Get("PropertyMain", "Value");
	}

	private void CheckValues()
	{
		string arg = ((((ListControl)comboBoxOperation1).SelectedIndex == 0) ? "<" : "<=");
		string arg2 = ((((ListControl)comboBoxOperation2).SelectedIndex == 0) ? "<" : "<=");
		string arg3 = ((((ListControl)comboBoxOperation3).SelectedIndex == 0) ? "<" : "<=");
		string arg4 = ((((ListControl)comboBoxOperation4).SelectedIndex == 0) ? "<" : "<=");
		((Control)comboBoxOperation3).Visible = ((Control)panelItem4).Visible;
		((Control)comboBoxType3).Visible = ((Control)panelItem4).Visible;
		((Control)textBoxValue3).Visible = ((Control)panelItem4).Visible;
		((Control)comboBoxOperation4).Visible = ((Control)panelItem5).Visible;
		((Control)comboBoxType4).Visible = ((Control)panelItem5).Visible;
		((Control)textBoxValue4).Visible = ((Control)panelItem5).Visible;
		((Control)comboBoxOperation5).Visible = false;
		((Control)comboBoxType5).Visible = false;
		((Control)textBoxValue5).Visible = false;
		if (((Control)panelItem5).Visible)
		{
			((Control)labelItem1).Text = StiLocalization.Get("Report", "WhenValueIs");
			((Control)labelItem2).Text = string.Format(StiLocalization.Get("Report", "WhenAnd"), arg, ((Control)textBoxValue1).Text);
			((Control)labelItem3).Text = string.Format(StiLocalization.Get("Report", "WhenAnd"), arg2, ((Control)textBoxValue2).Text);
			((Control)labelItem4).Text = string.Format(StiLocalization.Get("Report", "WhenAnd"), arg3, ((Control)textBoxValue3).Text);
			((Control)labelItem5).Text = string.Format(StiLocalization.Get("Report", "When"), arg4, ((Control)textBoxValue4).Text);
		}
		else if (((Control)panelItem4).Visible)
		{
			((Control)labelItem1).Text = StiLocalization.Get("Report", "WhenValueIs");
			((Control)labelItem2).Text = string.Format(StiLocalization.Get("Report", "WhenAnd"), arg, ((Control)textBoxValue1).Text);
			((Control)labelItem3).Text = string.Format(StiLocalization.Get("Report", "WhenAnd"), arg2, ((Control)textBoxValue2).Text);
			((Control)labelItem4).Text = string.Format(StiLocalization.Get("Report", "When"), arg3, ((Control)textBoxValue3).Text);
		}
		else
		{
			((Control)labelItem1).Text = StiLocalization.Get("Report", "WhenValueIs");
			((Control)labelItem2).Text = string.Format(StiLocalization.Get("Report", "WhenAnd"), arg, ((Control)textBoxValue1).Text);
			((Control)labelItem3).Text = string.Format(StiLocalization.Get("Report", "When"), arg2, ((Control)textBoxValue2).Text);
		}
	}

	public void ApplyIconSet(StiIconSet iconSet)
	{
		ApplyIconSet(iconSet, GetIconSet(iconSet));
	}

	public void ApplyIconSet(StiIconSet iconSet, Image image)
	{
		((Control)buttonIconSet).Tag = iconSet;
		buttonIconSet.Image = image;
		StiIcon[] iconSet2 = StiIconSetHelper.GetIconSet(iconSet);
		if (iconSet2.Length == 3)
		{
			((Control)textBoxValue1).Text = "67";
			((Control)textBoxValue2).Text = "33";
		}
		else if (iconSet2.Length == 4)
		{
			((Control)textBoxValue1).Text = "75";
			((Control)textBoxValue2).Text = "50";
			((Control)textBoxValue3).Text = "25";
		}
		else if (iconSet2.Length == 5)
		{
			((Control)textBoxValue1).Text = "80";
			((Control)textBoxValue2).Text = "60";
			((Control)textBoxValue3).Text = "40";
			((Control)textBoxValue4).Text = "20";
		}
		((ListControl)comboBoxType1).SelectedIndex = 0;
		((ListControl)comboBoxType2).SelectedIndex = 0;
		((ListControl)comboBoxType3).SelectedIndex = 0;
		((ListControl)comboBoxType4).SelectedIndex = 0;
		((ListControl)comboBoxType5).SelectedIndex = 0;
		SetIconControl(buttonIcon1, panelItem1, (iconSet2.Length != 0) ? iconSet2[0] : StiIcon.None);
		SetIconControl(buttonIcon2, panelItem2, (iconSet2.Length > 1) ? iconSet2[1] : StiIcon.None);
		SetIconControl(buttonIcon3, panelItem3, (iconSet2.Length > 2) ? iconSet2[2] : StiIcon.None);
		SetIconControl(buttonIcon4, panelItem4, (iconSet2.Length > 3) ? iconSet2[3] : StiIcon.None);
		SetIconControl(buttonIcon5, panelItem5, (iconSet2.Length > 4) ? iconSet2[4] : StiIcon.None);
	}

	private void AddPopup(ButtonX senderButton)
	{
		lastButton = senderButton;
		StiIcon stiIcon = (StiIcon)((Control)senderButton).Tag;
		ButtonItem buttonItem = new ButtonItem
		{
			ItemAlignment = eItemAlignment.Center,
			Text = StiLocalization.Get("PropertyMain", "NoIcon"),
			ButtonStyle = eButtonStyle.ImageAndText,
			ImageListSizeSelection = eButtonImageListSelection.NotSet,
			ImagePaddingHorizontal = StiScale.I8,
			Checked = (stiIcon == StiIcon.None)
		};
		buttonItem.Click += delegate
		{
			lastButton.Image = null;
			((Control)lastButton).Tag = StiIcon.None;
		};
		GalleryContainer galleryContainer = new GalleryContainer
		{
			PopupUsesStandardScrollbars = false,
			BeginGroup = true,
			MinimumSize = new Size(StiScale.XXI(250.0), StiScale.YYI(250.0))
		};
		senderButton.SubItems.Clear();
		senderButton.SubItems.Add(buttonItem);
		senderButton.SubItems.Add(galleryContainer);
		StiIcon[] iconSet = StiIconSetHelper.GetIconSet(StiIconSet.Full);
		StiIcon[] array = iconSet;
		foreach (StiIcon stiIcon2 in array)
		{
			if (stiIcon2 != 0)
			{
				ButtonItem buttonItem2 = new ButtonItem();
				buttonItem2.Click += delegate(object sender2, EventArgs e2)
				{
					ButtonItem buttonItem3 = sender2 as ButtonItem;
					((Control)lastButton).Tag = buttonItem3.Tag;
					lastButton.Image = buttonItem3.Image;
				};
				buttonItem2.Checked = stiIcon2 == stiIcon;
				buttonItem2.Tag = stiIcon2;
				buttonItem2.Image = StiImageResizer.GetImage(StiIconSetHelper.GetIcon(stiIcon2));
				galleryContainer.SubItems.Add(buttonItem2);
			}
		}
	}

	public Image GetIconSet(StiIconSet iconSet)
	{
		return (Image)(iconSet switch
		{
			StiIconSet.None => null, 
			StiIconSet.Full => null, 
			_ => GetIconSet(iconSet.ToString()), 
		});
	}

	private Image GetIconSet(string name)
	{
		return StiImageResizer.GetImage((Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Components.Images.IconSets." + name + ".png"));
	}

	private void SetIconControl(ButtonX buttonIcon, Panel panel, StiIcon icon)
	{
		((Control)buttonIcon).Tag = icon;
		buttonIcon.Image = StiImageResizer.GetImage(StiIconSetHelper.GetIcon(icon));
		((Control)panel).Visible = icon != StiIcon.None;
	}

	private void ButtonIcon1_PopupOpen(object sender, EventArgs e)
	{
		AddPopup(buttonIcon1);
	}

	private void ButtonIcon2_PopupOpen(object sender, EventArgs e)
	{
		AddPopup(buttonIcon2);
	}

	private void ButtonIcon3_PopupOpen(object sender, EventArgs e)
	{
		AddPopup(buttonIcon3);
	}

	private void ButtonIcon4_PopupOpen(object sender, EventArgs e)
	{
		AddPopup(buttonIcon4);
	}

	private void ButtonIcon5_PopupOpen(object sender, EventArgs e)
	{
		AddPopup(buttonIcon5);
	}

	private void ButtonIconSet_PopupOpen(object sender, EventArgs e)
	{
		StiIconSet stiIconSet = (StiIconSet)((Control)buttonIconSet).Tag;
		GalleryContainer galleryContainer = new GalleryContainer
		{
			PopupUsesStandardScrollbars = false,
			MinimumSize = new Size(StiScale.XXI(260.0), StiScale.YYI(330.0))
		};
		buttonIconSet.SubItems.Clear();
		buttonIconSet.SubItems.Add(galleryContainer);
		StiIconSet[] array = new StiIconSet[25]
		{
			StiIconSet.ArrowsColored3,
			StiIconSet.ArrowsGray3,
			StiIconSet.ArrowsColored4,
			StiIconSet.ArrowsGray4,
			StiIconSet.ArrowsColored5,
			StiIconSet.ArrowsGray5,
			StiIconSet.Triangles3,
			StiIconSet.Flags3,
			StiIconSet.SymbolsUncircled3,
			StiIconSet.SymbolsCircled3,
			StiIconSet.TrafficLightsUnrimmed3,
			StiIconSet.TrafficLightsRimmed3,
			StiIconSet.TrafficLights4,
			StiIconSet.Signs3,
			StiIconSet.RedToBlack4,
			StiIconSet.Ratings3,
			StiIconSet.Quarters5,
			StiIconSet.Ratings4,
			StiIconSet.QuartersGreen5,
			StiIconSet.Ratings5,
			StiIconSet.QuartersRed5,
			StiIconSet.Stars3,
			StiIconSet.Squares5,
			StiIconSet.Stars5,
			StiIconSet.Latin4
		};
		StiIconSet[] array2 = array;
		foreach (StiIconSet stiIconSet2 in array2)
		{
			ButtonItem buttonItem = new ButtonItem
			{
				Checked = (stiIconSet2 == stiIconSet),
				Tag = stiIconSet2,
				Image = GetIconSet(stiIconSet2),
				FixedSize = new Size(StiScale.XXI(126.0), StiScale.YYI(24.0)),
				ImagePaddingHorizontal = StiScale.I4,
				ButtonStyle = eButtonStyle.ImageAndText,
				Text = string.Empty
			};
			buttonItem.Click += delegate(object sender2, EventArgs e2)
			{
				ButtonItem buttonItem2 = sender2 as ButtonItem;
				ApplyIconSet((StiIconSet)buttonItem2.Tag, buttonItem2.Image);
			};
			galleryContainer.SubItems.Add(buttonItem);
		}
	}

	private void PanelItem1_VisibleChanged(object sender, EventArgs e)
	{
		Panel val = (Panel)((sender is Panel) ? sender : null);
		if (!((Control)val).Visible)
		{
			Panel obj = panelItems;
			((Control)obj).Height = ((Control)obj).Height - ((Control)val).Height;
			Control parent = ((Control)panelItems).Parent;
			parent.Height -= ((Control)val).Height;
		}
		if (((Control)val).Visible)
		{
			Panel obj2 = panelItems;
			((Control)obj2).Height = ((Control)obj2).Height + ((Control)val).Height;
			Control parent2 = ((Control)panelItems).Parent;
			parent2.Height += ((Control)val).Height;
		}
		CheckValues();
		if (((Control)panelItems).Controls.IndexOf((Control)(object)panelItem1) != 4 || ((Control)panelItems).Controls.IndexOf((Control)(object)panelItem2) != 3 || ((Control)panelItems).Controls.IndexOf((Control)(object)panelItem3) != 2 || ((Control)panelItems).Controls.IndexOf((Control)(object)panelItem4) != 1 || ((Control)panelItems).Controls.IndexOf((Control)(object)panelItem5) != 0)
		{
			((Control)panelItem1).BringToFront();
			((Control)panelItem2).BringToFront();
			((Control)panelItem3).BringToFront();
			((Control)panelItem4).BringToFront();
			((Control)panelItem5).BringToFront();
		}
	}

	private void ButtonReverse_Click(object sender, EventArgs e)
	{
		if (((Control)panelItem5).Visible && ((Control)panelItem4).Visible)
		{
			Image image = buttonIcon1.Image;
			Image image2 = buttonIcon2.Image;
			Image image3 = buttonIcon4.Image;
			Image image4 = buttonIcon5.Image;
			object tag = ((Control)buttonIcon1).Tag;
			object tag2 = ((Control)buttonIcon2).Tag;
			object tag3 = ((Control)buttonIcon4).Tag;
			object tag4 = ((Control)buttonIcon5).Tag;
			buttonIcon1.Image = image4;
			buttonIcon2.Image = image3;
			buttonIcon4.Image = image2;
			buttonIcon5.Image = image;
			((Control)buttonIcon1).Tag = tag4;
			((Control)buttonIcon2).Tag = tag3;
			((Control)buttonIcon4).Tag = tag2;
			((Control)buttonIcon5).Tag = tag;
		}
		else if (!((Control)panelItem5).Visible && ((Control)panelItem4).Visible)
		{
			Image image5 = buttonIcon1.Image;
			Image image6 = buttonIcon2.Image;
			Image image7 = buttonIcon3.Image;
			Image image8 = buttonIcon4.Image;
			object tag5 = ((Control)buttonIcon1).Tag;
			object tag6 = ((Control)buttonIcon2).Tag;
			object tag7 = ((Control)buttonIcon3).Tag;
			object tag8 = ((Control)buttonIcon4).Tag;
			buttonIcon1.Image = image8;
			buttonIcon2.Image = image7;
			buttonIcon3.Image = image6;
			buttonIcon4.Image = image5;
			((Control)buttonIcon1).Tag = tag8;
			((Control)buttonIcon2).Tag = tag7;
			((Control)buttonIcon3).Tag = tag6;
			((Control)buttonIcon4).Tag = tag5;
		}
		else if (!((Control)panelItem5).Visible && !((Control)panelItem4).Visible)
		{
			Image image9 = buttonIcon1.Image;
			Image image10 = buttonIcon3.Image;
			object tag9 = ((Control)buttonIcon1).Tag;
			object tag10 = ((Control)buttonIcon3).Tag;
			buttonIcon1.Image = image10;
			buttonIcon3.Image = image9;
			((Control)buttonIcon1).Tag = tag10;
			((Control)buttonIcon3).Tag = tag9;
		}
	}

	private void TextBoxValue1_TextChanged(object sender, EventArgs e)
	{
		CheckValues();
	}

	private void ComboBoxOperation1_SelectedIndexChanged(object sender, EventArgs e)
	{
		CheckValues();
	}

	private void LabelColumn_Click(object sender, EventArgs e)
	{
		((Control)this).Focus();
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_1ccf: Unknown result type (might be due to invalid IL or missing references)
		cbColumn = new StiTreeViewBox();
		labelColumn = new Label();
		labelIconSet = new Label();
		comboBoxType1 = new StiComboBox();
		labelType = new Label();
		textBoxValue1 = new StiTextBox();
		labelValue = new Label();
		buttonReverse = new Button();
		comboBoxOperation1 = new StiComboBox();
		labelOperation = new Label();
		buttonIcon1 = new ButtonX();
		labelIcon = new Label();
		labelItem1 = new Label();
		panelItems = new Panel();
		panelItem5 = new Panel();
		buttonIcon5 = new ButtonX();
		labelItem5 = new Label();
		textBoxValue5 = new StiTextBox();
		comboBoxOperation5 = new StiComboBox();
		comboBoxType5 = new StiComboBox();
		panelItem4 = new Panel();
		buttonIcon4 = new ButtonX();
		labelItem4 = new Label();
		textBoxValue4 = new StiTextBox();
		comboBoxOperation4 = new StiComboBox();
		comboBoxType4 = new StiComboBox();
		panelItem3 = new Panel();
		buttonIcon3 = new ButtonX();
		labelItem3 = new Label();
		textBoxValue3 = new StiTextBox();
		comboBoxOperation3 = new StiComboBox();
		comboBoxType3 = new StiComboBox();
		panelItem2 = new Panel();
		buttonIcon2 = new ButtonX();
		labelItem2 = new Label();
		textBoxValue2 = new StiTextBox();
		comboBoxOperation2 = new StiComboBox();
		comboBoxType2 = new StiComboBox();
		panelItem1 = new Panel();
		labelAlignment = new Label();
		comboBoxAlignment = new StiComboBox();
		buttonIconSet = new ButtonX();
		((Control)panelItems).SuspendLayout();
		((Control)panelItem5).SuspendLayout();
		((Control)panelItem4).SuspendLayout();
		((Control)panelItem3).SuspendLayout();
		((Control)panelItem2).SuspendLayout();
		((Control)panelItem1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)cbColumn).BackColor = SystemColors.Window;
		cbColumn.DropDownWidth = 400;
		cbColumn.Flat = false;
		((Control)cbColumn).ForeColor = SystemColors.ControlText;
		((Control)cbColumn).Location = new Point(179, 12);
		cbColumn.MaxDropDownHeight = 300;
		((Control)cbColumn).Name = "cbColumn";
		((Control)cbColumn).Size = new Size(232, 21);
		((Control)cbColumn).TabIndex = 0;
		((Control)labelColumn).BackColor = Color.Transparent;
		((Control)labelColumn).ForeColor = Color.DimGray;
		((Control)labelColumn).Location = new Point(12, 12);
		((Control)labelColumn).Name = "labelColumn";
		((Control)labelColumn).Size = new Size(160, 21);
		((Control)labelColumn).TabIndex = 8;
		((Control)labelColumn).Text = "Column";
		labelColumn.TextAlign = (ContentAlignment)16;
		labelColumn.UseCompatibleTextRendering = true;
		((Control)labelColumn).Click += LabelColumn_Click;
		((Control)labelIconSet).BackColor = Color.Transparent;
		((Control)labelIconSet).ForeColor = Color.DimGray;
		((Control)labelIconSet).Location = new Point(12, 51);
		((Control)labelIconSet).Name = "labelIconSet";
		((Control)labelIconSet).Size = new Size(160, 21);
		((Control)labelIconSet).TabIndex = 22;
		((Control)labelIconSet).Text = "Icon Set";
		labelIconSet.TextAlign = (ContentAlignment)16;
		labelIconSet.UseCompatibleTextRendering = true;
		((Control)labelIconSet).Click += LabelColumn_Click;
		comboBoxType1.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxType1).FormattingEnabled = true;
		((ComboBox)comboBoxType1).Items.AddRange(new object[2] { "Solid Brush", "Gradient Brush" });
		((Control)comboBoxType1).Location = new Point(325, 6);
		((Control)comboBoxType1).Name = "comboBoxType1";
		((Control)comboBoxType1).Size = new Size(130, 21);
		((Control)comboBoxType1).TabIndex = 2;
		((Control)labelType).BackColor = Color.Transparent;
		((Control)labelType).ForeColor = Color.DimGray;
		((Control)labelType).Location = new Point(330, 118);
		((Control)labelType).Name = "labelType";
		((Control)labelType).Size = new Size(136, 14);
		((Control)labelType).TabIndex = 32;
		((Control)labelType).Text = "Type";
		labelType.TextAlign = (ContentAlignment)16;
		labelType.UseCompatibleTextRendering = true;
		((Control)labelType).Click += LabelColumn_Click;
		((Control)textBoxValue1).BackColor = Color.White;
		((Control)textBoxValue1).Location = new Point(467, 6);
		((Control)textBoxValue1).Name = "textBoxValue1";
		((Control)textBoxValue1).Size = new Size(119, 21);
		((Control)textBoxValue1).TabIndex = 3;
		textBoxValue1.TextAlign = (HorizontalAlignment)1;
		((Control)textBoxValue1).TextChanged += TextBoxValue1_TextChanged;
		((Control)labelValue).BackColor = Color.Transparent;
		((Control)labelValue).ForeColor = Color.DimGray;
		((Control)labelValue).Location = new Point(473, 119);
		((Control)labelValue).Name = "labelValue";
		((Control)labelValue).Size = new Size(126, 13);
		((Control)labelValue).TabIndex = 34;
		((Control)labelValue).Text = "Value";
		labelValue.TextAlign = (ContentAlignment)16;
		labelValue.UseCompatibleTextRendering = true;
		((Control)labelValue).Click += LabelColumn_Click;
		((Control)buttonReverse).Location = new Point(360, 48);
		((Control)buttonReverse).Name = "buttonReverse";
		((Control)buttonReverse).Size = new Size(112, 26);
		((Control)buttonReverse).TabIndex = 2;
		((Control)buttonReverse).Text = "Reverse";
		((ButtonBase)buttonReverse).UseVisualStyleBackColor = true;
		((Control)buttonReverse).Click += ButtonReverse_Click;
		comboBoxOperation1.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxOperation1).FormattingEnabled = true;
		((ComboBox)comboBoxOperation1).Items.AddRange(new object[2] { ">=", ">" });
		((Control)comboBoxOperation1).Location = new Point(230, 6);
		((Control)comboBoxOperation1).Name = "comboBoxOperation1";
		((Control)comboBoxOperation1).Size = new Size(84, 21);
		((Control)comboBoxOperation1).TabIndex = 1;
		((ComboBox)comboBoxOperation1).SelectedIndexChanged += ComboBoxOperation1_SelectedIndexChanged;
		((Control)labelOperation).BackColor = Color.Transparent;
		((Control)labelOperation).ForeColor = Color.DimGray;
		((Control)labelOperation).Location = new Point(235, 118);
		((Control)labelOperation).Name = "labelOperation";
		((Control)labelOperation).Size = new Size(75, 14);
		((Control)labelOperation).TabIndex = 42;
		((Control)labelOperation).Text = "Operation";
		labelOperation.TextAlign = (ContentAlignment)16;
		labelOperation.UseCompatibleTextRendering = true;
		((Control)labelOperation).Click += LabelColumn_Click;
		((Control)buttonIcon1).AccessibleRole = (AccessibleRole)43;
		buttonIcon1.AutoExpandOnClick = true;
		buttonIcon1.ColorTable = eButtonColor.OrangeWithBackground;
		buttonIcon1.Is20Button = false;
		((Control)buttonIcon1).Location = new Point(7, 6);
		((Control)buttonIcon1).Name = "buttonIcon1";
		((Control)buttonIcon1).Size = new Size(48, 26);
		buttonIcon1.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)buttonIcon1).TabIndex = 0;
		buttonIcon1.ThemeAware = true;
		buttonIcon1.PopupOpen += ButtonIcon1_PopupOpen;
		((Control)labelIcon).BackColor = Color.Transparent;
		((Control)labelIcon).ForeColor = Color.DimGray;
		((Control)labelIcon).Location = new Point(12, 118);
		((Control)labelIcon).Name = "labelIcon";
		((Control)labelIcon).Size = new Size(113, 14);
		((Control)labelIcon).TabIndex = 42;
		((Control)labelIcon).Text = "Icon";
		labelIcon.TextAlign = (ContentAlignment)16;
		labelIcon.UseCompatibleTextRendering = true;
		((Control)labelIcon).Click += LabelColumn_Click;
		((Control)labelItem1).Location = new Point(69, 6);
		((Control)labelItem1).Name = "labelItem1";
		((Control)labelItem1).Size = new Size(158, 21);
		((Control)labelItem1).TabIndex = 44;
		((Control)labelItem1).Text = "when value is";
		labelItem1.TextAlign = (ContentAlignment)16;
		((Control)labelItem1).Click += LabelColumn_Click;
		((Control)panelItems).BackColor = Color.Transparent;
		((Control)panelItems).Controls.Add((Control)(object)panelItem5);
		((Control)panelItems).Controls.Add((Control)(object)panelItem4);
		((Control)panelItems).Controls.Add((Control)(object)panelItem3);
		((Control)panelItems).Controls.Add((Control)(object)panelItem2);
		((Control)panelItems).Controls.Add((Control)(object)panelItem1);
		((Control)panelItems).Location = new Point(5, 135);
		((Control)panelItems).Name = "panelItems";
		((Control)panelItems).Size = new Size(597, 182);
		((Control)panelItems).TabIndex = 4;
		((Control)panelItem5).BackColor = Color.Transparent;
		((Control)panelItem5).Controls.Add((Control)(object)buttonIcon5);
		((Control)panelItem5).Controls.Add((Control)(object)labelItem5);
		((Control)panelItem5).Controls.Add((Control)(object)textBoxValue5);
		((Control)panelItem5).Controls.Add((Control)(object)comboBoxOperation5);
		((Control)panelItem5).Controls.Add((Control)(object)comboBoxType5);
		((Control)panelItem5).Dock = (DockStyle)1;
		((Control)panelItem5).Location = new Point(0, 144);
		((Control)panelItem5).Name = "panelItem5";
		((Control)panelItem5).Size = new Size(597, 36);
		((Control)panelItem5).TabIndex = 4;
		((Control)panelItem5).VisibleChanged += PanelItem1_VisibleChanged;
		((Control)buttonIcon5).AccessibleRole = (AccessibleRole)43;
		buttonIcon5.AutoExpandOnClick = true;
		buttonIcon5.ColorTable = eButtonColor.OrangeWithBackground;
		buttonIcon5.Is20Button = false;
		((Control)buttonIcon5).Location = new Point(7, 6);
		((Control)buttonIcon5).Name = "buttonIcon5";
		((Control)buttonIcon5).Size = new Size(48, 26);
		buttonIcon5.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)buttonIcon5).TabIndex = 0;
		buttonIcon5.ThemeAware = true;
		buttonIcon5.PopupOpen += ButtonIcon5_PopupOpen;
		((Control)labelItem5).Location = new Point(69, 6);
		((Control)labelItem5).Name = "labelItem5";
		((Control)labelItem5).Size = new Size(155, 21);
		((Control)labelItem5).TabIndex = 44;
		((Control)labelItem5).Text = "when value is";
		labelItem5.TextAlign = (ContentAlignment)16;
		((Control)labelItem5).Click += LabelColumn_Click;
		((Control)textBoxValue5).BackColor = Color.White;
		((Control)textBoxValue5).Location = new Point(467, 6);
		((Control)textBoxValue5).Name = "textBoxValue5";
		((Control)textBoxValue5).Size = new Size(119, 21);
		((Control)textBoxValue5).TabIndex = 3;
		textBoxValue5.TextAlign = (HorizontalAlignment)1;
		((Control)textBoxValue5).TextChanged += TextBoxValue1_TextChanged;
		comboBoxOperation5.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxOperation5).FormattingEnabled = true;
		((ComboBox)comboBoxOperation5).Items.AddRange(new object[2] { ">=", ">" });
		((Control)comboBoxOperation5).Location = new Point(230, 6);
		((Control)comboBoxOperation5).Name = "comboBoxOperation5";
		((Control)comboBoxOperation5).Size = new Size(84, 21);
		((Control)comboBoxOperation5).TabIndex = 1;
		((ComboBox)comboBoxOperation5).SelectedIndexChanged += ComboBoxOperation1_SelectedIndexChanged;
		comboBoxType5.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxType5).FormattingEnabled = true;
		((ComboBox)comboBoxType5).Items.AddRange(new object[2] { "Solid Brush", "Gradient Brush" });
		((Control)comboBoxType5).Location = new Point(325, 6);
		((Control)comboBoxType5).Name = "comboBoxType5";
		((Control)comboBoxType5).Size = new Size(130, 21);
		((Control)comboBoxType5).TabIndex = 2;
		((Control)panelItem4).BackColor = Color.Transparent;
		((Control)panelItem4).Controls.Add((Control)(object)buttonIcon4);
		((Control)panelItem4).Controls.Add((Control)(object)labelItem4);
		((Control)panelItem4).Controls.Add((Control)(object)textBoxValue4);
		((Control)panelItem4).Controls.Add((Control)(object)comboBoxOperation4);
		((Control)panelItem4).Controls.Add((Control)(object)comboBoxType4);
		((Control)panelItem4).Dock = (DockStyle)1;
		((Control)panelItem4).Location = new Point(0, 108);
		((Control)panelItem4).Name = "panelItem4";
		((Control)panelItem4).Size = new Size(597, 36);
		((Control)panelItem4).TabIndex = 3;
		((Control)panelItem4).VisibleChanged += PanelItem1_VisibleChanged;
		((Control)buttonIcon4).AccessibleRole = (AccessibleRole)43;
		buttonIcon4.AutoExpandOnClick = true;
		buttonIcon4.ColorTable = eButtonColor.OrangeWithBackground;
		buttonIcon4.Is20Button = false;
		((Control)buttonIcon4).Location = new Point(7, 6);
		((Control)buttonIcon4).Name = "buttonIcon4";
		((Control)buttonIcon4).Size = new Size(48, 26);
		buttonIcon4.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)buttonIcon4).TabIndex = 0;
		buttonIcon4.ThemeAware = true;
		buttonIcon4.PopupOpen += ButtonIcon4_PopupOpen;
		((Control)labelItem4).Location = new Point(69, 6);
		((Control)labelItem4).Name = "labelItem4";
		((Control)labelItem4).Size = new Size(155, 21);
		((Control)labelItem4).TabIndex = 1;
		((Control)labelItem4).Text = "when value is";
		labelItem4.TextAlign = (ContentAlignment)16;
		((Control)labelItem4).Click += LabelColumn_Click;
		((Control)textBoxValue4).BackColor = Color.White;
		((Control)textBoxValue4).Location = new Point(467, 6);
		((Control)textBoxValue4).Name = "textBoxValue4";
		((Control)textBoxValue4).Size = new Size(119, 21);
		((Control)textBoxValue4).TabIndex = 4;
		textBoxValue4.TextAlign = (HorizontalAlignment)1;
		((Control)textBoxValue4).TextChanged += TextBoxValue1_TextChanged;
		comboBoxOperation4.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxOperation4).FormattingEnabled = true;
		((ComboBox)comboBoxOperation4).Items.AddRange(new object[2] { ">=", ">" });
		((Control)comboBoxOperation4).Location = new Point(230, 6);
		((Control)comboBoxOperation4).Name = "comboBoxOperation4";
		((Control)comboBoxOperation4).Size = new Size(84, 21);
		((Control)comboBoxOperation4).TabIndex = 2;
		((ComboBox)comboBoxOperation4).SelectedIndexChanged += ComboBoxOperation1_SelectedIndexChanged;
		comboBoxType4.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxType4).FormattingEnabled = true;
		((ComboBox)comboBoxType4).Items.AddRange(new object[2] { "Solid Brush", "Gradient Brush" });
		((Control)comboBoxType4).Location = new Point(325, 6);
		((Control)comboBoxType4).Name = "comboBoxType4";
		((Control)comboBoxType4).Size = new Size(130, 21);
		((Control)comboBoxType4).TabIndex = 3;
		((Control)panelItem3).BackColor = Color.Transparent;
		((Control)panelItem3).Controls.Add((Control)(object)buttonIcon3);
		((Control)panelItem3).Controls.Add((Control)(object)labelItem3);
		((Control)panelItem3).Controls.Add((Control)(object)textBoxValue3);
		((Control)panelItem3).Controls.Add((Control)(object)comboBoxOperation3);
		((Control)panelItem3).Controls.Add((Control)(object)comboBoxType3);
		((Control)panelItem3).Dock = (DockStyle)1;
		((Control)panelItem3).Location = new Point(0, 72);
		((Control)panelItem3).Name = "panelItem3";
		((Control)panelItem3).Size = new Size(597, 36);
		((Control)panelItem3).TabIndex = 2;
		((Control)panelItem3).VisibleChanged += PanelItem1_VisibleChanged;
		((Control)buttonIcon3).AccessibleRole = (AccessibleRole)43;
		buttonIcon3.AutoExpandOnClick = true;
		buttonIcon3.ColorTable = eButtonColor.OrangeWithBackground;
		buttonIcon3.Is20Button = false;
		((Control)buttonIcon3).Location = new Point(7, 6);
		((Control)buttonIcon3).Name = "buttonIcon3";
		((Control)buttonIcon3).Size = new Size(48, 26);
		buttonIcon3.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)buttonIcon3).TabIndex = 0;
		buttonIcon3.ThemeAware = true;
		buttonIcon3.PopupOpen += ButtonIcon3_PopupOpen;
		((Control)labelItem3).Location = new Point(69, 6);
		((Control)labelItem3).Name = "labelItem3";
		((Control)labelItem3).Size = new Size(155, 21);
		((Control)labelItem3).TabIndex = 44;
		((Control)labelItem3).Text = "when value is";
		labelItem3.TextAlign = (ContentAlignment)16;
		((Control)labelItem3).Click += LabelColumn_Click;
		((Control)textBoxValue3).BackColor = Color.White;
		((Control)textBoxValue3).Location = new Point(467, 6);
		((Control)textBoxValue3).Name = "textBoxValue3";
		((Control)textBoxValue3).Size = new Size(119, 21);
		((Control)textBoxValue3).TabIndex = 3;
		textBoxValue3.TextAlign = (HorizontalAlignment)1;
		((Control)textBoxValue3).TextChanged += TextBoxValue1_TextChanged;
		comboBoxOperation3.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxOperation3).FormattingEnabled = true;
		((ComboBox)comboBoxOperation3).Items.AddRange(new object[2] { ">=", ">" });
		((Control)comboBoxOperation3).Location = new Point(230, 6);
		((Control)comboBoxOperation3).Name = "comboBoxOperation3";
		((Control)comboBoxOperation3).Size = new Size(84, 21);
		((Control)comboBoxOperation3).TabIndex = 1;
		((ComboBox)comboBoxOperation3).SelectedIndexChanged += ComboBoxOperation1_SelectedIndexChanged;
		comboBoxType3.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxType3).FormattingEnabled = true;
		((ComboBox)comboBoxType3).Items.AddRange(new object[2] { "Solid Brush", "Gradient Brush" });
		((Control)comboBoxType3).Location = new Point(325, 6);
		((Control)comboBoxType3).Name = "comboBoxType3";
		((Control)comboBoxType3).Size = new Size(130, 21);
		((Control)comboBoxType3).TabIndex = 2;
		((Control)panelItem2).BackColor = Color.Transparent;
		((Control)panelItem2).Controls.Add((Control)(object)buttonIcon2);
		((Control)panelItem2).Controls.Add((Control)(object)labelItem2);
		((Control)panelItem2).Controls.Add((Control)(object)textBoxValue2);
		((Control)panelItem2).Controls.Add((Control)(object)comboBoxOperation2);
		((Control)panelItem2).Controls.Add((Control)(object)comboBoxType2);
		((Control)panelItem2).Dock = (DockStyle)1;
		((Control)panelItem2).Location = new Point(0, 36);
		((Control)panelItem2).Name = "panelItem2";
		((Control)panelItem2).Size = new Size(597, 36);
		((Control)panelItem2).TabIndex = 1;
		((Control)panelItem2).VisibleChanged += PanelItem1_VisibleChanged;
		((Control)buttonIcon2).AccessibleRole = (AccessibleRole)43;
		buttonIcon2.AutoExpandOnClick = true;
		buttonIcon2.ColorTable = eButtonColor.OrangeWithBackground;
		buttonIcon2.Is20Button = false;
		((Control)buttonIcon2).Location = new Point(7, 6);
		((Control)buttonIcon2).Name = "buttonIcon2";
		((Control)buttonIcon2).Size = new Size(48, 26);
		buttonIcon2.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)buttonIcon2).TabIndex = 0;
		buttonIcon2.ThemeAware = true;
		buttonIcon2.PopupOpen += ButtonIcon2_PopupOpen;
		((Control)labelItem2).Location = new Point(69, 6);
		((Control)labelItem2).Name = "labelItem2";
		((Control)labelItem2).Size = new Size(155, 21);
		((Control)labelItem2).TabIndex = 1;
		((Control)labelItem2).Text = "when value is";
		labelItem2.TextAlign = (ContentAlignment)16;
		((Control)labelItem2).Click += LabelColumn_Click;
		((Control)textBoxValue2).BackColor = Color.White;
		((Control)textBoxValue2).Location = new Point(467, 6);
		((Control)textBoxValue2).Name = "textBoxValue2";
		((Control)textBoxValue2).Size = new Size(119, 21);
		((Control)textBoxValue2).TabIndex = 4;
		textBoxValue2.TextAlign = (HorizontalAlignment)1;
		((Control)textBoxValue2).TextChanged += TextBoxValue1_TextChanged;
		comboBoxOperation2.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxOperation2).FormattingEnabled = true;
		((ComboBox)comboBoxOperation2).Items.AddRange(new object[2] { ">=", ">" });
		((Control)comboBoxOperation2).Location = new Point(230, 6);
		((Control)comboBoxOperation2).Name = "comboBoxOperation2";
		((Control)comboBoxOperation2).Size = new Size(84, 21);
		((Control)comboBoxOperation2).TabIndex = 2;
		((ComboBox)comboBoxOperation2).SelectedIndexChanged += ComboBoxOperation1_SelectedIndexChanged;
		comboBoxType2.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxType2).FormattingEnabled = true;
		((ComboBox)comboBoxType2).Items.AddRange(new object[2] { "Solid Brush", "Gradient Brush" });
		((Control)comboBoxType2).Location = new Point(325, 6);
		((Control)comboBoxType2).Name = "comboBoxType2";
		((Control)comboBoxType2).Size = new Size(130, 21);
		((Control)comboBoxType2).TabIndex = 3;
		((Control)panelItem1).BackColor = Color.Transparent;
		((Control)panelItem1).Controls.Add((Control)(object)buttonIcon1);
		((Control)panelItem1).Controls.Add((Control)(object)labelItem1);
		((Control)panelItem1).Controls.Add((Control)(object)textBoxValue1);
		((Control)panelItem1).Controls.Add((Control)(object)comboBoxOperation1);
		((Control)panelItem1).Controls.Add((Control)(object)comboBoxType1);
		((Control)panelItem1).Dock = (DockStyle)1;
		((Control)panelItem1).Location = new Point(0, 0);
		((Control)panelItem1).Name = "panelItem1";
		((Control)panelItem1).Size = new Size(597, 36);
		((Control)panelItem1).TabIndex = 0;
		((Control)panelItem1).VisibleChanged += PanelItem1_VisibleChanged;
		((Control)labelAlignment).BackColor = Color.Transparent;
		((Control)labelAlignment).ForeColor = Color.DimGray;
		((Control)labelAlignment).Location = new Point(12, 86);
		((Control)labelAlignment).Name = "labelAlignment";
		((Control)labelAlignment).Size = new Size(160, 21);
		((Control)labelAlignment).TabIndex = 22;
		((Control)labelAlignment).Text = "Alignment";
		labelAlignment.TextAlign = (ContentAlignment)16;
		labelAlignment.UseCompatibleTextRendering = true;
		((Control)labelAlignment).Click += LabelColumn_Click;
		comboBoxAlignment.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxAlignment).FormattingEnabled = true;
		((ComboBox)comboBoxAlignment).Items.AddRange(new object[2] { "No Borders", "Solid Borders" });
		((Control)comboBoxAlignment).Location = new Point(179, 86);
		((ComboBox)comboBoxAlignment).MaxDropDownItems = 9;
		((Control)comboBoxAlignment).Name = "comboBoxAlignment";
		((Control)comboBoxAlignment).Size = new Size(170, 21);
		((Control)comboBoxAlignment).TabIndex = 3;
		((Control)buttonIconSet).AccessibleRole = (AccessibleRole)43;
		buttonIconSet.AutoExpandOnClick = true;
		buttonIconSet.ColorTable = eButtonColor.OrangeWithBackground;
		buttonIconSet.Is20Button = false;
		((Control)buttonIconSet).Location = new Point(178, 48);
		((Control)buttonIconSet).Name = "buttonIconSet";
		((Control)buttonIconSet).Size = new Size(170, 26);
		buttonIconSet.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)buttonIconSet).TabIndex = 1;
		buttonIconSet.ThemeAware = true;
		buttonIconSet.PopupOpen += ButtonIconSet_PopupOpen;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((Control)this).BackColor = SystemColors.Control;
		((Control)this).Controls.Add((Control)(object)buttonIconSet);
		((Control)this).Controls.Add((Control)(object)comboBoxAlignment);
		((Control)this).Controls.Add((Control)(object)panelItems);
		((Control)this).Controls.Add((Control)(object)buttonReverse);
		((Control)this).Controls.Add((Control)(object)labelValue);
		((Control)this).Controls.Add((Control)(object)cbColumn);
		((Control)this).Controls.Add((Control)(object)labelIcon);
		((Control)this).Controls.Add((Control)(object)labelColumn);
		((Control)this).Controls.Add((Control)(object)labelAlignment);
		((Control)this).Controls.Add((Control)(object)labelIconSet);
		((Control)this).Controls.Add((Control)(object)labelType);
		((Control)this).Controls.Add((Control)(object)labelOperation);
		((Control)this).Margin = new Padding(3);
		((Control)this).Name = "StiIconSetConditionZoneControl";
		base.ShowClose = true;
		((Control)this).Size = new Size(608, 324);
		((Control)panelItems).ResumeLayout(false);
		((Control)panelItem5).ResumeLayout(false);
		((Control)panelItem4).ResumeLayout(false);
		((Control)panelItem3).ResumeLayout(false);
		((Control)panelItem2).ResumeLayout(false);
		((Control)panelItem1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
