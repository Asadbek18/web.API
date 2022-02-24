using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Data.Context;
using Web.Data.Models;
using Web.Services.DTO.StudentDto;

namespace Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;

        public StudentController(AppDbContext context, IMapper mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromForm]StudentCreationDto studentDto)
        {
            if(studentDto is not null)
            {
                var student = mapper.Map<Student>(studentDto);
                await context.Students.AddAsync(student);
                await context.SaveChangesAsync();
                return Ok(student);
            }
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await context.Students.ToListAsync();
            return Ok(students);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetStudent(int id)
        {
            var student = await context.Students.FirstOrDefaultAsync(p => p.Id == id);
            if (student is null)
                return NotFound("Student didn't find");
            return Ok(student);
        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdateStudent(int id, [FromForm] StudentCreationDto student)
        {
            if (student is not null)
            {
                var ustudent = await context.Students.FirstOrDefaultAsync(p => p.Id == id);
                if (ustudent is null)
                    return NotFound("Student is not found");
                mapper.Map(student, ustudent);
                context.Students.Attach(ustudent);
                context.Entry(ustudent).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(ustudent);
            }
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteStudent([FromForm]int id)
        {
            var student = await context.Students.FirstOrDefaultAsync(p => p.Id == id);
            if (student is null)
                return NotFound("Student didn't find");
            context.Students.Remove(student);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
