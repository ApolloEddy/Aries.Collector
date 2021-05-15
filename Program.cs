using System;

namespace Aries.Collector
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			var h = new Aries.Collector.Extractors.HuashitongCollector();
			//h.pictureList(json);
			h.rankList();
			h.catchRank( @"C:\Users\Administrator\Desktop\脚本\cache\");
		}
	}
}
