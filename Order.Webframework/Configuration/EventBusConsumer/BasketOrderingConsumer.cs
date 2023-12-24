using AutoMapper;
using EventBus.Message.Events;
using MassTransit;
using MassTransit.Mediator;
using MediatR;
using Microsoft.Extensions.Logging;
using Order.Application.Features.Commands;
using Order.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Webframework.Configuration.EventBusConsumer
{
    // IConsumer implementation
    public class BasketOrderingConsumer : IConsumer<BasketChekoutEvent>
    {
        private readonly ISender mediator;
        private readonly IMapper mapper;
        private readonly ILogger<BasketOrderingConsumer> logger;

        public BasketOrderingConsumer(ISender mediator, IMapper mapper, ILogger<BasketOrderingConsumer> logger)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task Consume(ConsumeContext<BasketChekoutEvent> context)
        {
            var command = mapper.Map<CheckoutOrderViewModel>(context.Message);
            await mediator.Send(new CheckoutOrderCommand(command));

            logger.LogInformation($"BasketCheckout event completed.");
        }
    }
}
