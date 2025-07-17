using LibrarySystem.Infrastructure.Enums;
using LibrarySystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace LibrarySystem.Infrastructure;
[HtmlTargetElement("book-filter-link")]
public class BooksFilterLinkTagHelper : TagHelper
{
    [HtmlAttributeName("filter-option-values")]
    public FilteringOptions CurrentFilter { get; set; }
    [HtmlAttributeName("filter-value")]
    public string FilterValue { get; set; }
    [HtmlAttributeName("filter-value-for")]
    public FilterValueFor FilterValueFor { get; set; }
    public string CssClass { get; set; } = "filter-pill";

    private List<string> queryParams;
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "a";
        output.Attributes.SetAttribute("class", CssClass);

        queryParams = [$"{FilterValueFor.ToString().ToLower()}={FilterValue}"];

        switch (FilterValueFor)
        {
            case FilterValueFor.Top:
                AddParam("genre", CurrentFilter.Genre);
                AddParam("format", CurrentFilter.Format);
                AddParam("search", CurrentFilter.SearchTerm);
                AddParam("page", CurrentFilter.Page);
                if (CurrentFilter.Top == FilterValue)
                    output.Attributes.SetAttribute("class", $"{CssClass} active");
            break;

            case FilterValueFor.Genre:
                AddParam("top", CurrentFilter.Top);
                AddParam("format", CurrentFilter.Format);
                AddParam("search", CurrentFilter.SearchTerm);
                AddParam("page", CurrentFilter.Page);
                if (CurrentFilter.Genre == FilterValue || (string.IsNullOrEmpty(CurrentFilter.Genre) && FilterValue.Equals("all", StringComparison.OrdinalIgnoreCase)))
                    output.Attributes.SetAttribute("class", $"{CssClass} active");
            break;

            case FilterValueFor.Format:
                AddParam("genre", CurrentFilter.Genre);
                AddParam("top", CurrentFilter.Top);
                AddParam("search", CurrentFilter.SearchTerm);
                AddParam("page", CurrentFilter.Page);
                if (CurrentFilter.Format == FilterValue || (string.IsNullOrEmpty(CurrentFilter.Format) && FilterValue.Equals("all", StringComparison.OrdinalIgnoreCase)))
                    output.Attributes.SetAttribute("class", $"{CssClass} active");
            break;

            case FilterValueFor.SearchTerm:
                AddParam("genre", CurrentFilter.Genre);
                AddParam("format", CurrentFilter.Format);
                AddParam("top", CurrentFilter.Top);
                AddParam("page", CurrentFilter.Page);
                if (CurrentFilter.SearchTerm == FilterValue)
                    output.Attributes.SetAttribute("class", $"{CssClass} active");
            break;

            case FilterValueFor.Page:
                AddParam("genre", CurrentFilter.Genre);
                AddParam("format", CurrentFilter.Format);
                AddParam("search", CurrentFilter.SearchTerm);
                AddParam("top", CurrentFilter.Top);
                if (CurrentFilter.Page.ToString() == FilterValue)
                    output.Attributes.SetAttribute("class", $"{CssClass} active");
            break;
        }//end switch

        string url = "/Books/BooksList?" + string.Join("&", queryParams);
        output.Attributes.SetAttribute("href", url);

        //output.Content.SetContent(FilterValue);
        base.Process(context, output);
    }
    private void AddParam(string key, object value)
    {
        if (value == null) return;

        var valueString = value.ToString();
        if (!string.IsNullOrEmpty(valueString))
        {
            queryParams.Add($"{key}={valueString}");
        }
    }
}
