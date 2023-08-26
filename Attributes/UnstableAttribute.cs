namespace Search.Attributes
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class UnstableAttribute : Attribute
	{
		public readonly string Description;
		public UnstableAttribute()
		{
			this.Description = string.Empty;
		}
		public UnstableAttribute(string? description = null)
		{
			this.Description = description ?? string.Empty;
		}
	}
}
