using Acme.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Acme.Dashboard
{
    public partial class DataProviderSettingsForm : SettingsForm
    {
        private IDataProviderLoader _dataProviderLoader;

        public IDataProvider DataProvider
        {
            get { return (IDataProvider)dataProviderComboBox.SelectedValue; }
        }

        public DataProviderSettingsForm(IDataProviderLoader dataProviderLoader, IDataProvider selectedDataProvider)
        {
            if (dataProviderLoader == null)
            {
                throw new ArgumentNullException("dataProviderLoader");
            }

            _dataProviderLoader = dataProviderLoader;

            InitializeComponent();

            dataProviderComboBox.DataSource = _dataProviderLoader.DataProviders;
            dataProviderComboBox.DisplayMember = "Name";

            if (selectedDataProvider != null)
            {
                dataProviderComboBox.SelectedItem = selectedDataProvider;
            }
        }

        private void dataProviderComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            okButton.Enabled = dataProviderComboBox.SelectedValue != null;
        }
    }
}
