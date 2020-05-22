using MassTransit;
using rabbitmq_message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stock_ms
{
    public class OrderValidateConsumer :
      IConsumer<ICardValidatorEvent>
    {
        public async Task Consume(ConsumeContext<ICardValidatorEvent> context)
        {
            var data = context.Message;

            if (data.PaymentCardNumber.Contains("test"))
            {
                await context.Publish<IOrderCancelEvent>(
          new { OrderId = context.Message.OrderId, PaymentCardNumber = context.Message.PaymentCardNumber });
            }
            else
            {
                // send to next microservice
            }
        }
    }
}
