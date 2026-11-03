using System;
using System.Collections;
using System.Collections.Generic;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

internal class StiColumnsContainer : StiContainer
{
	private int countOfItems;

	internal int Columns { get; set; }

	internal double ColumnWidth { get; set; }

	internal double ColumnGaps { get; set; }

	internal StiColumnDirection ColumnDirection { get; set; } = StiColumnDirection.AcrossThenDown;


	internal bool RightToLeft { get; set; }

	internal int MinRowsInColumn { get; set; }

	public void AddContainer(StiContainer container)
	{
		Components.Add(container);
		if (!(container is StiLevelContainer))
		{
			countOfItems++;
		}
	}

	public double HowMuchAdditionalSpaceNeeded(double currentHeight, StiContainer container)
	{
		countOfItems++;
		Components.Add(container);
		double num = FinishColumns(onlyCalc: true);
		Components.Remove(container);
		countOfItems--;
		return (double)Math.Max((decimal)num - (decimal)currentHeight, 0m);
	}

	public double FinishColumns()
	{
		return FinishColumns(onlyCalc: false);
	}

	private double FinishColumns(bool onlyCalc)
	{
		double num = 0.0;
		int num2 = 1;
		int columns = Columns;
		double columnWidth = ColumnWidth;
		double columnGaps = ColumnGaps;
		double num3 = columnWidth + columnGaps;
		if (ColumnDirection == StiColumnDirection.AcrossThenDown)
		{
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			lock (((ICollection)Components).SyncRoot)
			{
				foreach (StiContainer component in Components)
				{
					if (component is StiLevelContainer)
					{
						num4++;
						num6++;
					}
					else
					{
						if (Page != null && Page.UnlimitedHeight && Page.UnlimitedBreakable)
						{
							double num7 = Page.PageHeight - Page.Margins.Top - Page.Margins.Bottom;
							if ((int)((Top + num) / num7) != (int)((Top + num + component.Height) / num7))
							{
								num += num7 - (Top + num) % num7;
							}
						}
						double num8 = 0.0;
						num8 = (component.Left = ((!RightToLeft) ? (num3 * (double)(num2 - 1)) : (component.Parent.Width - num3 * (double)num2 + columnGaps)));
						component.Width = num8 - component.Left + columnWidth;
						component.Top = num;
						num4++;
						num6++;
						num2++;
					}
					if (num2 <= columns && num4 != Components.Count)
					{
						continue;
					}
					double num10 = 0.0;
					for (int i = num5; i < num6; i++)
					{
						StiContainer stiContainer2 = Components[i] as StiContainer;
						if (!(stiContainer2 is StiLevelContainer))
						{
							num10 = Math.Max(stiContainer2.Height, num10);
						}
					}
					if (!onlyCalc)
					{
						for (int j = num5; j < num6; j++)
						{
							StiContainer stiContainer3 = Components[j] as StiContainer;
							if (stiContainer3 is StiLevelContainer)
							{
								continue;
							}
							lock (((ICollection)stiContainer3.Components).SyncRoot)
							{
								foreach (StiComponent component2 in stiContainer3.Components)
								{
									if (component2.Bottom == stiContainer3.Height)
									{
										component2.Height = num10 - component2.Top;
									}
								}
							}
							stiContainer3.Height = num10;
						}
					}
					num += num10;
					num2 = 1;
					num5 = num4;
					num6 = num4;
				}
			}
		}
		else
		{
			int[] array = new int[columns];
			int num11 = countOfItems / columns;
			int num12 = Math.Min(MinRowsInColumn, countOfItems);
			if (num12 > 0)
			{
				num11 = Math.Max(num12, num11);
			}
			for (int k = 0; k < columns; k++)
			{
				array[k] = num11;
			}
			if (MinRowsInColumn > 0)
			{
				if (num12 <= num11)
				{
					num11 = countOfItems - num11 * columns;
					for (int l = 0; l < num11; l++)
					{
						array[l]++;
					}
				}
			}
			else
			{
				int num13 = countOfItems - num11 * columns;
				if (num13 > 0)
				{
					num11++;
					for (int m = 0; m < columns; m++)
					{
						array[m] = num11;
					}
				}
			}
			int num14 = 0;
			double num15 = 0.0;
			List<StiContainer>[] array2 = new List<StiContainer>[columns];
			for (int n = 0; n < columns; n++)
			{
				array2[n] = new List<StiContainer>();
			}
			double num16 = -1.0;
			bool flag = false;
			lock (((ICollection)Components).SyncRoot)
			{
				foreach (StiContainer component3 in Components)
				{
					if (component3 is StiLevelContainer)
					{
						continue;
					}
					if (!flag)
					{
						if (num16 == -1.0)
						{
							num16 = component3.Height;
						}
						else if (num16 != component3.Height)
						{
							flag = true;
						}
					}
					num14++;
					if (num14 > array[num2 - 1])
					{
						num14 = 1;
						num2++;
						num15 = 0.0;
					}
					if (Page != null && Page.UnlimitedHeight && Page.UnlimitedBreakable)
					{
						double num17 = Page.PageHeight - Page.Margins.Top - Page.Margins.Bottom;
						if ((int)((Top + num15) / num17) != (int)((Top + num15 + component3.Height) / num17))
						{
							num15 += num17 - (Top + num15) % num17;
						}
					}
					double num18 = 0.0;
					num18 = (component3.Left = ((!RightToLeft) ? (num3 * (double)(num2 - 1)) : (component3.Parent.Width - num3 * (double)num2 + columnGaps)));
					component3.Width = num18 - component3.Left + columnWidth;
					component3.Top = num15;
					num15 += component3.Height;
					num = Math.Max(num, num15);
					array2[num2 - 1].Add(component3);
				}
			}
			if (flag)
			{
				int num20 = 0;
				for (int num21 = 0; num21 < columns; num21++)
				{
					List<StiContainer> list = array2[num21];
					num20 = Math.Max(list.Count, num20);
				}
				Hashtable hashtable = new Hashtable();
				for (int num22 = 0; num22 < num20; num22++)
				{
					double num23 = 0.0;
					for (int num24 = 0; num24 < columns; num24++)
					{
						if (array2[num24].Count > num22)
						{
							StiContainer stiContainer5 = array2[num24][num22];
							if (!(stiContainer5 is StiLevelContainer))
							{
								num23 = Math.Max(num23, stiContainer5.Height);
							}
						}
					}
					for (int num25 = 0; num25 < columns; num25++)
					{
						if (array2[num25].Count <= num22)
						{
							continue;
						}
						StiContainer stiContainer6 = array2[num25][num22];
						if (stiContainer6 is StiLevelContainer)
						{
							continue;
						}
						if (!onlyCalc)
						{
							if (stiContainer6.Height == num23)
							{
								continue;
							}
							lock (((ICollection)stiContainer6.Components).SyncRoot)
							{
								foreach (StiComponent component4 in stiContainer6.Components)
								{
									if (component4.Bottom == stiContainer6.Height)
									{
										component4.Height = num23 - component4.Top;
									}
								}
							}
							stiContainer6.Height = num23;
						}
						else
						{
							hashtable[stiContainer6] = num23;
						}
					}
				}
				num = 0.0;
				for (int num26 = 0; num26 < columns; num26++)
				{
					num15 = 0.0;
					lock (((ICollection)array2[num26]).SyncRoot)
					{
						foreach (StiContainer item in array2[num26])
						{
							if (!(item is StiLevelContainer))
							{
								item.Top = num15;
								double num27 = (onlyCalc ? ((double)hashtable[item]) : item.Height);
								num15 += num27;
							}
						}
					}
					num = Math.Max(num, num15);
				}
			}
		}
		return num;
	}

	public int GetCurrentColumn()
	{
		if (ColumnDirection == StiColumnDirection.DownThenAcross)
		{
			return 1;
		}
		int num = countOfItems / Columns;
		return countOfItems - num * Columns + 1;
	}

	public int GetLengthOfLastRow()
	{
		int num = countOfItems;
		int num2 = num / Columns;
		int num3 = num2 * Columns;
		if (num != num3)
		{
			return num - num3;
		}
		return Columns;
	}
}
