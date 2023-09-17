using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CmdApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CmdApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly CommandContext _commandContext;

        public CommandsController(CommandContext context)
        {
            _commandContext = context;
        }

        //GET:      api/commands
        [HttpGet]
        public ActionResult<IEnumerable<Command>> GetCommands()
        {
            return _commandContext.CommandItems;
        }

        //GET:      api/commands/n
        [HttpGet("{id}")]
        public ActionResult<Command> GetCommandItem(int id)
        {
            var item = _commandContext.CommandItems.Find(id);

            if(item == null)
                return NotFound();
            else
                return item;
        }

        //POST:     api/commands
        [HttpPost]
        public ActionResult<Command> PostCommandItem(Command command)
        {
            _commandContext.Add(command);
            _commandContext.SaveChanges();

            return CreatedAtAction("PostCommandItem", new Command{Id = command.Id}, command);
        }

        //PUT:      api/commands/n
        [HttpPut("{id}")]
        public ActionResult PutCommandItem(int id, Command command)
        {
            if(id != command.Id) 
                return BadRequest();

            _commandContext.Entry(command).State = EntityState.Modified;
            _commandContext.SaveChanges();

            return NoContent();
        }

        //DELETE:       api/commands/n
        [HttpDelete("{id}")]
        public ActionResult<Command> DeleteCommandItem(int id)
        {
            var commandItem = _commandContext.CommandItems.Find(id);
            if(commandItem == null)
                return NotFound();

            _commandContext.CommandItems.Remove(commandItem);
            _commandContext.SaveChanges();

            return commandItem;
        }

    }
}