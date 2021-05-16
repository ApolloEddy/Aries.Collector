using System;
using Aries.Collector.contact;

namespace Aries.Collector
{
	class Program
	{
		static void Main(string[] args)
		{
			//Console.WriteLine("Hello World!");
			//ArgumentParser argLex = new ArgumentParser();
			//var url = new Argument(ArgType.String, "url") { isDefault = true };
			//argLex.add(url);
			//argLex.Parse(args);
			//Console.WriteLine(argLex[url].ToString(url));
			//Console.ReadLine();
			var h = new Aries.Collector.Extractors.HuashitongCollector();
			//Console.WriteLine(h._tsParam);
			//Console.ReadKey();
			if (args.Length == 0) { h.catchRank(Environment.CurrentDirectory + '\\'); }
			var url = args[0];
			if (url.Contains("/draw/")) { h.download(url, Environment.CurrentDirectory + '\\'); }
			if (!url.Contains(h.HOST)) { h.downloadByTitle(url, Environment.CurrentDirectory + '\\'); }
			//h.downloadByTitle("白丝", Environment.CurrentDirectory + '\\');
			//h.rankList();
			//h.catchRank(Environment.CurrentDirectory);
		}
	}
}
