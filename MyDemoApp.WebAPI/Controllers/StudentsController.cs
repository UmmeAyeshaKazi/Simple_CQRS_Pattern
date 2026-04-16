using Microsoft.AspNetCore.Mvc;
using MyDemoApp.WebAPI.Commands;
using MyDemoApp.WebAPI.Handlers;
using MyDemoApp.WebAPI.Models;
using MyDemoApp.WebAPI.Queries;

namespace MyDemoApp.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly ICommandHandler<CreateStudentCommand> _createHandler;
        private readonly ICommandHandler<UpdateStudentCommand> _updateHandler;
        private readonly ICommandHandler<DeleteStudentCommand> _deleteHandler;
        private readonly IQueryHandler<GetStudentsQuery, IEnumerable<Student>> _getAllHandler;
        private readonly IQueryHandler<GetStudentByIdQuery, Student?> _getByIdHandler;

        public StudentsController(
            ICommandHandler<CreateStudentCommand> createHandler,
            ICommandHandler<UpdateStudentCommand> updateHandler,
            ICommandHandler<DeleteStudentCommand> deleteHandler,
            IQueryHandler<GetStudentsQuery, IEnumerable<Student>> getAllHandler,
            IQueryHandler<GetStudentByIdQuery, Student?> getByIdHandler)
        {
            _createHandler = createHandler;
            _updateHandler = updateHandler;
            _deleteHandler = deleteHandler;
            _getAllHandler = getAllHandler;
            _getByIdHandler = getByIdHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetStudentsQuery();
            var students = await _getAllHandler.Handle(query);
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetStudentByIdQuery { Id = id };
            var student = await _getByIdHandler.Handle(query);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStudentCommand command)
        {
            await _createHandler.Handle(command);
            return CreatedAtAction(nameof(GetById), new { id = 0 }, command); // Note: In real app, return the created id
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStudentCommand command)
        {
            command.Id = id;
            await _updateHandler.Handle(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteStudentCommand { Id = id };
            await _deleteHandler.Handle(command);
            return NoContent();
        }
    }
}
