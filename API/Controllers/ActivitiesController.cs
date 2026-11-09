using Application.Activities.Commands;
using Application.Activities.Queries;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ActivitiesController : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<List<Activity>>> GetActivities(CancellationToken cancellationToken)
    {
        var activities =  await Mediator.Send(new GetActivityList.Query(), cancellationToken);
        return Ok(activities);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Activity>> GetActivityDetails(string id, CancellationToken cancellationToken)
    {
        var activity = await Mediator.Send(new GetActivityById.Query { Id = id }, cancellationToken);
        if (activity == null)
        {
            return NotFound();
        }
        return Ok(activity);
    }
    
    [HttpPost]
    public async Task<ActionResult<string>> CreateActivity(Activity activity, CancellationToken cancellationToken)
    {
        var activityId = await Mediator.Send(new CreateActivity.Command { Activity = activity }, cancellationToken);
        return CreatedAtAction(nameof(GetActivityDetails), new { id = activityId }, activity);
    }
    
    [HttpPut]
    public async Task<ActionResult<string>> UpdateActivity(Activity activity, CancellationToken cancellationToken) 
    {
        var activityId = await Mediator.Send(new UpdateActivity.Command { Activity = activity }, cancellationToken);
        return Ok(activityId);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteActivity(string id, CancellationToken cancellationToken) 
    {
        await Mediator.Send(new DeleteActivity.Command { Id = id }, cancellationToken);
        return Ok();
    }
}