using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;
        private readonly IEmailService emailService;
        private readonly ILogger<CheckoutOrderCommand> logger;
        public CheckoutOrderCommandHandler(IOrderRepository orderRepository,
                                           IMapper mapper,
                                           IEmailService emailService,
                                           ILogger<CheckoutOrderCommand> logger)
        {
            this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = mapper.Map<Order>(request);
            var newOrder = await orderRepository.AddAsync(orderEntity);

            logger.LogInformation($"Order ${newOrder.Id} was sucessfully created.");

            await SendMail(newOrder);

            return newOrder.Id;
        }


        private async Task SendMail(Order order)
        {
            var email = new Email
            {
                To = "test@test.com",
                Body = "Order sucessfully placed !",
                Subject = "New order placed"
            };

            try
            {
                await emailService.SendEmail(email);
            } 
            catch (Exception ex)
            {
                logger.LogInformation($"Order ${order.Id} failed due to an error with the mail service: ${ex.Message}");
            }

        }
    }
}
