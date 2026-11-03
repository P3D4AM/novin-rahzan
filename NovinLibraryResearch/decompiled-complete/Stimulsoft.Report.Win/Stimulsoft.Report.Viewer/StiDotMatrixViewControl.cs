using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Viewer;

[ToolboxItem(false)]
public class StiDotMatrixViewControl : UserControl
{
	private Splitter splitter1;

	public StiDotMatrixSettingsControl settings;

	public RichTextBox textBoxReport;

	private Container components;

	public StiDotMatrixViewControl()
	{
		InitializeComponent();
		((Control)splitter1).BackColor = StiColorUtils.Dark(Color.White, (byte)40);
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		textBoxReport = new RichTextBox();
		splitter1 = new Splitter();
		settings = new StiDotMatrixSettingsControl();
		((Control)this).SuspendLayout();
		((TextBoxBase)textBoxReport).BorderStyle = (BorderStyle)0;
		((Control)textBoxReport).BackColor = Color.FromArgb(250, 250, 250);
		((Control)textBoxReport).Dock = (DockStyle)5;
		((Control)textBoxReport).Font = new Font("Courier New", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)textBoxReport).Location = new Point(0, 0);
		((Control)textBoxReport).Name = "textBoxReport";
		((TextBoxBase)textBoxReport).ReadOnly = true;
		((Control)textBoxReport).Size = new Size(425, 567);
		((Control)textBoxReport).TabIndex = 1;
		((Control)textBoxReport).Text = "";
		((TextBoxBase)textBoxReport).WordWrap = false;
		((Control)splitter1).BackColor = Color.FromArgb(242, 242, 242);
		((Control)splitter1).Dock = (DockStyle)4;
		((Control)splitter1).ForeColor = Color.Black;
		((Control)splitter1).Location = new Point(425, 0);
		((Control)splitter1).Name = "splitter1";
		((Control)splitter1).Size = new Size(4, 567);
		((Control)splitter1).TabIndex = 65;
		splitter1.TabStop = false;
		((ScrollableControl)settings).AutoScroll = true;
		((Control)settings).BackColor = Color.FromArgb(250, 250, 250);
		((Control)settings).Dock = (DockStyle)4;
		((Control)settings).Location = new Point(429, 0);
		((Control)settings).Name = "settings";
		((Control)settings).Padding = new Padding(4);
		((Control)settings).Size = new Size(194, 567);
		((Control)settings).TabIndex = 0;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.FromArgb(250, 250, 250);
		((Control)this).Controls.Add((Control)(object)textBoxReport);
		((Control)this).Controls.Add((Control)(object)splitter1);
		((Control)this).Controls.Add((Control)(object)settings);
		((Control)this).Name = "StiDotMatrixViewControl";
		((Control)this).Size = new Size(623, 567);
		((Control)this).ResumeLayout(false);
	}
}
