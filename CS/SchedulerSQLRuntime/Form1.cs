using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraScheduler;

namespace SchedulerSQLRuntime {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();

            // Subscribe to Storage events required for updating the data source. 
            this.schedulerDataStorage1.AppointmentsInserted += OnApptChangedInsertedDeleted;
            this.schedulerDataStorage1.AppointmentsChanged += OnApptChangedInsertedDeleted;
            this.schedulerDataStorage1.AppointmentsDeleted += OnApptChangedInsertedDeleted;

            //// Uncomment the code below to demonstrate how to store and retrieve data in the appointment custom field.
            //// Do not forget to uncomment event handlers.
            //this.schedulerControl1.InitAppointmentDisplayText += schedulerControl1_InitAppointmentDisplayText;
            //this.schedulerControl1.InitNewAppointment += schedulerControl1_InitNewAppointment;
        }

        // Modify this string if required to connect to your database.
        const string SchedulerDBConnection = "Data Source=(LocalDB)\\v11.0;AttachDbFilename=|DataDirectory|Data\\DXDBScheduler.mdf;Integrated Security=True;Connect Timeout=30";
        
        DataSet DXSchedulerDataset;        
        SqlDataAdapter AppointmentDataAdapter;
        SqlDataAdapter ResourceDataAdapter;
        SqlConnection DXSchedulerConn;

        private void Form1_Load(object sender, EventArgs e) {

            this.schedulerDataStorage1.Appointments.ResourceSharing= true;
            this.schedulerControl1.GroupType = SchedulerGroupType.Resource;
            this.schedulerControl1.Start = DateTime.Today;
            
            DXSchedulerDataset = new DataSet();
            string selectAppointments = "SELECT * FROM Appointments";
            string selectResources = "SELECT * FROM resources";

            DXSchedulerConn = new SqlConnection(SchedulerDBConnection);
            DXSchedulerConn.Open();

            AppointmentDataAdapter = new SqlDataAdapter(selectAppointments, DXSchedulerConn);
            // Subscribe to RowUpdated event to retrieve identity value for an inserted row.
            AppointmentDataAdapter.RowUpdated += new SqlRowUpdatedEventHandler(AppointmentDataAdapter_RowUpdated);
            AppointmentDataAdapter.Fill(DXSchedulerDataset, "Appointments");

            ResourceDataAdapter = new SqlDataAdapter(selectResources, DXSchedulerConn);
            ResourceDataAdapter.Fill(DXSchedulerDataset, "Resources");

            // Specify mappings.
            MapAppointmentData();
            MapResourceData();

            // Generate commands using CommandBuilder.  
            SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(AppointmentDataAdapter);
            AppointmentDataAdapter.InsertCommand = cmdBuilder.GetInsertCommand();
            AppointmentDataAdapter.DeleteCommand = cmdBuilder.GetDeleteCommand();
            AppointmentDataAdapter.UpdateCommand = cmdBuilder.GetUpdateCommand();

            DXSchedulerConn.Close();

            this.schedulerDataStorage1.Appointments.DataSource = DXSchedulerDataset;
            this.schedulerDataStorage1.Appointments.DataMember = "Appointments";
            this.schedulerDataStorage1.Resources.DataSource = DXSchedulerDataset;
            this.schedulerDataStorage1.Resources.DataMember = "Resources";
        }

        private void MapAppointmentData()
        {
            this.schedulerDataStorage1.Appointments.Mappings.AllDay = "AllDay";
            this.schedulerDataStorage1.Appointments.Mappings.Description = "Description";
            // Required mapping.
            this.schedulerDataStorage1.Appointments.Mappings.End = "EndDate";
            this.schedulerDataStorage1.Appointments.Mappings.Label = "Label";
            this.schedulerDataStorage1.Appointments.Mappings.Location = "Location";
            this.schedulerDataStorage1.Appointments.Mappings.RecurrenceInfo = "RecurrenceInfo";
            this.schedulerDataStorage1.Appointments.Mappings.ReminderInfo = "ReminderInfo";
            // Required mapping.
            this.schedulerDataStorage1.Appointments.Mappings.Start = "StartDate";
            this.schedulerDataStorage1.Appointments.Mappings.Status = "Status";
            this.schedulerDataStorage1.Appointments.Mappings.Subject = "Subject";
            this.schedulerDataStorage1.Appointments.Mappings.Type = "Type";
            this.schedulerDataStorage1.Appointments.Mappings.ResourceId = "ResourceIDs";
            this.schedulerDataStorage1.Appointments.CustomFieldMappings.Add(new AppointmentCustomFieldMapping("MyNote", "CustomField1")); 
        }

        private void MapResourceData()
        {
            this.schedulerDataStorage1.Resources.Mappings.Id = "ResourceID";
            this.schedulerDataStorage1.Resources.Mappings.Caption = "ResourceName";
        }

        // Retrieve identity value for an inserted appointment.
        void AppointmentDataAdapter_RowUpdated(object sender, SqlRowUpdatedEventArgs e)
        {
            if (e.Status == UpdateStatus.Continue && e.StatementType == StatementType.Insert) {
                int id = 0;
                using (SqlCommand cmd = new SqlCommand("SELECT IDENT_CURRENT('Appointments')", DXSchedulerConn))
                {
                    id = Convert.ToInt32(cmd.ExecuteScalar());
                }
                e.Row["UniqueID"] = id;
            }
        }

        // Store modified data in the database
        private void OnApptChangedInsertedDeleted(object sender, PersistentObjectsEventArgs e) {
            AppointmentDataAdapter.Update(DXSchedulerDataset.Tables["Appointments"]);
            DXSchedulerDataset.AcceptChanges();
        }
        
        //// Uncomment the code below to demonstrate how to store and retrieve data in the appointment custom field.
        //// Do not forget to uncomment event subscription code in the form constructor.

        //// Store a custom value in the newly created appointment.
        //private void schedulerControl1_InitNewAppointment(object sender, AppointmentEventArgs e)
        //{
        //    e.Appointment.CustomFields["MyNote"] = String.Format("Created on {0:d} at {0:t} \n", DateTime.Now);
        //}

        // Modify default appointment text to display a custom value.
        //private void schedulerControl1_InitAppointmentDisplayText(object sender, AppointmentDisplayTextEventArgs e)
        //{
        //    e.Text = (e.Appointment.CustomFields["MyNote"] is DBNull) ? String.Empty : (string)e.Appointment.CustomFields["MyNote"];
        //}
    }
}