using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Dashboard.Design.Controls;
using Stimulsoft.Report;

namespace Stimulsoft.Dashboard.Design.Editors;

public class StiHtmlEditorForm : Form
{
	private IContainer components;

	private ToolTip toolTip;

	private ImageList imageList1;

	private StiHtmlTextEditorControl textBoxText;

	private Timer timerTextChanged;

	private StiGroupLine groupLineButtons;

	private Button buttonCancel;

	private Button buttonOk;

	internal string HtmlText
	{
		get
		{
			return textBoxText.HtmlText;
		}
		set
		{
			textBoxText.HtmlText = value;
		}
	}

	public StiHtmlEditorForm()
		: this(null)
	{
	}

	public StiHtmlEditorForm(string htmlText, List<string> idents = null)
	{
		((Form)(object)this).InitializeNetCoreFont();
		InitializeComponent();
		StiFormSettings.Load((Form)(object)this, skipLocation: true);
		HtmlText = htmlText;
		textBoxText.ShowInsertContstantButton = idents != null && idents.Count > 0;
		textBoxText.Idents = idents;
		Localize();
	}

	private void Localize()
	{
		((Control)this).Text = Loc.GetMain("Text");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		textBoxText.Localize();
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		((Control)this).OnKeyDown(e);
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			((Form)this).Close();
		}
	}

	protected override void OnFormClosing(FormClosingEventArgs e)
	{
		((Form)this).OnFormClosing(e);
		StiFormSettings.Save((Form)(object)this);
		HtmlText = textBoxText.HtmlText;
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
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiToolTipEditorForm));
		toolTip = new ToolTip(components);
		imageList1 = new ImageList(components);
		timerTextChanged = new Timer(components);
		groupLineButtons = new StiGroupLine();
		buttonCancel = new Button();
		buttonOk = new Button();
		textBoxText = new StiHtmlTextEditorControl();
		((Control)groupLineButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		imageList1.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageList1.ImageStream");
		imageList1.TransparentColor = Color.Transparent;
		imageList1.Images.SetKeyName(0, "File");
		imageList1.Images.SetKeyName(1, "Hyperlink");
		imageList1.Images.SetKeyName(2, "DataColumn");
		imageList1.Images.SetKeyName(3, "Expression");
		timerTextChanged.Interval = 500;
		((Control)groupLineButtons).BackColor = Color.White;
		((Control)groupLineButtons).Controls.Add((Control)(object)buttonCancel);
		((Control)groupLineButtons).Controls.Add((Control)(object)buttonOk);
		((Control)groupLineButtons).Dock = (DockStyle)2;
		groupLineButtons.LinePosition = StiLinePosition.Top;
		groupLineButtons.LineStyle = StiLineStyle.Dot;
		((Control)groupLineButtons).Location = new Point(0, 687);
		((Control)groupLineButtons).Margin = new Padding(6, 6, 6, 6);
		((Control)groupLineButtons).Name = "groupLineButtons";
		((Control)groupLineButtons).Size = new Size(1351, 92);
		((Control)groupLineButtons).TabIndex = 2;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(1175, 23);
		((Control)buttonCancel).Margin = new Padding(6, 6, 6, 6);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(150, 46);
		((Control)buttonCancel).TabIndex = 1;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		buttonOk.DialogResult = (DialogResult)1;
		((Control)buttonOk).Location = new Point(1013, 23);
		((Control)buttonOk).Margin = new Padding(6, 6, 6, 6);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(150, 46);
		((Control)buttonOk).TabIndex = 0;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)textBoxText).Anchor = (AnchorStyles)15;
		((Control)textBoxText).BackColor = Color.White;
		textBoxText.Element = null;
		((Control)textBoxText).Location = new Point(24, 23);
		((Control)textBoxText).Margin = new Padding(12);
		((Control)textBoxText).Name = "textBoxText";
		((Control)textBoxText).Size = new Size(1303, 637);
		((Control)textBoxText).TabIndex = 0;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(1351, 779);
		((Control)this).Controls.Add((Control)(object)groupLineButtons);
		((Control)this).Controls.Add((Control)(object)textBoxText);
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(4, 4, 4, 4);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(1377, 850);
		((Control)this).Name = "StiHtmlEditorForm";
		((Form)this).ShowIcon = false;
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "ToolTip";
		((Control)groupLineButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
