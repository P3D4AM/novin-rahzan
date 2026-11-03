using System;
using System.Collections.Generic;

namespace Stimulsoft.Report.BarCodes;

public class StiBarcodeUtils
{
	public sealed class GaloisField
	{
		public static GaloisField Aztec_Data_12 = new GaloisField(4201, 4096, 1);

		public static GaloisField Aztec_Data_10 = new GaloisField(1033, 1024, 1);

		public static GaloisField Aztec_Data_8 = new GaloisField(301, 256, 1);

		public static GaloisField Aztec_Data_6 = new GaloisField(67, 64, 1);

		public static GaloisField Aztec_Param = new GaloisField(19, 16, 1);

		public static GaloisField QRCode_256 = new GaloisField(285, 256, 0);

		public static GaloisField Maxicode_64 = Aztec_Data_6;

		public static GaloisField DataMatrix_256 = Aztec_Data_8;

		private readonly int[] expTable;

		private readonly int[] logTable;

		private readonly GaloisFieldPolynomial zero;

		private readonly GaloisFieldPolynomial one;

		private readonly int size;

		private readonly int generatorBase;

		public GaloisField(int primitive, int size, int genBase)
		{
			this.size = size;
			generatorBase = genBase;
			expTable = new int[size];
			logTable = new int[size];
			int num = 1;
			for (int i = 0; i < size; i++)
			{
				expTable[i] = num;
				num <<= 1;
				if (num >= size)
				{
					num ^= primitive;
					num &= size - 1;
				}
			}
			for (int j = 0; j < size - 1; j++)
			{
				logTable[expTable[j]] = j;
			}
			zero = new GaloisFieldPolynomial(this, new int[1]);
			one = new GaloisFieldPolynomial(this, new int[1] { 1 });
		}

		internal GaloisFieldPolynomial GetZero()
		{
			return zero;
		}

		internal GaloisFieldPolynomial GetOne()
		{
			return one;
		}

		internal int GetGeneratorBase()
		{
			return generatorBase;
		}

		internal GaloisFieldPolynomial BuildMonomial(int degree, int coefficient)
		{
			if (degree < 0)
			{
				throw new ArgumentException();
			}
			if (coefficient == 0)
			{
				return zero;
			}
			int[] array = new int[degree + 1];
			array[0] = coefficient;
			return new GaloisFieldPolynomial(this, array);
		}

		internal static int AddOrSubtract(int a, int b)
		{
			return a ^ b;
		}

		internal int Exp(int a)
		{
			return expTable[a];
		}

		internal int Inverse(int a)
		{
			if (a == 0)
			{
				throw new ArithmeticException();
			}
			return expTable[size - logTable[a] - 1];
		}

		internal int Multiply(int a, int b)
		{
			if (a == 0 || b == 0)
			{
				return 0;
			}
			return expTable[(logTable[a] + logTable[b]) % (size - 1)];
		}
	}

	internal sealed class GaloisFieldPolynomial
	{
		private readonly GaloisField field;

		private readonly int[] coefficients;

		internal bool IsZero => coefficients[0] == 0;

		internal GaloisFieldPolynomial(GaloisField field, int[] coefficients)
		{
			if (coefficients == null || coefficients.Length == 0)
			{
				throw new ArgumentException();
			}
			this.field = field;
			int num = coefficients.Length;
			if (num > 1 && coefficients[0] == 0)
			{
				int i;
				for (i = 1; i < num && coefficients[i] == 0; i++)
				{
				}
				if (i == num)
				{
					this.coefficients = new int[1];
					return;
				}
				this.coefficients = new int[num - i];
				Array.Copy(coefficients, i, this.coefficients, 0, this.coefficients.Length);
			}
			else
			{
				this.coefficients = coefficients;
			}
		}

		internal int[] GetCoefficients()
		{
			return coefficients;
		}

		internal int GetDegree()
		{
			return coefficients.Length - 1;
		}

		internal int GetCoefficient(int degree)
		{
			return coefficients[coefficients.Length - 1 - degree];
		}

		internal GaloisFieldPolynomial AddOrSubtract(GaloisFieldPolynomial other)
		{
			if (!field.Equals(other.field))
			{
				throw new ArgumentException("GaloisFieldPolynomial do not have same GaloisField field");
			}
			if (IsZero)
			{
				return other;
			}
			if (other.IsZero)
			{
				return this;
			}
			int[] array = coefficients;
			int[] array2 = other.coefficients;
			if (array.Length > array2.Length)
			{
				int[] array3 = array;
				array = array2;
				array2 = array3;
			}
			int[] array4 = new int[array2.Length];
			int num = array2.Length - array.Length;
			Array.Copy(array2, 0, array4, 0, num);
			for (int i = num; i < array2.Length; i++)
			{
				array4[i] = GaloisField.AddOrSubtract(array[i - num], array2[i]);
			}
			return new GaloisFieldPolynomial(field, array4);
		}

		internal GaloisFieldPolynomial Multiply(GaloisFieldPolynomial other)
		{
			if (!field.Equals(other.field))
			{
				throw new ArgumentException("GaloisFieldPolynomial do not have same GaloisField field");
			}
			if (IsZero || other.IsZero)
			{
				return field.GetZero();
			}
			int[] array = coefficients;
			int num = array.Length;
			int[] array2 = other.coefficients;
			int num2 = array2.Length;
			int[] array3 = new int[num + num2 - 1];
			for (int i = 0; i < num; i++)
			{
				int a = array[i];
				for (int j = 0; j < num2; j++)
				{
					array3[i + j] = GaloisField.AddOrSubtract(array3[i + j], field.Multiply(a, array2[j]));
				}
			}
			return new GaloisFieldPolynomial(field, array3);
		}

		internal GaloisFieldPolynomial MultiplyByMonomial(int degree, int coefficient)
		{
			if (degree < 0)
			{
				throw new ArgumentException();
			}
			if (coefficient == 0)
			{
				return field.GetZero();
			}
			int num = coefficients.Length;
			int[] array = new int[num + degree];
			for (int i = 0; i < num; i++)
			{
				array[i] = field.Multiply(coefficients[i], coefficient);
			}
			return new GaloisFieldPolynomial(field, array);
		}

		internal GaloisFieldPolynomial[] Divide(GaloisFieldPolynomial other)
		{
			if (!field.Equals(other.field))
			{
				throw new ArgumentException("GaloisFieldPolynomial do not have same GaloisField field");
			}
			if (other.IsZero)
			{
				throw new ArgumentException("Divide by zero");
			}
			GaloisFieldPolynomial galoisFieldPolynomial = field.GetZero();
			GaloisFieldPolynomial galoisFieldPolynomial2 = this;
			int coefficient = other.GetCoefficient(other.GetDegree());
			int b = field.Inverse(coefficient);
			while (galoisFieldPolynomial2.GetDegree() >= other.GetDegree() && !galoisFieldPolynomial2.IsZero)
			{
				int degree = galoisFieldPolynomial2.GetDegree() - other.GetDegree();
				int coefficient2 = field.Multiply(galoisFieldPolynomial2.GetCoefficient(galoisFieldPolynomial2.GetDegree()), b);
				GaloisFieldPolynomial other2 = other.MultiplyByMonomial(degree, coefficient2);
				GaloisFieldPolynomial other3 = field.BuildMonomial(degree, coefficient2);
				galoisFieldPolynomial = galoisFieldPolynomial.AddOrSubtract(other3);
				galoisFieldPolynomial2 = galoisFieldPolynomial2.AddOrSubtract(other2);
			}
			return new GaloisFieldPolynomial[2] { galoisFieldPolynomial, galoisFieldPolynomial2 };
		}
	}

	public sealed class ReedSolomonEncoder
	{
		private readonly GaloisField field;

		private readonly IList<GaloisFieldPolynomial> cachedGenerators;

		public ReedSolomonEncoder(GaloisField field)
		{
			this.field = field;
			cachedGenerators = new List<GaloisFieldPolynomial>();
			cachedGenerators.Add(new GaloisFieldPolynomial(field, new int[1] { 1 }));
		}

		private GaloisFieldPolynomial BuildGenerator(int degree)
		{
			if (degree >= cachedGenerators.Count)
			{
				GaloisFieldPolynomial galoisFieldPolynomial = cachedGenerators[cachedGenerators.Count - 1];
				for (int i = cachedGenerators.Count; i <= degree; i++)
				{
					GaloisFieldPolynomial galoisFieldPolynomial2 = galoisFieldPolynomial.Multiply(new GaloisFieldPolynomial(field, new int[2]
					{
						1,
						field.Exp(i - 1 + field.GetGeneratorBase())
					}));
					cachedGenerators.Add(galoisFieldPolynomial2);
					galoisFieldPolynomial = galoisFieldPolynomial2;
				}
			}
			return cachedGenerators[degree];
		}

		public void Encode(int[] toEncode, int ecBytes)
		{
			if (ecBytes == 0)
			{
				throw new ArgumentException("No ErrorCorrection bytes");
			}
			int num = toEncode.Length - ecBytes;
			if (num <= 0)
			{
				throw new ArgumentException("No data bytes");
			}
			GaloisFieldPolynomial other = BuildGenerator(ecBytes);
			int[] array = new int[num];
			Array.Copy(toEncode, 0, array, 0, num);
			GaloisFieldPolynomial galoisFieldPolynomial = new GaloisFieldPolynomial(field, array);
			galoisFieldPolynomial = galoisFieldPolynomial.MultiplyByMonomial(ecBytes, 1);
			GaloisFieldPolynomial galoisFieldPolynomial2 = galoisFieldPolynomial.Divide(other)[1];
			int[] coefficients = galoisFieldPolynomial2.GetCoefficients();
			int num2 = ecBytes - coefficients.Length;
			for (int i = 0; i < num2; i++)
			{
				toEncode[num + i] = 0;
			}
			Array.Copy(coefficients, 0, toEncode, num + num2, coefficients.Length);
		}
	}

	public sealed class BitMatrix
	{
		private readonly int width;

		private readonly int height;

		private readonly int rowSize;

		private readonly int[] bits;

		public int Width => width;

		public int Height => height;

		public bool this[int x, int y]
		{
			get
			{
				int num = y * rowSize + (x >> 5);
				return ((bits[num] >>> x) & 1) != 0;
			}
			set
			{
				if (value)
				{
					int num = y * rowSize + (x >> 5);
					bits[num] |= 1 << x;
				}
				else
				{
					int num2 = y * rowSize + x / 32;
					bits[num2] &= ~(1 << x);
				}
			}
		}

		public BitMatrix(int dimension)
			: this(dimension, dimension)
		{
		}

		public BitMatrix(int width, int height)
		{
			if (width < 1 || height < 1)
			{
				throw new ArgumentException("Both dimensions must be greater than 0");
			}
			this.width = width;
			this.height = height;
			rowSize = width + 31 >> 5;
			bits = new int[rowSize * height];
		}
	}
}
