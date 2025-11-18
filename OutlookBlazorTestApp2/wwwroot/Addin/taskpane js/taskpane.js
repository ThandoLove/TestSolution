async function getEmailContext() {
    const item = Office.context.mailbox.item;

    const email = {
        subject: item.subject,
        bodyPreview: item.body ? await item.body.getAsync() : "",
        from: item.from ? item.from.emailAddress : "",
        to: item.to ? item.to.map(x => x.emailAddress) : [],
        attachments: item.attachments.map(att => ({
            id: att.id,
            name: att.name,
            size: att.size,
            isInline: att.isInline,
            contentType: att.contentType
        }))
    };

    return email;
}

// Download a single attachment
async function downloadAttachment(attachmentId) {
    return new Promise((resolve, reject) => {
        Office.context.mailbox.item.getAttachmentContentAsync(attachmentId, (res) => {
            if (res.status === Office.AsyncResultStatus.Succeeded) {
                resolve(res.value);
            } else {
                reject(res.error);
            }
        });
    });
}

window.getEmailContext = getEmailContext;
window.downloadAttachment = downloadAttachment;
