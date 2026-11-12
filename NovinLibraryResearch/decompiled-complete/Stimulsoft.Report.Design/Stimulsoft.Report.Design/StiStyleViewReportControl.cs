using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Security.Cryptography;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Design;

[ToolboxItem(false)]
public class StiStyleViewReportControl : UserControl
{
	private float factor;

	private Color borderColor;

	private Color defaultColor = Color.FromArgb(216, 216, 216);

	private List<string> columnLong = new List<string>();

	private List<string> columnShort = new List<string>();

	private List<string> columnPrice = new List<string>();

	private IContainer components;

	public Color BaseColor { get; set; } = Color.Transparent;


	public int MaxNestedLevel { get; set; } = 1;


	public StiNestedFactor NestedFactor { get; set; } = StiNestedFactor.Low;


	private float ColorFactor
	{
		get
		{
			if (NestedFactor == StiNestedFactor.High)
			{
				return 1.5f;
			}
			if (NestedFactor == StiNestedFactor.Normal)
			{
				return 1f;
			}
			return 0.5f;
		}
	}

	public bool Borders { get; set; } = true;


	public bool GroupHeader { get; set; } = true;


	public bool GroupFooter { get; set; } = true;


	public bool Header { get; set; } = true;


	public bool Data { get; set; } = true;


	public bool Footer { get; set; } = true;


	public bool ReportTitle { get; set; } = true;


	public bool ReportSummary { get; set; } = true;


	public bool PageHeader { get; set; } = true;


	public bool PageFooter { get; set; } = true;


	public StiStyleViewReportControl()
	{
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			Random random = new Random();
			for (int i = 0; i < 30; i++)
			{
				int num = random.Next(8);
				int num2 = random.Next(4);
				columnLong.Add(RandomBigLetter(1) + RandomSmallLetter(12 - num));
				columnShort.Add(RandomBigLetter(1) + RandomSmallLetter(7 - num2));
				columnPrice.Add("$" + RandomNumber(2));
			}
		}
	}

	private void This_Paint(object sender, PaintEventArgs e)
	{
		Build(e.Graphics);
	}

	private void Build(Graphics graphic)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		if (BaseColor.IsEmpty || BaseColor == Color.FromArgb(0, 0, 0, 0))
		{
			BaseColor = Color.White;
		}
		Brush val = (Brush)new SolidBrush(((Control)this).BackColor);
		try
		{
			graphic.FillRectangle(val, 0, 0, ((Control)this).Width, ((Control)this).Height);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		factor = (float)(BaseColor.R + BaseColor.G + BaseColor.B) / 3f;
		borderColor = StiColorUtils.Dark(BaseColor, (byte)(100f * ColorFactor));
		CreatePageHeader("Page 1 of 1", graphic, ((Control)this).Width - StiScale.XXI(40.0), StiScale.I10);
		CreatePageFooter("Page 1 of 1", graphic);
		CreateReportTitle("Collection Styles                                                     Stimulsoft", graphic, StiScale.I10, StiScale.YYI(23.0));
		CreateReportTitle("INVOICE", graphic, StiScale.XXI(90.0), StiScale.YYI(33.0));
		int num = StiScale.YYI(43.0);
		CreateHeader("Unit Name                Description         Qty         Item Price              Total", StiScale.I10, num, graphic);
		for (int i = 1; i < 10; i++)
		{
			num += StiScale.I10;
			bool odd = i % 2 != 0;
			CreateData(columnLong[i], columnShort[i], columnPrice[i - 1], columnPrice[i], columnPrice[i + 3], StiScale.I10, num, odd, graphic);
		}
		num += StiScale.I10;
		CreateFooter("Items per page: 9", StiScale.I10, num, graphic);
		num += StiScale.I12;
		CreateReportSummary("Total: 9562,0", graphic, StiScale.XXI(158.0), num);
	}

	private void CreatePageHeader(string text, Graphics g, int x, int y)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		Color color = ((!PageHeader) ? defaultColor : ((factor > 150f) ? StiColorUtils.Dark(BaseColor, 200) : BaseColor));
		Font val = new Font("Arial", 4f);
		try
		{
			Brush val2 = (Brush)new SolidBrush(color);
			try
			{
				g.DrawString(text, val, val2, (float)x, (float)y);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void CreatePageFooter(string text, Graphics g)
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		Color color = ((!PageFooter) ? defaultColor : ((factor > 150f) ? StiColorUtils.Dark(BaseColor, 200) : BaseColor));
		float num = ((Control)this).Width - StiScale.XXI(40.0);
		float num2 = ((Control)this).Height - StiScale.YYI(17.0);
		Font val = new Font("Arial", 4f);
		try
		{
			Brush val2 = (Brush)new SolidBrush(color);
			try
			{
				g.DrawString(text, val, val2, num, num2);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void CreateReportTitle(string text, Graphics g, int x, int y)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		Color color = ((!ReportTitle) ? defaultColor : ((factor > 150f) ? StiColorUtils.Dark(BaseColor, 200) : BaseColor));
		Font val = new Font("Arial", 5f, (FontStyle)1);
		try
		{
			Brush val2 = (Brush)new SolidBrush(color);
			try
			{
				g.DrawString(text, val, val2, (float)x, (float)y);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void CreateReportSummary(string text, Graphics g, int x, int y)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		Color color = ((!ReportSummary) ? defaultColor : ((factor > 150f) ? StiColorUtils.Dark(BaseColor, 200) : BaseColor));
		Brush val = (Brush)new SolidBrush(color);
		try
		{
			Font val2 = new Font("Arial", 4f, (FontStyle)1);
			try
			{
				g.DrawString(text, val2, val, (float)x, (float)y);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (Borders && ReportSummary)
		{
			Pen val3 = new Pen(borderColor);
			try
			{
				g.DrawLine(val3, StiScale.I10, y + StiScale.I7, ((Control)this).Width - StiScale.XXI(15.0), y + StiScale.I7);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
	}

	private void CreateHeader(string text, int x, int y, Graphics g)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		Color color = defaultColor;
		Color color2 = Color.White;
		Pen val = new Pen(Color.Transparent);
		if (Header)
		{
			color = StiColorUtils.Dark(BaseColor, 200);
			color2 = StiColorUtils.Light(BaseColor, 0);
			if (Borders)
			{
				val = new Pen(borderColor);
			}
		}
		Brush val2 = (Brush)new SolidBrush(color2);
		try
		{
			g.FillRectangle(val2, x + StiScale.I1, y + StiScale.I1, ((Control)this).Width - StiScale.XXI(24.0), StiScale.YYI(10.0));
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		Rectangle rectangle = new Rectangle(x, y, ((Control)this).Width - StiScale.XXI(23.0), StiScale.YYI(10.0));
		g.DrawRectangle(val, rectangle);
		SolidBrush val3 = new SolidBrush(color);
		try
		{
			Font val4 = new Font("Arial", 4f, (FontStyle)1);
			try
			{
				g.DrawString(text, val4, (Brush)(object)val3, (float)(x + StiScale.I2), (float)(y + StiScale.I2));
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		val.Dispose();
	}

	private void CreateFooter(string text, int x, int y, Graphics g)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Expected O, but got Unknown
		Color color = defaultColor;
		Color color2 = Color.White;
		Pen val = new Pen(Color.Transparent);
		if (Footer)
		{
			color = StiColorUtils.Dark(BaseColor, 200);
			int num = (int)(20f * ColorFactor);
			if (num > 255)
			{
				num = 255;
			}
			color2 = StiColorUtils.Light(BaseColor, (byte)num);
			if (Borders)
			{
				val = new Pen(borderColor);
			}
		}
		Brush val2 = (Brush)new SolidBrush(color2);
		try
		{
			g.FillRectangle(val2, x + StiScale.I1, y + StiScale.I1, ((Control)this).Width - StiScale.XXI(24.0), StiScale.YYI(9.0));
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		Rectangle rectangle = new Rectangle(x, y, ((Control)this).Width - StiScale.XXI(23.0), StiScale.YYI(10.0));
		g.DrawRectangle(val, rectangle);
		SolidBrush val3 = new SolidBrush(color);
		try
		{
			Font val4 = new Font("Arial", 4f, (FontStyle)1);
			try
			{
				g.DrawString(text, val4, (Brush)(object)val3, (float)StiScale.XXI(155.0), (float)(y + StiScale.I2));
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		val.Dispose();
	}

	private void CreateData(string textLong, string textShort, string textPrice1, string textPrice2, string textPrice3, int x, int y, bool odd, Graphics g)
	{
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Expected O, but got Unknown
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		double num = 1.0;
		if (!odd)
		{
			num = 0.5;
		}
		Color color = defaultColor;
		Color color2 = Color.White;
		Color transparent = Color.Transparent;
		if (Data)
		{
			int num2 = (int)(150f * ColorFactor);
			if (num2 > 255)
			{
				num2 = 255;
			}
			color2 = StiColorUtils.Light(BaseColor, (byte)((double)num2 * num));
			color = StiColorUtils.Dark(BaseColor, 200);
			if (Borders)
			{
				transparent = borderColor;
			}
		}
		Rectangle rectangle = new Rectangle(x, y, ((Control)this).Width - StiScale.XXI(23.0), StiScale.YYI(10.0));
		SolidBrush val = new SolidBrush(color2);
		try
		{
			g.FillRectangle((Brush)(object)val, x + StiScale.I1, y + StiScale.I1, ((Control)this).Width - StiScale.XXI(24.0), StiScale.YYI(10.0));
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		Pen val2 = new Pen(transparent);
		try
		{
			g.DrawRectangle(val2, rectangle);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		SolidBrush val3 = new SolidBrush(color);
		try
		{
			Font val4 = new Font("Arial", 4f);
			try
			{
				g.DrawString(textLong, val4, (Brush)(object)val3, (float)StiScale.XXI(12.0), (float)(y + StiScale.I2));
				g.DrawString(textShort, val4, (Brush)(object)val3, (float)StiScale.XXI(62.0), (float)(y + StiScale.I2));
				g.DrawString(textPrice1, val4, (Brush)(object)val3, (float)StiScale.XXI(104.0), (float)(y + StiScale.I2));
				g.DrawString(textPrice2, val4, (Brush)(object)val3, (float)StiScale.XXI(134.0), (float)(y + StiScale.I2));
				g.DrawString(textPrice3, val4, (Brush)(object)val3, (float)StiScale.XXI(175.0), (float)(y + StiScale.I2));
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
	}

	public string RandomSmallLetter(int length)
	{
		RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
		char[] array = new char[length];
		string text = "abcdefghijklmnopqrstuvwxyz";
		for (int i = 0; i < length; i++)
		{
			byte[] array2 = new byte[1];
			randomNumberGenerator.GetBytes(array2);
			Random random = new Random(array2[0]);
			array[i] = text[random.Next(0, 25)];
		}
		return new string(array);
	}

	public string RandomBigLetter(int length)
	{
		RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
		char[] array = new char[length];
		string text = "ABCEDFGHIJKLMNOPQRSTUVWXYZ";
		for (int i = 0; i < length; i++)
		{
			byte[] array2 = new byte[1];
			randomNumberGenerator.GetBytes(array2);
			Random random = new Random(array2[0]);
			array[i] = text[random.Next(0, 25)];
		}
		return new string(array);
	}

	public string RandomNumber(int length)
	{
		RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
		char[] array = new char[length];
		string text = "1234567890";
		for (int i = 0; i < length; i++)
		{
			byte[] array2 = new byte[1];
			randomNumberGenerator.GetBytes(array2);
			Random random = new Random(array2[0]);
			array[i] = text[random.Next(0, 9)];
		}
		return new string(array);
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
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = SystemColors.ButtonHighlight;
		((UserControl)this).BorderStyle = (BorderStyle)1;
		((Control)this).DoubleBuffered = true;
		((Control)this).MaximumSize = new Size(217, 287);
		((Control)this).MinimumSize = new Size(217, 287);
		((Control)this).Name = "StiStyleViewReportControl";
		((Control)this).Padding = new Padding(18);
		((Control)this).Size = new Size(215, 285);
		((Control)this).Paint += new PaintEventHandler(This_Paint);
		((Control)this).ResumeLayout(false);
	}
}
