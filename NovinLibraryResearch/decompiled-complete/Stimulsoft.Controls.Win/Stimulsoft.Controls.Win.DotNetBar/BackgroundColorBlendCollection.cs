using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class BackgroundColorBlendCollection : CollectionBase
{
	private ElementStyle m_Parent;

	internal ElementStyle Parent
	{
		get
		{
			return m_Parent;
		}
		set
		{
			m_Parent = value;
		}
	}

	public BackgroundColorBlend this[int index]
	{
		get
		{
			return (BackgroundColorBlend)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public int Add(BackgroundColorBlend item)
	{
		return base.List.Add(item);
	}

	public void AddRange(BackgroundColorBlend[] items)
	{
		foreach (BackgroundColorBlend item in items)
		{
			Add(item);
		}
	}

	public void Insert(int index, BackgroundColorBlend value)
	{
		base.List.Insert(index, value);
	}

	public int IndexOf(BackgroundColorBlend value)
	{
		return base.List.IndexOf(value);
	}

	public bool Contains(BackgroundColorBlend value)
	{
		return base.List.Contains(value);
	}

	public void Remove(BackgroundColorBlend value)
	{
		base.List.Remove(value);
	}

	public void CopyTo(BackgroundColorBlend[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	internal void CopyTo(BackgroundColorBlend[] array)
	{
		base.List.CopyTo(array, 0);
	}

	public ColorBlend GetColorBlend()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		ColorBlend val = new ColorBlend();
		Color[] array = new Color[base.Count];
		float[] array2 = new float[base.Count];
		for (int i = 0; i < base.Count; i++)
		{
			BackgroundColorBlend backgroundColorBlend = this[i];
			array[i] = backgroundColorBlend.Color;
			array2[i] = backgroundColorBlend.Position;
		}
		val.Colors = array;
		val.Positions = array2;
		return val;
	}

	public void CopyFrom(BackgroundColorBlendCollection col)
	{
		foreach (BackgroundColorBlend item in col)
		{
			Add(item);
		}
	}

	internal eBackgroundColorBlendType GetBlendType()
	{
		if (Count == 1)
		{
			return eBackgroundColorBlendType.SolidColor;
		}
		if (Count <= 1)
		{
			return eBackgroundColorBlendType.Invalid;
		}
		eBackgroundColorBlendType eBackgroundColorBlendType2 = eBackgroundColorBlendType.Invalid;
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				BackgroundColorBlend backgroundColorBlend = (BackgroundColorBlend)enumerator.Current;
				if (backgroundColorBlend.Position == 0f || backgroundColorBlend.Position == 1f)
				{
					continue;
				}
				if (backgroundColorBlend.Position <= 1f)
				{
					switch (eBackgroundColorBlendType2)
					{
					case eBackgroundColorBlendType.Invalid:
						eBackgroundColorBlendType2 = eBackgroundColorBlendType.Relative;
						break;
					case eBackgroundColorBlendType.Absolute:
						eBackgroundColorBlendType2 = eBackgroundColorBlendType.Invalid;
						goto end_IL_0074;
					}
				}
				else
				{
					switch (eBackgroundColorBlendType2)
					{
					case eBackgroundColorBlendType.Invalid:
						eBackgroundColorBlendType2 = eBackgroundColorBlendType.Absolute;
						break;
					case eBackgroundColorBlendType.Relative:
						eBackgroundColorBlendType2 = eBackgroundColorBlendType.Invalid;
						goto end_IL_0074;
					}
				}
				continue;
				end_IL_0074:
				break;
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
		if (Count == 2 && this[0].Position == 0f && this[1].Position == 1f)
		{
			return eBackgroundColorBlendType.Relative;
		}
		switch (eBackgroundColorBlendType2)
		{
		case eBackgroundColorBlendType.Invalid:
			return eBackgroundColorBlendType2;
		case eBackgroundColorBlendType.Relative:
			if (this[0].Position != 0f && this[Count - 1].Position != 1f)
			{
				return eBackgroundColorBlendType.Invalid;
			}
			break;
		}
		if (eBackgroundColorBlendType2 == eBackgroundColorBlendType.Absolute && Count / 2 * 2 != Count)
		{
			return eBackgroundColorBlendType.Invalid;
		}
		return eBackgroundColorBlendType2;
	}

	public static void InitializeCollection(BackgroundColorBlendCollection collection, int backColor1, int backColor2)
	{
		InitializeCollection(collection, ColorScheme.GetColor(backColor1), ColorScheme.GetColor(backColor2));
	}

	public static void InitializeCollection(BackgroundColorBlendCollection collection, Color backColor1, Color backColor2)
	{
		collection.Clear();
		collection.Add(new BackgroundColorBlend(backColor1, 0f));
		collection.Add(new BackgroundColorBlend(backColor2, 1f));
	}

	public static void InitializeCollection(BackgroundColorBlendCollection collection, Color backColor)
	{
		collection.Clear();
		collection.Add(new BackgroundColorBlend(backColor, 0f));
	}
}
