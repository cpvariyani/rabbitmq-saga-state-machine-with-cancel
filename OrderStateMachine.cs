using Automatonymous;
using rabbitmq_message;
using System;
using System.Collections.Generic;
using System.Text;

namespace rabbitmq_saga.StateMachine
{
    public class OrderStateMachine : MassTransitStateMachine<OrderStateData>
    {
        public State Validation { get; private set; }

        public Event<IOrderStartEvent> StartOrderProcess { get; private set; }

        public Event<IOrderCancelEvent> OrderCancelled { get; private set; }

        public OrderStateMachine()
        {
            InstanceState(s => s.CurrentState);

            Event(() => StartOrderProcess, x => x.CorrelateById(m => m.Message.OrderId));

            Event(() => OrderCancelled, x => x.CorrelateById(context => context.Message.OrderId));

            Initially(
                When(StartOrderProcess)
                    .Then(context =>
                    {
                        context.Instance.OrderId = context.Data.OrderId;
                        context.Instance.PaymentCardNumber = context.Data.PaymentCardNumber;
                        context.Instance.ProductName = context.Data.ProductName;
                    })
                    .TransitionTo(Validation)
                    .Publish(context => new CardValidateEvent(context.Instance))
                     .Finalize()
                );

            During(Validation,
                When(OrderCancelled)
                 .Then(context =>
                 {
                     context.Instance.OrderCancelDateTime = DateTime.Now;
                     context.Instance.OrderId = context.Data.OrderId;
                 })
                    .Finalize()
                );


            SetCompletedWhenFinalized();
        }
    }
}
