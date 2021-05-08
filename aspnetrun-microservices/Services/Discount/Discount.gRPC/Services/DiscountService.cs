using AutoMapper;
using Discount.gRPC.Models;
using Discount.gRPC.Protos;
using Discount.gRPC.Repositories;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.gRPC.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository discountRepository;
        private readonly ILogger<DiscountService> logger;
        private readonly IMapper mapper;

        public DiscountService(IDiscountRepository discountRepository,
                               ILogger<DiscountService> logger,
                               IMapper mapper)
        {
            this.discountRepository = discountRepository ?? throw new ArgumentNullException(nameof(discountRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await discountRepository.GetDiscount(request.ProductName);
            if(coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with product name {request.ProductName} not found"));
            }


            var couponModel = mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = mapper.Map<Coupon>(request.Coupon);
            var success = await discountRepository.CreateDiscount(coupon);
            if(!success)
            {
                throw new RpcException(new Status(StatusCode.Internal, $"Discount was not created"));
            }
            var couponModel = mapper.Map<CouponModel>(coupon); // Mapping nedded because of id that is created while inserting to db 
            return couponModel;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = mapper.Map<Coupon>(request.Coupon);
            var success = await discountRepository.UpdateDiscount(coupon);
            if (!success)
            {
                throw new RpcException(new Status(StatusCode.Internal, $"Discount was not created"));
            }
            return request.Coupon;
        }

        public override async Task<DeleteDiscountReponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var success = await discountRepository.DeleteDiscount(request.ProductName);
            return new DeleteDiscountReponse
            {
                Success = success
            };
        }
    }
}
