using System;
using MassTransit;

namespace BusDepotTest2
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			var bus = GetServiceBus();

			for (int i = 0; i < 100; i++)
			{
				bus.Publish(new ContentImage
				{
					ProductNum = "6789934",
					ImageUrl = "myurl" + i,
					System = "mysystem",
					Caption = "my caption",
					Identifier = i.ToString()
				});
			}

			Console.WriteLine("Done");

			bus.Dispose();
		}


		private static IServiceBus GetServiceBus()
		{
			return ServiceBusFactory.New(sb =>
			{
				sb.UseRabbitMq(r => r.ConfigureHost(new Uri("rabbitmq://localhost/images/completed_images"), h =>
				{
					h.SetUsername("guest");
					h.SetPassword("guest");
				}));
				sb.UseJsonSerializer();
				sb.ReceiveFrom(new Uri("rabbitmq://localhost/images/completed_images"));
			});

		}
	}

	
}
