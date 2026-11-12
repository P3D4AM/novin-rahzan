using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Components.Design;

public class StiReportIconEditorForm : Form
{
	private byte[] reportIcon;

	private Image image;

	private IContainer components;

	private Button buttonRemove;

	private Button buttonOpen;

	private Panel panelImage;

	private ToolTip toolTip;

	private StiGroupLine groupLineButtons;

	private Button buttonCancel;

	private Button buttonOk;

	public byte[] ReportIcon
	{
		get
		{
			if (image == null)
			{
				return null;
			}
			using MemoryStream memoryStream = new MemoryStream();
			image.Save((Stream)memoryStream, ImageFormat.Png);
			memoryStream.Flush();
			return memoryStream.ToArray();
		}
	}

	public StiReportIconEditorForm(byte[] reportIcon)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			this.reportIcon = reportIcon;
			Localize();
			DoScaling();
			CheckStates();
			StiSettings.Load();
			StiFormSettings.Load((Form)(object)this);
			((Form)this).ShowIcon = false;
			((Form)this).Load += This_Load;
		}
	}

	private void Localize()
	{
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)this).Text = StiLocalization.Get("PropertyMain", "Icon");
		toolTip.SetToolTip((Control)(object)buttonOpen, StiLocalization.Get("Buttons", "Open"));
		toolTip.SetToolTip((Control)(object)buttonRemove, StiLocalization.Get("Buttons", "Remove"));
	}

	private void DoScaling()
	{
		((ButtonBase)buttonOpen).Image = StiReportDesignImages.Actions.OpenFile();
		((ButtonBase)buttonRemove).Image = StiReportDesignImages.Actions.Remove();
	}

	internal void CheckStates()
	{
		((Control)buttonRemove).Enabled = ReportIcon != null;
	}

	private void This_Load(object sender, EventArgs e)
	{
		((Form)this).Load -= This_Load;
		if (reportIcon == null)
		{
			return;
		}
		try
		{
			image = (StiImageHelper.IsSvg(reportIcon) ? StiImageHelper.TryFromBytes(reportIcon, 200, 200) : Image.FromStream((Stream)new MemoryStream(reportIcon)));
			CheckStates();
			((Control)panelImage).Invalidate();
		}
		catch
		{
		}
	}

	private void This_Closed(object sender, EventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			((Form)this).Close();
		}
	}

	private void PanelImage_DoubleClick(object sender, EventArgs e)
	{
		ButtonOpen_Click(sender, e);
	}

	private void PanelImage_Paint(object sender, PaintEventArgs e)
	{
		StiImageDialogPainter.Paint((Control)(object)panelImage, e.Graphics, image, drawDragDrop: true, drawBorder: true, isImage: true);
	}

	private void ButtonOpen_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Invalid comparison between Unknown and I4
		OpenFileDialog val = new OpenFileDialog();
		try
		{
			((FileDialog)val).Filter = StiFileDialogHelper.GetImageFilter();
			((FileDialog)val).RestoreDirectory = true;
			if ((int)((CommonDialog)val).ShowDialog() != 1)
			{
				return;
			}
			try
			{
				if (StiMaximumResourceSizeMessageBox.Check((Form)(object)this, ((FileDialog)val).FileName))
				{
					image = StiImageHelper.FromFile(((FileDialog)val).FileName);
					CheckStates();
					((Control)panelImage).Invalidate();
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void ButtonRemove_Click(object sender, EventArgs e)
	{
		image = null;
		CheckStates();
		((Control)panelImage).Invalidate();
	}

	private void PanelImage_DragDrop(object sender, DragEventArgs e)
	{
		string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
		if (array == null)
		{
			return;
		}
		foreach (string item in array.Where((string f) => f != null))
		{
			string ext = Path.GetExtension(item).ToLowerInvariant();
			if (StiResourceTypeHelper.IsImageType(ext))
			{
				if (StiMaximumResourceSizeMessageBox.Check((Form)(object)this, item))
				{
					image = StiImageHelper.TryFromFile(item);
					CheckStates();
					((Control)panelImage).Invalidate();
				}
				break;
			}
		}
	}

	private void PanelImage_DragOver(object sender, DragEventArgs e)
	{
		if (!e.Data.GetDataPresent(DataFormats.FileDrop, false))
		{
			return;
		}
		string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
		if (array != null)
		{
			foreach (string item in array.Where((string f) => f != null))
			{
				string ext = Path.GetExtension(item).ToLowerInvariant();
				if (StiResourceTypeHelper.IsImageType(ext))
				{
					e.Effect = (DragDropEffects)(-2147483645);
					return;
				}
			}
		}
		e.Effect = (DragDropEffects)0;
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
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Expected O, but got Unknown
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Expected O, but got Unknown
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Expected O, but got Unknown
		//IL_051a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0524: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiReportIconEditorForm));
		buttonRemove = new Button();
		buttonOpen = new Button();
		panelImage = new Panel();
		toolTip = new ToolTip(components);
		groupLineButtons = new StiGroupLine();
		buttonCancel = new Button();
		buttonOk = new Button();
		((Control)panelImage).SuspendLayout();
		((Control)groupLineButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)buttonRemove).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonRemove).Image = (Image)componentResourceManager.GetObject("buttonRemove.Image");
		((Control)buttonRemove).Location = new Point(402, 32);
		((Control)buttonRemove).Name = "buttonRemove";
		((Control)buttonRemove).Size = new Size(24, 24);
		((Control)buttonRemove).TabIndex = 3;
		((Control)buttonRemove).TabStop = false;
		((ButtonBase)buttonRemove).UseVisualStyleBackColor = true;
		((Control)buttonRemove).Click += ButtonRemove_Click;
		((Control)buttonOpen).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonOpen).Image = (Image)componentResourceManager.GetObject("buttonOpen.Image");
		((Control)buttonOpen).Location = new Point(402, 6);
		((Control)buttonOpen).Name = "buttonOpen";
		((Control)buttonOpen).Size = new Size(24, 24);
		((Control)buttonOpen).TabIndex = 2;
		((Control)buttonOpen).TabStop = false;
		((ButtonBase)buttonOpen).UseVisualStyleBackColor = true;
		((Control)buttonOpen).Click += ButtonOpen_Click;
		((Control)panelImage).AllowDrop = true;
		((Control)panelImage).Anchor = (AnchorStyles)15;
		((Control)panelImage).Controls.Add((Control)(object)buttonOpen);
		((Control)panelImage).Controls.Add((Control)(object)buttonRemove);
		((Control)panelImage).Location = new Point(12, 12);
		((Control)panelImage).Name = "panelImage";
		((Control)panelImage).Size = new Size(432, 406);
		((Control)panelImage).TabIndex = 1;
		((Control)panelImage).DragDrop += new DragEventHandler(PanelImage_DragDrop);
		((Control)panelImage).DragOver += new DragEventHandler(PanelImage_DragOver);
		((Control)panelImage).Paint += new PaintEventHandler(PanelImage_Paint);
		((Control)panelImage).DoubleClick += PanelImage_DoubleClick;
		((Control)groupLineButtons).BackColor = Color.White;
		((Control)groupLineButtons).Controls.Add((Control)(object)buttonCancel);
		((Control)groupLineButtons).Controls.Add((Control)(object)buttonOk);
		((Control)groupLineButtons).Dock = (DockStyle)2;
		groupLineButtons.LinePosition = StiLinePosition.Top;
		groupLineButtons.LineStyle = StiLineStyle.Dot;
		((Control)groupLineButtons).Location = new Point(0, 432);
		((Control)groupLineButtons).Name = "groupLineButtons";
		((Control)groupLineButtons).Size = new Size(456, 46);
		((Control)groupLineButtons).TabIndex = 0;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(370, 12);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 1;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		buttonOk.DialogResult = (DialogResult)1;
		((Control)buttonOk).Location = new Point(289, 12);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 0;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(456, 478);
		((Control)this).Controls.Add((Control)(object)groupLineButtons);
		((Control)this).Controls.Add((Control)(object)panelImage);
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiReportIconEditorForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Image";
		((Form)this).Closed += This_Closed;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)panelImage).ResumeLayout(false);
		((Control)groupLineButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
