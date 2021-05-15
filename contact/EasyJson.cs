using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aries
{
	public class EasyJson
	{
		public EasyJson(string input) { sourceBlock = input;current = new StringBuilder(input);peek = current[0]; }
		public string sourceBlock { get; }
		public int position = 0;

		private StringBuilder current;
		private StringBuilder temp = new StringBuilder('\"');
		private char peek;
		private int depth = 0;
		int memory;
		/// <summary>
		/// 获取下一个深度的json子类块
		/// </summary>
		/// <param name="block">块名称</param>
		/// <returns>json子类块字符串</returns>
		public EasyJson matchBlock(string block)
		{
			var json = new EasyJson(match(block));
			json.depth = depth;
			json.position = position;
			json.peek = peek;
			return json;
		}

		private char readChar()
		{
			peek = current[position];
			if (peek == '{' || peek =='[' ) { depth++; }
			if (peek == '}' || peek == ']') { depth--; }
			position++;
			return peek;
		}
		private void move(params char[] chars)
		{
			for(; ; )
			{
				readChar();
				if (chars.Contains(peek)) { break; }
			}
		}
		private string match(string block)
		{
			block = '\"' + block + '\"';
			memory = depth;
			for(int i = position; i < current.Length; )
			{
				if (position == current.Length - 1) { break; }
				if(depth == memory + 1)
				{
					comparePosition(block);
					if(temp.Length == 0) { readChar(); continue; }
					if (temp.ToString(1, temp.Length - 1) == block) { break; }
				}
				readChar();
			}
			catchAllAllowings();
			return temp.ToString();
		}
		private void catchAllAllowings()
		{
			var cache = new StringBuilder();
			do
			{
				temp.Append(readChar());
				//if(depth == memory + 1) { temp.Append(cache.ToString());cache.Clear(); }
			} while ((depth == memory + 1)||(position < current.Length));			
		}
		private void comparePosition(string input)
		{
			if(position + input.Length > current.Length - 1) { return; }
			StringBuilder sb = new StringBuilder();
			for(int i = position; i < position + input.Length - 1;)
			{
				sb.Append(readChar());
			}
			if (sb.ToString() == input) { temp.Append(sb.ToString()); }
		}
	}
}
