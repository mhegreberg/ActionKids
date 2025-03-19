// Model of a Kid and identifying Information
// Mark Hegreberg
//  Mon 17 Mar 2025 01:23:12 PM PDT

namespace ActionKids.Models;


public class Kid
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateOnly? Birthday { get; set; }
    public int Points { get; set; } = 0;
    public int TotalLostStars { get; set; } = 0;

    public ICollection<KidServiceRecord> ServiceRecords { get; set; } = new List<KidServiceRecord>();

}








// Soli Deo Gloria
