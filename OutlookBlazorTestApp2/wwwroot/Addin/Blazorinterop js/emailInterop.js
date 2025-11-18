// ✅ emailInterop.js
// Used by Blazor (via IJSRuntime) to access Outlook email context.

window.getEmailContext = async function () {
    try {
        // Ensure Office.js is ready
        await Office.onReady();

        const item = Office.context.mailbox.item;
        if (!item) {
            console.warn("No email context found — likely not running inside Outlook.");
            return { from: "", subject: "", to: [], cc: [] };
        }

        // Extract sender (for messageRead mode)
        const from = item.from
            ? item.from.emailAddress
            : (item.sender ? item.sender.emailAddress : "");

        // Extract recipients
        const toRecipients = (item.to || []).map(r => r.emailAddress);
        const ccRecipients = (item.cc || []).map(r => r.emailAddress);

        // Subject line
        const subject = item.subject || "";

        // Return an object matching your EmailDto shape
        return {
            from: from,
            subject: subject,
            to: toRecipients,
            cc: ccRecipients
        };
    } catch (err) {
        console.error("Error in getEmailContext:", err);
        return { from: "", subject: "", to: [], cc: [] };
    }
};
