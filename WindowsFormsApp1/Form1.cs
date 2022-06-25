using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Cloud.Firestore;
using System.Data.SqlClient;

namespace WindowsFormsApp1
{
    /// <summary>
    /// Brief summary: Class Form1, class where windows forms funcionality lies in
    /// Purpose: of the class is to work as windows forms class
    /// How to use: In this class after double clicking in Form1 there will be automaticly generetade methods for every element
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// string, allows me to hold connection to my MSSQL database
        /// </summary>
        string connection = @"Server=DESKTOP-COBGPTD\SQLEXPRESS;Database=007App;Trusted_Connection=True;";
        /// <summary>
        /// instanc of Firebase, allows to connect to firebase, and exctract specific data
        /// </summary>
        FirestoreDb db;
        /// <summary>
        /// global int, allows to track elements in list
        /// </summary>
        int a = 0;
        /// <summary>
        /// List if class Model, in this list lies all important data from firebase, firestore
        /// </summary>
        List<Model> notes = new List<Model>();
        /// <summary>
        /// constructor, starts entire windows forms 
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// windows forms methode, allows to initialize variables on the start
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"app-42451-firebase-adminsdk-pva4e-132312811a.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("app-42451");
            GetAllData_Of_A_Document();
        }
        /// <summary>
        /// asynchronous funcion collecting data from firebase firestore
        /// </summary>
        /// <returns>list of models from firebase, every single note is one note</returns>
        async Task<List<Model>> GetAllData_Of_A_Document()
        {
            Query query = db.Collection("Notes");
            QuerySnapshot snap = await query.GetSnapshotAsync();
            foreach(DocumentSnapshot docsnap in snap)
            {
                Model model = docsnap.ConvertTo<Model>();
                if (docsnap.Exists&&! model.added)
                {
                    notes.Add(model);
                }
            }
            createDate_TextBox.Text = notes[a].createDate.ToString().Substring(11);
            title_TextBox.Text = notes[a].title;
            localisation_TextBox.Text = notes[a].localisation;
            richTextBox1.Text = notes[a].title;

            return notes;
        }
        /// <summary>
        /// asynchronous methode that allows us to go throw a elements in a List notes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button1_ClickAsync(object sender, EventArgs e)//back
        {
            List<Model> notes = await GetAllData_Of_A_Document();
            if (a != 0 && !notes[a].added)
            {
                a--;
                createDate_TextBox.Text = notes[a].createDate.ToString().Substring(11);
                title_TextBox.Text = notes[a].title;
                localisation_TextBox.Text = notes[a].localisation;
                richTextBox1.Text = notes[a].title;
            }
            else
            {
                MessageBox.Show("Warning, there is no older messeges");
            }
        }

        /// <summary>
        /// asynchronous methode that allows us to go throw a elements in a List notes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Next_Button_Click(object sender, EventArgs e)//next
        {
            List<Model> notes = await GetAllData_Of_A_Document();
            if (a+1 < notes.Count() && !notes[a].added)
            {
                a++;
                createDate_TextBox.Text = notes[a].createDate.ToString().Substring(11);
                title_TextBox.Text = notes[a].title;
                localisation_TextBox.Text = notes[a].localisation;
                richTextBox1.Text = notes[a].title;
            }
            else
            {
                MessageBox.Show("Warning, there are not more elements");
            }
        }

        public int divide(int a, int b)
        {
            return a / b;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void createDate_TextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void localisation_TextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void title_TextBox_TextChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// methode adding selected element from a list notes, a single note into a MSSQL database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Button_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlCon = new SqlConnection(connection))
            {
                sqlCon.Open();
                SqlCommand sqlCmd = new SqlCommand("NotesAdd", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@createDate", notes[a].createDate);
                sqlCmd.Parameters.AddWithValue("@id", notes[a].id);
                sqlCmd.Parameters.AddWithValue("@localisation", notes[a].localisation);
                sqlCmd.Parameters.AddWithValue("@notes", notes[a].notes);
                sqlCmd.Parameters.AddWithValue("@title", notes[a].title.ToString());
                sqlCmd.Parameters.AddWithValue("@uid", notes[a].uid);
                sqlCmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// asynchronous methode updating one element in firebase firestore
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Delete_Button_Click(object sender, EventArgs e)
        {
            DocumentReference docref = db.Collection("Notes").Document(notes[a].id);
            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                { "added",true},
            };
            DocumentSnapshot snap = await docref.GetSnapshotAsync();
            if (snap.Exists)
            {
                await docref.UpdateAsync(data);
            }
        }
    }
}
