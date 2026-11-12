using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Design.Globalization;

[ToolboxItem(false)]
public class StiHtmlTextEditorControl : UserControl
{
	private Control editor;

	private IContainer components;

	public TextBox TextBoxEditor
	{
		get
		{
			Control obj = editor;
			return (TextBox)(object)((obj is TextBox) ? obj : null);
		}
	}

	public IStiHtmlTextEditorControl HtmlTextEditor => editor as IStiHtmlTextEditorControl;

	public StiHtmlTextEditorControl()
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		InitializeComponent();
		ref Control reference = ref editor;
		IStiHtmlTextEditorControl htmlTextEditor = StiDashboardDesignAssembly.GetHtmlTextEditor();
		reference = (Control)((htmlTextEditor is Control) ? htmlTextEditor : null);
		if (editor != null)
		{
			IStiHtmlTextEditorControl stiHtmlTextEditorControl = editor as IStiHtmlTextEditorControl;
			stiHtmlTextEditorControl.ShowFontNameBox = false;
			stiHtmlTextEditorControl.ShowFontSizeBox = false;
			stiHtmlTextEditorControl.ShowAlignLeft = false;
			stiHtmlTextEditorControl.ShowAlignCenter = false;
			stiHtmlTextEditorControl.ShowAlignRight = false;
			stiHtmlTextEditorControl.ShowAlignJustify = false;
			stiHtmlTextEditorControl.ShowClearAllFormatting = false;
		}
		else if (editor == null)
		{
			editor = (Control)new TextBox
			{
				Multiline = true
			};
		}
		editor.BackColor = Color.White;
		SetLocation();
		editor.Anchor = (AnchorStyles)13;
		((Control)this).Controls.Add(editor);
	}

	public void SetText(string text)
	{
		if (editor is IStiHtmlTextEditorControl)
		{
			((IStiHtmlTextEditorControl)editor).HtmlText = text;
		}
		else
		{
			editor.Text = text;
		}
	}

	public string GetText()
	{
		if (editor is IStiHtmlTextEditorControl)
		{
			return ((IStiHtmlTextEditorControl)editor).HtmlText;
		}
		return editor.Text;
	}

	private void SetLocation()
	{
		if (editor != null)
		{
			editor.Left = 2;
			editor.Top = 2;
			editor.Width = ((Control)this).Width - 5;
			editor.Height = ((Control)this).Height - 5;
		}
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		((Control)this).OnSizeChanged(e);
		SetLocation();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		((Control)this).OnPaint(e);
		if (editor is IStiHtmlTextEditorControl)
		{
			e.Graphics.DrawRectangle(Pens.DimGray, 0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1);
		}
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
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Name = "StiHtmlTextEditorControl";
		((Control)this).Size = new Size(441, 377);
		((Control)this).ResumeLayout(false);
	}
}
