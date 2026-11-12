using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Design.Editors;

public class StiRichTextEditorForm : StiFormUX
{
	private string textValue = string.Empty;

	private IContainer components;

	private StiButtonUX btOk;

	private StiButtonUX btCancel;

	private StiRichTextEditorControl richTextEditorControl;

	private StiButtonPanelUX stiGroupLine1;

	public string TextValue
	{
		get
		{
			return textValue;
		}
		set
		{
			textValue = value;
		}
	}

	public bool ShowExpressionButton
	{
		get
		{
			return richTextEditorControl.tbInsert.Visible;
		}
		set
		{
			richTextEditorControl.tbInsert.Visible = value;
		}
	}

	public StiRichTextEditorForm(StiReport report, string text, bool showOpenSaveButtons = true)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			richTextEditorControl.Report = report;
			richTextEditorControl.TextValue = text;
			Localize();
			DoScaling();
			StiSettings.Load();
			StiFormSettings.Load((Form)(object)this);
			((Form)this).ShowIcon = false;
			richTextEditorControl.tbOpen.Visible = (richTextEditorControl.tbSave.Visible = showOpenSaveButtons);
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("Components", "StiRichText");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void DoScaling()
	{
		richTextEditorControl.DoScaling();
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		TextValue = richTextEditorControl.TextValue;
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
	}

	private void This_Closing(object sender, CancelEventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/report_internals_rich_text_output_rich_text_editor.htm");
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
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		btOk = new StiButtonUX();
		btCancel = new StiButtonUX();
		stiGroupLine1 = new StiButtonPanelUX();
		richTextEditorControl = new StiRichTextEditorControl();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(687, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 14;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(768, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 15;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btCancel).Click += This_CancelClick;
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)btCancel);
		((Control)stiGroupLine1).Controls.Add((Control)(object)btOk);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 443);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(855, 46);
		((Control)stiGroupLine1).TabIndex = 17;
		((Control)richTextEditorControl).Anchor = (AnchorStyles)15;
		((Control)richTextEditorControl).BackColor = Color.White;
		((Control)richTextEditorControl).Location = new Point(12, 4);
		((Control)richTextEditorControl).Margin = new Padding(4);
		((Control)richTextEditorControl).Name = "richTextEditorControl";
		richTextEditorControl.Report = null;
		((Control)richTextEditorControl).Size = new Size(828, 424);
		((Control)richTextEditorControl).TabIndex = 16;
		richTextEditorControl.TextValue = "{\\rtf1\\ansi\\ansicpg1251\\deff0\\nouicompat\\deflang1049{\\fonttbl{\\f0\\fnil\\fcharset204 Microsoft Sans Serif;}}\r\n{\\*\\generator Riched20 10.0.18362}\\viewkind4\\uc1 \r\n\\pard\\f0\\fs16\\par\r\n}\r\n";
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(855, 489);
		((Control)this).Controls.Add((Control)(object)richTextEditorControl);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiRichTextEditorForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Rich editor";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Closing += This_Closing;
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
