using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Viewer;

public class StiGoToPageForm : Form
{
	private int maxPage;

	private IContainer components;

	private StiGroupLine glButtons;

	private Button btOk;

	private Button btCancel;

	private Label labelPageN;

	private Label labelOfM;

	private TextBox textBoxPageN;

	public int PageNumber
	{
		get
		{
			string text = ((Control)textBoxPageN).Text;
			try
			{
				int num = int.Parse(text.Trim());
				if (num > maxPage)
				{
					return -1;
				}
				if (num < 1)
				{
					return -1;
				}
				return num;
			}
			catch
			{
				return -1;
			}
		}
	}

	public StiGoToPageForm(int maxPage)
	{
		StiFormExt.InitializeNetCoreFont((Form)(object)this);
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		this.maxPage = maxPage;
		if (StiOptions.Appearance.ShowFormsHelp)
		{
			((Form)this).HelpButton = true;
		}
		Localize();
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("FormViewer", "GoToPage");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)labelPageN).Text = Loc.GetCleaned("FormViewer", "LabelPageN");
		((Control)labelOfM).Text = string.Format(StiLocalization.Get("FormViewer", "PageofM"), maxPage);
	}

	private void ThisHelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/viewing_reports_page_navigation.htm");
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		glButtons = new StiGroupLine();
		btOk = new Button();
		btCancel = new Button();
		textBoxPageN = new TextBox();
		labelPageN = new Label();
		labelOfM = new Label();
		((Control)glButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)glButtons).BackColor = Color.White;
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Controls.Add((Control)(object)btCancel);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = (StiLinePosition)0;
		glButtons.LineStyle = (StiLineStyle)0;
		((Control)glButtons).Location = new Point(0, 49);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(292, 46);
		((Control)glButtons).TabIndex = 3;
		((Control)btOk).Anchor = (AnchorStyles)10;
		btOk.DialogResult = (DialogResult)1;
		((Control)btOk).Location = new Point(125, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		btCancel.DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(206, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)textBoxPageN).Location = new Point(100, 14);
		((Control)textBoxPageN).Name = "textBoxPageN";
		((Control)textBoxPageN).Size = new Size(100, 20);
		((Control)textBoxPageN).TabIndex = 0;
		((Control)labelPageN).ForeColor = Color.DimGray;
		((Control)labelPageN).Location = new Point(12, 15);
		((Control)labelPageN).Name = "labelPageN";
		((Control)labelPageN).Size = new Size(82, 19);
		((Control)labelPageN).TabIndex = 1;
		((Control)labelPageN).Text = "Page";
		labelPageN.TextAlign = (ContentAlignment)64;
		((Control)labelOfM).Location = new Point(203, 15);
		((Control)labelOfM).Name = "labelOfM";
		((Control)labelOfM).Size = new Size(82, 19);
		((Control)labelOfM).TabIndex = 2;
		((Control)labelOfM).Text = "of M";
		labelOfM.TextAlign = (ContentAlignment)16;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(292, 95);
		((Control)this).Controls.Add((Control)(object)labelOfM);
		((Control)this).Controls.Add((Control)(object)labelPageN);
		((Control)this).Controls.Add((Control)(object)textBoxPageN);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiGoToPageForm";
		((Form)this).ShowIcon = false;
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Go To Page";
		((Form)this).HelpButtonClicked += ThisHelpButtonClicked;
		((Control)glButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
