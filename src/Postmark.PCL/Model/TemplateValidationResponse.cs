using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostmarkDotNet.Model
{
    /// <summary>
    /// Indicates the result of validating and test rendering template content against the API.
    /// </summary>
    public class TemplateValidationResponse
    {
        /// <summary>
        /// Indicates whether all of the parts included in the validation request are valid and rendered properly using a test model.
        /// </summary>
        public bool AllContentIsValid { get; set; }

        /// <summary>
        /// If HTMLBody was present in validation request, indicates the outcome of the validation/rendering
        /// </summary>
        public TemplateValidationResult HtmlBody { get; set; }

        /// <summary>
        /// If TextBody was present in validation request, indicates the outcome of the validation/rendering
        /// </summary>
        public TemplateValidationResult TextBody { get; set; }

        /// <summary>
        /// If Subject was present in validation request, indicates the outcome of the validation/rendering
        /// </summary>
        public TemplateValidationResult Subject { get; set; }

        private dynamic _suggestedModel = null;

        /// <summary>
        /// The merged request model, with any additional values that are referenced by any of the supplied templates.
        /// </summary>
        public dynamic SuggestedTemplateModel { get { return _suggestedModel; } set { _suggestedModel = ConvertJsonResponse(value); } }

        private dynamic ConvertJsonResponse(dynamic value)
        {
            dynamic retval = null;
            if (value is JObject)
            {
                var dictionary = new ExpandoObject() as IDictionary<string, object>;
                var o = value as JObject;
                foreach (var prop in o.Properties())
                {
                    dictionary[prop.Name] = ConvertJsonResponse(prop.Value);
                }
                retval = dictionary as ExpandoObject;
            }
            else if (value is JArray)
            {
                var o = value as JArray;
                retval = o.Select(ConvertJsonResponse).ToArray();
            }
            else if (value is JValue)
            {
                var o = value as JValue;
                retval = o.Value;
            }
            return retval;
        }

        /// <summary>
        /// Indicates the outcome of validation of a given template.
        /// </summary>
        public class TemplateValidationResult
        {
            public bool ContentIsValid { get; set; }
            public string ValidationError { get; set; }
            public string RenderedContent { get; set; }
        }
    }
}
