using Microsoft.EntityFrameworkCore;
using MyDemoApp.WebAPI.Data;
using MyDemoApp.WebAPI.Handlers;
using MyDemoApp.WebAPI.Models;
using MyDemoApp.WebAPI.Queries;

namespace MyDemoApp.WebAPI.Handlers
{
    public class GetStudentsQueryHandler : IQueryHandler<GetStudentsQuery, IEnumerable<Student>>
    {
        private readonly AppDbContext _context;

        public GetStudentsQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> Handle(GetStudentsQuery query)
        {
            return await _context.Students.ToListAsync();
        }
    }
}
