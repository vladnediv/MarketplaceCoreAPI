using AutoMapper;
using BLL.Model.DTO.Cart;
using BLL.Model.DTO.Category;
using BLL.Model.DTO.Order;
using BLL.Model.DTO.Order.IncludedModels;
using BLL.Model.DTO.Product;
using BLL.Model.DTO.Product.IncludedModels;
using BLL.Model.DTO.Product.IncludedModels.DeliveryOption;
using BLL.Model.DTO.Product.IncludedModels.ProductQuestion;
using BLL.Model.DTO.Product.IncludedModels.ProductQuestionAnswer;
using BLL.Model.DTO.Product.IncludedModels.ProductReview;
using Domain.Model.Cart;
using Domain.Model.Category;
using Domain.Model.Order;
using Domain.Model.Product;

namespace BLL.Service.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //Product
        CreateMap<Product, CreateProduct>();
        CreateMap<CreateProduct, Product>();

        CreateMap<Product, MarketplaceProductView>();
        CreateMap<MarketplaceProductView, Product>();
        
        CreateMap<ShopProductView, Product>();
        CreateMap<Product, ShopProductView>();

        CreateMap<Product, AdminProductView>();
        CreateMap<AdminProductView, Product>();
        
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

        CreateMap<ProductReview, ShopProductReviewView>()
            .ForMember(x => x.ProductName,
                opt =>
                    opt.MapFrom(x => x.Product.Name));
        CreateMap<ShopProductReviewView, ProductReview>();
        
        //DeliveryOption
        CreateMap<DeliveryOptionDTO, DeliveryOption>();
        CreateMap<DeliveryOption, DeliveryOptionDTO>();
        
        CreateMap<DeliveryOption, UpdateDeliveryOption>();
        CreateMap<UpdateDeliveryOption, DeliveryOption>();
        
        //Cart
        CreateMap<CartItem, CartItemDTO>();
        CreateMap<CartItemDTO, CartItem>();
        
        CreateMap<Cart, CartDTO>();
        CreateMap<CartDTO, Cart>();
        
        //Category
        CreateMap<Category, CategoryDTO>();
        CreateMap<CategoryDTO, Category>();
        
        CreateMap<Category, CreateCategory>();
        CreateMap<CreateCategory, Category>();
        
        CreateMap<Category, UpdateCategory>();
        CreateMap<UpdateCategory, Category>();
        
        //Order
        CreateMap<Order, CreateOrder>();
        CreateMap<CreateOrder, Order>();
        
        CreateMap<Order, OrderDTO>();
        CreateMap<OrderDTO, Order>();
        

        CreateMap<OrderItem, OrderItemDTO>();
        CreateMap<OrderItemDTO, OrderItem>();
    }
}