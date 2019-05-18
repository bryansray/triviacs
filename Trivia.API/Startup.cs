using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using Trivia.API.Common;
using Trivia.API.Controllers;

namespace Trivia.API
{
    public class Question
    {
        public int Id { get; protected set; }
        
        public string Text { get; protected set; }
        
        public IList<Answer> Answers { get; set; }
    }

    public class Answer
    {
        protected Answer() {}
        
        public int Id { get; protected set; }
        
        public Question Question { get; set; }
        
        public string Text { get; protected set; }
        
        public int Count { get; protected set; }
    }
    
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IDbConnection>(provider => new MySqlConnection(Configuration.GetConnectionString("TriviaConnection")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddTransient<IEmbeddedResourceProvider, EmbeddedResourceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
