// officeInterop.js - Enhanced Office.js integration for Outlook Add-in
window.officeInterop = (function () {
    let isInitialized = false;

    // Helper function to convert Outlook recipients to standard format
    function convertRecipients(recipients) {
        if (!recipients || !Array.isArray(recipients)) return [];
        return recipients.map(recipient => ({
            name: recipient.displayName || '',
            address: recipient.emailAddress || ''
        }));
    }

    // Helper function to convert Outlook attachments to standard format
    function convertAttachments(attachments) {
        if (!attachments || !Array.isArray(attachments)) return [];
        return attachments.map(attachment => ({
            id: attachment.id || '',
            name: attachment.name || '',
            size: attachment.size || 0,
            contentType: attachment.contentType || ''
        }));
    }

    return {
        // Initialize Office.js
        async initialize() {
            if (isInitialized) return { success: true };

            try {
                if (typeof Office === 'undefined') {
                    return { success: false, error: 'Office.js not available' };
                }

                await Office.onReady();
                isInitialized = true;
                return { success: true };
            } catch (ex) {
                return { success: false, error: ex.message || String(ex) };
            }
        },

        // Get comprehensive email context
        async getEmailContext() {
            try {
                if (typeof Office === 'undefined') {
                    return { error: 'Office.js not available' };
                }

                if (!isInitialized) {
                    const initResult = await this.initialize();
                    if (!initResult.success) {
                        return { error: initResult.error };
                    }
                }

                const item = Office.context && Office.context.mailbox && Office.context.mailbox.item;
                if (!item) {
                    return { error: 'No mailbox item selected' };
                }

                // Get basic email properties
                const context = {
                    itemId: item.itemId || '',
                    subject: item.subject || '',
                    receivedTime: item.dateTimeCreated ? item.dateTimeCreated.toISOString() : '',
                    conversationId: item.conversationId || '',
                    conversationTopic: item.conversationTopic || '',
                    isRead: item.isRead !== undefined ? item.isRead : true,
                    importance: item.importance || 'normal'
                };

                // Get sender information
                try {
                    if (item.from) {
                        context.sender = {
                            name: item.from.displayName || '',
                            address: item.from.emailAddress || ''
                        };
                    } else if (item.sender) {
                        context.sender = {
                            name: item.sender.displayName || '',
                            address: item.sender.emailAddress || ''
                        };
                    }
                } catch (e) {
                    context.sender = { name: '', address: '' };
                }

                // Get recipients
                try {
                    context.toRecipients = convertRecipients(item.to);
                    context.ccRecipients = convertRecipients(item.cc);
                    context.bccRecipients = convertRecipients(item.bcc);
                } catch (e) {
                    context.toRecipients = [];
                    context.ccRecipients = [];
                    context.bccRecipients = [];
                }

                // Get attachments
                try {
                    context.attachments = convertAttachments(item.attachments || []);
                } catch (e) {
                    context.attachments = [];
                }

                return context;
            } catch (ex) {
                return { error: ex.message || String(ex) };
            }
        },

        // Get email body in specified format
        async getEmailBody(format = "text") {
            return new Promise((resolve) => {
                try {
                    if (typeof Office === 'undefined') {
                        resolve({ error: 'Office.js not available' });
                        return;
                    }

                    if (!isInitialized) {
                        resolve({ error: 'Office.js not initialized' });
                        return;
                    }

                    const item = Office.context && Office.context.mailbox && Office.context.mailbox.item;
                    if (!item) {
                        resolve({ error: 'No mailbox item selected' });
                        return;
                    }

                    if (item.body && item.body.getAsync) {
                        item.body.getAsync(format, function (result) {
                            if (result.status === Office.AsyncResultStatus.Succeeded) {
                                resolve({ body: result.value || '' });
                            } else {
                                resolve({ error: result.error?.message || 'Failed to get email body' });
                            }
                        });
                    } else {
                        resolve({ error: 'Body API not available' });
                    }
                } catch (ex) {
                    resolve({ error: ex.message || String(ex) });
                }
            });
        },

        // Get email attachments
        async getAttachments() {
            try {
                if (typeof Office === 'undefined') {
                    return { error: 'Office.js not available' };
                }

                if (!isInitialized) {
                    return { error: 'Office.js not initialized' };
                }

                const item = Office.context && Office.context.mailbox && Office.context.mailbox.item;
                if (!item) {
                    return { error: 'No mailbox item selected' };
                }

                const attachments = item.attachments || [];
                return {
                    attachments: convertAttachments(attachments)
                };
            } catch (ex) {
                return { error: ex.message || String(ex) };
            }
        },

        // Get selected text in email
        async getSelectedText() {
            return new Promise((resolve) => {
                try {
                    if (typeof Office === 'undefined') {
                        resolve({ error: 'Office.js not available' });
                        return;
                    }

                    if (!isInitialized) {
                        resolve({ error: 'Office.js not initialized' });
                        return;
                    }

                    const item = Office.context && Office.context.mailbox && Office.context.mailbox.item;
                    if (!item) {
                        resolve({ error: 'No mailbox item selected' });
                        return;
                    }

                    if (item.getSelectedDataAsync) {
                        item.getSelectedDataAsync(Office.CoercionType.Text, function (result) {
                            if (result.status === Office.AsyncResultStatus.Succeeded) {
                                resolve({ text: result.value || '' });
                            } else {
                                resolve({ error: result.error?.message || 'Failed to get selected text' });
                            }
                        });
                    } else {
                        resolve({ error: 'Selected text API not available' });
                    }
                } catch (ex) {
                    resolve({ error: ex.message || String(ex) });
                }
            });
        },

        // Get user profile information
        async getUserProfile() {
            try {
                if (typeof Office === 'undefined') {
                    return { error: 'Office.js not available' };
                }

                if (!isInitialized) {
                    return { error: 'Office.js not initialized' };
                }

                const user = Office.context && Office.context.mailbox && Office.context.mailbox.userProfile;
                if (!user) {
                    return { error: 'User profile not available' };
                }

                return {
                    displayName: user.displayName || '',
                    emailAddress: user.emailAddress || '',
                    timeZone: user.timeZone || ''
                };
            } catch (ex) {
                return { error: ex.message || String(ex) };
            }
        }
    };
})();
