using System.Runtime.CompilerServices;
using Riss.Devices;

internal abstract class j
{
	public enum a
	{
		a,
		b,
		c
	}

	internal const int a = 30000;

	internal const int b = 1;

	internal const int c = -1;

	private int d;

	private Device m_e;

	private ZdCommBase m_f;

	public j()
	{
		d = 30000;
	}

	[SpecialName]
	public Device n()
	{
		return this.m_e;
	}

	[SpecialName]
	public void a(Device A_0)
	{
		this.m_e = A_0;
	}

	[SpecialName]
	protected virtual ZdCommBase l()
	{
		return this.m_f;
	}

	[SpecialName]
	protected virtual void a(ZdCommBase A_0)
	{
		this.m_f = A_0;
	}

	protected virtual void m()
	{
		if (l() != null)
		{
			l().Close();
			a((ZdCommBase)null);
		}
	}

	public abstract int s();

	public abstract int e(k A_0, ulong A_1, ulong A_2);

	public abstract bool f(k A_0, int A_1);

	public virtual bool g(k A_0)
	{
		return f(A_0, d);
	}

	public abstract int h(ref ulong A_0, ref ulong A_1);

	public abstract int i(ref byte[] A_0, int A_1);

	public abstract int j(ref byte[] A_0, int A_1, int A_2);

	public virtual int t(ref byte[] A_0, int A_1)
	{
		return j(ref A_0, A_1, 30000);
	}

	public abstract int k(ref byte[] A_0, int A_1);

	public abstract int l(ref byte[] A_0, int A_1);

	public abstract int r(int A_0, int A_1, byte[] A_2);

	public abstract a m(ref ulong A_0, int A_1);

	public virtual a a(ref ulong A_0)
	{
		return m(ref A_0, 30000);
	}

	public abstract bool n(uint A_0);

	public abstract bool o();

	public abstract void p();

	public abstract void q();
}
