
public class EmailService
{
    public Task<List<string>> GetCurrentEmailAddresses()
    {
        // Fetch email addresses from Office.js or cached context
        return Task.FromResult(new List<string>
        {
            "customer@example.com", "info@example.com"
        });
    }
}

