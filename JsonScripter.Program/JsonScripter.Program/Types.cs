using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace JsonScripter.Program
{
  internal static class Types
  {
    public static readonly Type String = typeof(string);

    public static readonly Type Object = typeof(object);

    public static readonly Type JObject = typeof(JObject);

    public static readonly Type JToken = typeof(JToken);
  }
}
