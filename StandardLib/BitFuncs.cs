using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandardLib
{
	public class BitFuncs
	{
		public static bool IsBitSet(byte b, int position)
		{
			return (b & (1 << position)) != 0;
		}

	}
}
