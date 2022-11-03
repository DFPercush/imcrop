using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imcrop
{
	class Mat3
	{
		float [] n = new float[9];
		public float this[int row, int col]
		{
			get
			{
				return n[col * 3 + row];
			}
			set
			{
				n[col * 3 + row] = value;
			}
		}
		//public static Mat3 operator*(Mat3 a, Mat3 b)
		//{
		//	Mat3 ret = new Mat3();
		//	ret.n[0] = 
		//}
	}
}
