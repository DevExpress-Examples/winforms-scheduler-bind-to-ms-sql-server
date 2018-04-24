Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.Data.SqlClient
Imports System.Data.OracleClient
Imports DevExpress.XtraScheduler

Namespace SchedulerSQLRuntime
    Partial Public Class Form1
        Inherits Form

        Public Sub New()
            InitializeComponent()

            ' Subscribe to Storage events required for updating the data source. 
            AddHandler Me.schedulerStorage1.AppointmentsInserted, AddressOf OnApptChangedInsertedDeleted
            AddHandler Me.schedulerStorage1.AppointmentsChanged, AddressOf OnApptChangedInsertedDeleted
            AddHandler Me.schedulerStorage1.AppointmentsDeleted, AddressOf OnApptChangedInsertedDeleted

            '// Uncomment the code below to demonstrate how to store and retrieve data in the appointment custom field.
            '// Do not forget to uncomment event handlers.
            'this.schedulerControl1.InitAppointmentDisplayText += schedulerControl1_InitAppointmentDisplayText;
            'this.schedulerControl1.InitNewAppointment += schedulerControl1_InitNewAppointment;


        End Sub

        ' Modify this string if required to connect to your database.
        Private Const SchedulerDBConnection As String = "Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|Data\DXDBScheduler.mdf;Integrated Security=True;Connect Timeout=30"

        Private DXSchedulerDataset As DataSet
        Private AppointmentDataAdapter As SqlDataAdapter
        Private ResourceDataAdapter As SqlDataAdapter
        Private DXSchedulerConn As SqlConnection

        Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

            Me.schedulerStorage1.Appointments.ResourceSharing= True
            Me.schedulerControl1.GroupType = SchedulerGroupType.Resource
            Me.schedulerControl1.Start = Date.Today

            DXSchedulerDataset = New DataSet()
            Dim selectAppointments As String = "SELECT * FROM Appointments"
            Dim selectResources As String = "SELECT * FROM resources"

            DXSchedulerConn = New SqlConnection(SchedulerDBConnection)
            DXSchedulerConn.Open()

            AppointmentDataAdapter = New SqlDataAdapter(selectAppointments, DXSchedulerConn)
            ' Subscribe to RowUpdated event to retrieve identity value for an inserted row.
            AddHandler AppointmentDataAdapter.RowUpdated, AddressOf AppointmentDataAdapter_RowUpdated
            AppointmentDataAdapter.Fill(DXSchedulerDataset, "Appointments")

            ResourceDataAdapter = New SqlDataAdapter(selectResources, DXSchedulerConn)
            ResourceDataAdapter.Fill(DXSchedulerDataset, "Resources")

            ' Specify mappings.
            MapAppointmentData()
            MapResourceData()

            ' Generate commands using CommandBuilder.  
            Dim cmdBuilder As New SqlCommandBuilder(AppointmentDataAdapter)
            AppointmentDataAdapter.InsertCommand = cmdBuilder.GetInsertCommand()
            AppointmentDataAdapter.DeleteCommand = cmdBuilder.GetDeleteCommand()
            AppointmentDataAdapter.UpdateCommand = cmdBuilder.GetUpdateCommand()

            DXSchedulerConn.Close()

            Me.schedulerStorage1.Appointments.DataSource = DXSchedulerDataset
            Me.schedulerStorage1.Appointments.DataMember = "Appointments"
            Me.schedulerStorage1.Resources.DataSource = DXSchedulerDataset
            Me.schedulerStorage1.Resources.DataMember = "Resources"

        End Sub

        Private Sub MapAppointmentData()
            Me.schedulerStorage1.Appointments.Mappings.AllDay = "AllDay"
            Me.schedulerStorage1.Appointments.Mappings.Description = "Description"
            ' Required mapping.
            Me.schedulerStorage1.Appointments.Mappings.End = "EndDate"
            Me.schedulerStorage1.Appointments.Mappings.Label = "Label"
            Me.schedulerStorage1.Appointments.Mappings.Location = "Location"
            Me.schedulerStorage1.Appointments.Mappings.RecurrenceInfo = "RecurrenceInfo"
            Me.schedulerStorage1.Appointments.Mappings.ReminderInfo = "ReminderInfo"
            ' Required mapping.
            Me.schedulerStorage1.Appointments.Mappings.Start = "StartDate"
            Me.schedulerStorage1.Appointments.Mappings.Status = "Status"
            Me.schedulerStorage1.Appointments.Mappings.Subject = "Subject"
            Me.schedulerStorage1.Appointments.Mappings.Type = "Type"
            Me.schedulerStorage1.Appointments.Mappings.ResourceId = "ResourceIDs"
            Me.schedulerStorage1.Appointments.CustomFieldMappings.Add(New AppointmentCustomFieldMapping("MyNote", "CustomField1"))
        End Sub

        Private Sub MapResourceData()
            Me.schedulerStorage1.Resources.Mappings.Id = "ResourceID"
            Me.schedulerStorage1.Resources.Mappings.Caption = "ResourceName"
        End Sub

        ' Retrieve identity value for an inserted appointment.
        Private Sub AppointmentDataAdapter_RowUpdated(ByVal sender As Object, ByVal e As SqlRowUpdatedEventArgs)
            If e.Status = UpdateStatus.Continue AndAlso e.StatementType = StatementType.Insert Then
                Dim id As Integer = 0
                Using cmd As New SqlCommand("SELECT IDENT_CURRENT('Appointments')", DXSchedulerConn)
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

        '// Uncomment the code below to demonstrate how to store and retrieve data in the appointment custom field.
        '// Do not forget to uncomment event subscription code in the form constructor.

        '// Store a custom value in the newly created appointment.
        'private void schedulerControl1_InitNewAppointment(object sender, AppointmentEventArgs e)
        '{
        '    e.Appointment.CustomFields["MyNote"] = String.Format("Created on {0:d} at {0:t} \n", DateTime.Now);
        '}

        '// Modify default appointment text to display a custom value.
        'private void schedulerControl1_InitAppointmentDisplayText(object sender, AppointmentDisplayTextEventArgs e)
        '{
        '    e.Text = (e.Appointment.CustomFields["MyNote"] is DBNull) ? String.Empty : (string)e.Appointment.CustomFields["MyNote"];
        '}

    End Class
End Namespace