using System.Collections;
using System.Drawing;
using System.Reflection;

namespace DevComponents.DotNetBar.Presentation;

[DefaultMember("Item")]
internal class _166D : CollectionBase
{
	public ᙝ _0CCA
	{
		get
		{
			return (ᙝ)base.List[ԡ];
		}
		set
		{
			base.List[ԡ] = value;
		}
	}

	public int _0CCB(ᙝ _166E)
	{
		return base.List.Add(_166E);
	}

	public void _0CCC(int ԡ, ᙝ ԝ)
	{
		base.List.Insert(ԡ, ԝ);
	}

	public int _0CCD(ᙝ ԝ)
	{
		return base.List.IndexOf(ԝ);
	}

	public bool _0CCE(ᙝ ԝ)
	{
		return base.List.Contains(ԝ);
	}

	public void _0CCF(ᙝ ԝ)
	{
		base.List.Remove(ԝ);
	}

	public void _0CD0(ᙝ[] ঀ, int ԡ)
	{
		base.List.CopyTo(ঀ, ԡ);
	}

	internal void _0CD0(ᙝ[] ঀ)
	{
		base.List.CopyTo(ঀ, 0);
	}
}
internal class _0CF3 : ᙝ
{
	private ᙞ _20AC = new ᙞ();

	private ᙞ _20AD = new ᙞ();

	private ᙔ ᖾ = new ᙔ();

	public ᙞ _20AA => _20AC;

	public ᙞ _20AB => _20AD;

	public ᙔ ᙴ => ᖾ;

	public _0CF3()
	{
	}

	public _0CF3(ᙞ startPoint, ᙞ endPoint, ᙔ border)
	{
		_20AC = startPoint;
		_20AD = endPoint;
		ᖾ = border;
	}

	public override void Paint(ᙰ p)
	{
		Point location = GetLocation(p.ঋ, _20AC);
		Point location2 = GetLocation(p.ঋ, _20AD);
		Graphics ܓ = p.ܓ;
		if (ᖾ.ᙖ.IsEmpty)
		{
			if (!ᖾ.ᙕ.IsEmpty)
			{
				DisplayHelp.DrawLine(ܓ, location, location2, ᖾ.ᙕ, ᖾ.ܐ);
			}
		}
		else
		{
			DisplayHelp.DrawGradientLine(ܓ, location, location2, ᖾ.ᙕ, ᖾ.ᙖ, ᖾ.ᙗ, ᖾ.ܐ);
		}
		base.Paint(p);
	}
}
