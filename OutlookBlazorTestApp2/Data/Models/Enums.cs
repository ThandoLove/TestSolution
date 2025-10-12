
namespace OutlookBlazorTestApp2.Data.Models
{
    // ------------------------------
    // Activity-related enums
    // ------------------------------
    public enum ActivityType
    {
        Task,
        Email,
        PhoneCall,
        Appointment,
        Call,
        Meeting,
        Note
        
    }
    


    public enum ActivityStatus
    {
        NotStarted,
        InProgress,
        Completed,
        Canceled,
        Deferred
    }

    public enum PriorityLevel
    {
        Low,
        Normal,
        High
    }

    // ------------------------------
    // CRM entity reference
    // ------------------------------
    public enum EntityType
    {
        None,
        Contact,
        Lead,
        Opportunity,
        User
    }

    public enum EntityStatus
    {
        Active,
        Inactive,
        Archived
    }


// ------------------------------
// Contact-related enums
// ------------------------------
public enum ContactType
    {
        Individual,
        Company
    }

    public enum CustomerType
    {
        Regular,
        VIP,
        Prospect,
        Supplier
    }

    // ------------------------------
    // Lead-related enums
    // ------------------------------
    public enum LeadStatus
    {
        New,
        Qualified,
        Disqualified,
        Converted
    }

    public enum LeadSource
    {
        Email,
        Website,
        Phone,
        Referral,
        SocialMedia,
        Event,
        Other
    }

    // ------------------------------
    // Opportunity-related enums
    // ------------------------------
    public enum OpportunityStage
    {
        Prospecting,
        Qualification,
        Proposal,
        Negotiation,
        ClosedWon,
        ClosedLost
    }

    public enum OpportunityPriority
    {
        Low,
        Medium,
        High
    }

    // ------------------------------
    // Address & Phone
    // ------------------------------
    public enum AddressType
    {
        Billing,
        Shipping,
        Home,
        Office,
        Other
    }

    public enum PhoneType
    {
        Mobile,
        Home,
        Work,
        Fax,
        Other
    }

    // ------------------------------
    // Product Line Items
    // ------------------------------
    public enum ProductLineStatus
    {
        Active,
        BackOrdered,
        Discontinued
    }

    // ------------------------------
    // User/Auth
    // ------------------------------
    public enum UserRole
    {
        Admin,
        Employee,
        Manager,
        Guest
    }

    public enum PreferenceType
    {
        Theme,
        Language,
        Notification
    }
}
