using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

public static class HtmlHelperExtensions
{
    public static MvcHtmlString EditorForMany<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, IEnumerable<TValue>>> expression, string htmlFieldName = null) where TModel : class
    {
        Debug.Print("Made it to the editor for many function");
        var items = expression.Compile()(html.ViewData.Model);
        var sb = new StringBuilder();

        if (String.IsNullOrEmpty(htmlFieldName))
        {
            var prefix = html.ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix;

            htmlFieldName = (prefix.Length > 0 ? (prefix + ".") : String.Empty) + ExpressionHelper.GetExpressionText(expression);
        }

        foreach (var item in items)
        {
            var dummy = new { Item = item };
            var guid = Guid.NewGuid().ToString();

            var memberExp = Expression.MakeMemberAccess(Expression.Constant(dummy), dummy.GetType().GetProperty("Item"));
            var singleItemExp = Expression.Lambda<Func<TModel, TValue>>(memberExp, expression.Parameters);

            sb.Append(String.Format(@"<input type=""hidden"" name=""{0}.Index"" value=""{1}"" />", htmlFieldName, guid));
            sb.Append(html.EditorFor(singleItemExp, null, String.Format("{0}[{1}]", htmlFieldName, guid), new { htmlAttributes = new { @class = "form-control specifyother", placeholder = "Specify other..."} }));
//            sb.Append(String.Format(
//@"<span class=""input-group-btn"">
//        <button class=""btn"" style=""background:none;border:none;"" type=""button"" id=""add-complaint"" value=""x"">
//            <span class=""glyphicon glyphicon-remove""></span>
//        </button>
//      </span>;",)
        }

        return new MvcHtmlString(sb.ToString());
    }
}