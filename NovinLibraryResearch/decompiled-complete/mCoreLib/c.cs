using System;
using Microsoft.VisualBasic.CompilerServices;
using mCore;

internal class c
{
	private SMS m_a;

	private f m_b;

	internal c(ref SMS A_0)
	{
		this.m_a = A_0;
		this.m_b = this.m_a.gm;
	}

	internal void a(int A_0, int A_1, string A_2, string A_3, DateTime A_4, string A_5, int A_6, string A_7, int A_8, int A_9, int A_10)
	{
		i i2 = new i(A_0, A_1, A_2, A_3, A_4, A_5, A_6, A_7, A_8, A_9, A_10);
		try
		{
			this.m_b.cz.Add((object)i2, A_0.ToString(), (object)null, (object)null);
			i2 = null;
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			ProjectData.ClearProjectError();
		}
	}

	internal void a(int A_0, string A_1, string A_2, string A_3, DateTime A_4, string A_5, int A_6)
	{
		try
		{
			Message message = new Message(ref this.m_a, A_0, A_1, A_3, A_4, A_5, A_6, A_2);
			this.m_b.c0.Add((object)message, A_0.ToString(), (object)null, (object)null);
			if (message != null)
			{
				message = null;
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			ProjectData.ClearProjectError();
		}
	}

	internal void a()
	{
		try
		{
			this.m_b.cz.Clear();
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.m_b.a.b("[SIMBox:Clear:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
	}

	internal long b()
	{
		long result = default(long);
		try
		{
			result = this.m_b.cz.Count;
			return result;
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.m_b.a.b("[SIMBox:Count:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		return result;
	}

	internal void a(int A_0)
	{
		try
		{
			this.m_b.cz.Remove(A_0);
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.m_b.a.b("[SIMBox:Remove:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
	}

	internal i b(int A_0)
	{
		i result = null;
		try
		{
			result = (i)this.m_b.cz[A_0];
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			this.m_b.a.b("[SIMBox:Item:1] " + ex2.Message);
			ProjectData.ClearProjectError();
		}
		return result;
	}
}
