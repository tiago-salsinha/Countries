namespace Countries.Servicos
{
    using System.Windows.Forms;
    public class DialogService
    {
        /// <summary>
        /// //Método para comunicar as mensagens de erro
        /// </summary>
        public void ShowMessage(string title, string message)
        {
            MessageBox.Show(message, title);
        }
    }
}
