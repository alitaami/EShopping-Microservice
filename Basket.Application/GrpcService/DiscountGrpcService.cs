using Discount.Grpc.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Basket.Application.GrpcService
{
    public class DiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
        {
            _discountProtoServiceClient = discountProtoServiceClient ?? throw new ArgumentNullException(nameof(discountProtoServiceClient));
        }

        // Your methods here
    }

}
