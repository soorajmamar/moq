using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Reflection;
using System.Linq.Expressions;

namespace Reflector.Tests
{
	[TestFixture]
	public class ReflectorFixture
	{
		[ExpectedException(typeof(ArgumentNullException))]
		[Test]
		public void ShouldThrowIfNullMethodLambda()
		{
			Reflector<Mock>.GetMethod((Expression<Action<Mock>>)null);
		}

		[ExpectedException(typeof(ArgumentNullException))]
		[Test]
		public void ShouldThrowIfNullPropertyLambda()
		{
			Reflector<Mock>.GetProperty((Expression<Func<Mock, object>>)null);
		}

		[ExpectedException(typeof(ArgumentNullException))]
		[Test]
		public void ShouldThrowIfNullFieldLambda()
		{
			Reflector<Mock>.GetField((Expression<Func<Mock, object>>)null);
		}

		[ExpectedException(typeof(ArgumentException))]
		[Test]
		public void ShouldThrowIfNotMethodLambda()
		{
			Reflector<Mock>.GetMethod(x => new object());
		}

		[ExpectedException(typeof(ArgumentException))]
		[Test]
		public void ShouldThrowIfNotPropertyLambda()
		{
			Reflector<Mock>.GetProperty(x => x.PublicField);
		}

		[ExpectedException(typeof(ArgumentException))]
		[Test]
		public void ShouldThrowIfNotFieldLambda()
		{
			Reflector<Mock>.GetField(x => x.PublicProperty);
		}

		[Test]
		public void ShouldGetPublicProperty()
		{
			PropertyInfo info = Reflector<Mock>.GetProperty(x => x.PublicProperty);
			Assert.That(info == typeof(Mock).GetProperty("PublicProperty"));
		}

		[Test]
		public void ShouldGetPublicField()
		{
			FieldInfo info = Reflector<Mock>.GetField(x => x.PublicField);
			Assert.That(info == typeof(Mock).GetField("PublicField"));
		}

		[Test]
		public void ShouldGetPublicVoidMethod()
		{
			MethodInfo info = Reflector<Mock>.GetMethod(x => x.PublicVoidMethod());
			Assert.That(info == typeof(Mock).GetMethod("PublicVoidMethod"));
		}

		[Test]
		public void ShouldGetPublicMethodParameterless()
		{
			MethodInfo info = Reflector<Mock>.GetMethod(x => x.PublicMethodNoParameters());
			Assert.That(info == typeof(Mock).GetMethod("PublicMethodNoParameters"));
		}

		[Test]
		public void ShouldGetPublicMethodParameters()
		{
			MethodInfo info = Reflector<Mock>.GetMethod<string, int>(
				(x, y, z) => x.PublicMethodParameters(y, z));
			Assert.That(info == typeof(Mock).GetMethod("PublicMethodParameters", new Type[] { typeof(string), typeof(int) }));
		}

		[Test]
		public void ShouldGetNonPublicProperty()
		{
			PropertyInfo info = Reflector<ReflectorFixture>.GetProperty(x => x.NonPublicProperty);
			Assert.That(info == typeof(ReflectorFixture).GetProperty("NonPublicProperty", BindingFlags.Instance | BindingFlags.NonPublic));
		}

		[Test]
		public void ShouldGetNonPublicField()
		{
			FieldInfo info = Reflector<ReflectorFixture>.GetField(x => x.NonPublicField);
			Assert.That(info == typeof(ReflectorFixture).GetField("NonPublicField", BindingFlags.Instance | BindingFlags.NonPublic));
		}

		[Test]
		public void ShouldGetNonPublicMethod()
		{
			MethodInfo info = Reflector<ReflectorFixture>.GetMethod(x => x.NonPublicMethod());
			Assert.That(info == typeof(ReflectorFixture).GetMethod("NonPublicMethod", BindingFlags.Instance | BindingFlags.NonPublic));
		}

		private int NonPublicField;

		private int NonPublicProperty
		{
			get { return NonPublicField; }
			set { NonPublicField = value; }
		}

		private object NonPublicMethod()
		{
			throw new NotImplementedException();
		}

		public class Mock
		{
			public int Value;
			public bool PublicField;
			private int valueProp;

			public Mock()
			{
			}

			public Mock(string foo, int bar)
			{
			}

			public int PublicProperty
			{
				get { return valueProp; }
				set { valueProp = value; }
			}

			public bool PublicMethodNoParameters()
			{
				throw new NotImplementedException();
			}

			public bool PublicMethodParameters(string foo, int bar)
			{
				throw new NotImplementedException();
			}

			public void PublicVoidMethod()
			{
				throw new NotImplementedException();
			}
		}
	}
}
