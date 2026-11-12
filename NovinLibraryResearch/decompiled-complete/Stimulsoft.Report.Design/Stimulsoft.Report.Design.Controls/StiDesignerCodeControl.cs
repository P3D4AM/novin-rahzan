using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Editor;
using Stimulsoft.Report.Design.Editors;

namespace Stimulsoft.Report.Design.Controls;

[ToolboxItem(false)]
public class StiDesignerCodeControl : UserControl
{
	public StiCodeEditor CodeEditor;

	private Container components;

	private StiDesignerControl designer;

	public StiDesignerCodeControl(StiDesignerControl designer)
	{
		this.designer = designer;
		InitializeComponent();
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
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Name = "StiDesignerCodeControl";
		((Control)this).Size = new Size(680, 560);
		((Control)this).ResumeLayout(false);
	}

	internal void CheckCodeControl()
	{
		if (CodeEditor == null)
		{
			CodeEditor = new StiCodeEditor();
			CodeEditor.Init(designer.Report, StiCodeEditorMode.EditorReportCode);
			((Control)CodeEditor).Dock = (DockStyle)5;
			CodeEditor.CollapseToDefinitions();
			((Control)CodeEditor).TabIndex = 1;
			((Control)this).Controls.Add((Control)(object)CodeEditor);
		}
		CheckCodeEditorLanguage();
	}

	private void CheckCodeEditorLanguage()
	{
		if (designer == null || designer.Report == null)
		{
			return;
		}
		if (designer.Report.ScriptLanguage == StiReportLanguageType.CSharp || designer.Report.ScriptLanguage == StiReportLanguageType.JS)
		{
			if (CodeEditor.Editor.Lexer is VbParser || CodeEditor.Editor.Lexer == null)
			{
				CodeEditor.Editor.Lexer = new CsParser();
			}
		}
		else if (CodeEditor.Editor.Lexer is CsParser || CodeEditor.Editor.Lexer == null)
		{
			CodeEditor.Editor.Lexer = new VbParser();
		}
	}
}
