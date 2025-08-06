namespace Domain.Model.Order;

public class Address
{
    public int Id { get; set; }
    public string StreetName  { get; set; }
    public string StreetNumber { get; set; }
    public string FloorNumber { get; set; }
    public string PostalCode { get; set; }
    public string CityName { get; set; }
    public string CountryName { get; set; }
    
    public int OrderId { get; set; }
    public int? UserId { get; set; }
}