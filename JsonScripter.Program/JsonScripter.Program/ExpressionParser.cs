using System;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;
using DynamicExpression = System.Linq.Dynamic.DynamicExpression;

namespace JsonScripter.Program
{
  internal static class ExpressionParser
  {
    public static Action<JObject> Parse(string fieldName, string expression)
    {
      var rootParameter = Expression.Parameter(Types.JObject, "root");
      var partName = Expression.Constant(fieldName, Types.String);
      var indexer = Types.JObject.GetProperty("Item", Types.JToken, new[] { Types.String });
      var rootProperty = Expression.Property(rootParameter, indexer, partName);
      var newValueLambda = DynamicExpression.ParseLambda(new[] { rootParameter }, Types.Object, expression);
      var newValueLambdaInvokation = Expression.Invoke(newValueLambda, rootParameter);
      var castNewValue = Expression.Call(Types.JToken, "FromObject", null, newValueLambdaInvokation);
      var assign = Expression.Assign(rootProperty, castNewValue);
      var lambda = Expression.Lambda<Action<JObject>>(assign, rootParameter);
      return lambda.Compile();
    }
  }
}
