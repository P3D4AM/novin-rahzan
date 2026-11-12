using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Design.Editors;

public class StiEventEditorForm : StiFormUX
{
	private StiEvent ev;

	private StiReport report;

	private IContainer components;

	private StiButtonUX btOk;

	private StiButtonUX btCancel;

	private StiCodeEditor CodeEditor;

	private ImageList ilTool;

	private StiButtonPanelUX glButtons;

	private StiButtonUX buttonSwitchToBlocks;

	public bool SwitchToBlocks { get; set; }

	private string CodeEditorText
	{
		get
		{
			if (report == null)
			{
				return ((Control)CodeEditor).Text;
			}
			if (report.Designer != null)
			{
				return StiExpressionPacker.UnPackExpression(((Control)CodeEditor).Text, report.Designer, useBraces: true);
			}
			return StiExpressionPacker.UnPackExpression(((Control)CodeEditor).Text, report, useBraces: true);
		}
		set
		{
			if (report == null)
			{
				((Control)CodeEditor).Text = value;
			}
			else if (report.Designer != null)
			{
				((Control)CodeEditor).Text = StiExpressionPacker.PackExpression(value, report.Designer, useBraces: true);
			}
			else
			{
				((Control)CodeEditor).Text = StiExpressionPacker.PackExpression(value, report, useBraces: true);
			}
		}
	}

	public StiEventEditorForm(StiReport report, StiEvent ev, bool switchBlocks = true)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			this.ev = ev;
			this.report = report;
			CodeEditor.Init(report, StiCodeEditorMode.EditorCode);
			if (report != null && report.Designer != null)
			{
				CodeEditorText = StiExpressionPacker.PackExpression(ev.Script, report.Designer, useBraces: false);
			}
			else
			{
				CodeEditorText = StiExpressionPacker.PackExpression(ev.Script, report, useBraces: false);
			}
			Localize();
			((Control)this).Text = ((Control)this).Text + $" [{ev}]";
			StiSettings.Load();
			StiFormSettings.Load((Form)(object)this);
			if (CodeEditor.DictionaryControl != null)
			{
				((Control)CodeEditor.DictionaryControl).Width = StiSettings.GetInt("StiEventEditorForm", "Spl", ((Control)CodeEditor.DictionaryControl).Width);
			}
			((Form)this).ShowIcon = false;
			((Control)buttonSwitchToBlocks).Visible = switchBlocks;
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("FormTitles", "EventEditorForm");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)buttonSwitchToBlocks).Text = string.Format(StiLocalization.Get("Buttons", "SwitchTo"), "Blockly");
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		if (report != null && report.Designer != null)
		{
			ev.Script = StiExpressionPacker.UnPackExpression(CodeEditorText, report.Designer, useBraces: false);
		}
		else
		{
			ev.Script = StiExpressionPacker.UnPackExpression(CodeEditorText, report, useBraces: false);
		}
		((Form)this).Close();
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void This_Activated(object sender, EventArgs e)
	{
		((Control)CodeEditor).Focus();
	}

	private void This_Closed(object sender, EventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
		if (CodeEditor.DictionaryControl != null)
		{
			StiSettings.Load();
			StiSettings.Set("StiEventEditorForm", "Spl", ((Control)CodeEditor.DictionaryControl).Width);
			if (CodeEditor.NodesMemoryForEditor != null)
			{
				CodeEditor.NodesMemoryForEditor.SaveExpNodes((TreeView)(object)CodeEditor.DictionaryControl.tvData);
			}
			CodeEditor.DesignerLoadNodes();
			StiSettings.Save();
		}
	}

	private void This_Closing(object sender, CancelEventArgs e)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Invalid comparison between Unknown and I4
		if (!SwitchToBlocks && ((Control)CodeEditor).Text != ev.Script)
		{
			e.Cancel = (int)MessageBox.Show(StiLocalization.Get("DesignerFx", "CloseEditor"), ((Control)this).Text, (MessageBoxButtons)4) == 7;
		}
	}

	private void ButtonSwitchToBlocks_Click(object sender, EventArgs e)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Invalid comparison between Unknown and I4
		DialogResult val = MessageBox.Show(StiLocalization.Get("Messages", "SwitchingBetweenModes"), StiOptions.Designer.GetDesignerTitle(), (MessageBoxButtons)1);
		if ((int)val == 1)
		{
			SwitchToBlocks = true;
			((Form)this).Close();
		}
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
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiEventEditorForm));
		btOk = new StiButtonUX();
		btCancel = new StiButtonUX();
		CodeEditor = new StiCodeEditor();
		ilTool = new ImageList(components);
		glButtons = new StiButtonPanelUX();
		buttonSwitchToBlocks = new StiButtonUX();
		((Control)glButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Button)btOk).DialogResult = (DialogResult)1;
		((Control)btOk).Location = new Point(408, 14);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 23);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(489, 14);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 23);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btCancel).Click += This_CancelClick;
		CodeEditor.AllowOutlining = false;
		CodeEditor.AllowSQLFormatting = false;
		((Control)CodeEditor).Anchor = (AnchorStyles)15;
		((Control)CodeEditor).BackColor = Color.White;
		CodeEditor.ForceShowAutoGeneratedCode = false;
		((Control)CodeEditor).Location = new Point(12, 12);
		((Control)CodeEditor).Name = "CodeEditor";
		((Control)CodeEditor).Padding = new Padding(1);
		CodeEditor.ReplaceMode = false;
		CodeEditor.ShowAutoGeneratedCode = false;
		CodeEditor.ShowInsertSymbol = false;
		CodeEditor.ShowLineNumbersInEditor = false;
		((Control)CodeEditor).Size = new Size(552, 261);
		((Control)CodeEditor).TabIndex = 0;
		CodeEditor.TextFont = null;
		ilTool.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilTool.ImageStream");
		ilTool.TransparentColor = Color.Magenta;
		ilTool.Images.SetKeyName(0, "");
		((Control)glButtons).BackColor = Color.White;
		((Control)glButtons).Controls.Add((Control)(object)buttonSwitchToBlocks);
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Controls.Add((Control)(object)btCancel);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = StiLinePosition.Top;
		glButtons.LineStyle = StiLineStyle.Dot;
		((Control)glButtons).Location = new Point(0, 288);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(576, 46);
		((Control)glButtons).TabIndex = 1;
		((Control)buttonSwitchToBlocks).Anchor = (AnchorStyles)6;
		((Control)buttonSwitchToBlocks).AutoSize = true;
		((Control)buttonSwitchToBlocks).Location = new Point(12, 14);
		((Control)buttonSwitchToBlocks).Name = "buttonSwitchToBlocks";
		((Control)buttonSwitchToBlocks).Size = new Size(115, 23);
		((Control)buttonSwitchToBlocks).TabIndex = 2;
		((Control)buttonSwitchToBlocks).Text = "Switch to Blockly";
		((ButtonBase)buttonSwitchToBlocks).UseVisualStyleBackColor = true;
		((Control)buttonSwitchToBlocks).Click += ButtonSwitchToBlocks_Click;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(576, 334);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Control)this).Controls.Add((Control)(object)CodeEditor);
		((Form)this).KeyPreview = true;
		((Control)this).Name = "StiEventEditorForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Event editor";
		((Form)this).Activated += This_Activated;
		((Form)this).Closing += This_Closing;
		((Form)this).Closed += This_Closed;
		((Control)glButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
