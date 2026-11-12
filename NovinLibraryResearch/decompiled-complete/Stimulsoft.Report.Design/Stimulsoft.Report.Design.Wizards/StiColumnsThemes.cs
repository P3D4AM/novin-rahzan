using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Report.Design.Wizards;

[ToolboxItem(false)]
public class StiColumnsThemes : UserControl
{
	private ListViewGroup groupThemes;

	private IContainer components;

	private ImageList imageListStyle;

	private ListView listViewThemes;

	private Panel panelStylesCollection;

	public StiStylesCollection SelectedStylesCollection => GetSelectedStylesCollection();

	private void PanelStylesCollection_SizeChanged(object sender, EventArgs e)
	{
		((Control)panelStylesCollection).Invalidate();
	}

	private void ListViewThemes_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
	{
		((Control)panelStylesCollection).Invalidate();
	}

	private void PanelStylesCollection_Paint(object sender, PaintEventArgs e)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		StiStylesCollection selectedStylesCollection = GetSelectedStylesCollection();
		int width = ((Control)panelStylesCollection).Width;
		int num = StiScale.YYI(300.0);
		Rectangle rectangle = new Rectangle(StiScale.I10, StiScale.I10, width - StiScale.I20, num - StiScale.I20);
		Brush val = (Brush)new SolidBrush(Color.White);
		try
		{
			graphics.FillRectangle(val, rectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		Pen val2 = new Pen(Color.Gray);
		try
		{
			graphics.DrawRectangle(val2, rectangle);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		Rectangle rectangle2 = new Rectangle(StiScale.I10, StiScale.I10, width - StiScale.I20, num - StiScale.I20);
		ShadowPaintInfo shadowPaintInfo = new ShadowPaintInfo();
		shadowPaintInfo.Graphics = graphics;
		shadowPaintInfo.Rectangle = rectangle2;
		shadowPaintInfo.Size = 4;
		ShadowPainter.Paint(shadowPaintInfo);
		if (selectedStylesCollection == null)
		{
			return;
		}
		foreach (StiBaseStyle item in selectedStylesCollection)
		{
			if (!(item is StiStyle))
			{
				continue;
			}
			StiStyle stiStyle = (StiStyle)item;
			for (int i = 1; i <= stiStyle.Conditions.Count; i++)
			{
				{
					IEnumerator enumerator2 = stiStyle.Conditions.GetEnumerator();
					try
					{
						if (enumerator2.MoveNext())
						{
							StiStyleCondition stiStyleCondition = (StiStyleCondition)enumerator2.Current;
							if ((stiStyleCondition.Placement & StiStyleComponentPlacement.PageHeader) > StiStyleComponentPlacement.None)
							{
								string[] texts = new string[1] { "Page 1 of 1" };
								DrawElement(graphics, width - StiScale.I(60), StiScale.I(25), StiScale.I(30), StiScale.I10, stiStyle, texts, 4);
							}
							else if ((stiStyleCondition.Placement & StiStyleComponentPlacement.PageFooter) > StiStyleComponentPlacement.None)
							{
								string[] texts2 = new string[1] { "Page 1 of 1" };
								DrawElement(graphics, width - StiScale.I(60), num - StiScale.I(30), StiScale.I(30), StiScale.I10, stiStyle, texts2, 4);
							}
							else if ((stiStyleCondition.Placement & StiStyleComponentPlacement.ReportTitle) > StiStyleComponentPlacement.None)
							{
								string[] texts3 = new string[1] { "Report" };
								DrawElement(graphics, width - StiScale.I(130), StiScale.I(35), StiScale.I(30), StiScale.I10, stiStyle, texts3, 5);
							}
							else if ((stiStyleCondition.Placement & StiStyleComponentPlacement.ReportSummary) > StiStyleComponentPlacement.None)
							{
								string[] texts4 = new string[3]
								{
									"",
									"",
									"      " + DateTime.Now.ToShortDateString()
								};
								DrawElement(graphics, StiScale.I(20), StiScale.I(205), width - StiScale.I(40), StiScale.I10, stiStyle, texts4, 4);
							}
							else if ((stiStyleCondition.Placement & StiStyleComponentPlacement.Header) > StiStyleComponentPlacement.None)
							{
								string[] texts5 = new string[3] { "Company", "Address", "Phone" };
								DrawElement(graphics, StiScale.I(20), StiScale.I(45), width - StiScale.I(40), StiScale.I10, stiStyle, texts5, 5);
							}
							else if ((stiStyleCondition.Placement & StiStyleComponentPlacement.Footer) > StiStyleComponentPlacement.None)
							{
								string[] texts6 = new string[3] { "", "", "          Count: 8" };
								DrawElement(graphics, StiScale.I(20), StiScale.I(195), width - StiScale.I(40), StiScale.I10, stiStyle, texts6, 4);
							}
							else if ((stiStyleCondition.Placement & StiStyleComponentPlacement.DataOddStyle) > StiStyleComponentPlacement.None)
							{
								string[] texts7 = new string[3] { "Alfreds Futterkiste", "Obbere Str.57", "030-0074321" };
								DrawElement(graphics, StiScale.I(20), StiScale.I(65), width - StiScale.I(40), StiScale.I10, stiStyle, texts7, 4);
								string[] texts8 = new string[3] { "Ana Trujillo Empareddados", "Avda.de la 22", "232-4562845" };
								DrawElement(graphics, StiScale.I(20), StiScale.I(85), width - StiScale.I(40), StiScale.I10, stiStyle, texts8, 4);
								string[] texts9 = new string[3] { "Blauer See Deli", "Forsterstr. 61", "308-1923548" };
								DrawElement(graphics, StiScale.I(20), StiScale.I(125), width - StiScale.I(40), StiScale.I10, stiStyle, texts9, 4);
								string[] texts10 = new string[3] { "Centro comercial Moctez", "Sieras de Granada 99", "357-8542545" };
								DrawElement(graphics, StiScale.I(20), StiScale.I(165), width - StiScale.I(40), StiScale.I10, stiStyle, texts10, 4);
							}
							else if ((stiStyleCondition.Placement & StiStyleComponentPlacement.DataEvenStyle) > StiStyleComponentPlacement.None)
							{
								string[] texts11 = new string[3] { "Around the Horn", "Mataderos 2313", "546-8542545" };
								DrawElement(graphics, StiScale.I(20), StiScale.I(75), width - StiScale.I(40), StiScale.I(10), stiStyle, texts11, 4);
								string[] texts12 = new string[3] { "Berglunds snabbkop", "Berguvsvagen 8", "568-6581321" };
								DrawElement(graphics, StiScale.I(20), StiScale.I(115), width - StiScale.I(40), StiScale.I(10), stiStyle, texts12, 4);
								string[] texts13 = new string[3] { "Blondel pere et fils", "24? place Klebe", "889-6814391" };
								DrawElement(graphics, StiScale.I(20), StiScale.I(135), width - StiScale.I(40), StiScale.I10, stiStyle, texts13, 4);
								string[] array = new string[3] { "Shop-suery Chinese", "Haupstr. 23", "567-6548321" };
								DrawElement(graphics, StiScale.I(20), StiScale.I(175), width - StiScale.I(40), StiScale.I10, stiStyle, texts13, 4);
							}
							else if ((stiStyleCondition.Placement & StiStyleComponentPlacement.GroupHeader) > StiStyleComponentPlacement.None)
							{
								string[] texts14 = new string[1] { "A" };
								DrawElement(graphics, StiScale.I(20), StiScale.I(55), width - StiScale.I(40), StiScale.I10, stiStyle, texts14, 5);
								string[] texts15 = new string[1] { "B" };
								DrawElement(graphics, StiScale.I(20), StiScale.I(105), width - StiScale.I(40), StiScale.I10, stiStyle, texts15, 5);
								string[] texts16 = new string[1] { "C" };
								DrawElement(graphics, StiScale.I(20), StiScale.I(155), width - StiScale.I(40), StiScale.I10, stiStyle, texts16, 5);
							}
							else if ((stiStyleCondition.Placement & StiStyleComponentPlacement.GroupFooter) > StiStyleComponentPlacement.None)
							{
								string[] texts17 = new string[3] { "", "", "          Count: 3" };
								DrawElement(graphics, StiScale.I(20), StiScale.I(95), width - StiScale.I(40), StiScale.I(10), stiStyle, texts17, 4);
								string[] texts18 = new string[3] { "", "", "          Count: 3" };
								DrawElement(graphics, StiScale.I(20), StiScale.I(145), width - StiScale.I(40), StiScale.I10, stiStyle, texts18, 4);
								string[] texts19 = new string[3] { "", "", "          Count: 2" };
								DrawElement(graphics, StiScale.I(20), StiScale.I(185), width - StiScale.I(40), StiScale.I10, stiStyle, texts19, 4);
							}
							else if ((stiStyleCondition.Placement & StiStyleComponentPlacement.Data) > StiStyleComponentPlacement.None)
							{
								DrawBorderElement(graphics, StiScale.I(20), StiScale.I(65), width - StiScale.I(40), StiScale.I(10), stiStyle);
								DrawBorderElement(graphics, StiScale.I(20), StiScale.I(85), width - StiScale.I(40), StiScale.I(10), stiStyle);
								DrawBorderElement(graphics, StiScale.I(20), StiScale.I(125), width - StiScale.I(40), StiScale.I(10), stiStyle);
								DrawBorderElement(graphics, StiScale.I(20), StiScale.I(75), width - StiScale.I(40), StiScale.I(10), stiStyle);
								DrawBorderElement(graphics, StiScale.I(20), StiScale.I(115), width - StiScale.I(40), StiScale.I(10), stiStyle);
								DrawBorderElement(graphics, StiScale.I(20), StiScale.I(135), width - StiScale.I(40), StiScale.I(10), stiStyle);
								DrawBorderElement(graphics, StiScale.I(20), StiScale.I(165), width - StiScale.I(40), StiScale.I(10), stiStyle);
								DrawBorderElement(graphics, StiScale.I(20), StiScale.I(175), width - StiScale.I(40), StiScale.I(10), stiStyle);
							}
						}
					}
					finally
					{
						IDisposable disposable = enumerator2 as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
				}
			}
		}
	}

	public void AddStyles(StiStylesCollection styles)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		if (styles.Count != 0)
		{
			Image icon = (Image)(object)GetIcon(styles);
			imageListStyle.Images.Add(imageListStyle.Images.Count.ToString(), icon);
			string text = string.Empty;
			if (styles.Count > 0)
			{
				text = styles[0].CollectionName;
			}
			ListViewItem val = new ListViewItem(text, (imageListStyle.Images.Count - 1).ToString());
			val.Tag = styles;
			val.Group = groupThemes;
			listViewThemes.Items.Add(val);
		}
	}

	private void LoadStylesCollections()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Expected O, but got Unknown
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Expected O, but got Unknown
		groupThemes = new ListViewGroup(StiLocalization.Get("Wizards", "Themes"));
		listViewThemes.Groups.Add(groupThemes);
		Image icon = (Image)(object)GetIcon(null);
		imageListStyle.Images.Add("None", icon);
		ListViewItem val = new ListViewItem(StiLocalization.Get("PropertyEnum", "StiCheckStyleNone"), "None", groupThemes);
		listViewThemes.Items.Add(val);
		string text = StiOptions.Designer.DefaultStylesCollectionsPath;
		if (string.IsNullOrEmpty(text))
		{
			text = StiOptions.Configuration.ApplicationDirectory + "\\Themes\\";
		}
		if (Directory.Exists(text))
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(text);
			FileInfo[] files = directoryInfo.GetFiles();
			FileInfo[] array = files;
			foreach (FileInfo fileInfo in array)
			{
				if (string.Equals(Path.GetExtension(fileInfo.FullName), ".sts", StringComparison.InvariantCultureIgnoreCase))
				{
					StiStylesCollection stiStylesCollection = new StiStylesCollection();
					StiStylesSheet stiStylesSheet = new StiStylesSheet(stiStylesCollection);
					stiStylesSheet.Load(fileInfo.FullName);
					string text2 = null;
					if (stiStylesCollection.Count > 0)
					{
						text2 = stiStylesCollection[0].CollectionName;
					}
					Image icon2 = (Image)(object)GetIcon(stiStylesCollection);
					imageListStyle.Images.Add(imageListStyle.Images.Count.ToString(), icon2);
					ListViewItem val2 = new ListViewItem(text2, (imageListStyle.Images.Count - 1).ToString())
					{
						Tag = stiStylesCollection,
						Group = groupThemes
					};
					listViewThemes.Items.Add(val2);
				}
			}
		}
		ListViewGroup val3 = new ListViewGroup(StiLocalization.Get("Wizards", "DefaultThemes"));
		listViewThemes.Groups.Add(val3);
		using StiReport report = new StiReport();
		StiStylesCreator stiStylesCreator = new StiStylesCreator(report);
		stiStylesCreator.MaxNestedLevel = 1;
		stiStylesCreator.ShowBorders = true;
		CreateDefaultColorThemes(ColorTranslator.FromHtml("#90d7cf"), StiLocalization.Get("Chart", "Style") + 21, stiStylesCreator, val3);
		CreateDefaultColorThemes(ColorTranslator.FromHtml("#bf717f"), StiLocalization.Get("Chart", "Style") + 24, stiStylesCreator, val3);
		CreateDefaultColorThemes(ColorTranslator.FromHtml("#5065a1"), StiLocalization.Get("Chart", "Style") + 25, stiStylesCreator, val3);
		CreateDefaultColorThemes(ColorTranslator.FromHtml("#ff885e"), StiLocalization.Get("Chart", "Style") + 26, stiStylesCreator, val3);
		CreateDefaultColorThemes(ColorTranslator.FromHtml("#23abdf"), StiLocalization.Get("Chart", "Style") + 27, stiStylesCreator, val3);
	}

	private void CreateDefaultColorThemes(Color colorBase, string name, StiStylesCreator creator, ListViewGroup groupDefaultThemes)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		List<StiBaseStyle> list = creator.CreateStyles(name, colorBase);
		StiStylesCollection stiStylesCollection = new StiStylesCollection();
		foreach (StiBaseStyle item in list)
		{
			stiStylesCollection.Add(item);
		}
		imageListStyle.Images.Add(name, (Image)(object)GetIcon(stiStylesCollection));
		ListViewItem val = new ListViewItem(name, name)
		{
			Tag = stiStylesCollection,
			Group = groupDefaultThemes
		};
		listViewThemes.Items.Add(val);
	}

	private StiStylesCollection GetSelectedStylesCollection()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		SelectedListViewItemCollection selectedItems = listViewThemes.SelectedItems;
		ListViewItem val = null;
		IEnumerator enumerator = selectedItems.GetEnumerator();
		try
		{
			if (enumerator.MoveNext())
			{
				ListViewItem val2 = (ListViewItem)enumerator.Current;
				val = val2;
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		return ((val != null) ? val.Tag : null) as StiStylesCollection;
	}

	private Bitmap GetIcon(StiStylesCollection stylesCollection)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fa: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Expected O, but got Unknown
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Expected O, but got Unknown
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Expected O, but got Unknown
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c1: Expected O, but got Unknown
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Expected O, but got Unknown
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0421: Expected O, but got Unknown
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Expected O, but got Unknown
		//IL_0423: Unknown result type (might be due to invalid IL or missing references)
		//IL_042a: Expected O, but got Unknown
		Bitmap val = new Bitmap(StiScale.XXI(32.0), StiScale.YYI(32.0));
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		if (stylesCollection != null)
		{
			int num = 0;
			StiStyle stiStyle = null;
			foreach (StiBaseStyle item in stylesCollection)
			{
				if (item is StiCrossTabStyle || !(item is StiStyle))
				{
					continue;
				}
				StiStyle stiStyle2 = (StiStyle)item;
				stiStyle = stiStyle2;
				for (int i = 1; i <= stiStyle2.Conditions.Count; i++)
				{
					{
						IEnumerator enumerator2 = stiStyle2.Conditions.GetEnumerator();
						try
						{
							if (!enumerator2.MoveNext())
							{
								continue;
							}
							StiStyleCondition stiStyleCondition = (StiStyleCondition)enumerator2.Current;
							num++;
							if ((stiStyleCondition.Placement & StiStyleComponentPlacement.Header) > StiStyleComponentPlacement.None)
							{
								int num2 = 1;
								val2.Clear(StiBrush.ToColor(stiStyle2.Brush));
								Color color = StiBrush.ToColor(stiStyle2.TextBrush);
								Brush val3 = (Brush)new SolidBrush(Color.FromArgb(100, color.R, color.G, color.B));
								try
								{
									Pen val4 = new Pen(val3);
									try
									{
										val2.DrawLine(val4, StiScale.I3, num2 + StiScale.I3, StiScale.XXI(29.0), num2 + StiScale.I3);
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
							else if ((stiStyleCondition.Placement & StiStyleComponentPlacement.Footer) > StiStyleComponentPlacement.None)
							{
								int num3 = StiScale.YYI(25.0);
								Brush val5 = (Brush)new SolidBrush(StiBrush.ToColor(stiStyle2.Brush));
								try
								{
									val2.FillRectangle(val5, 0, num3, StiScale.XXI(32.0), StiScale.I6);
								}
								finally
								{
									((IDisposable)val5)?.Dispose();
								}
								Color color2 = StiBrush.ToColor(stiStyle2.TextBrush);
								Brush val6 = (Brush)new SolidBrush(Color.FromArgb(100, color2.R, color2.G, color2.B));
								try
								{
									Pen val7 = new Pen(val6);
									try
									{
										val2.DrawLine(val7, StiScale.I3, num3 + StiScale.I3, StiScale.XXI(29.0), num3 + StiScale.I3);
									}
									finally
									{
										((IDisposable)val7)?.Dispose();
									}
								}
								finally
								{
									((IDisposable)val6)?.Dispose();
								}
							}
							else if ((stiStyleCondition.Placement & StiStyleComponentPlacement.DataOddStyle) > StiStyleComponentPlacement.None)
							{
								Brush val8 = (Brush)new SolidBrush(StiBrush.ToColor(stiStyle2.Brush));
								try
								{
									val2.FillRectangle(val8, 0, StiScale.YYI(7.0), StiScale.XXI(32.0), StiScale.YYI(6.0));
									val2.FillRectangle(val8, 0, StiScale.YYI(19.0), StiScale.XXI(32.0), StiScale.YYI(6.0));
								}
								finally
								{
									((IDisposable)val8)?.Dispose();
								}
								Color color3 = StiBrush.ToColor(stiStyle2.TextBrush);
								Brush val9 = (Brush)new SolidBrush(Color.FromArgb(100, color3.R, color3.G, color3.B));
								try
								{
									Pen val10 = new Pen(val9);
									try
									{
										val2.DrawLine(val10, StiScale.XXI(3.0), StiScale.YYI(10.0), StiScale.XXI(29.0), StiScale.YYI(10.0));
										val2.DrawLine(val10, StiScale.XXI(3.0), StiScale.YYI(22.0), StiScale.XXI(29.0), StiScale.YYI(22.0));
									}
									finally
									{
										((IDisposable)val10)?.Dispose();
									}
								}
								finally
								{
									((IDisposable)val9)?.Dispose();
								}
							}
							else
							{
								if ((stiStyleCondition.Placement & StiStyleComponentPlacement.DataEvenStyle) <= StiStyleComponentPlacement.None)
								{
									continue;
								}
								Brush val11 = (Brush)new SolidBrush(StiBrush.ToColor(stiStyle2.Brush));
								try
								{
									val2.FillRectangle(val11, 0, StiScale.I13, StiScale.XXI(32.0), StiScale.I6);
								}
								finally
								{
									((IDisposable)val11)?.Dispose();
								}
								Color color4 = StiBrush.ToColor(stiStyle2.TextBrush);
								Brush val12 = (Brush)new SolidBrush(Color.FromArgb(100, color4.R, color4.G, color4.B));
								try
								{
									Pen val13 = new Pen(val12);
									try
									{
										val2.DrawLine(val13, StiScale.I3, StiScale.YYI(16.0), StiScale.XXI(29.0), StiScale.I16);
									}
									finally
									{
										((IDisposable)val13)?.Dispose();
									}
								}
								finally
								{
									((IDisposable)val12)?.Dispose();
								}
								continue;
							}
						}
						finally
						{
							IDisposable disposable = enumerator2 as IDisposable;
							if (disposable != null)
							{
								disposable.Dispose();
							}
						}
					}
				}
			}
			if (num == 0 && stiStyle != null)
			{
				val2.Clear(StiBrush.ToColor(stiStyle.Brush));
				Pen val14 = new Pen(stiStyle.Border.Color);
				try
				{
					val2.DrawRectangle(val14, 0, 0, StiScale.XXI(32.0) - 1, StiScale.YYI(32.0) - 1);
				}
				finally
				{
					((IDisposable)val14)?.Dispose();
				}
			}
		}
		else
		{
			val2.Clear(Color.White);
		}
		val2.DrawRectangle(Pens.Gray, 0, 0, StiScale.XXI(32.0) - 1, StiScale.YYI(32.0) - 1);
		val2.Dispose();
		return val;
	}

	private void DrawElement(Graphics g, int x, int y, int width, int height, StiStyle style, string[] texts, int size)
	{
		FillElement(g, x, y, width, height, style);
		int num = 0;
		foreach (string text in texts)
		{
			num++;
		}
		int num2 = 0;
		foreach (string text2 in texts)
		{
			int x2 = x + num2 * (width + StiScale.XXI(30.0)) / num;
			AddTextInElement(g, x2, y, style, text2, size);
			num2++;
		}
		DrawBorderElement(g, x, y, width, height, style);
	}

	private void FillElement(Graphics g, int x, int y, int width, int height, StiStyle style)
	{
		Brush val = StiBrush.GetBrush(rect: new Rectangle(x, y, width, height), brush: style.Brush);
		try
		{
			g.FillRectangle(val, x + 1, y + 1, width - 1, height - 1);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void AddTextInElement(Graphics g, int x, int y, StiStyle style, string text, int size)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		Brush val = (Brush)new SolidBrush(StiBrush.ToColor(style.TextBrush));
		try
		{
			Font val2 = new Font(style.Font.FontFamily, (float)size);
			try
			{
				g.DrawString(text, val2, val, (float)(x + StiScale.I2), (float)(y + StiScale.I2));
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

	private void DrawBorderElement(Graphics g, int x, int y, int width, int height, StiStyle style)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		Color color = style.Border.Color;
		Pen val = new Pen(color);
		try
		{
			if (style.Border.IsAllBorderSidesPresent || style.AllowUseBorderSidesFromLocation)
			{
				g.DrawRectangle(val, x, y, width, height);
				return;
			}
			if (style.Border.IsRightBorderSidePresent)
			{
				g.DrawLine(val, x + width, y, x + width, y + height - 1);
			}
			if (style.Border.IsBottomBorderSidePresent)
			{
				g.DrawLine(val, x, y + height, x + width, y + height);
			}
			if (style.Border.IsLeftBorderSidePresent)
			{
				g.DrawLine(val, x, y, x, y + height - 1);
			}
			if (style.Border.IsTopBorderSidePresent)
			{
				g.DrawLine(val, x, y, x + width, y);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void DoScaling()
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		if (StiScale.Factor != 1.0)
		{
			imageListStyle.Images.Clear();
			imageListStyle.ImageSize = new Size(StiScale.XXI(32.0), StiScale.YYI(32.0));
			imageListStyle.Images.Add((Image)new Bitmap(StiScale.XXI(32.0), StiScale.YYI(32.0)));
		}
	}

	public StiColumnsThemes()
	{
		InitializeComponent();
		((object)panelStylesCollection).GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(panelStylesCollection, true, null);
		DoScaling();
		LoadStylesCollections();
		listViewThemes.Items[0].Selected = true;
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
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Expected O, but got Unknown
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Expected O, but got Unknown
		components = new Container();
		imageListStyle = new ImageList(components);
		listViewThemes = new ListView();
		panelStylesCollection = new Panel();
		((Control)this).SuspendLayout();
		imageListStyle.ColorDepth = (ColorDepth)32;
		imageListStyle.ImageSize = new Size(32, 32);
		imageListStyle.TransparentColor = Color.Transparent;
		listViewThemes.BorderStyle = (BorderStyle)0;
		((Control)listViewThemes).Dock = (DockStyle)5;
		listViewThemes.HideSelection = false;
		listViewThemes.LargeImageList = imageListStyle;
		((Control)listViewThemes).Location = new Point(0, 0);
		listViewThemes.MultiSelect = false;
		((Control)listViewThemes).Name = "listViewThemes";
		((Control)listViewThemes).Size = new Size(313, 319);
		((Control)listViewThemes).TabIndex = 7;
		listViewThemes.UseCompatibleStateImageBehavior = false;
		listViewThemes.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(ListViewThemes_ItemSelectionChanged);
		((Control)panelStylesCollection).BackColor = Color.White;
		((Control)panelStylesCollection).Dock = (DockStyle)4;
		((Control)panelStylesCollection).Location = new Point(313, 0);
		((Control)panelStylesCollection).Name = "panelStylesCollection";
		((Control)panelStylesCollection).Size = new Size(219, 319);
		((Control)panelStylesCollection).TabIndex = 5;
		((Control)panelStylesCollection).SizeChanged += PanelStylesCollection_SizeChanged;
		((Control)panelStylesCollection).Paint += new PaintEventHandler(PanelStylesCollection_Paint);
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)listViewThemes);
		((Control)this).Controls.Add((Control)(object)panelStylesCollection);
		((Control)this).Name = "StiColumnsThemes";
		((Control)this).Size = new Size(532, 319);
		((Control)this).ResumeLayout(false);
	}
}
