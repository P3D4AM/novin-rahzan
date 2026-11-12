using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Components.TextFormats.Design;

public class StiTextFormatEditorForm : StiFormUX
{
	private IStiTextFormat textComponent;

	private StiDesignerControl designer;

	private IContainer components;

	private StiButtonUX buttonOk;

	private StiButtonUX buttonCancel;

	private StiOffice2013GroupBox groupBoxSample;

	private StiOffice2013GroupBox groupBoxProperties;

	private StiLabelUX lbSample;

	private StiOffice2013GroupBox groupBoxFormats;

	private StiListBoxV2 listBoxFormats;

	private StiButtonPanelUX glButtons;

	public StiTextFormatEditorForm(IStiTextFormat textComponent, StiDesignerControl designer)
	{
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			((Form)this).ShowIcon = false;
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			this.textComponent = textComponent;
			this.designer = designer;
			if (textComponent == null)
			{
				textComponent.TextFormat = new StiGeneralFormatService();
			}
			Build();
			Localize();
		}
	}

	private void Localize()
	{
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)this).Text = StiLocalization.Get("FormFormatEditor", "title");
		((Control)groupBoxFormats).Text = StiLocalization.Get("FormFormatEditor", "Formats");
		((Control)groupBoxSample).Text = StiLocalization.Get("FormFormatEditor", "Sample");
		((Control)groupBoxProperties).Text = StiLocalization.Get("FormFormatEditor", "Properties");
	}

	public StiFormatEditor GetFormatEditor(StiFormatService formatService)
	{
		if (!(formatService.GetType().GetCustomAttributes(typeof(StiFormatEditorAttribute), inherit: true) is StiFormatEditorAttribute[] array) || array.Length == 0)
		{
			return null;
		}
		string editorTypeName = array[0].EditorTypeName;
		Type type = Type.GetType(editorTypeName);
		return StiActivator.CreateObject(type, new object[2] { formatService, designer }) as StiFormatEditor;
	}

	private void Build()
	{
		int num = 0;
		foreach (StiFormatService item in StiOptions.Services.Formats.Where((StiFormatService s) => s.ServiceEnabled))
		{
			if (textComponent.TextFormat != null && item.GetType() == textComponent.TextFormat.GetType() && item.ServiceName == textComponent.TextFormat.ServiceName)
			{
				((ListBox)listBoxFormats).Items.Add(textComponent.TextFormat.Clone());
				((ListControl)listBoxFormats).SelectedIndex = num;
			}
			else
			{
				((ListBox)listBoxFormats).Items.Add(item.Clone());
			}
			num++;
		}
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		if (((ArrangedElementCollection)((Control)groupBoxProperties).Controls).Count > 0 && ((Control)groupBoxProperties).Controls[0] is StiFormatEditor stiFormatEditor)
		{
			stiFormatEditor.ApplyChanges();
		}
		if (designer != null)
		{
			designer.UndoRedoSave(StiLocalization.Get("FormFormatEditor", "title"));
			designer.Report.IsModified = true;
		}
		textComponent.TextFormat = (StiFormatService)((ICloneable)((ListBox)listBoxFormats).SelectedItem).Clone();
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/report_internals_text_formatting.htm");
	}

	private void Editor_UpdateSample(object sender, EventArgs e)
	{
		StiFormatService stiFormatService = ((ListBox)listBoxFormats).SelectedItem as StiFormatService;
		if (stiFormatService.IsFormatStringFromVariable)
		{
			return;
		}
		CultureInfo cultureInfo = null;
		try
		{
			if (!string.IsNullOrWhiteSpace(designer?.Report?.Culture))
			{
				cultureInfo = Thread.CurrentThread.CurrentCulture;
				Thread.CurrentThread.CurrentCulture = new CultureInfo(designer.Report.Culture);
			}
			((Control)lbSample).Text = stiFormatService.Format(stiFormatService.Sample);
		}
		finally
		{
			if (cultureInfo != null)
			{
				Thread.CurrentThread.CurrentCulture = cultureInfo;
			}
		}
	}

	private void ListBoxFormats_SelectedIndexChanged(object sender, EventArgs e)
	{
		Editor_UpdateSample(sender, e);
		StiFormatService formatService = ((ListBox)listBoxFormats).SelectedItem as StiFormatService;
		StiFormatEditor formatEditor = GetFormatEditor(formatService);
		((Control)groupBoxProperties).Controls.Clear();
		if (formatEditor != null)
		{
			((Control)groupBoxProperties).Controls.Add((Control)(object)formatEditor);
			((Control)formatEditor).Dock = (DockStyle)5;
			formatEditor.UpdateSample += Editor_UpdateSample;
			if (StiUX.IsDark)
			{
				((Control)formatEditor).BackColor = StiUX.Background;
			}
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
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_0422: Unknown result type (might be due to invalid IL or missing references)
		buttonOk = new StiButtonUX();
		buttonCancel = new StiButtonUX();
		groupBoxFormats = new StiOffice2013GroupBox();
		listBoxFormats = new StiListBoxV2();
		groupBoxSample = new StiOffice2013GroupBox();
		lbSample = new StiLabelUX();
		groupBoxProperties = new StiOffice2013GroupBox();
		glButtons = new StiButtonPanelUX();
		((Control)groupBoxFormats).SuspendLayout();
		((Control)groupBoxSample).SuspendLayout();
		((Control)glButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		((Control)buttonOk).Location = new Point(284, 12);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 25);
		((Control)buttonOk).TabIndex = 0;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		((Button)buttonCancel).DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(366, 12);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 25);
		((Control)buttonCancel).TabIndex = 1;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonCancel).Click += This_CancelClick;
		((Control)groupBoxFormats).Anchor = (AnchorStyles)7;
		((Control)groupBoxFormats).Controls.Add((Control)(object)listBoxFormats);
		groupBoxFormats.HeaderHeight = 24;
		((Control)groupBoxFormats).Location = new Point(12, 12);
		((Control)groupBoxFormats).Name = "groupBoxFormats";
		((Control)groupBoxFormats).Size = new Size(145, 484);
		((Control)groupBoxFormats).TabIndex = 2;
		((Control)groupBoxFormats).TabStop = false;
		((Control)groupBoxFormats).Text = "Formats";
		((Control)listBoxFormats).BackColor = SystemColors.Window;
		((Control)listBoxFormats).ForeColor = SystemColors.ControlText;
		((Control)listBoxFormats).Location = new Point(0, 40);
		((Control)listBoxFormats).Margin = new Padding(2);
		((Control)listBoxFormats).Name = "listBoxFormats";
		((Control)listBoxFormats).Size = new Size(145, 357);
		((Control)listBoxFormats).TabIndex = 0;
		((ListBox)listBoxFormats).SelectedIndexChanged += ListBoxFormats_SelectedIndexChanged;
		((Control)groupBoxSample).Anchor = (AnchorStyles)13;
		((Control)groupBoxSample).Controls.Add((Control)(object)lbSample);
		groupBoxSample.HeaderHeight = 24;
		((Control)groupBoxSample).Location = new Point(170, 12);
		((Control)groupBoxSample).Name = "groupBoxSample";
		((Control)groupBoxSample).Size = new Size(271, 72);
		((Control)groupBoxSample).TabIndex = 3;
		((Control)groupBoxSample).TabStop = false;
		((Control)groupBoxSample).Text = "Sample";
		((Control)lbSample).Anchor = (AnchorStyles)13;
		((Control)lbSample).ForeColor = SystemColors.ControlText;
		((Control)lbSample).Location = new Point(12, 40);
		((Control)lbSample).Name = "lbSample";
		((Control)lbSample).Size = new Size(246, 24);
		((Control)lbSample).TabIndex = 0;
		((Label)lbSample).TextAlign = (ContentAlignment)16;
		((Control)groupBoxProperties).Anchor = (AnchorStyles)15;
		((Control)groupBoxProperties).BackColor = Color.White;
		groupBoxProperties.HeaderHeight = 24;
		((Control)groupBoxProperties).Location = new Point(170, 90);
		((Control)groupBoxProperties).Name = "groupBoxProperties";
		((Control)groupBoxProperties).Padding = new Padding(0, 28, 0, 0);
		((Control)groupBoxProperties).Size = new Size(271, 406);
		((Control)groupBoxProperties).TabIndex = 4;
		((Control)groupBoxProperties).TabStop = false;
		((Control)groupBoxProperties).Text = "Properties";
		((Control)glButtons).BackColor = Color.White;
		((Control)glButtons).Controls.Add((Control)(object)buttonCancel);
		((Control)glButtons).Controls.Add((Control)(object)buttonOk);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = StiLinePosition.Top;
		glButtons.LineStyle = StiLineStyle.Dot;
		((Control)glButtons).Location = new Point(0, 515);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(454, 46);
		((Control)glButtons).TabIndex = 5;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(454, 561);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Control)this).Controls.Add((Control)(object)groupBoxProperties);
		((Control)this).Controls.Add((Control)(object)groupBoxSample);
		((Control)this).Controls.Add((Control)(object)groupBoxFormats);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiTextFormatEditorForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Format";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Control)groupBoxFormats).ResumeLayout(false);
		((Control)groupBoxSample).ResumeLayout(false);
		((Control)glButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
