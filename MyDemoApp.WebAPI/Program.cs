using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using MyDemoApp.WebAPI.Data;
using MyDemoApp.WebAPI.Handlers;
using MyDemoApp.WebAPI.Commands;
using MyDemoApp.WebAPI.Queries;

namespace MyDemoApp.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register CQRS Handlers
            builder.Services.AddScoped<ICommandHandler<CreateStudentCommand>, CreateStudentCommandHandler>();
            builder.Services.AddScoped<ICommandHandler<UpdateStudentCommand>, UpdateStudentCommandHandler>();
            builder.Services.AddScoped<ICommandHandler<DeleteStudentCommand>, DeleteStudentCommandHandler>();
            builder.Services.AddScoped<IQueryHandler<GetStudentsQuery, IEnumerable<MyDemoApp.WebAPI.Models.Student>>, GetStudentsQueryHandler>();
            builder.Services.AddScoped<IQueryHandler<GetStudentByIdQuery, MyDemoApp.WebAPI.Models.Student?>, GetStudentByIdQueryHandler>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
