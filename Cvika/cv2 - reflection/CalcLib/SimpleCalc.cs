using System;

namespace CalcLib
{
	class SimpleCalc
	{
		private int x;
		private int y;

		private int result;
		public void SetXY(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public void Add()
		{
			result = x + y;
		}

		private void Multiply()
		{
			result = x * y;
		}

		public void ShowResult()
		{
			Console.WriteLine(result);
		}
	}
}
