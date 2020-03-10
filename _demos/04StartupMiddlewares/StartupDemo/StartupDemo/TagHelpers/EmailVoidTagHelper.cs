using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StartupDemo.TagHelpers
{
    public class EmailVoidTagHelper : TagHelper
    {
        public string MailtTo { get; set; }
        public string MailtInfo { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            var content = await output.GetChildContentAsync();
            output.Attributes.SetAttribute("href", "mailto: " + MailtTo);
            output.Content.SetContent(MailtInfo);
        }

        protected EmailVoidTagHelper()
        {
        }
    }
}
