using MyDemoApp.WebAPI.Commands;
using MyDemoApp.WebAPI.Data;
using MyDemoApp.WebAPI.Handlers;

namespace MyDemoApp.WebAPI.Handlers
{
    public class UpdateStudentCommandHandler : ICommandHandler<UpdateStudentCommand>
    {
        private readonly AppDbContext _context;

        public UpdateStudentCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateStudentCommand command)
        {
            var student = await _context.Students.FindAsync(command.Id);
            if (student != null)
            {
                student.Name = command.Name;
                student.Email = command.Email;
                student.Age = command.Age;
                await _context.SaveChangesAsync();
            }
        }
    }
}
