using System.Linq.Expressions;
using AutoMapper;
using BLL.Service.Interface;
using BLL.Service.Model;
using DAL.Repository.DTO;
using DAL.Repository.Interface;
using Domain.Model.Product;

namespace BLL.Service;

public class AdminService : IAdminService
{
    private readonly IProductService  _productService;
    private readonly IAdvancedService<DeliveryOption> _deliveryOptionService;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;

    public AdminService(IProductService productService,
        IAdvancedService<DeliveryOption> deliveryOptionService,
        IFileService fileService,
        IMapper mapper)
    {
        _productService = productService;
        _deliveryOptionService = deliveryOptionService;
        _fileService = fileService;
        _mapper = mapper;
    }


    public async Task<ServiceResponse> EditProductApprovedStatusAsync(int productId, bool isApproved)
    {
        ServiceResponse serviceResponse = new ServiceResponse();
        
        var product = await _productService.GetAsync(productId);

        if (!product.IsSuccess)
        {
            serviceResponse.IsSuccess = false;
            serviceResponse.Message = product.Message;
            
            return serviceResponse;
        }
        
        product.Entity.IsReviewed = true;
        product.Entity.IsApproved = isApproved;
        
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

    public async Task<ServiceResponse<AdminProductView>> GetProductsByParameterAsync(Expression<Func<Product, bool>> predicate)
    {
        //get the products by parameter
        ServiceResponse<Product> response = await _productService.GetAllAsync(predicate);
        ServiceResponse<AdminProductView> res = new ServiceResponse<AdminProductView>();
        if (response.IsSuccess)
        {
            //map the products to List<AdminProductView>
            List<AdminProductView> entities = response.Entities.Select(x => _mapper.Map<Product, AdminProductView>(x)).ToList();

            int i = 0;
            foreach (var product in response.Entities)
            {
                //get the path to the main picture
                string path = product.MediaFiles.FirstOrDefault(x => x.MediaType == MediaType.Image).Url;
                
                //load the main picture
                var loadRes = await _fileService.GetPictureAsync(path);

                if (loadRes.IsSuccess)
                {
                    //if load succeeded, assign the media content
                    entities[i].MediaFiles
                        .FirstOrDefault(x => x.MediaType == MediaType.Image)
                        .MediaContent = loadRes.Entity;
                }

                i++;
            }
            
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

    public async Task<ServiceResponse> DeleteProductAsync(int productId)
    {
        ServiceResponse serviceResponse = new ServiceResponse();

        //get the product to get the picture locations
        var prodRes = await _productService.GetAsync(productId);
        
        //if couldnt find product, return error message
        if (!prodRes.IsSuccess)
        {
            serviceResponse.IsSuccess = false;
            serviceResponse.Message = prodRes.Message;
            
            return serviceResponse;
        }
        //delete the product by ID
        var deleteRes = await _productService.DeleteByIdAsync(productId);

        //if couldnt delete product, return error message
        if (!deleteRes.IsSuccess)
        {
            serviceResponse.IsSuccess = false;
            serviceResponse.Message = deleteRes.Message;
            
            return serviceResponse;
        }
        
        //delete product pictures
        foreach (var mediaFile in prodRes.Entity.MediaFiles)
        { 
            await _fileService.DeletePictureAsync(mediaFile.Url);
        }
        
        serviceResponse.IsSuccess = true;
        
        return serviceResponse;
    }

    public async Task<ServiceResponse> EditProductReviewApprovedStatusAsync(int reviewId, bool isApproved)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<ProductReviewDTO>> GetProductReviewsByParameterAsync(Expression<Func<ProductReview, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse> EditProductQuestionApprovedStatusAsync(int reviewId, bool isApproved)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<ProductQuestionDTO>> GetProductQuestionsByParameterAsync(Expression<Func<ProductQuestion, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse> AnswerProductQuestionAsync(int questionId, string answerText)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse> CreateDeliveryOptionAsync(string deliveryOption, decimal price)
    {
        var res = await _deliveryOptionService.CreateAsync(new DeliveryOption() { Name = deliveryOption, Price = price });

        return new ServiceResponse()
        {
            IsSuccess = res.IsSuccess,
            Message = res.Message
        };
    }

    public async Task<ServiceResponse<DeliveryOption>> GetAllDeliveryOptionsAsync()
    {
        ServiceResponse<DeliveryOption> res = await _deliveryOptionService.GetAllAsync();
        return res;
    }
}