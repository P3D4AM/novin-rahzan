using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Controls.Win;

[ToolboxItem(false)]
public class StiColorPipetControl : UserControl
{
	private int previewSize = 40;

	private const int previewX = 0;

	private const int previewY = 0;

	private readonly Timer getColor;

	private Bitmap sampleBitmap;

	private Button btnPickColor;

	private const int sampleSize = 5;

	private bool hasSampled;

	private readonly Color[,] previewColors;

	private ToolTip toolTip;

	private Color selectedColor;

	public Color SelectedColor => selectedColor;

	public event EventHandler SelectedColorChanged;

	public StiColorPipetControl()
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			((Control)this).SetStyle((ControlStyles)8192, true);
			((Control)this).SetStyle((ControlStyles)2, true);
			((Control)this).SetStyle((ControlStyles)4, true);
			((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
			((Control)this).SetStyle((ControlStyles)16, true);
			previewColors = new Color[previewSize, previewSize];
			getColor = new Timer
			{
				Interval = 50
			};
			getColor.Tick += GetColor_Tick;
			using (LocalizationManager localizationManager = new LocalizationManager())
			{
				string localizedString = localizationManager.GetLocalizedString("ClickAndDragPickColor");
				toolTip = new ToolTip();
				toolTip.ShowAlways = true;
				toolTip.SetToolTip((Control)(object)btnPickColor, localizedString);
			}
			previewSize = 40 * (int)StiScale2.Step;
			SetDropperIcon();
		}
	}

	private void InvokeSelectedColorChanged(EventArgs e)
	{
		this.SelectedColorChanged?.Invoke(selectedColor, e);
	}

	private void GetColor_Tick(object sender, EventArgs e)
	{
		int x = Control.MousePosition.X;
		int y = Control.MousePosition.Y;
		Screen screen = Screen.FromRectangle(new Rectangle(x, y * 2, 1, 1));
		Bitmap obj = sampleBitmap;
		if (obj != null)
		{
			((Image)obj).Dispose();
		}
		sampleBitmap = GetSampleRegion(screen, x, y);
		selectedColor = sampleBitmap.GetPixel(2, 2);
		((Control)this).Invalidate(new Rectangle(0, 0, previewSize, previewSize));
	}

	private void ButtonPickColor_MouseDown(object sender, MouseEventArgs e)
	{
		getColor.Start();
		if (!hasSampled)
		{
			((Control)btnPickColor).BackgroundImage = null;
			hasSampled = true;
		}
		((Control)this).Cursor = GetCursor("Stimulsoft.Controls.Win", "Stimulsoft.Controls.Win.PIPETTE.CUR");
	}

	private void ButtonPickColor_MouseUp(object sender, MouseEventArgs e)
	{
		getColor.Stop();
		((Control)this).Cursor = Cursors.Default;
		if (sampleBitmap != null)
		{
			((Image)sampleBitmap).Dispose();
			sampleBitmap = null;
		}
		GC.Collect();
		InvokeSelectedColorChanged(EventArgs.Empty);
		hasSampled = false;
		ClearPreviewColors();
		SetDropperIcon();
	}

	private void SetDropperIcon()
	{
		string text = ((StiScale2.Step >= 2.0) ? "_x2" : "");
		((Control)btnPickColor).BackgroundImage = (Image)(object)BarFunctions.LoadBitmap("Png.dropper" + text + ".png");
	}

	private void ClearPreviewColors()
	{
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				previewColors[i, j] = Color.White;
			}
		}
	}

	public static Cursor GetCursor(string assemblyName, string cursorName)
	{
		Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault((Assembly a) => a.GetName().Name == assemblyName);
		if (assembly != null)
		{
			return GetCursor(assembly, cursorName);
		}
		throw new Exception("Can't find assembly '" + assemblyName + "'");
	}

	private static Cursor GetCursor(Assembly cursorAssembly, string cursorName)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		Stream manifestResourceStream = cursorAssembly.GetManifestResourceStream(cursorName);
		if (manifestResourceStream != null)
		{
			return new Cursor(manifestResourceStream);
		}
		throw new Exception("Can't find cursor '" + cursorName + "' in resources");
	}

	private static Bitmap GetSampleRegion(Screen screen, int mouseX, int mouseY)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		Bitmap val = new Bitmap(5, 5, (PixelFormat)2498570);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		val2.CopyFromScreen(mouseX - 2, mouseY - 2, 0, 0, new Size(5, 5));
		val2.Save();
		val2.Dispose();
		return val;
	}

	protected override void OnPaint(PaintEventArgs paintEvnt)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		Graphics graphics = paintEvnt.Graphics;
		int num = (int)StiScale2.Step;
		Pen val = new Pen(Color.FromArgb(255, 255, 0, 0), (float)num);
		try
		{
			int num2 = previewSize / 5;
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					if (sampleBitmap != null)
					{
						previewColors[i, j] = sampleBitmap.GetPixel(i, j);
					}
					Brush val2 = (Brush)new SolidBrush(previewColors[i, j]);
					graphics.FillRectangle(val2, new Rectangle(i * num2, j * num2, num2, num2));
				}
			}
			if (hasSampled)
			{
				graphics.DrawRectangle(val, num2 * 2, num2 * 2, num2, num2);
			}
			if (sampleBitmap != null)
			{
				((Image)sampleBitmap).Dispose();
				sampleBitmap = null;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void InitializeComponent()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Expected O, but got Unknown
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiColorPipetControl));
		btnPickColor = new Button();
		((Control)this).SuspendLayout();
		((Control)btnPickColor).BackColor = Color.Transparent;
		((Control)btnPickColor).BackgroundImage = (Image)componentResourceManager.GetObject("btnPickColor.BackgroundImage");
		((Control)btnPickColor).BackgroundImageLayout = (ImageLayout)2;
		((Control)btnPickColor).Dock = (DockStyle)5;
		((ButtonBase)btnPickColor).FlatAppearance.BorderColor = Color.FromArgb(100, 100, 100);
		((ButtonBase)btnPickColor).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)btnPickColor).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)btnPickColor).FlatStyle = (FlatStyle)0;
		((Control)btnPickColor).Location = new Point(0, 0);
		((Control)btnPickColor).Margin = new Padding(5);
		((Control)btnPickColor).Name = "btnPickColor";
		((Control)btnPickColor).Size = new Size(150, 150);
		((Control)btnPickColor).TabIndex = 7;
		((Control)btnPickColor).TabStop = false;
		((ButtonBase)btnPickColor).UseVisualStyleBackColor = false;
		((Control)btnPickColor).MouseDown += new MouseEventHandler(ButtonPickColor_MouseDown);
		((Control)btnPickColor).MouseUp += new MouseEventHandler(ButtonPickColor_MouseUp);
		((Control)this).Controls.Add((Control)(object)btnPickColor);
		((Control)this).Name = "StiColorPipetControl";
		((Control)this).ResumeLayout(false);
	}
}
