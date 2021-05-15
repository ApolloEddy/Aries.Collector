using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aries.Collector.DataContainer
{
	public abstract class Video
	{
		public string title { get; }
		public string artist { get; }
		public string link { get; }
		public DateTime update { get; }
	}
}
