using Domain.Model.Order;
using Domain.Model.Product;

namespace BLL.HtmlTemplates;

public static class HtmlTemplatesService
{
    public static string GetBasicLayout(string subject, string content)
    {
        string baseTemplate = "";
        try
        {
            baseTemplate = File.ReadAllText("../BLL/HtmlTemplates/Templates/BasicLayout.html");
        }
        catch (Exception ex)
        {
            return "Error while loading page.";
        }

        string finalHtml = baseTemplate
            .Replace("{{Subject}}", subject)
            .Replace("{{Content}}", content)
            .Replace("{{BrandName}}", "PickPlace™")
            .Replace("{{Year}}", DateTime.UtcNow.Year.ToString());

        return finalHtml;
    }

    public static string GetOrderConfirmation(string email, string orderId, string orderTotal, List<OrderItem> listOrderItems, string apiBaseUrl)
    {
        string orderConfirmation = "";
        string templateOrderItem = "";
        try
        {
            orderConfirmation =
                File.ReadAllText("../BLL/HtmlTemplates/Templates/OrderConfirmation/ContentOrderConfirmation.html");
            templateOrderItem =
                File.ReadAllText("../BLL/HtmlTemplates/Templates/OrderConfirmation/OrderItem.html");
        }
        catch (Exception ex)
        {
            return "Error while loading page.";
        }


        string orderItems = "";
        foreach (var orderItem in listOrderItems)
        {
            var lineTotal = (orderItem.Product.Price - orderItem.Product.DiscountValue) * orderItem.Quantity;
            orderItems += templateOrderItem
                .Replace("{{PictureUrl}}",
                    apiBaseUrl + orderItem.Product.MediaFiles.FirstOrDefault(x => x.MediaType == MediaType.Image).Url)
                .Replace("{{Name}}", orderItem.Product.Name)
                .Replace("{{Qty}}", orderItem.Quantity.ToString())
                .Replace("{{LineTotal}}", lineTotal.ToString());
        }

        string finalHtml = orderConfirmation
            .Replace("{{UserName}}", email)
            .Replace("{{OrderNumber}}", orderId)
            .Replace("{{OrderItems}}", orderItems)
            .Replace("{{OrderTotal}}", orderTotal);

        return finalHtml;
    }
}