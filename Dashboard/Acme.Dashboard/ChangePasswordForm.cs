using Acme.Data;
using System;
using System.ComponentModel;

namespace Acme.Dashboard
{
    public partial class ChangePasswordForm : SettingsForm
    {
        private readonly IDataRepository _repository;

        public string NewPassword
        {
            get { return newPasswordTextBox.Text; }
        }

        public ChangePasswordForm(IDataRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }

            _repository = repository;

            InitializeComponent();
        }

        private void oldPasswordTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (_repository.Password != oldPasswordTextBox.Text)
            {
                e.Cancel = true;
                errorProvider.SetError(oldPasswordTextBox, "Incorrect old password");
            }
        }
    }
}
