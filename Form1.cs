using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SenykEkzamen_
{
    public partial class Form1 : Form
    {
        EkzKPZ model;

        public Form1()
        {
            InitializeComponent();
            model = new EkzKPZ();
            dataGridView.DataSource = model.Calendar.ToList();
            SetBoxes("", "", "", "", "Add", false);
        }

        private void SetBoxes(string _name, string _desc, string _time, string _freq, string _addBtn, bool _enable)
        {
            NameTextBox.Text = _name;
            DescriptionTextBox.Text = _desc;
            TimeTextBox.Text = _time;
            FrequencyTextBox.Text = _freq;
            AddBtn.Text = _addBtn;
            DeleteBtn.Enabled = _enable;
        }

        private string[] GetDataFromTextBoxes()
        {
            return new string[] 
            { 
                NameTextBox.Text.Trim(), 
                DescriptionTextBox.Text.Trim(), 
                TimeTextBox.Text.Trim(),
                FrequencyTextBox.Text.Trim() 
            };
        }

        private void dataGridView_DoubleClick(object sender, EventArgs e)
        {
            this.dataGridView.CurrentCell.Selected = false;
            SetBoxes("", "", "", "", "Add", false);
        }

        private void dataGridView_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int id = Convert.ToInt32(dataGridView.CurrentRow.Cells[0].Value); ;
            Calendar c = model.Calendar.Where(x => x.Id == id).FirstOrDefault();
            SetBoxes(c.Name, c.Description, c.Time.ToString(), c.Frequency, "Update", true);
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            string _name = GetDataFromTextBoxes()[0];
            string _desc = GetDataFromTextBoxes()[1];
            string _time = GetDataFromTextBoxes()[2];
            string _freq = GetDataFromTextBoxes()[3];

            if (AddBtn.Text == "Add")
            {
                model.Calendar.Add(new Calendar { Name = _name, Description = _desc, Time = _time, Frequency = _freq });
            }
            else
            {
                int id = Convert.ToInt32(dataGridView.CurrentRow.Cells[0].Value); ;
                Calendar c = model.Calendar.Where(x => x.Id == id).FirstOrDefault();
                if (c != null)
                {
                    c.Name = _name;
                    c.Description = _desc;
                    c.Time = _time;
                    c.Frequency = _freq;
                    model.Entry(c).State = EntityState.Modified;
                }
            }
            model.SaveChanges();
            SetBoxes("", "", "", "", "Add", false);
            dataGridView.DataSource = model.Calendar.ToList();
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dataGridView.CurrentRow.Cells[0].Value); ;
            Calendar c = model.Calendar.Find(id);
            if (c != null)
            {
                model.Entry(c).State = EntityState.Deleted;
                model.SaveChanges();
            }
            SetBoxes("", "", "", "", "Add", false);
            dataGridView.DataSource = model.Calendar.ToList();
        }

        private void FirstBtn_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dataGridView.Rows[0].Cells[0].Value);
            Calendar c = model.Calendar.Where(x => x.Id == id).FirstOrDefault();
            SetBoxes(c.Name, c.Description, c.Time.ToString(), c.Frequency, "Update", true);
            dataGridView.CurrentCell = dataGridView.Rows[0].Cells[0];
            PrevBtn.Enabled = false;
            NextBtn.Enabled = true;
        }

        private void LastBtn_Click(object sender, EventArgs e)
        {
            List<Calendar> list = model.Calendar.ToList();
            Calendar c = list.First();
            SetBoxes(c.Name, c.Description, c.Time.ToString(), c.Frequency, "Update", true);
            dataGridView.CurrentCell = dataGridView.Rows[dataGridView.RowCount - 1].Cells[0];
            NextBtn.Enabled = false;
            PrevBtn.Enabled = true;
        }

        private void NextBtn_Click(object sender, EventArgs e)
        {
            dataGridView.CurrentCell = dataGridView.Rows[dataGridView.CurrentRow.Index + 1].Cells[0];
            int id = Convert.ToInt32(dataGridView.CurrentRow.Cells[0].Value); ;
            Calendar c = model.Calendar.Where(x => x.Id == id).FirstOrDefault();
            SetBoxes(c.Name, c.Description, c.Time.ToString(), c.Frequency, "Update", true);
            if (dataGridView.CurrentCell.RowIndex != dataGridView.RowCount - 1)
            {
                PrevBtn.Enabled = true;
                NextBtn.Enabled = true;
            }
            else
            {
                PrevBtn.Enabled = true;
                NextBtn.Enabled = false;
            }
        }

        private void PrevBtn_Click(object sender, EventArgs e)
        {
            dataGridView.CurrentCell = dataGridView.Rows[dataGridView.CurrentRow.Index - 1].Cells[0];
            int id = Convert.ToInt32(dataGridView.CurrentRow.Cells[0].Value); ;
            Calendar c = model.Calendar.Where(x => x.Id == id).FirstOrDefault();
            SetBoxes(c.Name, c.Description, c.Time.ToString(), c.Frequency, "Update", true);
            if (dataGridView.CurrentCell.RowIndex != 0)
            {
                PrevBtn.Enabled = true;
                NextBtn.Enabled = true;
            }
            else
            {
                PrevBtn.Enabled = false;
                NextBtn.Enabled = true;
            }
        }
    }
}
