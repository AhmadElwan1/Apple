namespace Domain.Entities.LeaveType;

public class LeaveWorkflow
{

    public int Id { get; set; }
    public Structure? structure { get; set; }
    public Workflow.WorkflowStep[] Steps { get; set; }



    public Unit? unit { get; set; }
    public int UnitID { get; set; }

}
