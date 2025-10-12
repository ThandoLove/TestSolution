
using Microsoft.EntityFrameworkCore;
using OutlookBlazorTestApp2.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace OutlookBlazorTestApp2.services
{
    public interface ICrmService
    {
        Task<OperationResult<CrmRecord?>> FindRecordByEmailAsync(string email);
        Task<OperationResult<CrmRecord?>> FindRecordByContentAsync(string content);
        Task<OperationResult<CrmRecord>> CreateContactAsync(CreateContactRequest request);
        Task<OperationResult<CrmRecord>> CreateLeadAsync(CreateLeadRequest request);
        Task<OperationResult<Activity>> LogActivityAsync(CreateActivityRequest request);
        Task<OperationResult<List<Activity>>> GetActivityTimelineAsync(string entityId);
        Task<OperationResult<SearchResponse<CrmRecord>>> SearchAsync(SearchRequest request);
        Task<OperationResult<Contact>> GetContactAsync(string id);
        Task<OperationResult<Lead>> GetLeadAsync(string id);
        Task<OperationResult<Contact>> UpdateContactAsync(string id, UpdateContactRequest request);
        Task<OperationResult<Lead>> UpdateLeadAsync(string id, UpdateLeadRequest request);
        Task<OperationResult<CrmRecord>> ConvertLeadAsync(string id, ConvertLeadRequest request);
    }

    public class CrmService : ICrmService
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<CrmService> _logger;

        public CrmService(ApplicationDbContext db, ILogger<CrmService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<OperationResult<CrmRecord?>> FindRecordByEmailAsync(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return new OperationResult<CrmRecord?>
                    {
                        Success = false,
                        ErrorMessage = "Email is required",
                        StatusCode = 400
                    };
                }

                var contact = await _db.Contacts.FirstOrDefaultAsync(c => c.Email == email);
                if (contact != null)
                {
                    var record = new CrmRecord
                    {
                        Id = Guid.NewGuid(),
                        Type = "Contact",
                        Name = contact.FirstName,
                        Summary = $"Contact: {contact.FirstName}, {contact.Email}"
                    };

                    return new OperationResult<CrmRecord?>
                    {
                        Success = true,
                        Data = record,
                        StatusCode = 200
                    };
                }

                return new OperationResult<CrmRecord?>
                {
                    Success = true,
                    Data = null,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error finding record by email: {Email}", email);
                return new OperationResult<CrmRecord?>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    StatusCode = 500
                };
            }
        }

        public async Task<OperationResult<CrmRecord?>> FindRecordByContentAsync(string content)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(content))
                {
                    return new OperationResult<CrmRecord?>
                    {
                        Success = false,
                        ErrorMessage = "Content is required",
                        StatusCode = 400
                    };
                }

                // Search in contacts
                var contact = await _db.Contacts.FirstOrDefaultAsync(c =>
                    c.FirstName.Contains(content) || c.Email.Contains(content) || c.Company.Contains(content));

                if (contact != null)
                {
                    var record = new CrmRecord
                    {
                        Id = Guid.NewGuid(),
                        Type = "Contact",
                        Name = contact.FullName,
                        Summary = $"Contact: {contact.FullName}, {contact.Email}"
                    };

                    return new OperationResult<CrmRecord?>
                    {
                        Success = true,
                        Data = record,
                        StatusCode = 200
                    };
                }

                return new OperationResult<CrmRecord?>
                {
                    Success = true,
                    Data = null,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error finding record by content: {Content}", content);
                return new OperationResult<CrmRecord?>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    StatusCode = 500
                };
            }
        }

        public async Task<OperationResult<CrmRecord>> CreateContactAsync(CreateContactRequest request)
        {
            try
            {
                // Validate request
                var validationContext = new ValidationContext(request);
                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateObject(request, validationContext, validationResults, true))
                {
                    return new OperationResult<CrmRecord>
                    {
                        Success = false,
                        ErrorMessage = "Validation failed",
                        ValidationErrors = validationResults.Select(v => v.ErrorMessage ?? "").ToList(),
                        StatusCode = 400
                    };
                }

                var contact = new Contact
                {
                    Id = Guid.NewGuid(),
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Phone = request.Phone,
                    Company = request.Company,
                    Title = request.Title,
                    PrimaryAddress = request.PrimaryAddress,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    Status = EntityStatus.Active
                };

                _db.Contacts.Add(contact);
                await _db.SaveChangesAsync();

                var record = new CrmRecord
                {
                    Id = Guid.NewGuid(),
                    Type = "Contact",
                    Name = contact.FirstName,
                    Summary = $"New Contact: {contact.FirstName}, {contact.Email}"
                };

                return new OperationResult<CrmRecord>
                {
                    Success = true,
                    Data = record,
                    StatusCode = 201
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating contact");
                return new OperationResult<CrmRecord>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    StatusCode = 500
                };
            }
        }

        public Task<OperationResult<CrmRecord>> CreateLeadAsync(CreateLeadRequest request)
        {
            try
            {
                // Validate request
                var validationContext = new ValidationContext(request);
                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateObject(request, validationContext, validationResults, true))
                {
                    return Task.FromResult(new OperationResult<CrmRecord>
                    {
                        Success = false,
                        ErrorMessage = "Validation failed",
                        ValidationErrors = validationResults.Select(v => v.ErrorMessage ?? "").ToList(),
                        StatusCode = 400
                    });
                }

                // In a real implementation, you would create a lead entity
                // For now, we'll create a simple record
                var record = new CrmRecord
                {
                    Id = Guid.NewGuid(),
                    Type = "Lead",
                    Name = $"{request.FirstName} {request.LastName}",
                    Summary = $"New Lead: {request.FirstName} {request.LastName}, {request.Email}"
                };

                return Task.FromResult(new OperationResult<CrmRecord>
                {
                    Success = true,
                    Data = record,
                    StatusCode = 201
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating lead");
                return Task.FromResult(new OperationResult<CrmRecord>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    StatusCode = 500
                });
            }
        }


        public Task<OperationResult<Activity>> LogActivityAsync(CreateActivityRequest request)
        {
            try
            {
                var validationContext = new ValidationContext(request);
                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateObject(request, validationContext, validationResults, true))
                {
                    return Task.FromResult(new OperationResult<Activity>
                    {
                        Success = false,
                        ErrorMessage = "Validation failed",
                        ValidationErrors = validationResults.Select(v => v.ErrorMessage ?? "").ToList(),
                        StatusCode = 400
                    });
                }

                var activity = new Activity
                {
                    Id = Guid.NewGuid(),
                    Type = request.Type,
                    Subject = request.Subject,
                    Description = request.Description,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    Priority = request.Priority,
                    RelatedEntities = request.RelatedEntities,
                    Location = request.Location,
                    Status = ActivityStatus.NotStarted,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow
                };

                return Task.FromResult(new OperationResult<Activity>
                {
                    Success = true,
                    Data = activity,
                    StatusCode = 201
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging activity");
                return Task.FromResult(new OperationResult<Activity>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    StatusCode = 500
                });
            }
        }


        public Task<OperationResult<List<Activity>>> GetActivityTimelineAsync(string entityId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(entityId))
                {
                    return Task.FromResult(new OperationResult<List<Activity>>
                    {
                        Success = false,
                        ErrorMessage = "Entity ID is required",
                        StatusCode = 400
                    });
                }

                return Task.FromResult(new OperationResult<List<Activity>>
                {
                    Success = true,
                    Data = new List<Activity>(),
                    StatusCode = 200
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting activity timeline for entity: {EntityId}", entityId);
                return Task.FromResult(new OperationResult<List<Activity>>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    StatusCode = 500
                });
            }
        }


        public Task<OperationResult<SearchResponse<CrmRecord>>> SearchAsync(SearchRequest request)
        {
            try
            {
                var response = new SearchResponse<CrmRecord>
                {
                    Results = new List<CrmRecord>(),
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    TotalCount = 0,
                    HasMore = false
                };

                return Task.FromResult(new OperationResult<SearchResponse<CrmRecord>>
                {
                    Success = true,
                    Data = response,
                    StatusCode = 200
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching CRM records");
                return Task.FromResult(new OperationResult<SearchResponse<CrmRecord>>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    StatusCode = 500
                });
            }
        }


        public async Task<OperationResult<Contact>> GetContactAsync(string id)
        {
            try
            {
                // 1️⃣ Check if the ID is empty
                if (string.IsNullOrWhiteSpace(id))
                {
                    return new OperationResult<Contact>
                    {
                        Success = false,
                        ErrorMessage = "Contact ID is required",
                        StatusCode = 400
                    };
                }

                // 🔹 2️⃣ Validate the format of the ID (new part)
                if (!Guid.TryParse(id, out var guid))
                {
                    return new OperationResult<Contact>
                    {
                        Success = false,
                        ErrorMessage = "Invalid ID format",
                        StatusCode = 400
                    };
                }

                // 3️⃣ Query the database using the parsed GUID
                var contact = await _db.Contacts.FirstOrDefaultAsync(c => c.Id == guid);

                if (contact == null)
                {
                    return new OperationResult<Contact>
                    {
                        Success = false,
                        ErrorMessage = "Contact not found",
                        StatusCode = 404
                    };
                }

                // 4️⃣ Return successful result
                return new OperationResult<Contact>
                {
                    Success = true,
                    Data = contact,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting contact: {Id}", id);
                return new OperationResult<Contact>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    StatusCode = 500
                };
            }
        }


        public Task<OperationResult<Lead>> GetLeadAsync(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return Task.FromResult(new OperationResult<Lead>
                    {
                        Success = false,
                        ErrorMessage = "Lead ID is required",
                        StatusCode = 400
                    });
                }

                return Task.FromResult(new OperationResult<Lead>
                {
                    Success = false,
                    ErrorMessage = "Lead not found",
                    StatusCode = 404
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting lead: {Id}", id);
                return Task.FromResult(new OperationResult<Lead>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    StatusCode = 500
                });
            }
        }
        public async Task<OperationResult<Contact>> UpdateContactAsync(string id, UpdateContactRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return new OperationResult<Contact>
                    {
                        Success = false,
                        ErrorMessage = "Contact ID is required",
                        StatusCode = 400
                    };
                }

                if (!Guid.TryParse(id, out var contactId))
                {
                    return new OperationResult<Contact>
                    {
                        Success = false,
                        ErrorMessage = "Invalid Contact ID format",
                        StatusCode = 400
                    };
                }

                var contact = await _db.Contacts.FirstOrDefaultAsync(c => c.Id == contactId);
                if (contact == null)
                {
                    return new OperationResult<Contact>
                    {
                        Success = false,
                        ErrorMessage = "Contact not found",
                        StatusCode = 404
                    };
                }

                // Update contact properties
                if (!string.IsNullOrWhiteSpace(request.FirstName))
                    contact.FirstName = request.FirstName;
                if (!string.IsNullOrWhiteSpace(request.LastName))
                    contact.LastName = request.LastName;
                if (!string.IsNullOrWhiteSpace(request.Email))
                    contact.Email = request.Email;
                if (!string.IsNullOrWhiteSpace(request.Phone))
                    contact.Phone = request.Phone;
                if (!string.IsNullOrWhiteSpace(request.Company))
                    contact.Company = request.Company;
                if (!string.IsNullOrWhiteSpace(request.Title))
                    contact.Title = request.Title;
                if (request.PrimaryAddress != null)
                    contact.PrimaryAddress = request.PrimaryAddress;

                contact.ModifiedDate = DateTime.UtcNow;

                await _db.SaveChangesAsync();

                return new OperationResult<Contact>
                {
                    Success = true,
                    Data = contact,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating contact: {Id}", id);
                return new OperationResult<Contact>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    StatusCode = 500
                };
            }
        }



        public Task<OperationResult<Lead>> UpdateLeadAsync(string id, UpdateLeadRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return Task.FromResult(new OperationResult<Lead>
                    {
                        Success = false,
                        ErrorMessage = "Lead ID is required",
                        StatusCode = 400
                    });
                }

                return Task.FromResult(new OperationResult<Lead>
                {
                    Success = false,
                    ErrorMessage = "Lead not found",
                    StatusCode = 404
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating lead: {Id}", id);
                return Task.FromResult(new OperationResult<Lead>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    StatusCode = 500
                });
            }
        }


        public Task<OperationResult<CrmRecord>> ConvertLeadAsync(string id, ConvertLeadRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return Task.FromResult(new OperationResult<CrmRecord>
                    {
                        Success = false,
                        ErrorMessage = "Lead ID is required",
                        StatusCode = 400
                    });
                }

                var record = new CrmRecord
                {
                    Id = Guid.NewGuid(),
                    Type = "Contact",
                    Name = "Converted Lead",
                    Summary = "Lead converted to contact"
                };

                return Task.FromResult(new OperationResult<CrmRecord>
                {
                    Success = true,
                    Data = record,
                    StatusCode = 200
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error converting lead: {Id}", id);
                return Task.FromResult(new OperationResult<CrmRecord>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    StatusCode = 500
                });
            }
        }
    }
   
}





