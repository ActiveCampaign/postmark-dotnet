using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// The merged request model, with any additional values that are referenced by any of the supplied templates.
        /// </summary>
        public dynamic SuggestedTemplateModel { get; set; }

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
