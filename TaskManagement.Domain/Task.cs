using TaskManagement.Domain.Common;
using System.Collections;

namespace TaskManagement.Domain
{
    public class Task : BaseDomainEntity
    {
    // write me fields title, desription, startDate, endDate, status and AppUser user
    public string Title { get; set; }
    public string Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool Status { get; set; }

    public ICollection<CheckList> CheckLists { get; set; } =  new List<CheckList>();


    public string CreatorId { get; set; }

    public AppUser Creator { get; set; }
    }
}
