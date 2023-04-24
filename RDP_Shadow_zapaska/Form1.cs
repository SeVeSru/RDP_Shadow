using System.Diagnostics;
using System.DirectoryServices;

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

            // ������� ������ System.Windows.Forms.Timer
            var timer = new System.Windows.Forms.Timer();

            // ������������� �������� ������� � 1 ������
            timer.Interval = 5000;

            // ��������� ���������� ������� Tick
            timer.Tick += refreshButton_Click_1;
        }

        private void LDAP_server()
        {

            DirectoryEntry entry = new DirectoryEntry("LDAP://DC=" + domainComboBox.Text + ",DC=loc"); // �������� �� ���� ����� ������� AD
            DirectorySearcher searcher = new DirectorySearcher(entry);
            searcher.Filter = "(&(objectClass=computer)(name=*))"; // ������ ������ �� ����������

            SearchResultCollection results = searcher.FindAll(); // ���������� ������ � ��������� �����������
            List<string> computerNames = new List<string>();

            foreach (SearchResult result in results)
            {
                string computerName = result.Properties["Name"][0].ToString(); // ��������� ����� ����������
                computerNames.Add(computerName); // ���������� ����� � List<string>
            }

            computerNames.Sort(); // ���������� �� ��������

            foreach (string computerName in computerNames)
            {
                serverComboBox.Items.Add(computerName); // ���������� ����� � ComboBox
            }
        }

        private void RefreshSessionList()
        {
            // ������� �������, ��� ���� - ������������� ������, � �������� - ������� ������
            var sessionDict = new Dictionary<string, ListViewItem>();
            foreach (ListViewItem item in sessionListView.Items)
            {
                sessionDict[item.SubItems[1].Text] = item;
            }

            Process process = new Process();
            string serverName = serverComboBox.Text;
            string domainName = domainComboBox.Text;
            process.StartInfo.FileName = "query.exe";
            process.StartInfo.Arguments = $"session /server:{serverName}";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.Domain = Environment.UserDomainName;
            process.StartInfo.UserName = "";
            process.StartInfo.PasswordInClearText = "";
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            // ��������� ��� ��������� �������� ������
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
                        // ��������� �������� �������� ������
                        item = sessionDict[sessionId];
                        item.SubItems[0].Text = fields[1];
                        item.SubItems[2].Text = fields[3];
                    }
                    else
                    {
                        // ��������� ����� ������� � ������
                        item = new ListViewItem(fields[1]);
                        item.SubItems.Add(sessionId);
                        item.SubItems.Add(fields[3]);
                        sessionListView.Items.Add(item);
                    }
                    // ������� ������ �� �������, ����� � ����� �������� ������ ��������� ������
                    sessionDict.Remove(sessionId);
                }
            }

            // ������� �������� ������, ��������������� ��������� �������
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
                // Start mstsc and shadow the selected session

                    process.StartInfo.Arguments = $"/shadow:{sessionId} /v:{serverComboBox.Text} /noConsentPrompt /control";

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
                // �������� ����� ������ �����
                RefreshSessionList();
                e.Handled = true; // ��� ������������� ���������� ��������� ������� ������� F5 ������
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