using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using OutlookBlazorTestApp2.Data.Models;
using OutlookBlazorTestApp2.Data.Models.Supporting;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace OutlookBlazorTestApp2.Services
{
    public interface ISageX3Service
    {
        Task<OperationResult<Lead>> CreateLeadAsync(CreateLeadRequest request);
        Task<OperationResult<Contact>> CreateContactAsync(CreateContactRequest request);
        Task<OperationResult<Opportunity>> CreateOpportunityAsync(CreateOpportunityRequest request);
        Task<OperationResult<List<EntityReference>>> SearchEntitiesAsync(string query);
        Task<SageDocument> GetDocumentAsync(string documentId);
        Task<string> UploadAttachmentToActivityAsync(string activityId, string name, byte[] fileBytes);
    }

    public class SageX3Service : ISageX3Service
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SageX3Service> _logger;
        private readonly IConfiguration _config;

        public SageX3Service(HttpClient httpClient, ILogger<SageX3Service> logger, IConfiguration config)
        {
            _httpClient = httpClient;
            _logger = logger;
            _config = config;

            // Set up Basic Auth
            var authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(
                $"{_config["SageX3:ApiUser"]}:{_config["SageX3:ApiPassword"]}"
            ));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authValue);
        }

        // --- Create Lead ---
        public async Task<OperationResult<Lead>> CreateLeadAsync(CreateLeadRequest request)
        {
            try
            {
                var payload = new
                {
                    LEAD = new[] {
                        new {
                            FIRSTNAME = request.FirstName,
                            LASTNAME = request.LastName,
                            EMAIL = request.Email,
                            PHONE = request.Phone,
                            COMPANY = request.Company,
                            TITLE = request.Title,
                            ESTIMATEDVALUE = request.EstimatedValue,
                            CURRENCY = request.Currency,
                            EXPECTEDCLOSEDATE = request.ExpectedCloseDate,
                            SOURCE = request.Source.ToString(),
                            DESCRIPTION = request.Description
                        }
                    }
                };

                var response = await _httpClient.PostAsync(
                    $"{_config["SageX3:BaseUrl"]}/sdata/crm/lead",
                    new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
                );

                var respText = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("CreateLead response: {status} {text}", response.StatusCode, respText);

                if (response.IsSuccessStatusCode)
                {
                    var lead = new Lead
                    {
                        Id = Guid.NewGuid(),
                        CompanyName = request.Company,
                        ContactName = $"{request.FirstName} {request.LastName}",
                        Email = request.Email,
                        ExternalId = null,
                        CreatedDate = DateTime.UtcNow
                    };

                    return new OperationResult<Lead> { Success = true, Data = lead, StatusCode = 200 };
                }

                return new OperationResult<Lead>
                {
                    Success = false,
                    ErrorMessage = $"HTTP {response.StatusCode}: {respText}",
                    StatusCode = (int)response.StatusCode
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating lead in Sage X3");
                return new OperationResult<Lead> { Success = false, ErrorMessage = ex.Message, StatusCode = 500 };
            }
        }

        // --- Create Contact ---
        public async Task<OperationResult<Contact>> CreateContactAsync(CreateContactRequest request)
        {
            try
            {
                var payload = new
                {
                    BPCT = new[] {
                        new {
                            BPCNUM = request.Email,
                            BPCNAM = $"{request.FirstName} {request.LastName}",
                            ZEMAIL = request.Email
                        }
                    }
                };

                var response = await _httpClient.PostAsync(
                    $"{_config["SageX3:BaseUrl"]}/BPCUSTOMER",
                    new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
                );

                var respText = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("CreateContact response: {status} {text}", response.StatusCode, respText);

                if (response.IsSuccessStatusCode)
                {
                    var contact = new Contact
                    {
                        Id = Guid.NewGuid(),
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Email = request.Email,
                        Phone = request.Phone,
                        PrimaryAddress = request.PrimaryAddress,
                        ExternalId = null,
                        CreatedDate = DateTime.UtcNow
                    };

                    return new OperationResult<Contact> { Success = true, Data = contact, StatusCode = 200 };
                }

                return new OperationResult<Contact>
                {
                    Success = false,
                    ErrorMessage = $"HTTP {response.StatusCode}: {respText}",
                    StatusCode = (int)response.StatusCode
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating contact in Sage X3");
                return new OperationResult<Contact> { Success = false, ErrorMessage = ex.Message, StatusCode = 500 };
            }
        }

        // --- Create Opportunity ---
        public async Task<OperationResult<Opportunity>> CreateOpportunityAsync(CreateOpportunityRequest request)
        {
            try
            {
                var payload = new
                {
                    OPPORTUNITY = new[] {
                        new {
                            NAME = request.Name,
                            VALUE = request.Value,
                            CURRENCY = request.Currency,
                            CLOSEDATE = request.CloseDate,
                            STAGE = request.Stage.ToString(),
                            CONTACTID = request.ContactId
                        }
                    }
                };

                var response = await _httpClient.PostAsync(
                    $"{_config["SageX3:BaseUrl"]}/sdata/crm/opportunity",
                    new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
                );

                var respText = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("CreateOpportunity response: {status} {text}", response.StatusCode, respText);

                if (response.IsSuccessStatusCode)
                {
                    var opportunity = new Opportunity
                    {
                        Id = Guid.NewGuid(),
                        Title = request.Name,
                        Value = request.Value,
                        Currency = request.Currency,
                        CloseDate = request.CloseDate,
                        Stage = request.Stage.ToString(),
                        ContactId = Guid.TryParse(request.ContactId, out var contactGuid) ? contactGuid : Guid.Empty,
                        Contact = null,
                        ExternalId = null,
                        CreatedDate = DateTime.UtcNow,
                        Status = EntityStatus.Active
                    };

                    return new OperationResult<Opportunity> { Success = true, Data = opportunity, StatusCode = 200 };
                }

                return new OperationResult<Opportunity>
                {
                    Success = false,
                    ErrorMessage = $"HTTP {response.StatusCode}: {respText}",
                    StatusCode = (int)response.StatusCode
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating opportunity in Sage X3");
                return new OperationResult<Opportunity> { Success = false, ErrorMessage = ex.Message, StatusCode = 500 };
            }
        }

        // --- Search Entities ---
        public async Task<OperationResult<List<EntityReference>>> SearchEntitiesAsync(string query)
        {
            try
            {
                var response = await _httpClient.GetAsync(
                    $"{_config["SageX3:BaseUrl"]}/sdata/crm/search?q={Uri.EscapeDataString(query)}"
                );

                var respText = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("SearchEntities response: {status} {text}", response.StatusCode, respText);

                if (response.IsSuccessStatusCode)
                {
                    // TODO: parse JSON response to EntityReference list
                    var entities = new List<EntityReference>();
                    return new OperationResult<List<EntityReference>> { Success = true, Data = entities, StatusCode = 200 };
                }

                return new OperationResult<List<EntityReference>>
                {
                    Success = false,
                    ErrorMessage = $"HTTP {response.StatusCode}: {respText}",
                    StatusCode = (int)response.StatusCode
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching entities in Sage X3");
                return new OperationResult<List<EntityReference>> { Success = false, ErrorMessage = ex.Message, StatusCode = 500 };
            }
        }

        // --- Get Document ---
        public async Task<SageDocument> GetDocumentAsync(string documentId)
        {
            var response = await _httpClient.GetAsync($"{_config["SageX3:BaseUrl"]}/api/documents/{documentId}");
            response.EnsureSuccessStatusCode();

            var bytes = await response.Content.ReadAsByteArrayAsync();
            var fileName = response.Content.Headers.ContentDisposition?.FileName?.Trim('"') ?? $"{documentId}.pdf";

            return new SageDocument { Id = documentId, FileName = fileName, Content = bytes };
        }

        // --- Upload Attachment ---
        public async Task<string> UploadAttachmentToActivityAsync(string activityId, string name, byte[] fileBytes)
        {
            var payload = new
            {
                ATTACHMENT = new[] {
                    new {
                        FATCOD = activityId,
                        ATTNAM = name,
                        FILEB64 = Convert.ToBase64String(fileBytes)
                    }
                }
            };

            var response = await _httpClient.PostAsync(
                $"{_config["SageX3:BaseUrl"]}/ATTACHMENT",
                new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
            );

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        // --- Create YACTIVITY in Sage via gateway ---
        public async Task<SageCreateResponse> CreateEmailActivityAsync(YActivityModel model)
        {
            try
            {
                var url = $"{_config["SageGateway:BasePath"]}{_config["SageGateway:YActivityPath"]}";
                var payload = JsonSerializer.Serialize(model);
                using var content = new StringContent(payload, Encoding.UTF8, "application/json");

                var resp = await _httpClient.PostAsync(url, content);
                var text = await resp.Content.ReadAsStringAsync();

                if (!resp.IsSuccessStatusCode)
                {
                    _logger.LogError("CreateEmailActivityAsync failed. Status: {Status}, Body: {Body}", resp.StatusCode, text);
                    return new SageCreateResponse { Success = false, Raw = text };
                }

                string activityId = text;
                try
                {
                    using var doc = JsonDocument.Parse(text);
                    if (doc.RootElement.TryGetProperty("ActivityId", out var aid))
                        activityId = aid.GetString() ?? activityId;
                    else if (doc.RootElement.TryGetProperty("id", out var id2))
                        activityId = id2.GetString() ?? activityId;
                }
                catch { }

                return new SageCreateResponse { Success = true, ActivityId = activityId, Raw = text };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateEmailActivityAsync exception");
                return new SageCreateResponse { Success = false, Raw = ex.Message };
            }
        }

        // --- Upload attachment to YACTIVITY ---
        public async Task<SageAttachmentResponse> UploadAttachmentToYActivityAsync(string activityId, string fileName, byte[] fileBytes)
        {
            try
            {
                var url = $"{_config["SageGateway:BasePath"]}{_config["SageGateway:AttachmentsPath"]}";
                var payload = new
                {
                    ActivityId = activityId,
                    FileName = fileName,
                    FileBase64 = Convert.ToBase64String(fileBytes)
                };
                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                var resp = await _httpClient.PostAsync(url, content);
                var text = await resp.Content.ReadAsStringAsync();

                if (!resp.IsSuccessStatusCode)
                {
                    _logger.LogError("UploadAttachmentToYActivityAsync failed. Status: {Status}, Body: {Body}", resp.StatusCode, text);
                    return new SageAttachmentResponse { Success = false, Raw = text };
                }

                string attachId = text;
                try
                {
                    using var doc = JsonDocument.Parse(text);
                    if (doc.RootElement.TryGetProperty("AttachmentId", out var aid))
                        attachId = aid.GetString() ?? attachId;
                    else if (doc.RootElement.TryGetProperty("id", out var id2))
                        attachId = id2.GetString() ?? attachId;
                }
                catch { }

                return new SageAttachmentResponse { Success = true, AttachmentId = attachId, Raw = text };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UploadAttachmentToYActivityAsync exception");
                return new SageAttachmentResponse { Success = false, Raw = ex.Message };
            }
        }

        // --- Optional Deep Link ---
        public string BuildYActivityDeepLink(string activityId)
        {
            var baseUrl = _config["SageGateway:DeepLinkBase"]?.TrimEnd('/') ?? "";
            if (string.IsNullOrWhiteSpace(baseUrl)) return "";
            return $"{baseUrl}/yactivity/{Uri.EscapeDataString(activityId)}";
        }

    }
}
