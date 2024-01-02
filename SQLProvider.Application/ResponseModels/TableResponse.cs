namespace SQLProvider.Application.ResponseModels;

public class TableResponse
{
    public IEnumerable<string>? Columns { get; set; } = new List<string>();

    public List<Dictionary<string, string>>? Values { get; set; } = new List<Dictionary<string, string>>();
}