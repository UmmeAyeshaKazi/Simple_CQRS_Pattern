using MyDemoApp.WebAPI.Data;
using MyDemoApp.WebAPI.Handlers;
using MyDemoApp.WebAPI.Models;
using MyDemoApp.WebAPI.Queries;

namespace MyDemoApp.WebAPI.Handlers
{
    public class GetStudentByIdQueryHandler : IQueryHandler<GetStudentByIdQuery, Student?>
    {
        private readonly AppDbContext _context;

        public GetStudentByIdQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Student?> Handle(GetStudentByIdQuery query)
        {
            return await _context.Students.FindAsync(query.Id);
        }
    }
}
