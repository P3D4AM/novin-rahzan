using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.Text;
using Stimulsoft.Dashboard.Design.Controls;
using Stimulsoft.Report;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Dashboard.Design.Editors;

public class StiTextElementEditorForm : StiElementEditorForm
{
	private IContainer components;

	private ToolTip toolTip;

	private ImageList imageList1;

	private StiHtmlTextEditorControl textBoxText;

	private Timer timerTextChanged;

	internal StiTextElement TextElement => base.Element as StiTextElement;

	public StiTextElementEditorForm()
		: this(null)
	{
	}

	public StiTextElementEditorForm(StiElement element)
		: base(element)
	{
		InitializeComponent();
		if (StiOptions.Appearance.ShowFormsHelp)
		{
			((Form)this).HelpButton = true;
		}
		textBoxText.Element = element;
		Localize();
	}

	public override void Localize()
	{
		((Control)this).Text = Loc.Get("Components", "StiText");
		textBoxText.Localize();
	}

	public override void ReadPropertiesFromElement()
	{
		textBoxText.HtmlText = TextElement.Text;
	}

	protected override void OnGlobalElementChanged(object sender, EventArgs e)
	{
		if (sender == base.Element)
		{
			ReadPropertiesFromElement();
		}
	}

	private void RichTextBox_TextChanged(object sender, EventArgs e)
	{
		if (((Control)textBoxText.RichTextBox).Text.Length < 50)
		{
			TextElement.Text = textBoxText.HtmlText;
			RefreshElementView();
		}
		else
		{
			timerTextChanged.Stop();
			timerTextChanged.Start();
		}
	}

	private void TimerTextChanged_Tick(object sender, EventArgs e)
	{
		timerTextChanged.Stop();
		TextElement.Text = textBoxText.HtmlText;
		RefreshElementView();
	}

	private void TextBoxText_ElementChanged(object sender, EventArgs e)
	{
		TextElement.Text = textBoxText.HtmlText;
		RefreshElementView();
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/dashboards_text.htm");
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		((Form)this).OnPaint(e);
		if (textBoxText.IsBackgroundExists)
		{
			Color color = StiDashboardStyleHelper.GetBackColor(textBoxText.Element);
			if (color.A != byte.MaxValue)
			{
				color = Color.FromArgb(255, color.R, color.G, color.B);
			}
			int y = ((Control)this).PointToClient(((Control)textBoxText.TopLine).PointToScreen(new Point(0, ((Control)textBoxText.TopLine).Height))).Y;
			StiDrawing.FillRectangle(e.Graphics, color, 0.0, y, ((Control)this).Width, ((Control)this).Height - y);
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		ReadPropertiesFromElement();
		((Control)textBoxText.RichTextBox).TextChanged += RichTextBox_TextChanged;
		textBoxText.ElementChanged += TextBoxText_ElementChanged;
	}

	protected override void OnFormClosing(FormClosingEventArgs e)
	{
		((Form)this).OnFormClosing(e);
		TextElement.Text = textBoxText.HtmlText;
		RefreshElementView();
	}

	protected override void OnFormClosed(FormClosedEventArgs e)
	{
		base.OnFormClosed(e);
		((Control)textBoxText.RichTextBox).TextChanged -= RichTextBox_TextChanged;
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
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiTextElementEditorForm));
		toolTip = new ToolTip(components);
		imageList1 = new ImageList(components);
		timerTextChanged = new Timer(components);
		textBoxText = new StiHtmlTextEditorControl();
		((Control)this).SuspendLayout();
		imageList1.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageList1.ImageStream");
		imageList1.TransparentColor = Color.Transparent;
		imageList1.Images.SetKeyName(0, "File");
		imageList1.Images.SetKeyName(1, "Hyperlink");
		imageList1.Images.SetKeyName(2, "DataColumn");
		imageList1.Images.SetKeyName(3, "Expression");
		timerTextChanged.Interval = 500;
		timerTextChanged.Tick += TimerTextChanged_Tick;
		((Control)textBoxText).BackColor = Color.White;
		((Control)textBoxText).Dock = (DockStyle)5;
		textBoxText.Element = null;
		((Control)textBoxText).Location = new Point(12, 12);
		((Control)textBoxText).Margin = new Padding(6);
		((Control)textBoxText).Name = "textBoxText";
		((Control)textBoxText).Size = new Size(580, 311);
		((Control)textBoxText).TabIndex = 0;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(604, 335);
		((Control)this).Controls.Add((Control)(object)textBoxText);
		((Form)this).FormBorderStyle = (FormBorderStyle)4;
		((Form)this).Margin = new Padding(2);
		((Control)this).MinimumSize = new Size(550, 320);
		((Control)this).Name = "StiTextElementEditorForm";
		((Control)this).Padding = new Padding(12);
		((Control)this).Text = "Text";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Control)this).ResumeLayout(false);
	}
}
