using FreeCourse.Services.FakePayment.Models;
using FreeCourse.Shared.BaseControllers;
using FreeCourse.Shared.Dtos;
using FreeCourse.Shared.Messages;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.FakePayment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakePaymentsController : CustomBaseController
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public FakePaymentsController(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPost]
        public async Task<IActionResult> ReceivePayment(PaymentDto paymentDto)
        {
            var sendEndPoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:create-order-service"));

            CreateOrderMessageCommand createOrderMessageCommand = new()
            {
                UserId = paymentDto.Order.UserId,
                Province = paymentDto.Order.Address.Provice,
                District = paymentDto.Order.Address.District,
                Street = paymentDto.Order.Address.Street,
                Line = paymentDto.Order.Address.Line
            };

            paymentDto.Order.OrderItems.ForEach(x =>
            {
                createOrderMessageCommand.OrderItems.Add(new()
                {
                    PictureUrl = x.PictureUrl,
                    Price = x.Price,
                    ProductId = x.ProductId,
                    ProductName = x.ProductName
                });
            });

            await sendEndPoint.Send<CreateOrderMessageCommand>(createOrderMessageCommand);

            return CreateActionResultInstance(Shared.Dtos.Response<NoContent>.Success(200));
        }
    }
}
