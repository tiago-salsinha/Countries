namespace Countries.Servicos
{
    using System.Windows.Forms;
    public class DialogService
    {
        /// <summary>
        /// Method to send error messages to the user
        /// </summary>
        public void ShowMessage(string title, string message)
        {
            MessageBox.Show(message, title);
        }
    }
}
