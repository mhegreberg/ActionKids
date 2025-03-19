// Model to describe a kids attendance to a service. records points earned, stars lost, etc.
// Mark Hegreberg
//  Mon 17 Mar 2025 01:31:09 PM PDT

using Microsoft.EntityFrameworkCore;

namespace ActionKids.Models;


[PrimaryKey(nameof(KidId), nameof(ServiceId))]
public class KidServiceRecord
{
    public required int KidId { get; set; }
    public required int ServiceId { get; set; }
    public int Stars { get; set; } = 3;
    public int PointsEarned { get; set; }
    public string? Notes { get; set; } // Notes a Leader can make on a serviceRecord. Especially good/bad behaviour, etc.

	public virtual Kid Kid { get; set; } 
	public virtual Service Service { get; set; }

}








// Soli Deo Gloria
