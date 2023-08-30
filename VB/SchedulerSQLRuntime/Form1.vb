Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Data.SqlClient
Imports DevExpress.XtraScheduler

Namespace SchedulerSQLRuntime

    Public Partial Class Form1
        Inherits Form

        Public Sub New()
            InitializeComponent()
            ' Subscribe to Storage events required for updating the data source. 
            AddHandler schedulerStorage1.AppointmentsInserted, AddressOf OnApptChangedInsertedDeleted
            AddHandler schedulerStorage1.AppointmentsChanged, AddressOf OnApptChangedInsertedDeleted
            AddHandler schedulerStorage1.AppointmentsDeleted, AddressOf OnApptChangedInsertedDeleted
        ''' Uncomment the code below to demonstrate how to store and retrieve data in the appointment custom field.
        ''' Do not forget to uncomment event handlers.
        'this.schedulerControl1.InitAppointmentDisplayText += schedulerControl1_InitAppointmentDisplayText;
        'this.schedulerControl1.InitNewAppointment += schedulerControl1_InitNewAppointment;
        End Sub

        ' Modify this string if required to connect to your database.
        Const SchedulerDBConnection As String = "Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|Data\DXDBScheduler.mdf;Integrated Security=True;Connect Timeout=30"

        Private DXSchedulerDataset As DataSet

        Private AppointmentDataAdapter As SqlDataAdapter

        Private ResourceDataAdapter As SqlDataAdapter

        Private DXSchedulerConn As SqlConnection

        Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs)
            schedulerStorage1.Appointments.ResourceSharing = True
            schedulerControl1.GroupType = SchedulerGroupType.Resource
            schedulerControl1.Start = Date.Today
            DXSchedulerDataset = New DataSet()
            Dim selectAppointments As String = "SELECT * FROM Appointments"
            Dim selectResources As String = "SELECT * FROM resources"
            DXSchedulerConn = New SqlConnection(SchedulerDBConnection)
            DXSchedulerConn.Open()
            AppointmentDataAdapter = New SqlDataAdapter(selectAppointments, DXSchedulerConn)
            ' Subscribe to RowUpdated event to retrieve identity value for an inserted row.
            AddHandler AppointmentDataAdapter.RowUpdated, New SqlRowUpdatedEventHandler(AddressOf AppointmentDataAdapter_RowUpdated)
            AppointmentDataAdapter.Fill(DXSchedulerDataset, "Appointments")
            ResourceDataAdapter = New SqlDataAdapter(selectResources, DXSchedulerConn)
            ResourceDataAdapter.Fill(DXSchedulerDataset, "Resources")
            ' Specify mappings.
            MapAppointmentData()
            MapResourceData()
            ' Generate commands using CommandBuilder.  
            Dim cmdBuilder As SqlCommandBuilder = New SqlCommandBuilder(AppointmentDataAdapter)
            AppointmentDataAdapter.InsertCommand = cmdBuilder.GetInsertCommand()
            AppointmentDataAdapter.DeleteCommand = cmdBuilder.GetDeleteCommand()
            AppointmentDataAdapter.UpdateCommand = cmdBuilder.GetUpdateCommand()
            DXSchedulerConn.Close()
            schedulerStorage1.Appointments.DataSource = DXSchedulerDataset
            schedulerStorage1.Appointments.DataMember = "Appointments"
            schedulerStorage1.Resources.DataSource = DXSchedulerDataset
            schedulerStorage1.Resources.DataMember = "Resources"
        End Sub

        Private Sub MapAppointmentData()
            schedulerStorage1.Appointments.Mappings.AllDay = "AllDay"
            schedulerStorage1.Appointments.Mappings.Description = "Description"
            ' Required mapping.
            schedulerStorage1.Appointments.Mappings.End = "EndDate"
            schedulerStorage1.Appointments.Mappings.Label = "Label"
            schedulerStorage1.Appointments.Mappings.Location = "Location"
            schedulerStorage1.Appointments.Mappings.RecurrenceInfo = "RecurrenceInfo"
            schedulerStorage1.Appointments.Mappings.ReminderInfo = "ReminderInfo"
            ' Required mapping.
            schedulerStorage1.Appointments.Mappings.Start = "StartDate"
            schedulerStorage1.Appointments.Mappings.Status = "Status"
            schedulerStorage1.Appointments.Mappings.Subject = "Subject"
            schedulerStorage1.Appointments.Mappings.Type = "Type"
            schedulerStorage1.Appointments.Mappings.ResourceId = "ResourceIDs"
            schedulerStorage1.Appointments.CustomFieldMappings.Add(New AppointmentCustomFieldMapping("MyNote", "CustomField1"))
        End Sub

        Private Sub MapResourceData()
            schedulerStorage1.Resources.Mappings.Id = "ResourceID"
            schedulerStorage1.Resources.Mappings.Caption = "ResourceName"
        End Sub

        ' Retrieve identity value for an inserted appointment.
        Private Sub AppointmentDataAdapter_RowUpdated(ByVal sender As Object, ByVal e As SqlRowUpdatedEventArgs)
            If e.Status = UpdateStatus.Continue AndAlso e.StatementType = StatementType.Insert Then
                Dim id As Integer = 0
                Using cmd As SqlCommand = New SqlCommand("SELECT IDENT_CURRENT('Appointments')", DXSchedulerConn)
                    id = Convert.ToInt32(cmd.ExecuteScalar())
                End Using

                e.Row("UniqueID") = id
            End If
        End Sub

        ' Store modified data in the database
        Private Sub OnApptChangedInsertedDeleted(ByVal sender As Object, ByVal e As PersistentObjectsEventArgs)
            AppointmentDataAdapter.Update(DXSchedulerDataset.Tables("Appointments"))
            DXSchedulerDataset.AcceptChanges()
        End Sub
    ''' Uncomment the code below to demonstrate how to store and retrieve data in the appointment custom field.
    ''' Do not forget to uncomment event subscription code in the form constructor.
    ''' Store a custom value in the newly created appointment.
    'private void schedulerControl1_InitNewAppointment(object sender, AppointmentEventArgs e)
    '{
    '    e.Appointment.CustomFields["MyNote"] = String.Format("Created on {0:d} at {0:t} \n", DateTime.Now);
    '}
    ''' Modify default appointment text to display a custom value.
    'private void schedulerControl1_InitAppointmentDisplayText(object sender, AppointmentDisplayTextEventArgs e)
    '{
    '    e.Text = (e.Appointment.CustomFields["MyNote"] is DBNull) ? String.Empty : (string)e.Appointment.CustomFields["MyNote"];
    '}
    End Class
End Namespace
