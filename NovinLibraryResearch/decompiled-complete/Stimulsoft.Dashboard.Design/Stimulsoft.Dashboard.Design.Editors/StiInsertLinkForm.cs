using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;

namespace Stimulsoft.Dashboard.Design.Editors;

public class StiInsertLinkForm : Form
{
	private IContainer components;

	private ToolTip toolTip;

	private StiTextBox textBoxUrl;

	private Label labelUrl;

	private StiGroupLine groupLineButtons;

	private Button buttonCancel;

	private Button buttonOk;

	private StiTextBox textBoxText;

	private Label labelText;

	internal string Link => "<a href=\"" + ((Control)textBoxUrl).Text + "\">" + ((Control)textBoxText).Text + "</a>";

	public StiInsertLinkForm(string defaultText)
	{
		InitializeComponent();
		((Control)textBoxText).Text = defaultText;
		((Control)this).DoubleBuffered = true;
		((Form)this).ShowIcon = false;
		Localize();
		DoScaling();
	}

	private void Localize()
	{
		((Control)this).Text = Loc.Get("Editor", "InsertLink");
		((Control)labelText).Text = Loc.GetMain("Text");
	}

	private void DoScaling()
	{
		((Control)textBoxUrl).AutoSize = false;
		((Control)textBoxUrl).Size = new Size(((Control)textBoxUrl).Width, StiScale.I(20));
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
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		toolTip = new ToolTip(components);
		textBoxUrl = new StiTextBox();
		labelUrl = new Label();
		groupLineButtons = new StiGroupLine();
		buttonCancel = new Button();
		buttonOk = new Button();
		textBoxText = new StiTextBox();
		labelText = new Label();
		((Control)groupLineButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)textBoxUrl).Anchor = (AnchorStyles)13;
		((Control)textBoxUrl).BackColor = Color.White;
		((Control)textBoxUrl).Location = new Point(135, 12);
		((Control)textBoxUrl).Margin = new Padding(2);
		((Control)textBoxUrl).Name = "textBoxUrl";
		((Control)textBoxUrl).Size = new Size(199, 21);
		((Control)textBoxUrl).TabIndex = 0;
		((Control)labelUrl).BackColor = Color.White;
		((Control)labelUrl).ForeColor = Color.DimGray;
		((Control)labelUrl).Location = new Point(12, 12);
		((Control)labelUrl).Name = "labelUrl";
		((Control)labelUrl).Size = new Size(118, 21);
		((Control)labelUrl).TabIndex = 3;
		((Control)labelUrl).Text = "Url";
		labelUrl.TextAlign = (ContentAlignment)16;
		((Control)groupLineButtons).BackColor = Color.White;
		((Control)groupLineButtons).Controls.Add((Control)(object)buttonCancel);
		((Control)groupLineButtons).Controls.Add((Control)(object)buttonOk);
		((Control)groupLineButtons).Dock = (DockStyle)2;
		groupLineButtons.LinePosition = StiLinePosition.Top;
		groupLineButtons.LineStyle = StiLineStyle.Dot;
		((Control)groupLineButtons).Location = new Point(0, 83);
		((Control)groupLineButtons).Name = "groupLineButtons";
		((Control)groupLineButtons).Size = new Size(345, 43);
		((Control)groupLineButtons).TabIndex = 2;
		((Control)buttonCancel).Anchor = (AnchorStyles)9;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(258, 12);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 1;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonOk).Anchor = (AnchorStyles)9;
		buttonOk.DialogResult = (DialogResult)1;
		((Control)buttonOk).Location = new Point(177, 12);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 0;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)textBoxText).Anchor = (AnchorStyles)13;
		((Control)textBoxText).BackColor = Color.White;
		((Control)textBoxText).Location = new Point(135, 45);
		((Control)textBoxText).Margin = new Padding(2);
		((Control)textBoxText).Name = "textBoxText";
		((Control)textBoxText).Size = new Size(199, 21);
		((Control)textBoxText).TabIndex = 1;
		((Control)labelText).BackColor = Color.White;
		((Control)labelText).ForeColor = Color.DimGray;
		((Control)labelText).Location = new Point(12, 46);
		((Control)labelText).Name = "labelText";
		((Control)labelText).Size = new Size(118, 21);
		((Control)labelText).TabIndex = 4;
		((Control)labelText).Text = "Text";
		labelText.TextAlign = (ContentAlignment)16;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(345, 126);
		((Control)this).Controls.Add((Control)(object)labelText);
		((Control)this).Controls.Add((Control)(object)labelUrl);
		((Control)this).Controls.Add((Control)(object)textBoxText);
		((Control)this).Controls.Add((Control)(object)textBoxUrl);
		((Control)this).Controls.Add((Control)(object)groupLineButtons);
		((Form)this).Margin = new Padding(2);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiInsertLinkForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Insert Link";
		((Control)groupLineButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
