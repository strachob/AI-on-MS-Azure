// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.10.3

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Mov4Anyone.Bots;
using Mov4Anyone.Dialogs;
using Mov4Anyone.Services;

namespace Mov4Anyone
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();

         
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

            
            services.AddSingleton<IStorage, MemoryStorage>();
            services.AddSingleton<UserState>();
            services.AddSingleton<ConversationState>();


            services.AddSingleton<MovieRecognizer>();
            services.AddSingleton<TMDBService>();
            services.AddSingleton<TransciptionService>();


            services.AddSingleton<SearchDialog>();
            services.AddSingleton<RecommendationDialog>();
            services.AddSingleton<VideoDialog>();
            services.AddSingleton<MainDialog>();

            services.AddTransient<IBot, DialogAndWelcomeBot<MainDialog>>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles()
                .UseStaticFiles()
                .UseWebSockets()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

            // app.UseHttpsRedirection();
        }
    }
}
