using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OracleClient;

namespace WindowsApplication1 {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        const string CarsDBConnection = "Data Source=(local); Initial Catalog=CarsDB; Integrated Security=SSPI";
        DataSet CarsDataset;
        
        SqlDataAdapter CarsAdapter;
        SqlConnection CarsConn;

        private void Form1_Load(object sender, EventArgs e) {
            CarsDataset = new DataSet();
            string selectAppointments = "SELECT * FROM CarScheduling";

            CarsConn = new SqlConnection(CarsDBConnection);
            CarsConn.Open();
            CarsAdapter = new SqlDataAdapter(selectAppointments, CarsConn);
            CarsAdapter.RowUpdated += new SqlRowUpdatedEventHandler(CarsAdapter_RowUpdated);
            CarsAdapter.Fill(CarsDataset, "CarScheduling");

            this.appointmentsBS.DataSource = CarsDataset;
            this.appointmentsBS.DataMember = "CarScheduling";
            this.appointmentsBS.Position = 0;


            this.schedulerStorage1.Appointments.DataSource = this.appointmentsBS;
            this.schedulerStorage1.Appointments.Mappings.AllDay = "AllDay";
            this.schedulerStorage1.Appointments.Mappings.Description = "Description";
            this.schedulerStorage1.Appointments.Mappings.End = "EndTime";
            this.schedulerStorage1.Appointments.Mappings.Label = "Label";
            this.schedulerStorage1.Appointments.Mappings.Location = "Location";
            this.schedulerStorage1.Appointments.Mappings.RecurrenceInfo = "RecurrenceInfo";
            this.schedulerStorage1.Appointments.Mappings.ReminderInfo = "ReminderInfo";
            this.schedulerStorage1.Appointments.Mappings.Start = "StartTime";
            this.schedulerStorage1.Appointments.Mappings.Status = "Status";
            this.schedulerStorage1.Appointments.Mappings.Subject = "Subject";
            this.schedulerStorage1.Appointments.Mappings.Type = "EventType";

            SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(CarsAdapter);
            CarsAdapter.InsertCommand = cmdBuilder.GetInsertCommand();
            CarsAdapter.DeleteCommand = cmdBuilder.GetDeleteCommand();
            CarsAdapter.UpdateCommand = cmdBuilder.GetUpdateCommand();
            CarsConn.Close();
        }

        void CarsAdapter_RowUpdated(object sender, SqlRowUpdatedEventArgs e) {
            if (e.Status == UpdateStatus.Continue && e.StatementType == StatementType.Insert) {
                int id = 0;
                using (SqlCommand cmd = new SqlCommand("SELECT IDENT_CURRENT('CarScheduling')", CarsConn)) {
                    id = Convert.ToInt32(cmd.ExecuteScalar());
                }
                e.Row["ID"] = id;
            }
        }

        private void OnApptChangedInsertedDeleted(object sender, DevExpress.XtraScheduler.PersistentObjectsEventArgs e) {
            CarsAdapter.Update(CarsDataset.Tables["CarScheduling"]);
        }

    }
}