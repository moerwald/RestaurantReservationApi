using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservationApi.Tests
{
    internal class RestaurantApiFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            builder.ConfigureServices(serviceCollection =>
            {
                serviceCollection.RemoveAll<IReservationRepository>();
                serviceCollection.TryAddSingleton<IReservationRepository>(new FakeDatabase());
            });
        }
        
        public async Task<HttpResponseMessage> PostReservationAsync(object content)
        {
            using var client = CreateClient();

            var c = new StringContent(JsonSerializer.Serialize(content));
            c.Headers!.ContentType!.MediaType = "application/json";

            return await client.PostAsync("reservations", c);
        }
    }
}
