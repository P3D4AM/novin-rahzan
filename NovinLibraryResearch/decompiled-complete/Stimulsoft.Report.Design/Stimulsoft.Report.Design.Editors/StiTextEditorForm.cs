using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Design.Editors;

public class StiTextEditorForm : StiFormUX
{
	private StiReport report;

	private bool allowPackExpression;

	private StiCodeEditorMode editorMode;

	private IContainer components;

	private ImageList ilTool;

	private StiButtonUX btOk;

	private StiButtonUX btCancel;

	private StiCodeEditor CodeEditor;

	private StiButtonPanelUX glButtons;

	private StiLabelUX labelSample;

	private StiLabelUX labelSample2;

	[Description("Returns edited text.")]
	public string TextValue { get; private set; } = "";


	private string CodeEditorText
	{
		get
		{
			if (report == null || !allowPackExpression)
			{
				return ((Control)CodeEditor).Text;
			}
			return StiExpressionPacker.UnPackExpression(((Control)CodeEditor).Text, report, useBraces: true);
		}
		set
		{
			((Control)CodeEditor).Text = ((report != null && allowPackExpression) ? StiExpressionPacker.PackExpression(value, report.Designer, useBraces: true) : value);
		}
	}

	public StiTextEditorForm(StiReport report, string text, StiCodeEditorMode editorMode, bool allowPackExpression = true, string[] hints = null)
	{
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (((Component)(object)this).DesignMode)
		{
			return;
		}
		if (StiOptions.Appearance.ShowFormsHelp)
		{
			((Form)this).HelpButton = true;
		}
		this.report = report;
		this.allowPackExpression = allowPackExpression;
		CodeEditor.Init(report, editorMode);
		CodeEditorText = text;
		TextValue = text;
		this.editorMode = editorMode;
		if (hints == null)
		{
			((Control)CodeEditor).Height = ((Control)labelSample).Bottom - ((Control)CodeEditor).Top;
		}
		else
		{
			Font font = ((Control)labelSample2).Font;
			((Control)labelSample2).Parent.Controls.Remove((Control)(object)labelSample2);
			int num = ((Control)labelSample).Right + StiScaleUI.I4;
			int top = ((Control)labelSample).Top;
			int num2 = 0;
			foreach (string text2 in hints)
			{
				string text3 = text2;
				if (num2 != hints.Length - 1)
				{
					text3 += ",";
				}
				Size size = TextRenderer.MeasureText(text3, font);
				StiSampleLabelUX stiSampleLabelUX = new StiSampleLabelUX();
				((Control)stiSampleLabelUX).Text = text3;
				((Control)stiSampleLabelUX).Anchor = ((Control)labelSample).Anchor;
				((Control)stiSampleLabelUX).ForeColor = Color.DimGray;
				((Control)stiSampleLabelUX).Font = font;
				((Control)stiSampleLabelUX).Bounds = new Rectangle(num, top, size.Width + StiScaleUI.I2, ((Control)labelSample).Height);
				StiSampleLabelUX stiSampleLabelUX2 = stiSampleLabelUX;
				((Control)stiSampleLabelUX2).MouseDown += (MouseEventHandler)delegate(object sender, MouseEventArgs e)
				{
					CodeEditorText = RemoveCommaAtEnd(((Control)((sender is Label) ? sender : null)).Text);
				};
				((Control)this).Controls.Add((Control)(object)stiSampleLabelUX2);
				num += size.Width;
				num2++;
			}
		}
		((Control)labelSample).Visible = (((Control)labelSample2).Visible = hints != null);
		Localize();
		if ((editorMode & StiCodeEditorMode.ExpressionTitle) > (StiCodeEditorMode)0)
		{
			((Control)this).Text = Loc.Get("FormTitles", "ExpressionEditorForm");
		}
		if ((editorMode & StiCodeEditorMode.OkButtonOnly) > (StiCodeEditorMode)0)
		{
			((Control)btOk).Visible = false;
			((Control)btCancel).Text = StiLocalization.Get("Buttons", "Ok");
		}
		StiSettings.Load();
		StiFormSettings.Load((Form)(object)this);
		if (CodeEditor.DictionaryControl != null)
		{
			((Control)CodeEditor.DictionaryControl).Width = StiSettings.GetInt("StiTextEditorForm", "Spl", ((Control)CodeEditor.DictionaryControl).Width);
		}
		((Form)this).ShowIcon = false;
		((Control)CodeEditor.Editor).Focus();
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("FormTitles", "TextEditorForm");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)labelSample).Text = Loc.Get("FormFormatEditor", "Sample") + ":";
	}

	private static string RemoveCommaAtEnd(string text)
	{
		text = text.Trim();
		if (text.EndsWith(","))
		{
			text = text.Substring(0, text.Length - 1);
		}
		return text;
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/report_internals_expressions.htm");
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		TextValue = CodeEditorText;
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
	}

	private void This_Activated(object sender, EventArgs e)
	{
		((Control)CodeEditor).Focus();
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			((Form)this).DialogResult = (DialogResult)2;
		}
	}

	private void This_Closed(object sender, EventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
		if (CodeEditor.DictionaryControl != null)
		{
			StiSettings.Load();
			StiSettings.Set("StiTextEditorForm", "Spl", ((Control)CodeEditor.DictionaryControl).Width);
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
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Invalid comparison between Unknown and I4
		if (CodeEditorText != TextValue && (editorMode & StiCodeEditorMode.OkButtonOnly) == 0 && (editorMode & StiCodeEditorMode.IgnoreAskOnClosing) == 0)
		{
			e.Cancel = (int)MessageBox.Show(StiLocalization.Get("DesignerFx", "CloseEditor"), ((Control)this).Text, (MessageBoxButtons)4) == 7;
		}
		if (CodeEditor.Editor.SearchDialog.Visible)
		{
			CodeEditor.Editor.SearchDialog.Close();
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
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Expected O, but got Unknown
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Expected O, but got Unknown
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f8: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiTextEditorForm));
		ilTool = new ImageList(components);
		btOk = new StiButtonUX();
		btCancel = new StiButtonUX();
		glButtons = new StiButtonPanelUX();
		labelSample = new StiLabelUX();
		labelSample2 = new StiLabelUX();
		CodeEditor = new StiCodeEditor();
		((Control)glButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		ilTool.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilTool.ImageStream");
		ilTool.TransparentColor = Color.Magenta;
		ilTool.Images.SetKeyName(0, "");
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(474, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(555, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btCancel).Click += This_CancelClick;
		((Control)glButtons).BackColor = Color.White;
		((Control)glButtons).Controls.Add((Control)(object)btCancel);
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = StiLinePosition.Top;
		glButtons.LineStyle = StiLineStyle.Dot;
		((Control)glButtons).Location = new Point(0, 321);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(642, 46);
		((Control)glButtons).TabIndex = 1;
		((Control)labelSample).Anchor = (AnchorStyles)6;
		((Control)labelSample).AutoSize = true;
		((Control)labelSample).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelSample).ForeColor = Color.DimGray;
		((Control)labelSample).Location = new Point(13, 294);
		((Control)labelSample).Name = "labelSample";
		((Control)labelSample).Size = new Size(46, 12);
		((Control)labelSample).TabIndex = 19;
		((Control)labelSample).Text = "Sample:";
		((Label)labelSample).TextAlign = (ContentAlignment)16;
		((Control)labelSample).Visible = false;
		((Control)labelSample2).Anchor = (AnchorStyles)6;
		((Control)labelSample2).AutoSize = true;
		((Control)labelSample2).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)labelSample2).ForeColor = Color.DimGray;
		((Control)labelSample2).Location = new Point(64, 294);
		((Control)labelSample2).Name = "labelSample2";
		((Control)labelSample2).Size = new Size(229, 13);
		((Control)labelSample2).TabIndex = 18;
		((Control)labelSample2).Text = "123; My text; 567f; 456.23f; Test String; A";
		((Label)labelSample2).TextAlign = (ContentAlignment)16;
		((Control)labelSample2).Visible = false;
		CodeEditor.AllowOutlining = false;
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
		((Control)CodeEditor).Size = new Size(618, 274);
		((Control)CodeEditor).TabIndex = 0;
		CodeEditor.TextFont = null;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(642, 367);
		((Control)this).Controls.Add((Control)(object)labelSample);
		((Control)this).Controls.Add((Control)(object)labelSample2);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Control)this).Controls.Add((Control)(object)CodeEditor);
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiTextEditorForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).SizeGripStyle = (SizeGripStyle)2;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Text editor";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Activated += This_Activated;
		((Form)this).Closing += This_Closing;
		((Form)this).Closed += This_Closed;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)glButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
