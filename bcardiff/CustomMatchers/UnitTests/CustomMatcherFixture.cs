using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Moq.Tests
{
	[TestFixture]
	public class CustomMatcherFixture
	{
		public interface IFoo
		{
			void Bar(string p);
		}

		[Test]
		public void Foo()
		{
			var mock = new Mock<IFoo>();
			mock.Expect(x => x.Bar(
				It.Is<string>(a => IsMagicString(a))
			));
			mock.Object.Bar("magic");
		}

		[Test]
		public void ShouldTranslateToUseMatcherImplementation()
		{
			var mock = new Mock<IFoo>();
			mock.Expect(x => x.Bar(IsMagicString()));
			mock.Object.Bar("magic");
		}

		[Test]
		[ExpectedException]
		public void ShouldTranslateToUseMatcherImplementation2()
		{
			var mock = new Mock<IFoo>();
			mock.Expect(x => x.Bar(IsMagicString()));
			mock.Object.Bar("no-magic");
		}

		[CustomMatcher]
		public static string IsMagicString() { return null; }
		public static bool IsMagicString(string arg)
		{
			return arg == "magic";
		}
	}
}
