using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Lab6TA
{
    public partial class Form1 : Form
    {
        private int tableSize = 22;
        private readonly Dictionary<int, KeyValuePair<string, string>> HashTable =
            new Dictionary<int, KeyValuePair<string, string>>(22);
        private Dictionary<string, string>[] HashValues;

        public Form1()
        {
            InitializeComponent();
            HashValues = new Dictionary<string, string>[tableSize];
            for (int i = 0; i < tableSize; i++)
            {
                listBox1.Items.Add(" ");
            }
        }

        private void AddByLinearProbing(object sender, EventArgs e)
        {
            string name = textBoxSurname.Text.Trim();
            string birthday = birthdayTimePicker.Value.ToShortDateString();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Введіть прізвище студентки!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (HashTable.Count >= tableSize)
            {
                MessageBox.Show("Таблиця вже заповнена! Необіхдно провести реструктуризацію.",
                    "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            else
            {
                int hash = DivisionMethod(name);
                int index = Math.Abs(hash) % tableSize;
                index = LinearProbing(index, tableSize);
                HashTable.Add(index, new KeyValuePair<string, string>(name, birthday));
                listBox1.Items[index] = $"Ім'я: {name}\t Дата: {birthday}\t Хеш: {hash}";

                if (HashValues[index] == null) {
                    HashValues[index] = new Dictionary<string, string>();
                }
                HashValues[index].Add(name, hash.ToString());
            }
        }

        private void AddByQuadraticProbing(object sender, EventArgs e)
        {
            string name = textBoxSurname.Text.Trim();
            string birthday = birthdayTimePicker.Value.ToShortDateString();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Введіть прізвище студентки!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (HashTable.Count >= tableSize)
            {
                MessageBox.Show("Таблиця вже заповнена! Необхідно провести реструктуризацію.", 
                    "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            else
            {
                int hash = DivisionMethod(name);
                int index = Math.Abs(hash) % tableSize;
                index = QuadraticProbing(index, tableSize);
                HashTable.Add(index, new KeyValuePair<string, string>(name, birthday));
                listBox1.Items[index] = $"Ім'я: {name}\t Дата: {birthday}\t Хеш: {hash}";

                if (HashValues[index] == null)
                {
                    HashValues[index] = new Dictionary<string, string>();
                }
                HashValues[index].Add(name, hash.ToString());
            }
        }

        private int LinearProbing(int index, int tableSize)
        {
            while (HashTable.ContainsKey(index))
            {
                index = Math.Abs(index + 1) % tableSize;
            }
            return index;
        }

        private int QuadraticProbing(int index, int tableSize)
        {
            int i = 0;
            while (HashTable.ContainsKey(index))
            {
                index = Math.Abs(index + i * i) % tableSize;
                i++;
            }
            return index;
        }

        private int DivisionMethod(string name)
        {
            int M = 491;
            return Math.Abs(name.GetHashCode() % M);
        }

        private void DeleteStudent(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Будь ласка, оберіть студентку для видалення!", "Помилка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (listBox1.SelectedItem.ToString() != " ")
            {
                int selectedIndex = listBox1.SelectedIndex;
                string selectedName = listBox1.SelectedItem.ToString().Split('\t')[0].Split(':')[1].Trim();

                if (HashTable.ContainsKey(selectedIndex))
                {
                    HashTable.Remove(selectedIndex);
                }

                if (HashValues[selectedIndex] != null)
                {
                    HashValues[selectedIndex].Remove(selectedName);
                }

                listBox1.Items.RemoveAt(selectedIndex);
                listBox1.Items.Insert(selectedIndex, " ");
            }
        }


        private void ShowStudentByDate(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            string birthday = chooseDate.Value.ToShortDateString();
            bool found = false;

            foreach (var entry in HashTable)
            {
                if (entry.Value.Value == birthday)
                {
                    listView1.Items.Add(entry.Value.Key);
                    found = true;
                }
            }

            if (!found)
            {
                MessageBox.Show($"Немає студенток, дата народження яких - {birthday}",
                    "Помилка", MessageBoxButtons.OK);
            }
        }

        private void ClearDateHashTable(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void ShowStudentByHash(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            int searchedHash;
            if (!int.TryParse(textBox1.Text.Trim(), out searchedHash))
            {
                MessageBox.Show("Введіть коректний хеш!", "Помилка", MessageBoxButtons.OK);
                return;
            }

            bool hashFound = false;

            foreach (var entry in HashValues)
            {
                if (entry != null && entry.ContainsValue(searchedHash.ToString()))
                {
                    foreach (var student in entry)
                    {
                        if (student.Key != null && student.Value == searchedHash.ToString())
                        {
                            listView2.Items.Add(student.Key);
                            hashFound = true;
                        }
                    }
                }
            }

            if (!hashFound)
            {
                MessageBox.Show("Не існує дати народження, якій відповідає введений хеш.",
                    "Помилка", MessageBoxButtons.OK);
            }

            textBox1.Clear();
        }

        private void ClearDateTable(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listView2.Items.Clear();
            textBox1.Clear();
            for (int i = 0; i < tableSize; i++)
            {
                listBox1.Items.Add(" ");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void LabelDate_Click(object sender, EventArgs e)
        {

        }

        private void LabelSurname_Click(object sender, EventArgs e)
        {

        }

        private void LabelDateChoose_Click(object sender, EventArgs e)
        {

        }
    }
}