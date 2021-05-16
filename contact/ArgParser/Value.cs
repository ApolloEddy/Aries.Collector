using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aries.Collector.contact
{ 
	public class Value
	{
		public Value(string v) { this.value = v; }
		public string value { get; }
		public ArgType type
		{
			get
			{
				if (value.StartsWith('-')) { return ArgType.Bool; }
				foreach (char item in value)
				{
					if (!char.IsDigit(item)) { goto L1; }
				}
				return ArgType.Int;
			L1:
				return ArgType.String;
			}
		}
		public bool vertify(Argument arg)
		{
			if (arg.type != type) { return false; }
			TextParser tp = new TextParser(value);
			if (arg.type == ArgType.Bool)
			{
				if (tp.extrim("-").ToLower().StartsWith(value.ToLower()[0])) { return true; }
			}
			if (!arg.isDefault)
			{
				if (value.ToLower().StartsWith(arg.key + "=")) { return true; }
				return true; // 存在逻辑漏洞
			}
			else if (value.ToLower().StartsWith(arg.key + "=")) { return true; }
			else{ return false; }
		}
		public string ToString(Argument arg)
		{
			return value.ToLower().Replace(arg.key + "=", "");
		}
	}
}
