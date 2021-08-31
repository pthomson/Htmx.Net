using System.Text.Json;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Htmx.TagHelpers
{
    [HtmlTargetElement("meta", Attributes = "[name=htmx-config]", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class HtmxConfigMetaTagHelper : TagHelper
    {
        /// <summary>
        /// defaults to true, really only useful for testing
        /// </summary>
        [HtmlAttributeName("historyEnabled")]
        public bool? HistoryEnabled { get; set; }

        /// <summary>
        /// defaults to 10
        /// </summary>
        [HtmlAttributeName("historyCacheSize")]
        public int? HistoryCacheSize { get; set; } = 10;
        
        /// <summary>
        /// defaults to false, if set to true htmx will issue a full page refresh on history misses rather than use an AJAX request
        /// </summary>
        [HtmlAttributeName("refreshOnHistoryMiss")]
        public bool? RefreshOnHistoryMiss { get; set; }

        /// <summary>
        /// defaults to innerHTML
        /// </summary>
        [HtmlAttributeName("defaultSwapStyle")]
        public string? DefaultSwapStyle { get; set; }
        
        /// <summary>
        /// defaults to 0
        /// </summary>
        [HtmlAttributeName("defaultSwapDelay")]
        public int? DefaultSwapDelay { get; set; }
        
        /// <summary>
        /// defaults to 100
        /// </summary>
        [HtmlAttributeName("defaultSettleDelay")]
        public int? DefaultSettleDelay { get; set; }
        
        /// <summary>
        /// defaults to true (determines if the indicator styles are loaded)
        /// </summary>
        [HtmlAttributeName("includeIndicatorStyles")]
        public bool? IncludeIndicatorStyles { get; set; }
        
        /// <summary>
        /// defaults to htmx-indicator
        /// </summary>
        [HtmlAttributeName("indicatorClass")]
        public string? IndicatorClass { get; set; }
        
        /// <summary>
        /// defaults to htmx-request
        /// </summary>
        [HtmlAttributeName("requestClass")]
        public string? RequestClass { get; set; }
        
        /// <summary>
        /// defaults to htmx-settling
        /// </summary>
        [HtmlAttributeName("settlingClass")]
        public string? SettlingClass { get; set; }
        
        /// <summary>
        /// defaults to htmx-swapping
        /// </summary>
        [HtmlAttributeName("swappingClass")]
        public string? SwappingClass { get; set; }
        
        /// <summary>
        /// defaults to true
        /// </summary>
        [HtmlAttributeName("allowEval")]
        public bool? AllowEval { get; set; }
        
        /// <summary>
        /// defaults to false, HTML template tags for parsing content from the server (not IE11 compatible!)
        /// </summary>
        [HtmlAttributeName("useTemplateFragments")]
        public bool? UseTemplateFragments { get; set; }
        
        /// <summary>
        /// defaults to full-jitter
        /// </summary>
        [HtmlAttributeName("wsReconnectDelay")]
        public string? WsReconnectDelay { get; set; }
        
        /// <summary>
        /// defaults to [disable-htmx], [data-disable-htmx], htmx will not process elements with this attribute on it or a parent
        /// </summary>
        [HtmlAttributeName("disableSelector")]
        public string? DisableSelector { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var config = JsonSerializer.Serialize(new {
                AllowEval,
                DisableSelector,
                HistoryEnabled,
                IndicatorClass,
                RequestClass,
                SettlingClass,
                SwappingClass,
                DefaultSettleDelay,
                DefaultSwapDelay,
                DefaultSwapStyle,
                HistoryCacheSize,
                IncludeIndicatorStyles,
                UseTemplateFragments,
                WsReconnectDelay,
                RefreshOnHistoryMiss
            }, new JsonSerializerOptions
            {
                WriteIndented = false,
                IgnoreNullValues = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            
            output.Attributes.RemoveAll("content");
            
            // make sure to use single quotes
            // because JSON uses double quotes
            output.Attributes.Add(new TagHelperAttribute(
                "content",
                new HtmlString(config),
                HtmlAttributeValueStyle.SingleQuotes)
            );
        }
    }
}