using Acme.Data;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Acme.Dashboard
{
    public partial class MainForm : Form
    {
        private const string Password = "VerySimplePassword";

        private readonly IDataProviderLoader _dataProviderLoader = new DataProviderLoader();

        private IDataProvider _dataProvider;

        private IDataRepository _repository;

        public MainForm()
        {
            InitializeComponent();

            dataGridView.AutoGenerateColumns = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LoadDataProviders();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseRepository();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                OpenRepository(openFileDialog.FileName);
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseRepository();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataProviderSettingsForm settingsForm = new DataProviderSettingsForm(_dataProviderLoader, _dataProvider);

            if (settingsForm.ShowDialog() == DialogResult.OK)
            {
                _dataProvider = settingsForm.DataProvider;
            }
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var changePasswordForm = new ChangePasswordForm(_repository);

            if (changePasswordForm.ShowDialog() == DialogResult.OK)
            {
                _repository.Password = changePasswordForm.NewPassword;
            }
        }

        private async void refreshButton_Click(object sender, EventArgs e)
        {
            await RefreshDataAsync();
        }

        private void LoadDataProviders()
        {
            _dataProviderLoader.Load();

            _dataProvider = _dataProviderLoader.DataProviders.FirstOrDefault();

            if (_dataProvider == null)
            {
                throw new InvalidOperationException("There are no available data providers");
            }
        }

        private async void OpenRepository(string databaseFilePath)
        {
            try
            {
                _repository = _dataProvider.Open(databaseFilePath);
            }
            catch (EncryptedDatabaseException)
            {
                // The database is encrypted with a password. We should swallow this exception and show the authentication form.
            }

            if (_repository == null)
            {
                if (_dataProvider.SupportsEncryption)
                {
                    var authenticationForm = new AuthenticationForm(_dataProvider, databaseFilePath);

                    if (authenticationForm.ShowDialog() == DialogResult.OK)
                    {
                        _repository = authenticationForm.Repository;
                    }
                }
                else
                {
                    ShowError(
                        string.Format("Cannot open the database. The database file '{0}' is encrypted, but provider '{1}' does not support the ability to work with encrypted databases.",
                            databaseFilePath,
                            _dataProvider.Name));
                }
            }

            await RefreshDataAsync();

            UpdateButtons();
            UpdateCaption(databaseFilePath);
        }

        private void CloseRepository()
        {
            var dataSource = bindingSource.DataSource as IDisposable;

            bindingSource.DataSource = null;

            if (dataSource != null)
            {
                dataSource.Dispose();
            }

            if (_repository != null)
            {
                _repository.Dispose();
                _repository = null;
            }

            UpdateButtons();
            UpdateCaption(null);
        }

        private async Task RefreshDataAsync()
        {
            var progressForm = new ProgressForm("Refreshing data...");

            BeginInvoke(
                new Action(() => { progressForm.ShowDialog(); })
            );

            try
            {
                bindingSource.DataSource = await Task<DataTable>.Factory.StartNew(
                    () =>
                    {
                        return _repository.GetData().Tables[0];
                    });
            }
            finally
            {
                progressForm.Dispose();
            }
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void UpdateButtons()
        {
            var enabled = _repository != null;

            changePasswordToolStripMenuItem.Enabled = enabled;
            refreshButton.Enabled = enabled;
            closeToolStripMenuItem.Enabled = enabled;
        }

        private void UpdateCaption(string databaseFilePath)
        {
            var caption = "Acme Dashboard";

            if (!string.IsNullOrEmpty(databaseFilePath))
            {
                caption += string.Format(" - {0}", databaseFilePath);
            }

            Text = caption;
        }
    }
}
