using System.ComponentModel;
using System.Diagnostics;
using System.DirectoryServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace RDP_Shadow_zapaska
{
    public partial class Form1 : Form
    {
        static System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        public Form1()
        {
            InitializeComponent();

            domainComboBox.Items.Add(Environment.UserDomainName);
            domainComboBox.SelectedIndex = 0;

            LDAP_server();

            // Populate session list view
            RefreshSessionList();

            // Устанавливаем интервал таймера в милисекундах
            timer.Interval = 5000;

            // Добавляем обработчик события Tick
            timer.Tick += refreshButton_Click_1;
        }

        private string GetFullNameByLogin(string login)
        {
            using (var entry = new DirectoryEntry($"LDAP://{Environment.UserDomainName}"))
            {
                using (var search = new DirectorySearcher(entry))
                {
                    search.Filter = $"(&(objectClass=user)(sAMAccountName={login}))";
                    search.PropertiesToLoad.Add("displayName");
                    var result = search.FindOne();
                    if (result != null)
                    {
                        return result.Properties["displayName"][0].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
            }
        }

        private void LDAP_server()
        {
            DirectoryEntry entry = new DirectoryEntry("LDAP://DC=" + domainComboBox.Text + ",DC=loc"); // замените на свой адрес сервера AD
            DirectorySearcher searcher = new DirectorySearcher(entry);
            searcher.Filter = "(&(objectClass=computer)(name=*))"; // фильтр поиска на компьютеры

            SearchResultCollection results = searcher.FindAll(); // выполнение поиска и получение результатов
            List<string> computerNames = new List<string>();

            foreach (SearchResult result in results)
            {
                string computerName = result.Properties["Name"][0].ToString(); // получение имени компьютера
                computerNames.Add(computerName); // добавление имени в List<string>
            }

            computerNames.Sort(); // сортировка по алфавиту

            foreach (string computerName in computerNames)
            {
                serverComboBox.Items.Add(computerName); // добавление имени в ComboBox
            }
        }

        private void RefreshSessionList()
        {
            // Создаем словарь, где ключ - идентификатор сессии, а значение - элемент списка
            var sessionDict = new Dictionary<string, ListViewItem>();
            foreach (ListViewItem item in sessionListView.Items)
            {
                if (item.SubItems.Count >= 2)
                {
                    sessionDict[item.SubItems[1].Text] = item;
                }
            }

            Process process = new Process();
            string serverName = serverComboBox.Text;
            string domainName = domainComboBox.Text;
            string userName = userNameTextBox.Text;
            string password = passwordTextBox.Text;
            process.StartInfo.FileName = "query.exe";
            process.StartInfo.Arguments = $"session /server:{serverName}";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.Domain = Environment.UserDomainName;
            process.StartInfo.UserName = userName;
            process.StartInfo.PasswordInClearText = password;
            process.StartInfo.CreateNoWindow = true;
            string output = "";
            try
            {
                process.Start();
                bool processExited = process.WaitForExit(10000); // ожидание 10 секунд
                if (processExited)
                {
                    output = process.StandardOutput.ReadToEnd();
                }
                else
                {
                    process.Kill(); // Убивает процесс
                    MessageBox.Show("Сервер не отвечает", "Сервер не отвечает", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Win32Exception)
            {
                MessageBox.Show("Проверьте логин или пароль", "Неверный логин или пароль", MessageBoxButtons.OK, MessageBoxIcon.Error);
                process.Close();
            }


            // Обновляем или добавляем элементы списка
            string[] lines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 1; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (fields.Length >= 3 && fields[0].Contains("rdp-tcp#"))
                {
                    string sessionId = fields[2];
                    ListViewItem item;
                    if (sessionDict.ContainsKey(sessionId))
                    {
                        // Обновляем значения элемента списка
                        item = sessionDict[sessionId];
                        item.SubItems[0].Text = fields[1];
                        item.SubItems[2].Text = fields[3];
                    }
                    else
                    {
                        // Добавляем новый элемент в список
                        item = new ListViewItem(fields[1]);
                        item.SubItems.Add(sessionId);
                        item.SubItems.Add(fields[3]);
                        sessionListView.Items.Add(item);

                        // Получаем ФИО пользователя и добавляем его в колонку списка
                        string login = fields[1].Substring(fields[1].IndexOf("\\") + 1);
                        string fullName = GetFullNameByLogin(login);
                        item.SubItems.Add(fullName);

                    }
                    // Удаляем сессию из словаря, чтобы в конце остались только удаленные сессии
                    sessionDict.Remove(sessionId);
                }
            }

            // Удаляем элементы списка, соответствующие удаленным сессиям
            foreach (var pair in sessionDict)
            {
                sessionListView.Items.Remove(pair.Value);
            }
        }

        private void refreshButton_Click_1(object sender, EventArgs e)
        {
            RefreshSessionList();
        }

        private void shadowButton_Click_1(object sender, EventArgs e)
        {
            // Get selected session ID
            if (sessionListView.SelectedItems.Count > 0 && sessionListView.SelectedItems[0].SubItems.Count > 1)
            {
                int sessionId = int.Parse(sessionListView.SelectedItems[0].SubItems[1].Text);
                Process process = new Process();
                process.StartInfo.FileName = "mstsc.exe";
                string Options = "";
                if (!checkRequest.Checked)
                {
                    Options = Options + "/noConsentPrompt ";
                }
                if (checkControl.Checked)
                {
                    Options = Options + "/control ";
                }
                // Start mstsc and shadow the selected session

                process.StartInfo.Arguments = $"/shadow:{sessionId} /v:{serverComboBox.Text} {Options}";

                process.Start();
            }
        }

        private void sessionListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            shadowButton_Click_1(sender, e);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                // Вызовите здесь нужный метод
                RefreshSessionList();
                e.Handled = true; // Это предотвращает дальнейшую обработку нажатия клавиши F5 формой
            }
        }

        private void AutoUpdate_CheckedChanged(object sender, EventArgs e)
        {
            if (AutoUpdate.Checked)
            {
                timer.Start();
            }
            else
            {
                timer.Stop();
            }
        }
    }
}