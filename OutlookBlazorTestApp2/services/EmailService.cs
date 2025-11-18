
public class EmailService
{
    public Task<List<string>> GetCurrentEmailAddresses()
    {
        // Fetch email addresses from Office.js or cached context
        return Task.FromResult(new List<string>
        {
            "customer@example.com", "info@example.com"
        });
        public async Task<EmailDetails> GetActiveEmail()
    {
        await Task.Delay(100);
        return new EmailDetails
        {
            Subject = "Welcome to Sage X3 Integration",
            From = "crm@company.com"
        };
    }
}
}

