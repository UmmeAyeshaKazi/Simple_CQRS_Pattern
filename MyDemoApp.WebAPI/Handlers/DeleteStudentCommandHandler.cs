using MyDemoApp.WebAPI.Commands;
using MyDemoApp.WebAPI.Data;
using MyDemoApp.WebAPI.Handlers;

namespace MyDemoApp.WebAPI.Handlers
{
    public class DeleteStudentCommandHandler : ICommandHandler<DeleteStudentCommand>
    {
        private readonly AppDbContext _context;

        public DeleteStudentCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteStudentCommand command)
        {
            var student = await _context.Students.FindAsync(command.Id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
        }
    }
}
