using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Helpers;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Images;

namespace Stimulsoft.Report.Design.Dictionary;

public class StiDeleteUsedResourceForm : StiFormUX
{
	private StiDesignerControl designer;

	private StiResource resource;

	private List<StiDatabase> usedFileDatabases;

	private List<StiComponent> usedComponents;

	private IContainer components;

	private StiButtonPanelUX stiGroupLine1;

	private StiButtonUX buttonForceDelete;

	private StiButtonUX buttonCancel;

	private StiPanelUX groupPanel;

	private StiGroupPanelUX panel;

	private PanelEx textPanel;

	private PictureBox pictureBox;

	private StiLinkLabelUX linkWhereUsed;

	public StiDeleteUsedResourceForm(StiDesignerControl designer, StiResource resource, List<StiDatabase> usedFileDatabeses, List<StiComponent> components)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			this.designer = designer;
			this.resource = resource;
			usedFileDatabases = usedFileDatabeses;
			usedComponents = components;
			DoTheming();
			DoScaling();
			Localize();
			Build();
			if (((ArrangedElementCollection)((Control)groupPanel).Controls).Count < 3)
			{
				StiPanelUX stiPanelUX = groupPanel;
				((Control)stiPanelUX).Height = ((Control)stiPanelUX).Height - 16;
				((Control)this).Height = ((Control)this).Height - 16;
			}
			CheckState();
		}
	}

	public void Localize()
	{
		((Control)this).Text = StiLocalization.Get("Cloud", "TextDelete");
		((Control)buttonForceDelete).Text = StiLocalization.Get("Buttons", "ForceDelete");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel").Replace("&", string.Empty);
		int num = TextRenderer.MeasureText(((Control)buttonForceDelete).Text, ((Control)buttonForceDelete).Font).Width + StiScale.I12;
		if (num > ((Control)buttonForceDelete).Width)
		{
			StiButtonUX stiButtonUX = buttonForceDelete;
			((Control)stiButtonUX).Left = ((Control)stiButtonUX).Left - (num - ((Control)buttonForceDelete).Width);
			((Control)buttonForceDelete).Width = num;
		}
		((Control)linkWhereUsed).Text = StiLocalization.Get("Cloud", "ButtonWhereUsed");
	}

	private void DoTheming()
	{
		if (StiUX.IsDark)
		{
			((Control)textPanel).BackColor = StiUX.InputForegroundHint;
		}
	}

	private void DoScaling()
	{
		pictureBox.Image = StiReportDesignImages.Actions.Error(StiImageSize.Double);
	}

	private void Build()
	{
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Expected O, but got Unknown
		if (resource != null)
		{
			((Control)textPanel).Text = string.Format(StiLocalization.Get("Messages", "ResourceCannotBeDeleted"), resource.Name);
		}
		int num = 0;
		int top = 1;
		foreach (StiDatabase usedFileDatabase in usedFileDatabases)
		{
			Image icon = StiReportImages.Dictionary.Database();
			string text = usedFileDatabase.Name + "[" + usedFileDatabase.Alias + "]";
			StiDatabaseSelectForm.RibbonFavoriteButton2016 ribbonFavoriteButton = new StiDatabaseSelectForm.RibbonFavoriteButton2016();
			((Control)ribbonFavoriteButton).Left = num;
			((Control)ribbonFavoriteButton).Top = top;
			ribbonFavoriteButton.Icon = icon;
			StiDatabaseSelectForm.RibbonFavoriteButton2016 ribbonFavoriteButton2 = ribbonFavoriteButton;
			((Control)ribbonFavoriteButton2).Text = StiStringHelper.GetMeasureText(text, ((Control)ribbonFavoriteButton2).Font, StiScale.I(78));
			ToolTip val = new ToolTip();
			val.SetToolTip((Control)(object)ribbonFavoriteButton2, text);
			num += StiScale.I(165);
			((Control)groupPanel).Controls.Add((Control)(object)ribbonFavoriteButton2);
		}
		foreach (StiComponent usedComponent in usedComponents)
		{
			Image bigImageType = designer.GetBigImageType(usedComponent.GetType(), typeof(StiComponent));
			StiDatabaseSelectForm.RibbonFavoriteButton2016 ribbonFavoriteButton3 = new StiDatabaseSelectForm.RibbonFavoriteButton2016();
			((Control)ribbonFavoriteButton3).Left = num;
			((Control)ribbonFavoriteButton3).Top = top;
			ribbonFavoriteButton3.Icon = bigImageType;
			StiDatabaseSelectForm.RibbonFavoriteButton2016 ribbonFavoriteButton4 = ribbonFavoriteButton3;
			((Control)ribbonFavoriteButton4).Text = StiStringHelper.GetMeasureText(usedComponent.Name, ((Control)ribbonFavoriteButton4).Font, StiScale.I(78));
			ToolTip val2 = new ToolTip();
			val2.SetToolTip((Control)(object)ribbonFavoriteButton4, usedComponent.Name);
			num += StiScale.I(165);
			((Control)groupPanel).Controls.Add((Control)(object)ribbonFavoriteButton4);
		}
	}

	private void CheckState()
	{
		((Control)this).Height = ((Control)this).Height + (((Control)groupPanel).Visible ? ((Control)groupPanel).Height : (-((Control)groupPanel).Height));
	}

	private void ButtonWhereUsed_Click(object sender, EventArgs e)
	{
		((Control)groupPanel).Visible = !((Control)groupPanel).Visible;
		CheckState();
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
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		groupPanel = new StiPanelUX();
		stiGroupLine1 = new StiButtonPanelUX();
		buttonForceDelete = new StiButtonUX();
		buttonCancel = new StiButtonUX();
		panel = new StiGroupPanelUX();
		linkWhereUsed = new StiLinkLabelUX();
		textPanel = new PanelEx();
		pictureBox = new PictureBox();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)panel).SuspendLayout();
		((ISupportInitialize)pictureBox).BeginInit();
		((Control)this).SuspendLayout();
		((Control)groupPanel).Anchor = (AnchorStyles)14;
		((ScrollableControl)groupPanel).AutoScroll = true;
		((Control)groupPanel).BackColor = Color.White;
		((Control)groupPanel).Location = new Point(24, 97);
		((Control)groupPanel).Name = "groupPanel";
		((Control)groupPanel).Size = new Size(405, 90);
		((Control)groupPanel).TabIndex = 2;
		((Control)groupPanel).Visible = false;
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonForceDelete);
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonCancel);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 191);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(455, 46);
		((Control)stiGroupLine1).TabIndex = 1;
		((Control)buttonForceDelete).Anchor = (AnchorStyles)10;
		((Button)buttonForceDelete).DialogResult = (DialogResult)1;
		((Control)buttonForceDelete).Location = new Point(257, 12);
		((Control)buttonForceDelete).Name = "btForceDelete";
		((Control)buttonForceDelete).Size = new Size(90, 24);
		((Control)buttonForceDelete).TabIndex = 0;
		((Control)buttonForceDelete).Text = "Force Delete";
		((ButtonBase)buttonForceDelete).UseVisualStyleBackColor = true;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		((Button)buttonCancel).DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(353, 12);
		((Control)buttonCancel).Name = "btCancel";
		((Control)buttonCancel).Size = new Size(90, 24);
		((Control)buttonCancel).TabIndex = 1;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)panel).BackColor = Color.White;
		((Control)panel).Controls.Add((Control)(object)linkWhereUsed);
		((Control)panel).Controls.Add((Control)(object)groupPanel);
		((Control)panel).Controls.Add((Control)(object)textPanel);
		((Control)panel).Controls.Add((Control)(object)pictureBox);
		((Control)panel).Dock = (DockStyle)5;
		panel.LinePosition = StiLinePosition.None;
		panel.LineStyle = StiLineStyle.Dot;
		((Control)panel).Location = new Point(0, 0);
		((Control)panel).Name = "panel";
		((Control)panel).Size = new Size(455, 191);
		((Control)panel).TabIndex = 0;
		((Control)linkWhereUsed).Anchor = (AnchorStyles)9;
		((Control)linkWhereUsed).Location = new Point(92, 66);
		((Control)linkWhereUsed).Name = "linkWhereUsed";
		((Control)linkWhereUsed).Size = new Size(331, 22);
		((Control)linkWhereUsed).TabIndex = 1;
		((LinkLabel)linkWhereUsed).TabStop = true;
		((Control)linkWhereUsed).Text = "Where it is used?";
		((Label)linkWhereUsed).TextAlign = (ContentAlignment)64;
		((Control)linkWhereUsed).Click += ButtonWhereUsed_Click;
		((Control)textPanel).Anchor = (AnchorStyles)13;
		textPanel.AntiAlias = false;
		textPanel.CanvasColor = SystemColors.Control;
		((Control)textPanel).Location = new Point(81, 24);
		((Control)textPanel).Name = "textPanel";
		((Control)textPanel).Size = new Size(344, 34);
		textPanel.Style.Border = eBorderType.SingleLine;
		textPanel.Style.BorderWidth = 0;
		textPanel.Style.ForeColor.ColorSchemePart = eColorSchemePart.ItemText;
		textPanel.Style.GradientAngle = 90;
		textPanel.Style.LineAlignment = (StringAlignment)0;
		textPanel.Style.WordWrap = true;
		((Control)textPanel).TabIndex = 0;
		((Control)pictureBox).BackColor = Color.Transparent;
		pictureBox.ErrorImage = null;
		pictureBox.InitialImage = null;
		((Control)pictureBox).Location = new Point(24, 24);
		((Control)pictureBox).Name = "pictureBox";
		((Control)pictureBox).Size = new Size(34, 34);
		pictureBox.TabIndex = 7;
		pictureBox.TabStop = false;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonForceDelete;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(455, 247);
		((Control)this).Controls.Add((Control)(object)panel);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Control)this).DoubleBuffered = true;
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiDeleteUsedResourceForm";
		((Form)this).ShowIcon = false;
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiDeleteUsedResource";
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)panel).ResumeLayout(false);
		((ISupportInitialize)pictureBox).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
