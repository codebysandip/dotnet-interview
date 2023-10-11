namespace ReviseDotnet.Models.Schemas;

public class BaseSchema {
    public BaseSchema() {
        this.CreateAt = DateTime.Now;
        this.UpdateAt = DateTime.Now;
    }

    public string CreatedBy { get; set; }

    public DateTime CreateAt { get; set; }

    public string UpdatedBy { get; set; }

    public DateTime UpdateAt { get; set; }
}