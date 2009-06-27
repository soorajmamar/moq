using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Moq.Tests
{
	public class IndexedPropertyFixture
	{
		public interface IFoo
		{
			Bar this[string key] { get; set; }
			Bar this[int key] { get; set; }
			Bar this[int key1, int key2] { get; set; }
		}

		public interface INotSupportedFoo
		{
			Bar this[int key1, int key2, int key3] { get; set; }
		}

		public class Bar { }

		Bar bar;
		Mock<IFoo> m;

		public IndexedPropertyFixture()
		{
			bar = new Bar();
			m = new Mock<IFoo>(MockBehavior.Strict);
		}

		#region Single indexed property
		[Fact]
		public void AllowGetByExactMatch()
		{
			m.SetupGet(x => x["magic-key"]).Returns(bar);
			Assert.Equal(bar, m.Object["magic-key"]);
		}

		[Fact]
		public void FailGetNotMatchingIndex()
		{
			m.SetupGet(x => x["magic-key"]).Returns(bar);
			Assert.Throws<MockException>(() => { var r = m.Object["boom"]; });
		}

		[Fact]
		public void AllowSetByExactMatch()
		{
			m.SetupSet(x => x["magic-key"]);
			m.Object["magic-key"] = bar;
		}

		[Fact]
		public void FailSetNotMatchingIndex()
		{
			m.SetupSet(x => x["magic-key"]);
			Assert.Throws<MockException>(() => { m.Object["boom"] = bar; });
		}

		[Fact]
		public void AllowSetForAny()
		{
			m.SetupSet(x => x[It.IsAny<string>()]);
			m.Object["foo"] = null;
			m.Object["bar"] = null;
		}

		[Fact]
		public void SetupIndexedPropertyAllowGetAndSet()
		{
			m.SetupProperty(x => x[It.IsAny<string>()]);
			m.Object["a"] = bar;
			Assert.Equal(bar, m.Object["a"]);
		}

		[Fact]
		public void SetupIndexedPropertyKeyAllowGetAndSet()
		{
			m.SetupProperty(x => x["good-key"]);
			m.Object["good-key"] = bar;
			Assert.Equal(bar, m.Object["good-key"]);
		}

		[Fact]
		public void SetupIndexedPropertyKeyFailSet()
		{
			m.SetupProperty(x => x["good-key"]);
			Assert.Throws<MockException>(() => { m.Object["other-key"] = bar; });
		}

		[Fact]
		public void SetupIndexedPropertyKeyFailGet()
		{
			m.SetupProperty(x => x["good-key"]);
			Assert.Throws<MockException>(() => { var r = m.Object["other-key"]; });
		}

		[Fact]
		public void SetupIndexedPropertyKeyAllowGetAndSetOnIntKeyedProperties()
		{
			m.SetupProperty(x => x[It.IsAny<int>()]);
			m.Object[4] = bar;
			Assert.Equal(bar, m.Object[4]);
		}

		[Fact]
		public void ShouldVerifyGet()
		{
			m.SetupProperty(x => x[It.IsAny<string>()]);
			m.Object["key"] = bar;
			var r = m.Object["key"];
			m.VerifyGet(x => x["key"]);
		}

		[Fact]
		public void ShouldNotVerifyGet()
		{
			m.SetupProperty(x => x[It.IsAny<string>()]);
			Assert.Throws<MockException>(() => m.VerifyGet(x => x["key"]));
		}

		[Fact]
		public void ShouldVerifySet()
		{
			m.SetupProperty(x => x[It.IsAny<string>()]);
			m.Object["key"] = bar;
			m.VerifySet(x => x["key"] = bar);
			m.VerifySet(x => x["key"]);
		}

		[Fact]
		public void ShouldNotVerifySet()
		{
			m.SetupProperty(x => x[It.IsAny<string>()]);
			Assert.Throws<MockException>(() => m.VerifySet(x => x["key"]));
			Assert.Throws<MockException>(() => m.VerifySet(x => x["key"] = bar));
		}
		#endregion

		#region Double indexed property
		[Fact]
		public void DIP_AllowGetByExactMatch()
		{
			m.SetupGet(x => x[0, 0]).Returns(bar);
			Assert.Equal(bar, m.Object[0, 0]);
		}

		[Fact]
		public void DIP_FailGetNotMatchingIndex()
		{
			m.SetupGet(x => x[0, 0]).Returns(bar);
			Assert.Throws<MockException>(() => { var r = m.Object[1, 1]; });
		}

		[Fact]
		public void DIP_AllowSetByExactMatch()
		{
			m.SetupSet(x => x[0, 0]);
			m.Object[0, 0] = bar;
		}

		[Fact]
		public void DIP_FailSetNotMatchingIndex()
		{
			m.SetupSet(x => x[0, 0]);
			Assert.Throws<MockException>(() => { m.Object[1, 1] = bar; });
		}

		[Fact]
		public void DIP_AllowSetForAny()
		{
			m.SetupSet(x => x[It.IsAny<int>(), It.IsAny<int>()]);
			m.Object[0, 0] = null;
			m.Object[0, 1] = null;
		}

		[Fact]
		public void DIP_SetupIndexedPropertyAllowGetAndSet()
		{
			m.SetupProperty(x => x[It.IsAny<int>(), It.IsAny<int>()]);
			m.Object[0, 0] = bar;
			Assert.Equal(bar, m.Object[0, 0]);
		}

		[Fact]
		public void DIP_SetupIndexedPropertyKeyAllowGetAndSet()
		{
			m.SetupProperty(x => x[0, 0]);
			m.Object[0, 0] = bar;
			Assert.Equal(bar, m.Object[0, 0]);
		}

		[Fact]
		public void DIP_SetupIndexedPropertyKeyFailSet()
		{
			m.SetupProperty(x => x[0, 0]);
			Assert.Throws<MockException>(() => { m.Object[1, 1] = bar; });
		}

		[Fact]
		public void DIP_SetupIndexedPropertyKeyFailGet()
		{
			m.SetupProperty(x => x[0, 0]);
			Assert.Throws<MockException>(() => { var r = m.Object[1, 1]; });
		}

		[Fact]
		public void DIP_SetupIndexedPropertyKeyAllowGetAndSetOnIntKeyedProperties()
		{
			m.SetupProperty(x => x[It.IsAny<int>(), It.IsAny<int>()]);
			m.Object[2, 2] = bar;
			Assert.Equal(bar, m.Object[2, 2]);
		}

		[Fact]
		public void DIP_ShouldVerifyGet()
		{
			m.SetupProperty(x => x[It.IsAny<int>(), It.IsAny<int>()]);
			m.Object[0, 0] = bar;
			var r = m.Object[0, 0];
			m.VerifyGet(x => x[0, 0]);
		}

		[Fact]
		public void DIP_ShouldNotVerifyGet()
		{
			m.SetupProperty(x => x[It.IsAny<int>(), It.IsAny<int>()]);
			Assert.Throws<MockException>(() => m.VerifyGet(x => x[1, 1]));
		}

		[Fact]
		public void DIP_ShouldVerifySet()
		{
			m.SetupProperty(x => x[It.IsAny<int>(), It.IsAny<int>()]);
			m.Object[0, 0] = bar;
			m.VerifySet(x => x[0, 0] = bar);
			m.VerifySet(x => x[0, 0]);
		}

		[Fact]
		public void DIP_ShouldNotVerifySet()
		{
			m.SetupProperty(x => x[It.IsAny<int>(), It.IsAny<int>()]);
			Assert.Throws<MockException>(() => m.VerifySet(x => x[1, 1]));
			Assert.Throws<MockException>(() => m.VerifySet(x => x[1, 1] = bar));
		}
		#endregion

		[Fact]
		public void MoreThanTwoIndexesAreNotSupported()
		{
			var m = new Mock<INotSupportedFoo>();
			Assert.Throws<NotSupportedException>(() => m.SetupProperty(x => x[It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()]));
		}
	}
}