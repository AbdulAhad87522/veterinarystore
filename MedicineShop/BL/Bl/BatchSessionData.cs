using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace MedicineShop.UI
{
    // Serializable class for batch persistence
    [Serializable]
    public class BatchSessionData
    {
        public string BatchName { get; set; } = "";
        public int CompanyID { get; set; }
        public string CompanyName { get; set; } = "";
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public bool BatchSaved { get; set; }
        public bool DetailsPanelVisible { get; set; }
        public DateTime SessionDate { get; set; }

        // Add parameterless constructor for XML serialization
        public BatchSessionData()
        {
            SessionDate = DateTime.Now;
        }
    }

    public class BatchSessionManager
    {
        private string sessionFilePath;
        private Timer autoSaveTimer;
        private bool hasUnsavedChanges = false;

        // Events to notify the form about session changes
        public event EventHandler<bool> UnsavedChangesChanged;

        public BatchSessionManager()
        {
            InitializeSessionPath();
            SetupAutoSaveTimer();
        }

        public bool HasUnsavedChanges
        {
            get => hasUnsavedChanges;
            private set
            {
                if (hasUnsavedChanges != value)
                {
                    hasUnsavedChanges = value;
                    UnsavedChangesChanged?.Invoke(this, value);
                }
            }
        }

        private void InitializeSessionPath()
        {
            try
            {
                string sessionDirectory = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "MedicineShop",
                    "Sessions"
                );

                // Ensure directory exists
                Directory.CreateDirectory(sessionDirectory);

                // Create unique session file name
                sessionFilePath = Path.Combine(sessionDirectory, "batch_session.xml");
            }
            catch (Exception ex)
            {
                // Fallback to temp directory
                sessionFilePath = Path.Combine(Path.GetTempPath(), "medicine_shop_batch_session.xml");
                System.Diagnostics.Debug.WriteLine($"Session path initialization warning: {ex.Message}");
            }
        }

        private void SetupAutoSaveTimer()
        {
            autoSaveTimer = new Timer();
            autoSaveTimer.Interval = 30000; // Auto-save every 30 seconds
            autoSaveTimer.Tick += (s, e) => AutoSave();
            autoSaveTimer.Start();
        }

        public void MarkUnsavedChanges()
        {
            HasUnsavedChanges = true;
        }

        public void ClearUnsavedChanges()
        {
            HasUnsavedChanges = false;
        }

        private void AutoSave()
        {
            // This will be called by the auto-save timer
            // The actual save logic will be triggered from the form
        }

        public bool SaveSession(BatchSessionData sessionData)
        {
            try
            {
                // Only save if there are meaningful changes to save
                if (!HasUnsavedChanges || string.IsNullOrWhiteSpace(sessionData.BatchName))
                    return false;

                sessionData.SessionDate = DateTime.Now;

                // Ensure directory exists
                string directory = Path.GetDirectoryName(sessionFilePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Save to temporary file first, then move to avoid corruption
                string tempFilePath = sessionFilePath + ".tmp";

                using (var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
                {
                    var serializer = new XmlSerializer(typeof(BatchSessionData));
                    serializer.Serialize(fileStream, sessionData);
                }

                // Move temp file to actual session file
                if (File.Exists(sessionFilePath))
                {
                    File.Delete(sessionFilePath);
                }
                File.Move(tempFilePath, sessionFilePath);

                System.Diagnostics.Debug.WriteLine($"Session saved successfully at: {DateTime.Now}");
                return true;
            }
            catch (Exception ex)
            {
                // Log error but don't show to user to avoid interrupting workflow
                System.Diagnostics.Debug.WriteLine($"Failed to save session: {ex.Message}");

                // Clean up temp file if it exists
                try
                {
                    string tempFilePath = sessionFilePath + ".tmp";
                    if (File.Exists(tempFilePath))
                        File.Delete(tempFilePath);
                }
                catch { }

                return false;
            }
        }

        public BatchSessionData RestoreSession(out bool shouldRestore)
        {
            shouldRestore = false;

            try
            {
                if (!File.Exists(sessionFilePath))
                    return null;

                // Check if session file is older than 24 hours
                var fileInfo = new FileInfo(sessionFilePath);
                if (DateTime.Now.Subtract(fileInfo.LastWriteTime).TotalHours > 24)
                {
                    // Delete old session file
                    File.Delete(sessionFilePath);
                    return null;
                }

                // Validate file size (avoid corrupted files)
                if (fileInfo.Length == 0)
                {
                    File.Delete(sessionFilePath);
                    return null;
                }

                BatchSessionData sessionData;
                using (var fileStream = new FileStream(sessionFilePath, FileMode.Open, FileAccess.Read))
                {
                    var serializer = new XmlSerializer(typeof(BatchSessionData));
                    sessionData = (BatchSessionData)serializer.Deserialize(fileStream);
                }

                if (sessionData != null && !string.IsNullOrWhiteSpace(sessionData.BatchName))
                {
                    DialogResult result = MessageBox.Show(
                        $"Found unsaved batch session: '{sessionData.BatchName}'\n" +
                        $"Created: {sessionData.SessionDate:yyyy-MM-dd HH:mm}\n\n" +
                        "Would you like to restore this session?",
                        "Restore Session",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        shouldRestore = true;
                        MarkUnsavedChanges(); // Mark as having changes to track
                        return sessionData;
                    }
                    else
                    {
                        // User chose not to restore, delete the session file
                        ClearSession();
                        return null;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                // Silent fail - delete corrupted session file
                try
                {
                    if (File.Exists(sessionFilePath))
                        File.Delete(sessionFilePath);
                }
                catch { }

                System.Diagnostics.Debug.WriteLine($"Failed to restore session: {ex.Message}");
                MessageBox.Show("Session file was corrupted and has been reset.", "Session Restore",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                return null;
            }
        }

        public void ClearSession()
        {
            try
            {
                if (File.Exists(sessionFilePath))
                {
                    File.Delete(sessionFilePath);
                    System.Diagnostics.Debug.WriteLine("Session file cleared successfully");
                }
                ClearUnsavedChanges();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to clear session: {ex.Message}");
            }
        }

        public DialogResult HandleFormClosing()
        {
            try
            {
                // Stop the auto-save timer
                autoSaveTimer?.Stop();

                if (HasUnsavedChanges)
                {
                    return MessageBox.Show(
                        "You have unsaved changes. Would you like to save the current session to restore later?",
                        "Save Session",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question);
                }
                else
                {
                    // No unsaved changes, clean up session file
                    ClearSession();
                    return DialogResult.No;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error during form closing: {ex.Message}");
                return DialogResult.No;
            }
        }

        public void Dispose()
        {
            autoSaveTimer?.Stop();
            autoSaveTimer?.Dispose();
        }

        // Method to be called by external auto-save timer
        public void RequestAutoSave()
        {
            // This will trigger the AutoSaveRequested event
            AutoSaveRequested?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler AutoSaveRequested;
    }
}