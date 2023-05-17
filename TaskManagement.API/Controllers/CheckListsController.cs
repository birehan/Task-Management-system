using API.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Features.CheckLists.CQRS.Commands;
using TaskManagement.Application.Features.CheckLists.CQRS.Queries;
using TaskManagement.Application.Features.CheckLists.DTOs;
namespace TaskManagement.API.Controllers
{
    public class CheckListsController: BaseApiController
    {
        private readonly IMediator _mediator;

        public CheckListsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<CheckListDto>>> Get()
        {
            return HandleResult(await _mediator.Send(new GetCheckListsQuery()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return HandleResult(await _mediator.Send(new GetCheckListDetailQuery { Id = id }));
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCheckListDto createCheckList)
        {
            var command = new CreateCheckListCommand { CheckListDto = createCheckList };
            return  HandleResult(await _mediator.Send(command));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateCheckListDto checkListDto)
        {

      
            var command = new UpdateCheckListCommand { CheckListDto = checkListDto };
            return HandleResult( await _mediator.Send(command));
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteCheckListCommand { Id = id };
            return HandleResult(await _mediator.Send(command));
        }

    }
}