using MyDemoApp.WebAPI.Commands;
using MyDemoApp.WebAPI.Data;
using MyDemoApp.WebAPI.Handlers;
using MyDemoApp.WebAPI.Models;

namespace MyDemoApp.WebAPI.Handlers
{
    public class CreateStudentCommandHandler : ICommandHandler<CreateStudentCommand>
    {
        private readonly AppDbContext _context;

        public CreateStudentCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task Handle(CreateStudentCommand command)
        {
            var student = new Student
            {
                Name = command.Name,
                Email = command.Email,
                Age = command.Age
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();
        }
    }
}
