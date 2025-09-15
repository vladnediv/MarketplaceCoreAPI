using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using BLL.Model;
using BLL.Model.Constants;
using BLL.Model.DTO.Category;
using BLL.Model.DTO.Order;
using BLL.Model.DTO.Order.IncludedModels;
using BLL.Model.DTO.Product;
using BLL.Model.DTO.Product.IncludedModels.ProductQuestion;
using BLL.Model.DTO.Product.IncludedModels.ProductQuestionAnswer;
using BLL.Model.DTO.Product.IncludedModels.ProductReview;
using BLL.Service.Interface;
using BLL.Service.Interface.BasicInterface;
using Domain.Model.Order;
using Domain.Model.Product;

namespace BLL.Service;

public class ShopService : IShopService
{
    private readonly IProductService _productService;
    private readonly IGenericService<ProductQuestionAnswer> _questionAnswerService;
    private readonly IAdvancedService<Order> _orderService;
    private readonly IAdvancedService<OrderItem> _orderItemService;
    private readonly IAdvancedService<ProductReview> _reviewService;
    private readonly IAdvancedService<ProductQuestion> _questionService;
    private readonly IAdvancedService<DeliveryOption> _deliveryOptionService;
    private readonly IFileService _fileService;
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;

    public ShopService(
        IProductService productService,
        IGenericService<ProductQuestionAnswer> questionAnswerService,
        IAdvancedService<ProductReview> reviewService,
        IAdvancedService<ProductQuestion> questionService,
        IAdvancedService<DeliveryOption> deliveryOptionService,
        IAdvancedService<Order> orderService,
        IFileService fileService,
        ICategoryService categoryService,
        IAdvancedService<OrderItem> orderItemService,
        IMapper mapper)
    {
        _productService = productService;
        _questionAnswerService = questionAnswerService;
        _reviewService = reviewService;
        _questionService = questionService;
        _deliveryOptionService = deliveryOptionService;
        _fileService = fileService;
        _categoryService = categoryService;
        _orderService = orderService;
        _orderItemService = orderItemService;
        _mapper = mapper;
    }

    public async Task<ServiceResponse> CreateProductAsync(CreateProduct product)
    {
        //map createProduct to Product
        Product entity = _mapper.Map<CreateProduct, Product>(product);
        //set status to "Awaiting"
        entity.Status = ProductStatus.Awaiting;
        
        //check if the category exists
        var categoryRes = await _categoryService.GetAsync(entity.CategoryId);
        if (!categoryRes.IsSuccess)
        {
            return new ServiceResponse()
            {
                IsSuccess = false,
                Message = categoryRes.Message
            };
        }

        int i = 0;
        //save every picture in the mediaFiles
        foreach (var media in product.MediaFiles)
        {
            if (media.MediaType == MediaType.Image)
            {
                //try save the picture
                var saveRes = await _fileService.SavePictureAsync(media.File);
                //if save completes, save the path to the picture into the Product model
                if (saveRes.IsSuccess)
                {
                    entity.MediaFiles.ElementAt(i).Url = saveRes.Entity;
                }
            }

            i++;
        }

        //after pictures are saved and the path urls are passed into the product entity, create Product
        ServiceResponse<Product> response = await _productService.CreateAsync(entity);
        
        ServiceResponse serviceResponse = new ServiceResponse();
        if (response.IsSuccess)
        {
            serviceResponse.IsSuccess = true;
        }
        else
        {
            serviceResponse.IsSuccess = false;
            serviceResponse.Message = response.Message;
        }
        
        return serviceResponse;
    }

    public async Task<ServiceResponse> UpdateProductAsync(UpdateProduct updateProduct)
    {
        Product entity = _mapper.Map<Product>(updateProduct);
        
        //get the old product
        var oldProduct = await _productService.GetAsync(entity.Id);
        
        //delete the pictures from the old product which have been removed
        if (oldProduct.IsSuccess)
        {
            foreach (var oldMedia in oldProduct.Entity.MediaFiles)
            {
                if (oldMedia.MediaType == MediaType.Image)
                {
                    if (updateProduct.MediaFiles.FirstOrDefault(x => x.Url == oldMedia.Url) == null)
                    {
                       await _fileService.DeletePictureAsync(oldMedia.Url); 
                    }
                }
            }
            
            //assign new values to the product
            oldProduct.Entity.Name = entity.Name;
            oldProduct.Entity.Price = entity.Price;
            oldProduct.Entity.Stock = entity.Stock;
            oldProduct.Entity.DiscountValue = entity.DiscountValue;
            oldProduct.Entity.MediaFiles = entity.MediaFiles;
            oldProduct.Entity.Characteristics = entity.Characteristics;
            oldProduct.Entity.CategoryId = entity.CategoryId;
            oldProduct.Entity.BrandName = entity.BrandName;
            oldProduct.Entity.ProductDeliveryOptions = entity.ProductDeliveryOptions;
        }
        else
        {
            return new ServiceResponse()
            {
                IsSuccess = false,
                Message = oldProduct.Message
            };
        }

        //save new pictures
        int i = 0;
        foreach (var media in updateProduct.MediaFiles)
        {
            if (media.MediaType == MediaType.Image)
            {
                if (media.File != null)
                {
                   var url = await _fileService.SavePictureAsync(media.File);
                   if (url.IsSuccess)
                   {
                       oldProduct.Entity.MediaFiles.ElementAt(i).Url = url.Entity;
                   }
                }
            }

            i++;
        }
        
        //update the db product
        ServiceResponse<Product> response = await _productService.UpdateAsync(oldProduct.Entity);
        
        ServiceResponse serviceResponse = new ServiceResponse();
        if (response.IsSuccess)
        {
            serviceResponse.IsSuccess = true;
        }
        else
        {
            serviceResponse.IsSuccess = false;
            serviceResponse.Message = response.Message;
        }
        return serviceResponse;
    }
    public async Task<ServiceResponse> DeleteProductByIdAsync(int id, int userId)
    {
        //check if the product exists
        var res = await _productService.FirstOrDefaultAsync(x => x.ProductBrandId == userId && x.Id == id);
        if (!res.IsSuccess)
        {
            return new ServiceResponse()
            {
                IsSuccess = false,
                Message = res.Message
            };
        }
        
        //delete the product by ID
        ServiceResponse<Product> response = await _productService.DeleteByIdAsync(id);
        
        ServiceResponse serviceResponse = new ServiceResponse();
        if (response.IsSuccess)
        {
            //delete the media files of type "Image" from the server
            foreach (var mediaFile in res.Entity.MediaFiles)
            {
                if (mediaFile.MediaType == MediaType.Image)
                { 
                    await _fileService.DeletePictureAsync(mediaFile.Url);
                }
            }
            
            serviceResponse.IsSuccess = true;
        }
        else
        {
            serviceResponse.IsSuccess = false;
            serviceResponse.Message = response.Message;
        }

        return serviceResponse;
    }
    public async Task<ServiceResponse<ShopProductView>> GetProductByIdAsync(int id)
    {
        //get the product by ID
        ServiceResponse<Product> res = await _productService.GetAsync(id);
        ServiceResponse<ShopProductView> response = new ServiceResponse<ShopProductView>();
        if (res.IsSuccess)
        {
            //map the product to ShopProductView
            ShopProductView entity = _mapper.Map<Product, ShopProductView>(res.Entity);
            response.Entity = entity;
            response.IsSuccess = true;
        }
        else
        {
            response.IsSuccess = false;
            response.Message = res.Message;
        }
        return response;
    }
    public async Task<ServiceResponse<ShopProductView>> GetProductsByParameterAsync(Expression<Func<Product, bool>> predicate)
    {
        //get the products by parameter
        ServiceResponse<Product> response = await _productService.GetAllAsync(predicate);
        ServiceResponse<ShopProductView> res = new ServiceResponse<ShopProductView>();
        if (response.IsSuccess)
        {
            //map the products to List<ShopProductView>
            List<ShopProductView> entities = response.Entities.Select(x => _mapper.Map<Product, ShopProductView>(x)).ToList();
            
            res.Entities = entities;
            res.IsSuccess = true;
        }
        else
        {
            res.IsSuccess = false;
            res.Message = response.Message;
        }
        
        return res;
    }

    public async Task<ServiceResponse<ShopProductCard>> GetShopProductCardsAsync(int shopId)
    {
        var res = new ServiceResponse<ShopProductCard>();
        
        //get the products by shopId
        var getRes = await _productService.GetAllAsync(x => x.ProductBrandId == shopId);

        if (!getRes.IsSuccess)
        {
            res.IsSuccess = false;
            res.Message = getRes.Message;
            
            return res;
        }
        
        res.Entities = new List<ShopProductCard>();
        //map the raw products to ShopProductCard
        foreach (var product in getRes.Entities)
        {
            //convert each delivery option to a string and add to res
            try
            {
                var buf = _mapper.Map<Product, ShopProductCard>(product);
                buf.ProductDeliveryOptions = product.ProductDeliveryOptions.Select(x => x.Name).ToList();
                res.Entities.Add(buf);
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Message = ex.Message;
                res.Entities = null;
                
                return res;
            }
        }

        res.IsSuccess = true;

        return res;
    }

    public async Task<ServiceResponse> EditProductStatusAsync(int productId, int shopId, ProductStatus status)
    {
        var response = new ServiceResponse();
        
        var res = await _productService.GetAsync(productId);

        if (!res.IsSuccess)
        {
            response.IsSuccess = false;
            response.Message = res.Message;
            
            return response;
        }

        if (res.Entity.ProductBrandId != shopId)
        {
            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.AccessDenied(nameof(Product), productId);
        }

        res.Entity.Status = status;
        var updateRes = await _productService.UpdateAsync(res.Entity);

        if (!updateRes.IsSuccess)
        {
            response.IsSuccess = false;
            response.Message = updateRes.Message;
            
            return response;
        }
        
        response.IsSuccess = true;
        return response;
    }
    public async Task<ServiceResponse> EditProductActiveStatusAsync(int productId, int userId, bool isActive)
    {
        ServiceResponse serviceResponse = new ServiceResponse();
        
        var product = await _productService.GetAsync(productId);

        if (!product.IsSuccess)
        {
            serviceResponse.IsSuccess = false;
            serviceResponse.Message = product.Message;
            
            return serviceResponse;
        }

        if (product.Entity.ProductBrandId != userId)
        {
            serviceResponse.IsSuccess = false;
            serviceResponse.Message = ServiceResponseMessages.AccessDenied(nameof(Product), userId);
            
            return serviceResponse;
        }
        
        product.Entity.IsActive = isActive;
        product.Entity.Status = isActive && product.Entity.IsApproved && product.Entity.IsReviewed ? ProductStatus.Active : ProductStatus.Awaiting;
        
        var updateRes = await _productService.UpdateAsync(product.Entity);

        if (!updateRes.IsSuccess)
        {
            serviceResponse.IsSuccess = false;
            serviceResponse.Message = updateRes.Message;
            
            return serviceResponse;
        }
        
        serviceResponse.IsSuccess = true;
        
        return serviceResponse;
    }
    

    
    public async Task<ServiceResponse<CreateProductQuestionAnswer>> CreateProductQuestionAnswerAsync(CreateProductQuestionAnswer createProductQuestionAnswer)
    {
        ProductQuestionAnswer entity = _mapper.Map<ProductQuestionAnswer>(createProductQuestionAnswer);
        entity.CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
        
        ServiceResponse<ProductQuestionAnswer> response = await _questionAnswerService.CreateAsync(entity);
        
        ServiceResponse<CreateProductQuestionAnswer> serviceResponse = new ServiceResponse<CreateProductQuestionAnswer>();

        if (response.IsSuccess)
        {
            serviceResponse.IsSuccess = true;
        }
        else
        {
            serviceResponse.IsSuccess = false;
            serviceResponse.Message = response.Message;
        }

        return serviceResponse;
    }
    public async Task<ServiceResponse<ShopProductReviewView>> GetProductReviewsByParameterAsync(Expression<Func<ProductReview, bool>> predicate)
    {
        ServiceResponse<ProductReview> response = await _reviewService.GetAllAsync(predicate);
        
        ServiceResponse<ShopProductReviewView> serviceResponse = new ServiceResponse<ShopProductReviewView>();
        if (response.IsSuccess)
        {
            serviceResponse.IsSuccess = true;
            serviceResponse.Entities = response.Entities.Select(x => _mapper.Map<ProductReview, ShopProductReviewView>(x)).ToList();
        }
        else
        {
            serviceResponse.IsSuccess = false;
            serviceResponse.Message = response.Message;
        }
        
        return serviceResponse;
    }
    public async Task<ServiceResponse<ProductQuestionDTO>> GetProductQuestionsByParameterAsync(Expression<Func<ProductQuestion, bool>> predicate)
    {
        ServiceResponse<ProductQuestion> response = await _questionService.GetAllAsync(predicate);
        
        ServiceResponse<ProductQuestionDTO> serviceResponse = new ServiceResponse<ProductQuestionDTO>();
        if (response.IsSuccess)
        {
            serviceResponse.IsSuccess = true;
            serviceResponse.Entities = response.Entities.Select(x => _mapper.Map<ProductQuestion, ProductQuestionDTO>(x)).ToList();
        }
        else
        {
            serviceResponse.IsSuccess = false;
            serviceResponse.Message = response.Message;
        }

        return serviceResponse;
    }
    
    

    public async Task<ServiceResponse<ShopOrderView>> GetShopOrdersAsync(ClaimsPrincipal user, OrderStatus? status, PaymentMethod? paymentMethod)
    {
        var response = new ServiceResponse<ShopOrderView>();
        
        var shopId = GetUserIdFromClaims(user);
        //if shopId == 0, return response
        if (shopId == 0)
        {
            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.UserNotFound;
            
            return response;
        }

        //get orders with orderItems which products belong to the shop

        var orders = new ServiceResponse<Order>();

        if (status != null && paymentMethod != null)
        {
            orders = await _orderService.GetAllAsync(x => x.OrderItems.FirstOrDefault(
                y =>
                y.Product.ProductBrandId == shopId) != null &&
                                                          x.Status == status &&
                                                          x.PaymentMethod == paymentMethod);
        }
        else if (paymentMethod != null)
        {
            orders = await _orderService.GetAllAsync(x => x.OrderItems.FirstOrDefault(
                                                              y =>
                                                                  y.Product.ProductBrandId == shopId) != null &&
                                                          x.PaymentMethod == paymentMethod);
        }
        else if (status != null)
        {
            orders = await _orderService.GetAllAsync(x => x.OrderItems.FirstOrDefault(y =>
                                                              y.Product.ProductBrandId == shopId) != null &&
                                                          x.Status == status);
        }
        else
        {
            orders = await _orderService.GetAllAsync(x => x.OrderItems.FirstOrDefault(y => y.Product.ProductBrandId == shopId) != null);
        }
        if (!orders.IsSuccess)
        {
            response.IsSuccess = false;
            response.Message = orders.Message;
            
            return response;
        }
        
        //select only shops orderItems and collect them into the ShopOrderView
        var shopOrders = orders.Entities.Select(x => _mapper.Map<ShopOrderView>(x)).ToList();
        
        int i = 0;
        foreach (var order in orders.Entities)
        {
            shopOrders[i].OrderItems = new List<ShopOrderItemView>();
            foreach (var orderItem in order.OrderItems)
            {
                if (orderItem.Product.ProductBrandId == shopId)
                {
                    var mappedOrderItem = _mapper.Map<OrderItem, ShopOrderItemView>(orderItem);
                    shopOrders.ElementAt(i).OrderItems.Add(mappedOrderItem);
                }
            }
            i++;
        }
        
        
        
        response.IsSuccess = true;
        response.Entities = shopOrders;
        
        return response;
    }
    public async Task<ServiceResponse<ShopOrderView>> GetOrderByIdAsync(int id, ClaimsPrincipal user)
    {
        var response = new ServiceResponse<ShopOrderView>();
        
        //get the shop id from claims
        var shopId = GetUserIdFromClaims(user);
        if (shopId == 0)
        {
            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.UserNotFound;
            
            return response;
        }
        
        //get the order by id
        var order = await _orderService.GetAsync(id);

        if (!order.IsSuccess)
        {
            response.IsSuccess = false;
            response.Message = order.Message;
            
            return response;
        }
        
        var orderItems = new List<ShopOrderItemView>();
        //if no orderItems in the order belong to the shop, return false
        foreach (var orderItem in order.Entity.OrderItems)
        {
            if (orderItem.Product.ProductBrandId == shopId)
            {
                orderItems.Add(_mapper.Map<OrderItem, ShopOrderItemView>(orderItem));
            }
        }

        if (orderItems.Count == 0)
        {
            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.AccessDenied(nameof(Order), id);
            
            return response;
        }
        
        var shopOrder = _mapper.Map<ShopOrderView>(order.Entity);
        shopOrder.OrderItems = orderItems;
        
        response.IsSuccess = true;
        response.Entity =  shopOrder;
        
        return response;
    }
    public async Task<ServiceResponse> EditOrderStatusAsync(int orderId, OrderStatus status)
    {
        var response = new ServiceResponse();
        
        var res = await _orderService.GetAsync(orderId);

        if (!res.IsSuccess)
        {
            response.IsSuccess = false;
            response.Message = res.Message;
            
            return response;
        }
        
        res.Entity.Status = status;
        var updateRes = await _orderService.UpdateAsync(res.Entity);
        
        if (!updateRes.IsSuccess)
        {
            response.IsSuccess = false;
            response.Message = updateRes.Message;
            
            return response;
        }
        
        response.IsSuccess = true;
        
        return response;
    }
    public async Task<ServiceResponse> CheckOrderUpdatePermission(ClaimsPrincipal user, int orderId)
    {
        var response = new ServiceResponse();
        
        var id = GetUserIdFromClaims(user);
        if (id == 0)
        {
            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.UserNotFound;
            
            return response;
        }
        
        var getRes = await _orderService.GetAsync(orderId);

        if (!getRes.IsSuccess)
        {
            response.IsSuccess = false;
            response.Message = getRes.Message;
            
            return response;
        }
        
        bool foundShopId = getRes.Entity.OrderItems.FirstOrDefault(x => x.Product.ProductBrandId == id) != null;

        if (foundShopId)
        {
            response.IsSuccess = true;
            
            return response;
        }
        response.IsSuccess = false;
        response.Message = ServiceResponseMessages.AccessDenied(nameof(Order), orderId);
        
        return response;
    }
    public async Task<ServiceResponse> EditDeliveryStatusAsync(int orderItemId, int shopId, DeliveryStatus status)
    {
        var response = new ServiceResponse();

        var orderItem = await _orderItemService.GetAsync(orderItemId);
        
        if (!orderItem.IsSuccess)
        {
            response.IsSuccess = false;
            response.Message = orderItem.Message;
                
            return response;
        }

        if (orderItem.Entity.Product.ProductBrandId != shopId)
        {
            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.AccessDenied(nameof(OrderItem), orderItemId);

            return response;
        }
        
        orderItem.Entity.DeliveryStatus = status;

        var updateRes = await _orderItemService.UpdateAsync(orderItem.Entity);

        if (!updateRes.IsSuccess)
        {
            response.IsSuccess = false;
            response.Message = updateRes.Message;
            
            return response;
        }
        
        response.IsSuccess = true;
        return response;
    }
    
    

    public async Task<ServiceResponse<CategoryDTO>> GetSubcategoriesAsync(int parentCategoryId)
    {
        var response = new ServiceResponse<CategoryDTO>();

        var res = await _categoryService.GetSubcategoriesByParentIdAsync(parentCategoryId);
        if (res.IsSuccess)
        {
            response.IsSuccess = true;
            response.Entities = res.Entities.Select(c => _mapper.Map<CategoryDTO>(c)).ToList();
        }
        else
        {
            response.IsSuccess = false;
            response.Message = res.Message;
        }
        return response;
    }
    public async Task<ServiceResponse<CategoryDTO>> GetCategoryTreeAsync()
    {
        var response = new ServiceResponse<CategoryDTO>();
        var res = await _categoryService.GetCategoryTreeAsync();
        if (res.IsSuccess)
        {
            response.IsSuccess = true;
            response.Entities = res.Entities.Select(c => _mapper.Map<CategoryDTO>(c)).ToList();
        }
        else
        {
            response.IsSuccess = false;
            response.Message = res.Message;
        }
        return response;
    }
    public int GetUserIdFromClaims(ClaimsPrincipal user)
    {
        var id = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0";
        return int.Parse(id);
    }
}