// a discrete service that kids attend and can earn points/lose stars
// Mark Hegreberg
//  Mon 17 Mar 2025 01:27:12 PM PDT

namespace ActionKids.Models;


public class Service
{
    public int Id { get; set; }
    public DateTime ServiceStart { get; set; }
    public DateTime? ServiceStop { get; set; }
    public ICollection<KidServiceRecord> ServiceRecords { get; set; } = new List<KidServiceRecord>();

}








// Soli Deo Gloria
