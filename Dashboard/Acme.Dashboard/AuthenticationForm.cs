using Acme.Data;
using System;
using System.ComponentModel;

namespace Acme.Dashboard
{
    public partial class AuthenticationForm : SettingsForm
    {
        private readonly IDataProvider _dataProvider;

        private readonly string _databaseFilePath;

        public IDataRepository Repository { get; private set; }

        public AuthenticationForm(IDataProvider dataProvider, string databaseFilePath)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
            if (string.IsNullOrEmpty(databaseFilePath))
            {
                throw new ArgumentException("Database file path must be non-empty");
            }

            InitializeComponent();

            _dataProvider = dataProvider;
            _databaseFilePath = databaseFilePath;
        }

        private void passwordTextBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                Repository = _dataProvider.Open(_databaseFilePath, passwordTextBox.Text);
            }
            catch (EncryptedDatabaseException)
            {
                e.Cancel = true;
                errorProvider.SetError(passwordTextBox, "Incorrect password");
            }
        }
    }
}
