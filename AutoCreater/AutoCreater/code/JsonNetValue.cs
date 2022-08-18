using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.IO;
using System.Web.Mvc;


namespace AutoCreater.code
{

    /// <summary>
    /// 修改系统的JSON序列化器
    /// </summary>
    public class JsonNetValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(ControllerContext ctlContext)
        {
            //if (!controllerContext.HttpContext.Request.ContentType.
            //    StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
            //{
            //    return null;
            //}

            var reader = new StreamReader(ctlContext.HttpContext.Request.InputStream);
            reader.BaseStream.Position = 0;
            var json = reader.ReadToEnd()?.TrimStart(' ', '\r', '\n', '\t');
            if (string.IsNullOrEmpty(json))
                return null;

            var jtoken = json.StartsWith("[")
                ? JArray.Parse(json) as JContainer
                : JObject.Parse(json) as JContainer;
            return new JsonNetValueProvider(jtoken);
        }
    }

    public class JsonNetValueProvider : IValueProvider
    {
        private JContainer _jValue;

        public JsonNetValueProvider(JContainer jval)
        {
            _jValue = jval;
        }

        public bool ContainsPrefix(string prefix)
        {
            return true;
        }

        public ValueProviderResult GetValue(string key)
        {
            var jtoken = _jValue.SelectToken(key);
            if (jtoken == null)
            {
                jtoken = _jValue;
            }
            return new JsonNetValueProviderResult(jtoken, key, null);
        }
    }

    public class JsonNetValueProviderResult : ValueProviderResult
    {
        private JToken _jtoken;
        public JsonNetValueProviderResult(JToken valueRaw, string key, CultureInfo info)
        {
            _jtoken = valueRaw;
        }
        //[System.Diagnostics.DebuggerHidden]
        public override object ConvertTo(Type type, CultureInfo culture)
        {
            return _jtoken?.ToObject(type);
        }
    }

    public class JsonNetModelBinder : DefaultModelBinder
    {
        [System.Diagnostics.DebuggerHidden]
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var provider = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (provider != null)
            {
                try
                {
                    return provider.ConvertTo(bindingContext.ModelType);
                }
                catch { }
            }
            return base.BindModel(controllerContext, bindingContext);
        }
    }
}