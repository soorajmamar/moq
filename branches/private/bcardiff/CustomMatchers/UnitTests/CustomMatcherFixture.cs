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
		public void ShouldTranslateToUseMatcherImplementation()
		{			
			var mock = new Mock<IFoo>();
			mock.Expect(x => x.Bar(IsMagicString()));
			IsMagicStringCalled = false;
			mock.Object.Bar("magic");
			Assert.IsTrue(IsMagicStringCalled);
		}

		[Test]
		//[ExpectedException] not used so IsMagicStringCalled can be verified
		public void ShouldTranslateToUseMatcherImplementation2()
		{
			var mock = new Mock<IFoo>();
			mock.Expect(x => x.Bar(IsMagicString()));
			IsMagicStringCalled = false;
			Exception expectedException = null;
			try
			{
				mock.Object.Bar("no-magic");
			}
			catch (Exception e)
			{
				expectedException = e;
			}
			Assert.IsNotNull(expectedException);
			Assert.IsTrue(IsMagicStringCalled);
		}

		static bool IsMagicStringCalled;

		[CustomMatcher]
		public static string IsMagicString() { return null; }
		public static bool IsMagicString(string arg)
		{
			IsMagicStringCalled = true;
			return arg == "magic";
		}

		[Test]
		public void ShouldUseAditionalArguments()
		{
			var mock = new Mock<IFoo>();
			mock.Expect(x => x.Bar(StartsWith("ma")));
			mock.Object.Bar("magic");
		}

		[Test]
		[ExpectedException]
		public void ShouldUseAditionalArguments2()
		{
			var mock = new Mock<IFoo>();
			mock.Expect(x => x.Bar(StartsWith("ma")));
			mock.Object.Bar("no-magic");
		}

		[CustomMatcher]
		public static string StartsWith(string prefix) { return null; }
		public static bool StartsWith(string arg, string prefix)
		{
			return arg.StartsWith(prefix);
		}
	}
}
