using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BabbleMouth.Windows
{
    /// <summary>
    /// Interaction logic for ExportBBCode.xaml
    /// </summary>
    public partial class ExportBBCode : Window
    {
        public ExportBBCode()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        public void FillRichTextBox(string value)
        {
            try
            {
                ExportRichTextBox.AppendText(value);
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

    }
}
