using System.Collections;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Engine;

public class StiTextInCellsV1Builder : StiSimpleTextV1Builder
{
	public override bool InternalRender(StiComponent masterComp, ref StiComponent renderedComponent, StiContainer outContainer)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_042a: Invalid comparison between Unknown and I4
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0446: Invalid comparison between Unknown and I4
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		StiTextInCells stiTextInCells = masterComp as StiTextInCells;
		bool result = base.InternalRender((StiComponent)stiTextInCells, ref renderedComponent, outContainer);
		if (outContainer.Components.IndexOf(renderedComponent) != -1)
		{
			outContainer.Components.Remove(renderedComponent);
		}
		StiContainer stiContainer = new StiContainer();
		stiContainer.ClientRectangle = renderedComponent.ClientRectangle;
		stiContainer.Name = renderedComponent.Name;
		outContainer.Components.Add(stiContainer);
		StiText stiText = renderedComponent as StiText;
		RectangleD clientRectangle = stiContainer.ClientRectangle;
		((RectangleD)(ref clientRectangle)).X = 0.0;
		((RectangleD)(ref clientRectangle)).Y = 0.0;
		StiUnit unit = stiTextInCells.Page.Unit;
		double num = unit.ConvertFromHInches(stiTextInCells.Border.Size / 2.0 * stiTextInCells.Page.Zoom);
		double num2 = stiTextInCells.HorSpacing;
		double num3 = stiTextInCells.VertSpacing;
		double num4 = stiTextInCells.CellWidth;
		double num5 = stiTextInCells.CellHeight;
		if (stiTextInCells.CellWidth == 0f)
		{
			num4 = unit.ConvertFromHInches((double)(stiTextInCells.Font.GetHeight() * 1.5f));
		}
		if (stiTextInCells.CellHeight == 0f)
		{
			num5 = unit.ConvertFromHInches((double)(stiTextInCells.Font.GetHeight() * 1.5f));
		}
		if (!stiTextInCells.ContinuousText)
		{
			Size size = new Size(1, 1);
			double num6 = (float)(((RectangleD)(ref clientRectangle)).X + num + num4);
			while (num6 + num2 + num4 < ((RectangleD)(ref clientRectangle)).Right)
			{
				num6 += num2 + num4;
				size.Width++;
			}
			double num7 = (float)(((RectangleD)(ref clientRectangle)).Y + num + num5);
			while (num7 + num3 + num5 < ((RectangleD)(ref clientRectangle)).Bottom)
			{
				num7 += num3 + num5;
				size.Height++;
			}
			if (!stiTextInCells.WordWrap)
			{
				size.Height = 1;
			}
			ArrayList arrayList = new ArrayList();
			string text = string.Empty;
			if (stiText.GetTextInternal() != null)
			{
				string textInternal = stiText.GetTextInternal();
				string text2 = textInternal;
				for (int i = 0; i < text2.Length; i++)
				{
					char c = text2[i];
					if (char.IsControl(c))
					{
						if (c == '\n')
						{
							arrayList.Add(StiTextInCellsHelper.TrimEndWhiteSpace(text));
							text = string.Empty;
						}
					}
					else
					{
						text += c;
					}
				}
			}
			if (text != string.Empty)
			{
				arrayList.Add(StiTextInCellsHelper.TrimEndWhiteSpace(text));
			}
			if (arrayList.Count == 0)
			{
				arrayList.Add(text);
			}
			if (stiTextInCells.WordWrap)
			{
				for (int j = 0; j < arrayList.Count; j++)
				{
					string text3 = (string)arrayList[j];
					if (text3.Length <= size.Width)
					{
						continue;
					}
					int[] array = new int[text3.Length];
					int num8 = 0;
					int k;
					for (k = 0; k < text3.Length && char.IsWhiteSpace(text3[k]); k++)
					{
						array[k] = num8;
					}
					for (int l = k; l < text3.Length; l++)
					{
						if (char.IsWhiteSpace(text3[l]))
						{
							num8++;
						}
						array[l] = num8;
					}
					int m = size.Width;
					int num9 = m - 1;
					if (array[m] > 0)
					{
						if (array[m] != array[num9])
						{
							for (; char.IsWhiteSpace(text3[m]); m++)
							{
							}
						}
						else
						{
							while (!char.IsWhiteSpace(text3[m]))
							{
								m--;
							}
							num9 = m++;
							while (char.IsWhiteSpace(text3[num9]))
							{
								num9--;
							}
						}
					}
					arrayList[j] = text3.Substring(0, num9 + 1);
					arrayList.Insert(j + 1, text3.Substring(m, text3.Length - m));
				}
			}
			num7 = (float)(((RectangleD)(ref clientRectangle)).Y + num);
			RectangleD clientRectangle2 = default(RectangleD);
			for (int n = 0; n < size.Height; n++)
			{
				string text4 = ((n < arrayList.Count) ? ((string)arrayList[n]) : string.Empty);
				int num10 = 0;
				if ((int)stiTextInCells.HorAlignment == 1)
				{
					num10 = (size.Width - text4.Length) / 2;
				}
				if ((int)stiTextInCells.HorAlignment == 2)
				{
					num10 = size.Width - text4.Length;
				}
				if (num10 > 0)
				{
					text4 = new string(' ', num10) + text4;
				}
				num6 = (float)(((RectangleD)(ref clientRectangle)).X + num);
				for (int num11 = 0; num11 < size.Width; num11++)
				{
					((RectangleD)(ref clientRectangle2))._002Ector(num6, num7, num4, num5);
					StiText stiText2 = ((StiService)renderedComponent).Clone() as StiText;
					stiText2.ClientRectangle = clientRectangle2;
					stiText2.HorAlignment = (StiTextHorAlignment)1;
					stiText2.VertAlignment = (StiVertAlignment)1;
					stiContainer.Components.Add(stiText2);
					string textInternal2 = string.Empty;
					int num12 = (stiTextInCells.RightToLeft ? (size.Width - num11 - 1) : num11);
					if (num12 < text4.Length)
					{
						textInternal2 = new string(text4[num12], 1);
					}
					stiText2.SetTextInternal(textInternal2);
					num6 += num4 + num2;
				}
				num7 += num5 + num3;
			}
		}
		else
		{
			double num13 = ((RectangleD)(ref clientRectangle)).X + num;
			double num14 = ((RectangleD)(ref clientRectangle)).Y + num;
			bool flag = true;
			int num15 = 0;
			RectangleD clientRectangle3 = default(RectangleD);
			while (true)
			{
				((RectangleD)(ref clientRectangle3))._002Ector(num13, num14, num4, num5);
				if (((RectangleD)(ref clientRectangle3)).Right + num2 < ((RectangleD)(ref clientRectangle)).Right || flag)
				{
					StiText stiText3 = ((StiService)renderedComponent).Clone() as StiText;
					stiText3.ClientRectangle = clientRectangle3;
					stiText3.HorAlignment = (StiTextHorAlignment)1;
					stiText3.VertAlignment = (StiVertAlignment)1;
					stiContainer.Components.Add(stiText3);
					string textInternal3 = string.Empty;
					if (stiText.GetTextInternal() != null && num15 < stiText.GetTextInternal().Length)
					{
						textInternal3 = new string(stiText.GetTextInternal()[num15], 1);
					}
					stiText3.SetTextInternal(textInternal3);
					num13 += num4 + num2;
					num15++;
					flag = false;
				}
				else
				{
					num14 += num5 + num3;
					num13 = (double)(float)((RectangleD)(ref clientRectangle)).X + num;
					flag = true;
					if (!stiTextInCells.WordWrap || ((RectangleD)(ref clientRectangle)).Bottom < num14 + num5 + num3)
					{
						break;
					}
				}
			}
		}
		return result;
	}
}
