using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiDownloadResourceButton : Panel
{
	private double stepScale = StiScaleUI.Step;

	private string stepScaleName = StiScaleUI.StepName;

	private PictureBox icon;

	private Label label;

	private Label labelDescription;

	private Panel button;

	private ContextMenuStrip menu;

	private ToolStripMenuItem itemView;

	private ToolStripMenuItem itemSave;

	private SaveFileDialog saveFileDialog;

	private ToolTip toolTip;

	private bool isMouseEnter;

	private bool isButtonMouseEnter;

	private string toolTipCaption;

	private string text;

	private Color backgroundColor = Color.White;

	public override string Text
	{
		get
		{
			return text;
		}
		set
		{
			toolTipCaption = ((ToolStripItem)itemView).Text + " " + value;
			text = value;
			((Control)label).Text = value;
			InitializeToolTips();
			MeasureWidth();
		}
	}

	public string Description
	{
		get
		{
			return ((Control)labelDescription).Text;
		}
		set
		{
			((Control)labelDescription).Text = value;
			MeasureWidth();
		}
	}

	public byte[] Content { get; set; }

	public string ContentType
	{
		get
		{
			return ((Control)icon).Name;
		}
		set
		{
			((Control)icon).Name = value;
			icon.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Controls", "Stimulsoft.Controls.Bmp.Resources." + value + stepScaleName + ".png");
		}
	}

	public string FileExtension { get; set; }

	public string SaveDialogFilter { get; set; }

	public Color BackgroundColor
	{
		get
		{
			return backgroundColor;
		}
		set
		{
			backgroundColor = value;
			((Control)this).BackColor = value;
			((Control)button).BackColor = value;
		}
	}

	public Color BorderColor { get; set; } = Color.Silver;


	public Color HighlightColor { get; set; } = Color.Gainsboro;


	private void OnSizeChanged(object sender, EventArgs e)
	{
		((Control)label).Width = ((Control)this).Width - (int)Math.Ceiling(74.0 * StiScaleUI.X);
		((Control)button).Left = ((Control)this).Width - (int)Math.Ceiling(28.0 * StiScaleUI.X);
	}

	private void OnPaint(object sender, PaintEventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		Color color = StiColorUtils.Dark(isMouseEnter ? HighlightColor : BorderColor, 30);
		Pen val = new Pen(color, 1f);
		e.Graphics.DrawRectangle(val, 0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1);
	}

	private void OnMouseEnter(object sender, EventArgs e)
	{
		isMouseEnter = true;
		((Control)this).BackColor = StiColorUtils.Light(HighlightColor, 20);
	}

	private void OnMouseLeave(object sender, EventArgs e)
	{
		isMouseEnter = false;
		((Control)this).BackColor = BackgroundColor;
	}

	private void OnSaveClick(object sender, EventArgs e)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Invalid comparison between Unknown and I4
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		if (!(e is MouseEventArgs) || (int)((MouseEventArgs)e).Button == 1048576)
		{
			if (saveFileDialog == null)
			{
				saveFileDialog = new SaveFileDialog();
				((FileDialog)saveFileDialog).FileOk += OnSaveFileDialogFileOk;
			}
			((FileDialog)saveFileDialog).FileName = GetFileName();
			((FileDialog)saveFileDialog).Filter = SaveDialogFilter;
			((CommonDialog)saveFileDialog).ShowDialog();
		}
	}

	private void OnViewClick(object sender, EventArgs e)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Invalid comparison between Unknown and I4
		if (!(e is MouseEventArgs) || (int)((MouseEventArgs)e).Button == 1048576)
		{
			string tempPath = Path.GetTempPath();
			string fileName = GetFileName();
			string text = Path.Combine(tempPath, fileName);
			File.WriteAllBytes(text, Content);
			StiProcess.Start(text);
		}
	}

	private void OnButtonPaint(object sender, PaintEventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		if (isButtonMouseEnter)
		{
			Color color = StiColorUtils.Dark(HighlightColor, 30);
			Pen val = new Pen(color, 1f);
			e.Graphics.DrawRectangle(val, 0, 0, ((Control)button).Width - 1, ((Control)button).Height - 1);
		}
		Bitmap image = StiImageUtils.GetImage("Stimulsoft.Controls", "Stimulsoft.Controls.Bmp.DropDown" + stepScaleName + ".png");
		e.Graphics.DrawImage((Image)(object)image, (((Control)button).Width - ((Image)image).Width) / 2, (((Control)button).Height - ((Image)image).Height) / 2, ((Image)image).Width, ((Image)image).Height);
	}

	private void OnButtonMouseEnter(object sender, EventArgs e)
	{
		isButtonMouseEnter = true;
		((Control)button).BackColor = StiColorUtils.Light(HighlightColor, 20);
		OnMouseEnter(sender, e);
	}

	private void OnButtonMouseLeave(object sender, EventArgs e)
	{
		isButtonMouseEnter = false;
		((Control)button).BackColor = BackgroundColor;
		OnMouseLeave(sender, e);
	}

	private void OnButtonClick(object sender, EventArgs e)
	{
		((ToolStripDropDown)((Control)this).ContextMenuStrip).Show((Control)(object)button, new Point(0, 0));
	}

	private void OnSaveFileDialogFileOk(object sender, CancelEventArgs e)
	{
		if (!string.IsNullOrEmpty(((FileDialog)saveFileDialog).FileName))
		{
			byte[] bytes = ((ContentType.StartsWith("Report") && (((FileDialog)saveFileDialog).FileName.EndsWith(".mrz") || ((FileDialog)saveFileDialog).FileName.EndsWith(".mdz"))) ? StiGZipHelper.Pack(Content) : Content);
			File.WriteAllBytes(((FileDialog)saveFileDialog).FileName, bytes);
		}
	}

	private void MeasureWidth()
	{
		Graphics val = ((Control)this).CreateGraphics();
		try
		{
			float num = val.MeasureString(((Control)this).Text, ((Control)this).Font).Width;
			if (num > (float)(int)Math.Ceiling(125.0 * StiScaleUI.X))
			{
				((Control)label).Text = GetMeasureText(((Control)this).Text, ((Control)label).Font, (int)Math.Ceiling(125.0 * StiScaleUI.X));
				num = (int)Math.Ceiling(125.0 * StiScaleUI.X);
			}
			float width = val.MeasureString(Description, ((Control)this).Font).Width;
			((Control)this).Width = (int)Math.Max(num, width) + (int)Math.Ceiling(75.0 * StiScaleUI.X);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	internal static string GetMeasureText(string text, Font font, double maxWidth)
	{
		for (int i = 0; text.Length > i; i++)
		{
			string text2 = text.Substring(0, i);
			if (maxWidth < (double)TextRenderer.MeasureText(text2, font).Width)
			{
				if (text2.Length <= 6)
				{
					return text2;
				}
				return text2.Substring(0, text2.Length - 3) + "...";
			}
		}
		return text;
	}

	private string GetFileName()
	{
		char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
		string text = string.Join("_", ((Control)this).Text.Split(invalidFileNameChars, StringSplitOptions.RemoveEmptyEntries)).TrimEnd(new char[1] { '.' });
		if (!string.IsNullOrEmpty(FileExtension))
		{
			return text + "." + FileExtension;
		}
		return text;
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Expected O, but got Unknown
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Expected O, but got Unknown
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Expected O, but got Unknown
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Expected O, but got Unknown
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Expected O, but got Unknown
		icon = new PictureBox();
		((Control)icon).Left = (int)Math.Ceiling(4.0 * StiScaleUI.X);
		((Control)icon).Top = (int)Math.Ceiling(4.0 * StiScaleUI.Y);
		((Control)icon).Width = (int)Math.Ceiling(32.0 * stepScale);
		((Control)icon).Height = (int)Math.Ceiling(32.0 * stepScale);
		((Control)this).Controls.Add((Control)(object)icon);
		label = new Label();
		((Control)label).Left = (int)Math.Ceiling(42.0 * StiScaleUI.X);
		((Control)label).Top = (int)Math.Ceiling(4.0 * StiScaleUI.Y);
		((Control)label).Height = (int)Math.Ceiling(13.0 * StiScaleUI.Y);
		((Control)this).Controls.Add((Control)(object)label);
		labelDescription = new Label();
		((Control)labelDescription).Left = (int)Math.Ceiling(42.0 * StiScaleUI.X);
		((Control)labelDescription).Top = ((Control)label).Bottom + (int)Math.Ceiling(1.0 * StiScaleUI.Y);
		((Control)labelDescription).Height = (int)Math.Ceiling(13.0 * StiScaleUI.Y);
		((Control)labelDescription).AutoSize = true;
		((Control)this).Controls.Add((Control)(object)labelDescription);
		button = new Panel();
		((Control)button).Width = (int)Math.Ceiling(22.0 * StiScaleUI.X);
		((Control)button).Height = (int)Math.Ceiling(22.0 * StiScaleUI.Y);
		((Control)button).Left = ((Control)this).Width - (int)Math.Ceiling(30.0 * StiScaleUI.X);
		((Control)button).Top = (int)Math.Ceiling((40.0 * stepScale - (double)((Control)button).Height) / 2.0);
		((Control)button).BackColor = BackgroundColor;
		((Control)this).Controls.Add((Control)(object)button);
		itemView = new ToolStripMenuItem();
		itemSave = new ToolStripMenuItem();
		menu = new ContextMenuStrip();
		((ToolStrip)menu).Items.Add((ToolStripItem)(object)itemView);
		((ToolStrip)menu).Items.Add((ToolStripItem)(object)itemSave);
		toolTip = new ToolTip();
		toolTip.ShowAlways = true;
		InitializeEvents();
		Localize();
	}

	private void InitializeEvents()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Expected O, but got Unknown
		((Control)this).Paint += new PaintEventHandler(OnPaint);
		((Control)this).SizeChanged += OnSizeChanged;
		((Control)this).MouseEnter += OnMouseEnter;
		((Control)this).MouseLeave += OnMouseLeave;
		((Control)this).Click += OnViewClick;
		((Control)icon).MouseEnter += OnMouseEnter;
		((Control)icon).MouseLeave += OnMouseLeave;
		((Control)icon).Click += OnViewClick;
		((Control)label).MouseEnter += OnMouseEnter;
		((Control)label).MouseLeave += OnMouseLeave;
		((Control)label).Click += OnViewClick;
		((Control)labelDescription).MouseEnter += OnMouseEnter;
		((Control)labelDescription).MouseLeave += OnMouseLeave;
		((Control)labelDescription).Click += OnViewClick;
		((Control)button).Paint += new PaintEventHandler(OnButtonPaint);
		((Control)button).MouseEnter += OnButtonMouseEnter;
		((Control)button).MouseLeave += OnButtonMouseLeave;
		((Control)button).Click += OnButtonClick;
		((ToolStripItem)itemView).Click += OnViewClick;
		((ToolStripItem)itemSave).Click += OnSaveClick;
	}

	private void Localize()
	{
		((ToolStripItem)itemView).Text = StiLocalization.Get("Cloud", "ButtonView");
		((ToolStripItem)itemSave).Text = StiLocalization.Get("Cloud", "SaveFile");
		InitializeToolTips();
	}

	private void InitializeToolTips()
	{
		toolTip.SetToolTip((Control)(object)this, toolTipCaption);
		toolTip.SetToolTip((Control)(object)icon, toolTipCaption);
		toolTip.SetToolTip((Control)(object)label, toolTipCaption);
		toolTip.SetToolTip((Control)(object)labelDescription, toolTipCaption);
	}

	public StiDownloadResourceButton()
	{
		InitializeComponent();
		((Control)this).ContextMenuStrip = menu;
		((Control)this).Height = (int)Math.Ceiling(40.0 * stepScale);
	}
}
