using LibplctagWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyPLCProject2
{
	class Program
	{
		private const int DataTimeout = 5000;
		private const int AlarmFileSize = 100;

		static void Main(string[] args)
		{
			try
			{
				var tag = new Tag("172.20.130.101", "1, 0", CpuType.LGX, "ALARM_FILE[0]", DataType.Int16, 100, 1);
				using (var client = new Libplctag())
				{
					client.AddTag(tag);

					while (client.GetStatus(tag) == Libplctag.PLCTAG_STATUS_PENDING)
					{
						Thread.Sleep(100);
					}

					if (client.GetStatus(tag) != Libplctag.PLCTAG_STATUS_OK)
					{
						Console.WriteLine($"Error Setting up tag internal state. Error { client.DecodeError(client.GetStatus(tag)) }\n");
						return;
					}

					// Do the read
					var result = client.ReadTag(tag, DataTimeout);

					// Check the read result
					if (result != Libplctag.PLCTAG_STATUS_OK)
					{
						Console.WriteLine($"Error: Unable to read the data! Got error code {result}: {client.DecodeError(result)}\n");
						return;
					}

					Console.WriteLine($"Command: { client.GetInt16Value(tag, (0 * tag.ElementSize))}\n");

					for (int i = 1; i < tag.ElementCount; i++)
					{
						Console.WriteLine($"         |012345678012345678|");
						Console.WriteLine($"ALARM[{i}]={Convert.ToString(client.GetInt16Value(tag, (i * tag.ElementSize)), 2)}\n");
					}
				}
			}
			finally
			{
				Console.ReadKey();
			}
		}
	}
}
