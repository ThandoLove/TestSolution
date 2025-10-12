using Microsoft.EntityFrameworkCore;
using OutlookBlazorTestApp2.Data.Models;
using OutlookBlazorTestApp2.Data.Models.Supporting;

using OutlookBlazorTestApp2.services;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;


namespace OutlookBlazorTestApp2.services
{
    public interface ISageX3Service
    {
        Task<OperationResult<Lead>> CreateLeadAsync(CreateLeadRequest request);
        Task<OperationResult<Contact>> CreateContactAsync(CreateContactRequest request);
        Task<OperationResult<Opportunity>> CreateOpportunityAsync(CreateOpportunityRequest request);
        Task<OperationResult<List<EntityReference>>> SearchEntitiesAsync(string query);
    }

    public class SageX3Service : ISageX3Service
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SageX3Service> _logger;
        private readonly IConfiguration _configuration;

        
        public SageX3Service(HttpClient httpClient, ILogger<SageX3Service> logger, IConfiguration configuration)
    {
            _httpClient = httpClient;
            _logger = logger;
            _configuration = configuration;
        }
     
        
            private readonly HttpClient _http;

            public 
            
            
            SageX3Service(HttpClient http)
            {
                _http = http;
            }

            public async Task<SageDocument> GetDocumentAsync(string documentId)
            {
                var response = await _http.GetAsync($"https://your-sagex3-endpoint/api/documents/{documentId}");
                response.EnsureSuccessStatusCode();

                var bytes = await response.Content.ReadAsByteArrayAsync();
                var fileName = response.Content.Headers.ContentDisposition?.FileName?.Trim('"')
                               ?? $"{documentId}.pdf";

                return new SageDocument
                {
                    Id = documentId,
                    FileName = fileName,
                    Content = bytes
                };
            }
        
    



    public async Task<OperationResult<Lead>> CreateLeadAsync(CreateLeadRequest request)
        {
            try
            {
                var baseUrl = _configuration["SageX3:BaseUrl"] ?? "https://your-sage-x3-host.example.com";
                var endpoint = $"{baseUrl}/sdata/crm/lead";

                var payload = new
                {
                    firstName = request.FirstName,
                    lastName = request.LastName,
                    email = request.Email,
                    phone = request.Phone,
                    company = request.Company,
                    title = request.Title,
                    estimatedValue = request.EstimatedValue,
                    currency = request.Currency,
                    expectedCloseDate = request.ExpectedCloseDate,
                    source = request.Source.ToString(),
                    description = request.Description
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(endpoint, content);
                var respText = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("SageX3 response: {status} {text}", response.StatusCode, respText);

                if (response.IsSuccessStatusCode)
                {
                    // Replace this block in CreateLeadAsync method
                    var lead = new Lead
                    {
                        Id = Guid.NewGuid(),  // or assign a valid int value if available
                        CompanyName = request.Company,
                        ContactName = $"{request.FirstName} {request.LastName}",                     
                        Email = request.Email,
                        ExternalId = null, // or assign as needed
                        CreatedDate = DateTime.UtcNow
                    };

                    return new OperationResult<Lead>
                    {
                        Success = true,
                        Data = lead,
                        StatusCode = 200
                    };
                }
                else
                {
                    return new OperationResult<Lead>
                    {
                        Success = false,
                        ErrorMessage = $"HTTP {response.StatusCode}: {respText}",
                        StatusCode = (int)response.StatusCode
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling Sage X3");
                return new OperationResult<Lead>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    StatusCode = 500
                };
            }
        }

        public async Task<OperationResult<Contact>> CreateContactAsync(CreateContactRequest request)
        {
            try
            {
                var baseUrl = _configuration["SageX3:BaseUrl"] ?? "https://your-sage-x3-host.example.com";
                var endpoint = $"{baseUrl}/sdata/crm/contact";

                var payload = new
                {
                    firstName = request.FirstName,
                    lastName = request.LastName,
                    email = request.Email,
                    phone = request.Phone,
                    company = request.Company,
                    title = request.Title,
                    address = request.PrimaryAddress
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(endpoint, content);
                var respText = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("SageX3 response: {status} {text}", response.StatusCode, respText);

                if (response.IsSuccessStatusCode)
                {
                    // Replace this block in CreateContactAsync method
                    var contact = new Contact
                    {
                        Id = Guid.NewGuid(),                        
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Email = request.Email,
                        Phone = request.Phone,
                        PrimaryAddress = request.PrimaryAddress,                        
                        ExternalId = null, // or assign as needed
                        CreatedDate = DateTime.UtcNow

                    };

                    return new OperationResult<Contact>
                    {
                        Success = true,
                        Data = contact,
                        StatusCode = 200
                    };
                }
                else
                {
                    return new OperationResult<Contact>
                    {
                        Success = false,
                        ErrorMessage = $"HTTP {response.StatusCode}: {respText}",
                        StatusCode = (int)response.StatusCode
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling Sage X3");
                return new OperationResult<Contact>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    StatusCode = 500
                };
            }
        }

        public async Task<OperationResult<Opportunity>> CreateOpportunityAsync(CreateOpportunityRequest request)
        {
            try
            {
                var baseUrl = _configuration["SageX3:BaseUrl"] ?? "https://your-sage-x3-host.example.com";
                var endpoint = $"{baseUrl}/sdata/crm/opportunity";

                var payload = new
                {
                    name = request.Name,
                    value = request.Value,
                    currency = request.Currency,
                    closeDate = request.CloseDate,
                    stage = request.Stage.ToString(),
                    contactId = request.ContactId
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(endpoint, content);
                var respText = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("SageX3 response: {status} {text}", response.StatusCode, respText);

                if (response.IsSuccessStatusCode)
                {
                    var opportunity = new Opportunity
                    {
                        Id = Guid.NewGuid(), // ✅ GUID instead of int
                        Title = request.Name,
                        Value = request.Value,
                        Currency = request.Currency,
                        CloseDate = request.CloseDate,
                        Stage = request.Stage.ToString(),

                        // ✅ This is the line you asked about:
                        ContactId = Guid.TryParse(request.ContactId, out var contactGuid) ? contactGuid : Guid.Empty,

                        Contact = null, // optional mapping
                        ExternalId = null, // or set from Sage response if available
                        CreatedDate = DateTime.UtcNow, // ✅ matches your models
                        Status = EntityStatus.Active
                    };

                    return new OperationResult<Opportunity>
                    {
                        Success = true,
                        Data = opportunity,
                        StatusCode = 200
                    };
                }

                else
                {
                    return new OperationResult<Opportunity>
                    {
                        Success = false,
                        ErrorMessage = $"HTTP {response.StatusCode}: {respText}",
                        StatusCode = (int)response.StatusCode
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling Sage X3");
                return new OperationResult<Opportunity>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    StatusCode = 500
                };
            }
        }

        public async Task<OperationResult<List<EntityReference>>> SearchEntitiesAsync(string query)
        {
            try
            {
                var baseUrl = _configuration["SageX3:BaseUrl"] ?? "https://your-sage-x3-host.example.com";
                var endpoint = $"{baseUrl}/sdata/crm/search?q={Uri.EscapeDataString(query)}";

                var response = await _httpClient.GetAsync(endpoint);
                var respText = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("SageX3 response: {status} {text}", response.StatusCode, respText);

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response to create a list of EntityReference objects
                    var entities = new List<EntityReference>();
                    // In a real implementation, you would parse the JSON response here
                    // This is a placeholder implementation

                    return new OperationResult<List<EntityReference>>
                    {
                        Success = true,
                        Data = entities,
                        StatusCode = 200
                    };
                }
                else
                {
                    return new OperationResult<List<EntityReference>>
                    {
                        Success = false,
                        ErrorMessage = $"HTTP {response.StatusCode}: {respText}",
                        StatusCode = (int)response.StatusCode
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling Sage X3");
                return new OperationResult<List<EntityReference>>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    StatusCode = 500
                };
            }
        }
    }
}



