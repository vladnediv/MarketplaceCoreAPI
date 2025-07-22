using System.Text.Json.Serialization;

namespace Domain.Model.Product;

public class KeyValue
{
    public int Id { get; set; }
    public int ProductCharacteristicId { get; set; }
    public ProductCharacteristic ProductCharacteristic { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
}