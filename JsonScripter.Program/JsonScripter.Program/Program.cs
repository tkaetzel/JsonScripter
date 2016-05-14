using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonScripter.Program
{
  class Program
  {
    private IList<Action<JObject>> _actions;
    private Program()
    {
      _actions = new List<Action<JObject>>();
    }

    static void Main(string[] args)
    {
      new Program().Run();
    }

    void Run()
    {
      var root = GetJson();
      Console.Clear();
      Console.WriteLine(root.ToString(Formatting.Indented));
      Console.WriteLine();
      Console.WriteLine("------------ Actions ------------");
      var done = false;
      while (!done)
      {
        Console.Write("> ");
        GetNextAction(Console.ReadLine(), out done);
      }
      RunActions(root);
      Console.WriteLine("------------ Result ------------");
      Console.WriteLine(root.ToString(Formatting.Indented));
      Console.Read();
    }

    JObject GetJson()
    {
      while (true)
      {
        Console.WriteLine("Enter JSON string:");
        try
        {
          return JObject.Parse(Console.ReadLine());
        }
        catch (JsonReaderException jex)
        {
          Console.WriteLine(jex.Message);
        }
      }
    }

    void GetNextAction(string expression, out bool done)
    {
      if (string.IsNullOrWhiteSpace(expression))
      {
        done = true;
        return;
      }
      var parts = expression.Split(new[] { ':' }, 2);
      if (parts.Length != 2)
      {
        Console.WriteLine("Invalid action - please try again");
        done = false;
        return;
      }
      var field = parts[0];
      var action = parts[1];
      if (action.Equals("remove", StringComparison.OrdinalIgnoreCase))
      {
        _actions.Add(jObject => jObject.Remove(field));
        done = false;
        return;
      }
      var updateAction = ExpressionParser.Parse(field, action);
      if (updateAction != null)
      {
        _actions.Add(updateAction);
      }
      done = false;
      return;
    }

    void RunActions(JObject root)
    {
      foreach (var action in _actions)
      {
        action(root);
      }
    }
  }
}
