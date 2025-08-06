using AutoMapper;
using DAL.Repository.DTO;
using Domain.Model.Product;

namespace BLL.Service.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //Product
        CreateMap<Product, CreateProduct>();
        CreateMap<CreateProduct, Product>()
            .ForMember(dest => dest.ProductDeliveryOptions,
                opt => opt.MapFrom(src =>
                    src.DeliveryOptionIds.Select(id => new ProductDeliveryOption { DeliveryOptionId = id })));

        CreateMap<Product, MarketplaceProductView>()
            .ForMember(dto => dto.DeliveryOptions,
                options => 
                    options.MapFrom(prod => prod.ProductDeliveryOptions.Select(d => new DeliveryOptionDTO() { Id = d.DeliveryOption.Id, Name = d.DeliveryOption.Name, Price = d.DeliveryOption.Price})));
        CreateMap<MarketplaceProductView, Product>();
        
        CreateMap<ShopProductView, Product>();
        CreateMap<Product, ShopProductView>().ForMember(x => x.ProductDeliveryOptions,
            x =>
                x.MapFrom(a => a.ProductDeliveryOptions.Select(i => i.DeliveryOption)));
        
        CreateMap<UpdateProduct, Product>();
        CreateMap<Product, UpdateProduct>();
        
        
        //KeyValue
        CreateMap<KeyValue, KeyValueDTO>();
        CreateMap<KeyValueDTO, KeyValue>();
        
        //ProductCharacteristic
        CreateMap<ProductCharacteristic, ProductCharacteristicDTO>();
        CreateMap<ProductCharacteristicDTO, ProductCharacteristic>();
        
        //ProductMedia
        CreateMap<ProductMedia, ProductMediaDTO>();
        CreateMap<ProductMediaDTO, ProductMedia>();
        
        //ProductQuestionAnswer
        CreateMap<ProductQuestionAnswer, ProductQuestionAnswerDTO>();
        CreateMap<ProductQuestionAnswerDTO, ProductQuestionAnswer>();
        
        CreateMap<CreateProductQuestionAnswer, ProductQuestionAnswer>();
        CreateMap<ProductQuestionAnswer, CreateProductQuestionAnswer>();
        
        
        //ProductQuestion
        CreateMap<ProductQuestion, ProductQuestionDTO>();
        CreateMap<ProductQuestionDTO, ProductQuestion>();
        
        CreateMap<CreateProductQuestion, ProductQuestion>();
        CreateMap<ProductQuestion, CreateProductQuestion>();
        
        //ProductReview
        CreateMap<ProductReview, ProductReviewDTO>();
        CreateMap<ProductReviewDTO, ProductReview>();
        
        CreateMap<CreateProductReview, ProductReview>();
        CreateMap<ProductReview, CreateProductReview>();
        
        //DeliveryOption
        CreateMap<DeliveryOptionDTO, DeliveryOption>();
        CreateMap<DeliveryOption, DeliveryOptionDTO>();
    }
}