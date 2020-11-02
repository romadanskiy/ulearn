using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Reflection.Differentiation
{
   public static class Algebra
   {
      public static Expression<Func<double, double>> Differentiate(Expression<Func<double, double>> func)
      {
         var exp = GetExpression(func.Body);
         return (Expression<Func<double, double>>) Expression.Lambda(exp, func.Parameters);
      }

      private static Expression GetExpression(Expression body)
      {
         switch (body)
         {
            // константа
            case ConstantExpression _:
               return Expression.Constant(0.0);
            
            // параметр
            case ParameterExpression _:
               return Expression.Constant(1.0);
            
            // операция
            case BinaryExpression operation:
               var left = operation.Left;
               var right = operation.Right;
               switch (operation.NodeType)
               {
                  case ExpressionType.Add:
                     return Expression.Add(GetExpression(left), GetExpression(right));
                  case ExpressionType.Multiply:
                     return Expression.Add(
                        Expression.Multiply(GetExpression(left), right),
                        Expression.Multiply(left, GetExpression(right))
                     );
                  default:
                     throw new ArgumentException($"{operation.NodeType} is not supported operation");
               }
               
            // метод
            case MethodCallExpression method:
               var parameter = method.Arguments[0];
               Expression exp;
               switch (method.Method.Name)
               {
                  case "Sin":
                     exp = Expression.Call(typeof(Math).GetMethod("Cos", new[] { typeof(double) }), parameter);
                     break;
                  case "Cos":
                     exp = Expression.Multiply(
                        Expression.Constant(-1.0),
                        Expression.Call(typeof(Math).GetMethod("Sin", new[] { typeof(double) }), parameter)
                        );
                     break;
                  default:
                     throw new ArgumentException($"{method.Method.Name} is not supported method");
               }
               return Expression.Multiply(GetExpression(parameter), exp);
            
            default:
               throw new ArgumentException($"{body} is not supported syntax");
         }
      }
   }
}
