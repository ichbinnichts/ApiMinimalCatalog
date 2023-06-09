namespace ApiMinimalCatalog.AppServicesExtensions
{
    public static class ApplicationBuilderExtensions
    {
        // Configure the HTTP request pipeline.

        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            return app;
        }
        public static IApplicationBuilder UseAppCors(this IApplicationBuilder app)
        {
            app.UseCors(p =>
            {
                p.AllowAnyOrigin();
                p.WithMethods("GET");
                p.AllowAnyHeader();
            });
            return app;
        }
        public static IApplicationBuilder UseSwaggerMiddleware(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}
