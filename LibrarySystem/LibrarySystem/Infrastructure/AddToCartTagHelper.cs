using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace LibrarySystem.Infrastructure;
[HtmlTargetElement("div", Attributes = "cart-item-book-id")]
public class AddToCartTagHelper : TagHelper
{
    public int CartItemBookID { get; set; }
    public int CartItemQuantity { get; set; }

    public int CartItemPrice { get;set; }
    public string PageWhereItemWasAdded { get; set; }
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        //Div container
        TagBuilder div = new("div");

        //Build form
        TagBuilder form  = new("form");
        div.InnerHtml.AppendHtml(form);
        form.Attributes.Add("method", "post");
        form.Attributes.Add("action", "/Books/AddToCart");

        //Build all fields
        //-Hiddent inputs 
        form.InnerHtml.AppendHtml(CreateHiddenInput("CartItem.BookID", CartItemBookID.ToString()));
        form.InnerHtml.AppendHtml(CreateHiddenInput("CartItem.Price", CartItemPrice.ToString("F2")));
        form.InnerHtml.AppendHtml(CreateHiddenInput("CartItem.Quantity", CartItemQuantity.ToString()));
        form.InnerHtml.AppendHtml(CreateHiddenInput("PageWhereItemWasAdded", PageWhereItemWasAdded));

        //Button
        var buttonDiv = new TagBuilder("div");
        buttonDiv.AddCssClass("buttons");

        var button = new TagBuilder("button");
        button.Attributes.Add("type", "submit");
        button.AddCssClass("btn button-blue button-stretch-width");
        button.InnerHtml.Append("Add to cart");

        //Attach button
        buttonDiv.InnerHtml.AppendHtml(button);
        form.InnerHtml.AppendHtml(buttonDiv);

        output.Content.SetHtmlContent(form);
    }//Process
    private TagBuilder CreateHiddenInput(string name, string value)
    {
        var input = new TagBuilder("input");
        input.Attributes["type"] = "hidden";
        input.Attributes["name"] = name;
        input.Attributes["value"] = value;
        return input;
    }
}//class
