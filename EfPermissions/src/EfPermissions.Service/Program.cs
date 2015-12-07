using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ServiceProcess;
using static System.Console;

namespace EfPermissions.Service
{
	public class Program : ServiceBase
	{
		public static void Main(string[] args)
		{
			WriteLine("EfPermissions");
			ReadKey();
		}
	}
}