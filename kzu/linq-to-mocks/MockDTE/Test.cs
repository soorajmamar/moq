using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace MockDTE
{
	class Test
	{
		void Query(Expression<Func<IEnumerable<int>>> expression)
		{
			//new TestVisitor().Do(expression);
			Console.WriteLine(expression.ToString());
		}

		//void Do()
		//{
		//    Query(Expression.Lambda<Func<IEnumerable<int>>>(Expression.Call(null, typeof(Enumerable).GetMethod("Select"), new Expression[] { Expression.Call(null, typeof(Enumerable).GetMethod("WhereWhere"),
		//        new Expression[] { Expression.Call(null, typeof(Enumerable).GetMethod("SelectMany"),
		//            new Expression[] { Expression.Call(null, typeof(Program).GetMethod("Create"), 
		//                new Expression[0]), 
		//                Expression.Lambda<Func<string, IEnumerable<int>>>(Expression.Call(null, 
		//                typeof(Program).GetMethod("Create"), 
		//                new Expression[0]), 
		//                new ParameterExpression[] { Expression.Parameter(typeof(string), "strings") }), 
		//                Expression.Lambda(
		//                    Expression.New((ConstructorInfo)null),
		//                    //anonymous select (ConstructorInfo)null), new Expression[] { 
		//                    //Expression.Parameter(typeof(string), "strings"), 
		//                    //Expression.Parameter(typeof(int), "ints") }, 
		//                    //new MethodInfo[0] 
		//                    //{ 
		//                    //    //(MethodInfo) methodof(<>f__AnonymousType0<string, int>.get_strings, 
		//                    //    //<>f__AnonymousType0<string, int>), 
		//                    //    //(MethodInfo) methodof(<>f__AnonymousType0<string, int>.get_ints, 
		//                    //    //<>f__AnonymousType0<string, int>) 
		//                    //}
		//                    new ParameterExpression[0] 
		//                    { 
		//                        //CS$0$0003, CS$0$0005 
		//                    }) 
		//        }), 
		//        Expression.Lambda(
		//            Expression.GreaterThan(
		//                Expression.Property(Expression.Property(
		//            Expression.Parameter(typeof(object))
		//        //<>f__AnonymousType0<string, int>
		//        ), "<>h__TransparentIdentifier0"), 
		//        (MethodInfo)null, // methodof(<>f__AnonymousType0<string, int>.get_strings, 
		//        null //<>f__AnonymousType0<string, int>
		//    )), (MethodInfo) methodof(string.get_Length)), 
		//    Expression.Constant(0, typeof(int))), 
		//    new ParameterExpression[] { CS$0$0003 }) }), 
		//    Expression.Lambda(Expression.Property(CS$0$0003 = 
		//        Expression.Parameter(typeof(<>f__AnonymousType0<string, int>), "<>h__TransparentIdentifier0"), 
		//            (MethodInfo) methodof(<>f__AnonymousType0<string, int>.get_ints, <>f__AnonymousType0<string, int>)), 
		//    new ParameterExpression[] { CS$0$0003 }) }), new ParameterExpression[0]));

		//}
	}
}
